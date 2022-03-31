using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatLogger : MonoBehaviour
{
    public static CombatLogger Instance;

    [SerializeField] TextMeshProUGUI combatLog;

    private void Awake()
    {
        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }*/
    }

    public void UpdateCombatLog(bool isPlayer, Monster attacker, Monster defender, int damage, float criticalDamage)
    {
        string timeStamp = $"[{DateTime.Now:HH:mm:ss}]";
        // The second call for AddName inverts isPlayer as it's referring to the defender's player status
        string newLog = $"{timeStamp} {AddName(!isPlayer, attacker.name)} [{attacker.AttackName}] " +
            $"{AddName(isPlayer, defender.name)} for <color=#FF0000>{damage}</color>";
        if (criticalDamage != 0)
        {
            newLog += " <color=#FFFF00>CRITICAL</color>";
        }
        newLog += " damage!\n";

        // Combat log will have most recent entries at the top
        combatLog.text = newLog + combatLog.text;
    }

    /// <summary>
    /// Produce a coloured string for the name of the attacker based on whether it's the player or the enemy.
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private string AddName(bool isPlayer, string name)
    {
        string nameString = "<color=";
        if (isPlayer)
        {
            nameString += "#000099>" + MainManager.Instance.PlayerName + "'s ";
        } else
        {
            nameString += "#990000>Enemy ";
        }
        nameString += $"{name}</color>";

        return nameString;
    }
}
