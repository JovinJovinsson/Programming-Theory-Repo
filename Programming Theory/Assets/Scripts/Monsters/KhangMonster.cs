using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    public override int Strength
    {
        get { return 12 + MainManager.Instance.StrengthMod; }
    }
    public override int Defense
    {
        get { return 12 + MainManager.Instance.DefenseMod; }
    }
    public override int Speed
    {
        get { return 10 + MainManager.Instance.SpeedMod; }
    }
    protected override void AssignBaseInfo()
    {
        AttackName = "wing buffets";
    }

    protected override void Attack()
    {
        // This timer ensure it continues it's idle animation when done attacking
        StartCoroutine(IdleAnimation());
        // Play the attack animation
        gameObject.GetComponent<Animation>().Play("KhangAttack");
        base.Attack();
    }

    private IEnumerator IdleAnimation()
    {
        yield return new WaitForSeconds(1);

        gameObject.GetComponent<Animation>().Play("Idle");
    }
}
