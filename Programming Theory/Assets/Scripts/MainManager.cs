using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [SerializeField] private BattlegroundController bgController;
    public BattlegroundController BGController
    {
        get { return bgController; }
    }

    // The array of monsters in the carousel along with the active index
    public Monster[] monsters = new Monster[2];

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
}
