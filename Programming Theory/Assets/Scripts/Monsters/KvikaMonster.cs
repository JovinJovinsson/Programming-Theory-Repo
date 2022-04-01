using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KvikaMonster : Monster
{
    public override int Strength
    {
        get { return 10 + MainManager.Instance.StrengthMod; }
    }
    public override int Defense
    {
        get { return 14 + MainManager.Instance.DefenseMod; }
    }
    public override int Speed
    {
        get { return 9 + MainManager.Instance.SpeedMod; }
    }
    protected override void AssignBaseInfo()
    {
        AttackName = "smashes";
    }
}
