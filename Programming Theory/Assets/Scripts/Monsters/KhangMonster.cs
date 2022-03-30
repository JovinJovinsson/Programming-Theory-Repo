using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    protected override void Attack(GameObject target)
    {
        // This timer ensure it continues it's idle animation when done attacking
        StartCoroutine(IdleAnimation());
        // Play the attack animation
        gameObject.GetComponent<Animation>().Play("KhangAttack");
        Debug.Log("Khang [WING BUFFETS] " + target.name);
    }

    private IEnumerator IdleAnimation()
    {
        yield return new WaitForSeconds(1);

        gameObject.GetComponent<Animation>().Play("Idle");
    }
}
