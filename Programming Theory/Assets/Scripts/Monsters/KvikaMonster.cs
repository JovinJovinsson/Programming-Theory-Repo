using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KvikaMonster : Monster
{
    public override int BaseStrength
    {
        get { return 10; }
    }
    public override int BaseDefense
    {
        get { return 14; }
    }
    public override int BaseSpeed
    {
        get { return 9; }
    }
    public override float AttackDelay
    {
        get { return (float)baseAttackDelay / (float)Speed(isPlayer); }
    }
    protected override void AssignBaseInfo()
    {
        AttackName = "smashes";
    }
}
