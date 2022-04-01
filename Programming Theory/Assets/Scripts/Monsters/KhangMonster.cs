using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    public override int BaseStrength
    {
        get { return 12; }
    }
    public override int BaseDefense
    {
        get { return 12; }
    }
    public override int BaseSpeed
    {
        get { return 10; }
    }
    public override float AttackDelay
    {
        get { return (float)baseAttackDelay / (float)Speed(isPlayer); }
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
