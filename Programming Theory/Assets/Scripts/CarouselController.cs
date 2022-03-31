using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarouselController : MonoBehaviour
{
    // Monster name so we can display correctly
    [SerializeField] private TextMeshProUGUI monsterName;
    // Monster stats to update with the carousel
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI hitPointsText;

    
    private int activeMonster;
    public int ActiveMonster
    {
        get { return activeMonster; }
        set
        {
            activeMonster = value;
            MainManager.Instance.SelectedMonster = value;
            // Automatically update the carousel information
            ChangeActiveMonster();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /// TODO: Remove this
        // Get all of the Monsters in the Carousel
        //MainManager.Instance.monsters = gameObject.GetComponentsInChildren<Monster>();
        // Set the active monster to a random one
        ActiveMonster = Random.Range(0, MainManager.Instance.monsters.Length);
    }

    /// <summary>
    /// This method changes the currently selected monster and triggers other UI elements to update
    /// </summary>
    private void ChangeActiveMonster()
    {
        /// TODO: CHANGE TO INSTANTIATE & DESTROY
        // Iterate over each Monster in the Carousel
        for (int i = 0; i < MainManager.Instance.monsters.Length; i++)
        {
            // If we're at the monster that should be visible, set it so
            if (i == activeMonster)
            {
                MainManager.Instance.monsters[i].gameObject.SetActive(true);
            } else
            {
                // Otherwise hide the monster
                MainManager.Instance.monsters[i].gameObject.SetActive(false);
            }
        }
        // Set the name as well
        monsterName.text = MainManager.Instance.monsters[activeMonster].name;

        UpdateMonsterStats();
    }

    private void UpdateMonsterStats()
    {
        strengthText.text = $"{MainManager.Instance.monsters[activeMonster].Strength}";
        defenseText.text = $"{MainManager.Instance.monsters[activeMonster].Defense}";
        speedText.text = $"{MainManager.Instance.monsters[activeMonster].Speed}";
        hitPointsText.text = $"{MainManager.Instance.monsters[activeMonster].MaxHitPoints}";
    }

    /// <summary>
    /// Decreases the Active Monster ID and wraps around to the end of array when ID = 0
    /// </summary>
    public void PreviousMonster()
    {
        // If we're at 0, wrap around to the last monster
        if (ActiveMonster == 0)
        {
            ActiveMonster = MainManager.Instance.monsters.Length - 1;
        } else
        {
            // Otherwise just decrease the ID
            ActiveMonster--;
        }
    }

    /// <summary>
    /// Increases the Active Monster ID and wraps around to the start of array when ID = monsters.Length
    /// </summary>
    public void NextMonster()
    {
        // If we're at the last monster, wrap around to the first monster
        if (ActiveMonster == MainManager.Instance.monsters.Length -1)
        {
            ActiveMonster = 0;
        }
        else
        {
            // Otherwise just increase the ID
            ActiveMonster++;
        }
    }
}
