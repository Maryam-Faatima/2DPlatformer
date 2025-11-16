//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LevelManager : MonoBehaviour
//{
//    public playerMovement player; // Reference to player
//    public cameraMovement cameraFollow; // Reference to main camera
//    private MovingPlatform[] platforms; // All moving platforms in the scene
//    private GameManager gameManager;

//    private void Awake()
//    {
//        platforms = FindObjectsOfType<MovingPlatform>(); // Find all platforms in the scene
//    }

//    // Reset everything for a new level
//    public void ResetLevel()
//    {

//        // 1️⃣ Reset player
//        if (player != null)
//            player.ResetForNewLevel();

//        // 2️⃣ Reset camera
//        if (cameraFollow != null)
//            cameraFollow.ResetCamera();

//        // 3️⃣ Reset all platforms
//        foreach (var platform in platforms)
//            platform.ResetPlatform();

//        // 4️⃣ Reset game stats
//        UIManager uiManager = FindObjectOfType<UIManager>();
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (uiManager != null && gameManager != null)
//        {
//            gameManager.lives = 3; // reset lives
//            gameManager.totalScore = 0; // reset score
//            uiManager.UpdateLives(gameManager.lives);
//            uiManager.UpdateScore(gameManager.totalScore);
//        }

//        // 5️⃣ Resume time (in case it was paused)
//        Time.timeScale = 1f;
//        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    // Load next scene
//    public void LoadNextLevel()
//    {
//        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
//        SceneManager.LoadScene(nextSceneIndex);
//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // These will be found automatically via GameManager
    private playerMovement player;
    private cameraMovement cameraFollow;
    private MovingPlatform[] platforms;

    private void Start()
    {
        platforms = FindObjectsOfType<MovingPlatform>();

        if (GameManager.instance != null)
        {
            if (GameManager.instance.player == null)
                GameManager.instance.player = FindObjectOfType<playerMovement>();

            if (GameManager.instance.cameraFollow == null)
                GameManager.instance.cameraFollow = FindObjectOfType<cameraMovement>();

            player = GameManager.instance.player;
            cameraFollow = GameManager.instance.cameraFollow;
        }
    }


    // Load next level
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if next level exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to main menu.");
            SceneManager.LoadScene(0);
        }
    }

    // Restart current level
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Reset platforms in current level (if you need to reset without reloading scene)
    public void ResetPlatforms()
    {
        foreach (var platform in platforms)
        {
            if (platform != null)
                platform.ResetPlatform();
        }
    }
}