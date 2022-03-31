using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatLogger : MonoBehaviour
{
    public static CombatLogger Instance;

    [SerializeField] TextMeshProUGUI combatLog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCombatLog(bool isPlayer, Monster attacker, Monster defender, int damage)
    {
        // The second call for AddName inverts isPlayer as it's referring to the defender's player status
        string newLog = $"{AddName(isPlayer, attacker.name)} [{attacker.AttackName}] {AddName(!isPlayer, defender.name)} for <color=#FF0000>{damage}</color> damage!\n";

        combatLog.text += newLog;
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
            nameString += "#000099>Player ";
        } else
        {
            nameString += "#990000>Enemy ";
        }
        nameString += $"{name}</color>";

        return nameString;
    }
}
