using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaloMonster : Monster
{
    protected override void Awake()
    {
        Speed = 15;
        base.Awake();
    }

    protected override void Attack(GameObject target)
    {
        Debug.Log("Nalo [STINGS] " + target.name);
    }
}
