using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    protected override void AssignBaseInfo()
    {
        AttackName = "wing buffets";
        Strength = 12;
        Defense = 12;
        Speed = 10;
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
