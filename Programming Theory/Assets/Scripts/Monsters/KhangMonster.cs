using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    protected override void Awake()
    {
        AttackName = "wing buffets";
        maxHitPoints = 150;
        base.Awake();
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
