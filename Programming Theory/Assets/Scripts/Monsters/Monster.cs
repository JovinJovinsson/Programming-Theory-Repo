using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    // Base Monster Stats
    private int strength = 10;
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    private int defense = 10;
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    private int speed = 10;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    private int hitPoints = 100;

    // The baseAttackDelay is alwyas 50 for every monster as it is divided by speed to determine attack rate
    private const int baseAttackDelay = 50;
    // The baseDamage is 5 and is modified based on the difference of strength & damage for attacks
    private const int baseDamage = 10;
    // This is a boolean that lets us know if it is the player or not
    public bool isPlayer = false;
    // Storage for the target for attack
    private GameObject target;

    protected virtual void Awake()
    {
        target = GetTarget();
        StartCoroutine(TurnTimer());
    }

    /// <summary>
    /// Waits for the calculated time based on the attack delay & speed of the monster.
    /// Then triggers an attack and the next wait for attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator TurnTimer()
    {
        yield return new WaitForSeconds(baseAttackDelay / Speed);

        Attack(target);
        StartCoroutine(TurnTimer());
    }

    /// <summary>
    /// The Attack() function is required by every monster, however is also unique to every monster
    /// </summary>
    protected abstract void Attack(GameObject target);

    /// <summary>
    /// Calculates the damage taken by this monster by comparing the other monsters strength with this monsters defense.
    /// Applies the damage taken after calculation.
    /// </summary>
    /// <param name="otherStrength">The strength of the target that initiated the attack</param>
    public void CalculateDamageTaken(int otherStrength)
    {
        // Calculate the damage by comparing the ratio of strength & damage
        int damage = Mathf.RoundToInt(baseDamage * (otherStrength / Defense));

        // We can't deal negative damage (that will heal)
        if (damage <= 0)
        {
            damage = 0;
        }
        // Subtract the damage dealt
        hitPoints -= damage;

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
            Debug.Log("Game Over!");
        } else
        {
            Debug.Log("Defeated Level!");
        }
    }

    private GameObject GetTarget()
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
        // There will only be a single target in this version of the game (might enhance later)
        GameObject target = targetContainer.transform.GetChild(0).gameObject;

        return target;
    }
}
