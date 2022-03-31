using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    // The array of monsters in the carousel along with the active index
    public Monster[] monsters = new Monster[2];

    public bool GameOver { get; set; }

    public string PlayerName { get; set; }
    public int SelectedMonster { get; set; }
    public int StrengthMod { get; set; }
    public int DefenseMod { get; set; }
    public int SpeedMod { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            GameOver = true;
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }
    }
}
