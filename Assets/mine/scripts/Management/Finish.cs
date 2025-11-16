////using System.Collections;
////using UnityEngine;

////public class Finish : MonoBehaviour
////{
////    public int scoreThreshold = 100; // Minimum score to complete level
////    public GameObject levelCompletePanel;
////    public GameObject scoreNotEnoughPanel;

////    private GameManager gameManager;

////    private void Start()
////    {
////        gameManager = GameManager.instance;

////        if (levelCompletePanel != null)
////            levelCompletePanel.SetActive(false);

////        if (scoreNotEnoughPanel != null)
////            scoreNotEnoughPanel.SetActive(false);
////    }

////    private void OnTriggerEnter2D(Collider2D collision)
////    {
////        if (!collision.CompareTag("Player")) return;

////        if (gameManager == null)
////        {
////            Debug.LogError("❌ GameManager instance not found!");
////            return;
////        }

////        int currentScore = gameManager.GetScore();

////        if (currentScore >= scoreThreshold)
////        {
////            Debug.Log("✅ Level Completed! Score: " + currentScore);
////            ShowLevelComplete();
////        }
////        else
////        {
////            Debug.Log("❌ Score not enough! Required: " + scoreThreshold + ", Got: " + currentScore);
////            ShowScoreNotEnough();
////        }
////    }

////    private void ShowLevelComplete()
////    {
////        if (levelCompletePanel != null)
////            levelCompletePanel.SetActive(true);

////        // Reset player, camera, and platforms
////        LevelManager levelManager = FindObjectOfType<LevelManager>();
////        if (levelManager != null)
////        {
////            levelManager.ResetLevel();
////            StartCoroutine(LoadNextAfterDelay(levelManager, 1.5f)); // optional delay before next level
////        }
////    }

////    private IEnumerator LoadNextAfterDelay(LevelManager levelManager, float delay)
////    {
////        yield return new WaitForSeconds(delay);
////        levelManager.LoadNextLevel();
////    }

////    private void ShowScoreNotEnough()
////    {
////        if (scoreNotEnoughPanel != null)
////            scoreNotEnoughPanel.SetActive(true);
////    }
////}


//using UnityEngine;

//public class Finish : MonoBehaviour
//{
//    [Header("Score Requirement")]
//    public int scoreThreshold = 100; // Minimum score needed to complete the level

//    [Header("UI Panels")]
//    public GameObject levelCompletePanel;
//    public GameObject scoreNotEnoughPanel;

//    private GameManager gameManager;

//    private void Start()
//    {
//        gameManager = GameManager.instance;

//        if (levelCompletePanel != null)
//            levelCompletePanel.SetActive(false);

//        if (scoreNotEnoughPanel != null)
//            scoreNotEnoughPanel.SetActive(false);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        // Check if the player touches the finish area
//        if (!collision.CompareTag("Player"))
//            return;

//        if (gameManager == null)
//        {
//            Debug.LogError("❌ GameManager instance not found!");
//            return;
//        }

//        int currentScore = gameManager.GetScore();

//        if (currentScore >= scoreThreshold)
//        {
//            Debug.Log("✅ Level Completed! Score: " + currentScore);
//            ShowLevelComplete();
//        }
//        else
//        {
//            Debug.Log("❌ Score not enough! Required: " + scoreThreshold + ", Got: " + currentScore);
//            ShowScoreNotEnough();
//        }
//    }

//    private void ShowLevelComplete()
//    {
//        if (levelCompletePanel != null)
//            levelCompletePanel.SetActive(true);

//        //Time.timeScale = 0f; // Pause the game
//    }

//    private void ShowScoreNotEnough()
//    {
//        if (scoreNotEnoughPanel != null)
//            scoreNotEnoughPanel.SetActive(true);

//        //Time.timeScale = 0f; // Pause the game
//    }
//}


using System.Collections;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("Score Requirement")]
    public int scoreThreshold = 100; // Minimum score needed to complete the level

    [Header("UI Panels")]
    public GameObject levelCompletePanel;
    public GameObject scoreNotEnoughPanel;

    private GameManager gameManager;
    private bool hasTriggered = false; // Prevent multiple triggers

    private void Start()
    {
        gameManager = GameManager.instance;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (scoreNotEnoughPanel != null)
            scoreNotEnoughPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only react if the player touches the finish area
        if (!collision.CompareTag("Player"))
            return;

        // Prevent multiple triggers
        if (hasTriggered)
            return;

        if (gameManager == null)
        {
            Debug.LogError("❌ GameManager instance not found!");
            return;
        }

        int currentScore = gameManager.GetScore();

        if (currentScore >= scoreThreshold)
        {
            Debug.Log("✅ Level Completed! Score: " + currentScore);
            hasTriggered = true;
            ShowLevelComplete();
        }
        else
        {
            Debug.Log("❌ Score not enough! Required: " + scoreThreshold + ", Got: " + currentScore);
            ShowScoreNotEnough();
        }
    }

    private void ShowLevelComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        // Disable player movement
        playerMovement player = FindObjectOfType<playerMovement>();
        if (player != null)
            player.canMove = false;

        // Load next level after delay
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            StartCoroutine(LoadNextAfterDelay(levelManager, 2f));
        }
        else
        {
            // If no LevelManager, just load next scene directly
            StartCoroutine(LoadNextLevelDirectly(2f));
        }
    }

    private IEnumerator LoadNextAfterDelay(LevelManager levelManager, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        levelManager.LoadNextLevel();
    }

    private IEnumerator LoadNextLevelDirectly(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to main menu.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void ShowScoreNotEnough()
    {
        if (scoreNotEnoughPanel != null)
        {
            scoreNotEnoughPanel.SetActive(true);
            // Auto-hide after a few seconds
            StartCoroutine(HideScoreNotEnoughPanel(2f));
        }
    }

    private IEnumerator HideScoreNotEnoughPanel(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (scoreNotEnoughPanel != null)
            scoreNotEnoughPanel.SetActive(false);
    }
}
