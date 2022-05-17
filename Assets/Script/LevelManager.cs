using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI missionText;

    [Header("Level 1 Settings")]
    public GameObject[] enemies;
    public Door lvl1Door;
    [HideInInspector] public int level1DeadEnemyCount = 0;
    bool level1Handled = false;
    public string level1Instruction = "Eliminate All Enemies";

    [Header("Level 2 Settings")]
    public string level2Instruction = "Find the Key";

    [Header("Level 3 Settings")]
    public GameObject[] lvl3enemies;
    public Door lvl3Door;
    [HideInInspector] public int level3DeadEnemyCount = 0;
    bool level3Handled = false;

    private void Start()
    {
        missionText.text = level1Instruction;
    }

    private void Update()
    {
        if (!level1Handled && level1DeadEnemyCount >= enemies.Length)
        {
            level1Handled = true;
            lvl1Door.keyFound = true;
            lvl1Door.Interact();
            missionText.text = level2Instruction;
        }
        if (!level3Handled && level3DeadEnemyCount >= lvl3enemies.Length)
        {
            level3Handled = true;
            lvl3Door.keyFound = true;
            lvl3Door.Interact();
        }
    }
}
