using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainManager : MonoBehaviour
{
    // This is the main driver of the game
    public static MainManager Instance { get; private set; }

    // This will allow the MainManager to provide access to the BattlegroundController
    [SerializeField] private BattlegroundController bgController;
    public BattlegroundController BGController
    {
        get { return bgController; }
    }

    // The array of monsters in the carousel along with the active index
    public Monster[] monsters = new Monster[5];

    // Current Round
    public int CurrentRound { get; set; }

    private bool gameActive = false;
    public bool GameActive
    {
        get { return gameActive; }
        set { gameActive = value; }
    }
    private int victory = -1;
    public int Victory
    {
        get { return victory; }
        set 
        { 
            victory = value; 
            
            // Check that we have a battlegroundController & hat Victory is not -1
            if (value != -1 && bgController != null)
            {
                // Trigger the results screen
                bgController.ShowResultScreen();
            }
        }
    }

    // Player attributes
    public string PlayerName { get; set; }
    public int SelectedMonster { get; set; }
    public int StrengthMod { get; set; }
    public int DefenseMod { get; set; }
    public int SpeedMod { get; set; }
    public int MonsterLevel { get; set; }
    public int MonsterExp { get; set; }
    public int UnspentPoints { get; set; }

    public List<HighScores> Scores = new List<HighScores>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            // We've just loaded up the game let's load the high scores
            LoadHighScores();
        } else
        {
            Destroy(gameObject);
        }
    }

    // This allows us to attach methods for when Scenes change
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're in the Battlegound Controller, make a reference to it
        if (scene.buildIndex == 1)
        {
            bgController = GameObject.Find("BattlegroundController").GetComponent<BattlegroundController>() as BattlegroundController;
        }
    }

    public void UpdateHighScores()
    {
        // Create a new high score
        HighScores newScore = new HighScores();
        newScore.round = CurrentRound;
        newScore.playerName = PlayerName;
        newScore.monsterID = SelectedMonster;
        newScore.monsterLevel = MonsterLevel;
        // The score will only track the raw data to reduce amount of math when rendering them
        newScore.strength = monsters[SelectedMonster].Strength;
        newScore.defense = monsters[SelectedMonster].Defense;
        newScore.speed = monsters[SelectedMonster].Speed;

        if (Scores.Count == 0)
        {
            // Our first score! Let's add it in
            Scores.Add(newScore);
        } else
        {
            // Iterate over all of the high scores to check if we beat them
            for (int i = 0; i < Scores.Count; i++)
            {
                if (CurrentRound >= Scores[i].round)
                {
                    // We beat it! So lets store this one at the index
                    Scores.Insert(i, newScore);
                }
            }
        }

        // Now drop all scores above the top 5!
        if (Scores.Count > 5)
        {
            Scores.RemoveRange(5, Scores.Count - 5);
        }

        // Now save them
        SaveHighScores();
    }

    /// <summary>
    /// HighScores is the data structure used to store the previous winning score
    /// </summary>
    [System.Serializable]
    public class HighScores
    {
        public int round;
        public string playerName;
        public int monsterID;
        public int monsterLevel;
        public int strength;
        public int defense;
        public int speed;
    }

    /// <summary>
    /// The ScoreCollection type is a wrapper for an array of HighScore data types
    /// </summary>
    [System.Serializable]
    public class ScoreCollection
    {
        public HighScores[] scores;
    }
    /// <summary>
    /// This function saves the highscores in JSON
    /// </summary>
    public void SaveHighScores()
    {
        // Create the array wrapper object
        ScoreCollection scoreList = new ScoreCollection();
        // Store the list of High Scores
        scoreList.scores = Scores.ToArray();
        // JSONify it
        string json = JsonUtility.ToJson(scoreList);
        // Store it
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    /// <summary>
    /// This function loads the previous high scores from JSON
    /// </summary>
    public void LoadHighScores()
    {
        // Store the path as we use it at least twice in this function
        string path = Application.persistentDataPath + "/highscores.json";
        // If the file exists let's load the scores
        if (File.Exists(path))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(path);
            // Parse the string into the Wrapper Object of ScoreCollection
            ScoreCollection scoreList = JsonUtility.FromJson<ScoreCollection>(json);
            // Iterate over each score and add to the High Scores List
            foreach (HighScores score in scoreList.scores)
            {
                Scores.Add(score);
            }
        }
    }
}
