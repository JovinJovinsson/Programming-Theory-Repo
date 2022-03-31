using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIController : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;

    /// <summary>
    /// Sets the players name & monsters level before loading the battlegound
    /// TODO: Load to level up screen first
    /// </summary>
    public void StartGame()
    {
        if (playerName.text.Length >= 1)
        {
            MainManager.Instance.PlayerName = playerName.text;
            
        } else
        {
            MainManager.Instance.PlayerName = "Emanon";
        }
        MainManager.Instance.MonsterLevel = 1;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads the High Scores screen
    /// </summary>
    public void ViewHighScores()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
