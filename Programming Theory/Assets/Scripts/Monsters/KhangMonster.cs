using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhangMonster : Monster
{
    protected override void Attack(GameObject target)
    {
        Debug.Log("Khang [BITES] " + target.name);
    }
}
