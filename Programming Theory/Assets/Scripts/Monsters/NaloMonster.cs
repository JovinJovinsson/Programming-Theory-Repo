using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE - All Monsters
public class NaloMonster : Monster
{
    // POLYMORPHISM - All Monsters
    public override int BaseStrength
    {
        get { return 9; }
    }
    public override int BaseDefense
    {
        get { return 10; }
    }
    public override int BaseSpeed
    {
        get { return 14; }
    }
    public override float AttackDelay
    {
        get { return (float)baseAttackDelay / (float)Speed(isPlayer); }
    }

    protected override void AssignBaseInfo()
    {
        AttackName = "stings";
    }
}
