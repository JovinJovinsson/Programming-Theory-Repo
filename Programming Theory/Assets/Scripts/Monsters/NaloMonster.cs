using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaloMonster : Monster
{
    protected override void Awake()
    {
        AttackName = "stings";
        Speed = 15;
        base.Awake();
    }
}
