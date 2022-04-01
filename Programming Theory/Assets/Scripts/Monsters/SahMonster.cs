using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahMonster : Monster
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
        AttackName = "strikes";
    }
}
