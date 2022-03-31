using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaloMonster : Monster
{
    protected override void AssignBaseInfo()
    {
        AttackName = "stings";
        Strength = 9;
        Defense = 10;
        Speed = 14;
    }
}
