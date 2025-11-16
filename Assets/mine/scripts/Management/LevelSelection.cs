using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [Header("Level Buttons (each corresponds to a level)")]
    public Button[] levelButtons;

    [Header("Lock Images for each Level")]
    public GameObject[] levelLocks;

    [Header("Required score to unlock each level")]
    public int[] requiredScores;

    private int highScore;

    private void Start()
    {
        // If no saved high score yet, default to 0
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        UpdateLockStatus();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetProgress();
        }
    }
    private void UpdateLockStatus()
    {
        int count = Mathf.Min(levelButtons.Length, levelLocks.Length, requiredScores.Length);

        for (int i = 0; i < count; i++)
        {
            bool isUnlocked = highScore >= requiredScores[i];

            // Show lock if not unlocked
            levelLocks[i].SetActive(!isUnlocked);

            // Disable the button if still locked
            levelButtons[i].interactable = isUnlocked;
        }

        // Save how many levels are currently unlocked
        int unlockedLevels = 0;
        for (int i = 0; i < count; i++)
            if (highScore >= requiredScores[i])
                unlockedLevels++;

        PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        PlayerPrefs.Save();
    }

    // 🧹 Call this when pressing a "Reset Progress" button or key
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("unlockedLevels");
        PlayerPrefs.Save();

        highScore = 0;
        Debug.Log("✅ Progress reset: HighScore and unlockedLevels cleared!");

        // Refresh locks visually and functionally
        UpdateLockStatus();
    }
}
