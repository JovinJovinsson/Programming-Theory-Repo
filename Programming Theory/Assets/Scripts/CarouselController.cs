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
    // Pre-defined transform to make sure the monsters are in the right spot
    [SerializeField] private GameObject monsterTransform;

    private Monster monsterView;

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

    // This sets the speed for the rotation of the monster
    private const float rotateSpeed = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the active monster to a random one
        ActiveMonster = Random.Range(0, MainManager.Instance.monsters.Length);
    }

    private void Update()
    {
        monsterView.transform.Rotate(Vector3.up * rotateSpeed);
    }

    /// <summary>
    /// This method changes the currently selected monster and triggers other UI elements to update
    /// </summary>
    private void ChangeActiveMonster()
    {
        /// TODO: CHANGE TO INSTANTIATE & DESTROY
        // Remove the previous Monster (if this isn't the first one we've spawned for the view)
        if (monsterView != null)
        {
            Destroy(monsterView.gameObject);
        }

        // Get the monster from the array and position it correctly
        monsterView = Instantiate(MainManager.Instance.monsters[activeMonster], monsterTransform.transform.position, monsterTransform.transform.rotation);
        // Scale it accordingly
        monsterView.transform.localScale = monsterTransform.transform.localScale;
        // Make it a child of the carousel
        monsterView.transform.parent = gameObject.transform;
        // Remove the progress bars
        foreach (ProgressBar bar in monsterView.GetComponentsInChildren<ProgressBar>())
        {
            bar.gameObject.SetActive(false);
        }

        // Set the name as well
        monsterName.text = monsterView.name.Replace("(Clone)","");

        // Update the stat block on the right
        UpdateMonsterStats();
    }

    private void UpdateMonsterStats()
    {
        strengthText.text = $"{monsterView.BaseStrength}";
        defenseText.text = $"{monsterView.BaseDefense}";
        speedText.text = $"{monsterView.BaseSpeed}";
        hitPointsText.text = $"{monsterView.MaxHitPoints}";
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
