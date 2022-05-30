using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI missionText;

    [Header("Level 1 Settings")]
    public string level1Instruction = "Eliminate All Enemies";
    public GameObject[] enemies;
    public Door lvl1Door;
    [HideInInspector] public int level1DeadEnemyCount = 0;
    bool level1Handled = false;

    [Header("Level 2 Settings")]
    public string level2Instruction = "Find the Key";

    [Header("Level 3 Settings")]
    public string level3Instruction = "Eliminate All Enemies";
    public GameObject[] lvl3enemies;
    public Door lvl3Door;
    [HideInInspector] public int level3DeadEnemyCount = 0;
    bool level3Handled = false;

    [Header("Level 4 Settings")]
    public string level4Instruction = "Eliminate All Enemies";
    public GameObject[] lvl4enemies;
    public Door lvl4Door;
    [HideInInspector] public int level4DeadEnemyCount = 0;
    bool level4Handled = false;

    [Header("Level 5 Settings")]
    public string level5Instruction = "Eliminate The Mini Boss";
    public GameObject[] lvl5enemies;
    public Door lvl5Door;
    [HideInInspector] public int level5DeadEnemyCount = 0;
    bool level5Handled = false;

    [Header("Level 6 Settings")]
    public string level6Instruction = "Eliminate The Boss";
    public GameObject[] lvl6enemies;
    [HideInInspector] public int level6DeadEnemyCount = 0;
    bool level6Handled = false;
    public GameObject youWonPanel;

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
            missionText.text = level4Instruction;
        }
        if (!level4Handled && level4DeadEnemyCount >= lvl4enemies.Length)
        {
            level4Handled = true;
            lvl4Door.keyFound = true;
            lvl4Door.Interact();
            missionText.text = level5Instruction;
        }
        if (!level5Handled && level5DeadEnemyCount >= lvl5enemies.Length)
        {
            level5Handled = true;
            lvl5Door.keyFound = true;
            lvl5Door.Interact();
            missionText.text = level6Instruction;
        }
        if (!level6Handled && level6DeadEnemyCount >= lvl6enemies.Length)
        {
            level6Handled = true;
            StartCoroutine(WinRoutine());
        }
    }

    IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(1f);
        youWonPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
