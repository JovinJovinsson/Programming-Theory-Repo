using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisqueuxMonster : Monster
{
    public override int BaseStrength
    {
        get { return 10; }
    }
    public override int BaseDefense
    {
        get { return 11; }
    }
    public override int BaseSpeed
    {
        get { return 13; }
    }
    public override float AttackDelay
    {
        get { return (float)baseAttackDelay / (float)Speed(isPlayer); }
    }
    protected override void AssignBaseInfo()
    {
        AttackName = "thumps";
    }
}
