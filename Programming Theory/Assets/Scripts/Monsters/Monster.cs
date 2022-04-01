using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    // Base Monster Stats
    // ENCAPSULATION
    public virtual int BaseStrength { get; set; }
    public virtual int BaseDefense { get; set; }
    public virtual int BaseSpeed { get; set; }
    public int MaxHitPoints { get; private set; }
    private int hitPoints;

    private string attackName;
    public string AttackName
    {
        get { return attackName; }
        set { attackName = value; }
    }

    // The baseAttackDelay is alwyas 20 for every monster as it is divided by speed to determine attack rate
    protected const int baseAttackDelay = 20;
    // Store the duration since the last attack to determine whether we should attack again, and to manage the turn timer
    private float timeSinceLastAttack = 0;
    // Attack delay is the minimum time between attacks
    public abstract float AttackDelay { get; }
    // The baseDamage is 5 and is modified based on the difference of strength & damage for attacks
    private const int baseDamage = 10;
    // This is a boolean that lets us know if it is the player or not
    public bool isPlayer = false;
    // Storage for the target for attack
    private Monster target;

    [SerializeField] ProgressBar healthBar;
    [SerializeField] ProgressBar turnTimer;

    protected virtual void Awake()
    {
        AssignBaseInfo();
        ApplyUpgrades();
    }

    /// <summary>
    /// Provides the adjusted monster strength
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public int Strength(bool isPlayer = false)
    {
        if (isPlayer)
        {
            return BaseStrength + MainManager.Instance.StrengthMod;
        } else
        {
            return BaseStrength + MainManager.Instance.EnemyStrMod;
        }
    }

    /// <summary>
    /// Provides the adjusted monster defense
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public int Defense(bool isPlayer = false)
    {
        if (isPlayer)
        {
            return BaseDefense + MainManager.Instance.DefenseMod;
        }
        else
        {
            return BaseDefense + MainManager.Instance.EnemyDefMod;
        }
    }

    /// <summary>
    /// Provides the adjusted monster speed
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public int Speed(bool isPlayer = false)
    {
        if (isPlayer)
        {
            return BaseSpeed + MainManager.Instance.SpeedMod;
        }
        else
        {
            return BaseSpeed + MainManager.Instance.EnemySpdMod;
        }
    }

    /// <summary>
    /// Assigns monster specific base stats to override the default 10's, must be implemented by each child class
    /// </summary>
    protected abstract void AssignBaseInfo();

    /// <summary>
    /// This applies the manually assigned modifications that the player has made to the stats.
    /// Then it calculates MaxHitPoints and the attackDelay
    /// </summary>
    private void ApplyUpgrades()
    {
        // Derive the max hitpoints from defense
        MaxHitPoints = Defense(isPlayer) * 10;
        // Assign this as the current hitpoints as well
        hitPoints = MaxHitPoints;
    }

    private void Update()
    {
        // Check that the level isn't over (win or lose)
        if (MainManager.Instance.GameActive)
        {
            // If we don't currently have a target, get the target
            if (target == null)
            {
                target = GetTarget();
            }
            // Previously used a Coroutine to manage attack time, but we need update to be able to visualise the turn timer
            // Check if enough time has passed since last attack
            if (timeSinceLastAttack >= AttackDelay)
            {
                // It has, so reset the last time we attacked
                timeSinceLastAttack = 0;
                // Trigger the attack
                Attack();
            }
            else
            {
                // Increment the time since last attacked as we haven't reached the delay yet
                timeSinceLastAttack += Time.deltaTime;
            }
            // Update how full the turn timer is
            turnTimer.UpdateFill(timeSinceLastAttack / AttackDelay);
        }
    }

    /// <summary>
    /// The Attack() function is required by every monster, however is also unique to every monster
    /// </summary>
    protected virtual void Attack()
    {
        target.CalculateDamageTaken(this);
    }

    /// <summary>
    /// Calculates the damage taken by this monster by comparing the other monsters strength with this monsters defense.
    /// Applies the damage taken after calculation.
    /// </summary>
    /// <param name="otherStrength">The strength of the target that initiated the attack</param>
    public void CalculateDamageTaken(Monster attacker)
    {
        // This acts a storage for how much critical damage to add, but also flags when critical damage is dealt
        float criticalDamage = 0;
        // If the random number is under the crit chance of 10 + (Speed * 0.1)
        if (Random.Range(0, 100) < (10 + (0.2 * attacker.Speed(attacker.isPlayer))))
        {
            // Critical damage is 20% of Speed + Strength
            criticalDamage = 0.2f * ((float)attacker.Speed(attacker.isPlayer) + (float)attacker.Strength(attacker.isPlayer));
        }
        // Calculate the damage by comparing the ratio of strength & damage
        int damage = Mathf.RoundToInt(baseDamage * ((((float)attacker.Strength(attacker.isPlayer) + criticalDamage) / (float)Defense(isPlayer)))); ;

        // We can't deal negative damage (that will heal)
        if (damage <= 0)
        {
            damage = 0;
        }
        // Subtract the damage dealt
        hitPoints -= damage;

        // Update the Health Bar fill
        healthBar.UpdateFill((float)hitPoints / (float)MaxHitPoints);

        // Update the combat log
        MainManager.Instance.BGController.CombatLog.UpdateCombatLog(isPlayer, attacker, this, damage, criticalDamage);

        // Check if the hitPoints are now below 0
        if (hitPoints <= 0)
        {
            HandleMonsterKnockout();
        }
    }

    /// <summary>
    /// This is a generic catchall code for when the monster reaches 0 hit points.
    /// If it's an enemy it'll trigger the next enemy, if it's the player it does Game Over
    /// </summary>
    private void HandleMonsterKnockout()
    {
        if (isPlayer)
        {
            MainManager.Instance.Victory = 0;
            MainManager.Instance.GameActive = false;
        } else
        {
            MainManager.Instance.Victory = 1;
            MainManager.Instance.GameActive = false;
        }
    }

    private Monster GetTarget()
    {
        // This is the string to search for to find the target container
        string targetName = "PlayerContainer";
        // If it's actually the player, then change this to enemy
        if (isPlayer)
        {
            targetName = "EnemyContainer";
        }
        // Find the target container
        GameObject targetContainer = GameObject.Find(targetName);

        if (targetContainer != null)
        {
            // There will only be a single target in this version of the game (might enhance later)
            Monster target = targetContainer.transform.GetChild(0).gameObject.GetComponent<Monster>();

            return target;
        } else
        {
            // If we got here we couldn't find containers
            return null;
        }
    }
}
