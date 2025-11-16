
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public UIManager manager;
    [HideInInspector] public playerMovement player;
    [HideInInspector] public cameraMovement cameraFollow;

    public int totalScore = 0;
    public int lives = 3;
    private int highScore;

   
    // Track if we're starting fresh or continuing
    private bool isNewGame = true;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        else
        {
            //Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
       
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log($"Scene Loaded: {scene.name}");

    // Always find the UI, Player, and Camera in the new scene
    manager = FindObjectOfType<UIManager>();
    player = FindObjectOfType<playerMovement>();
    cameraFollow = FindObjectOfType<cameraMovement>();

    // If any of these are missing, log a warning
    if (cameraFollow == null)
        Debug.LogWarning("No cameraMovement found in the scene!");
    if (player == null)
        Debug.LogWarning("No playerMovement found in the scene!");
    if (manager == null)
        Debug.LogWarning("No UIManager found in the scene!");

    // Reset stats only on main menu
    if (scene.buildIndex == 0)
    {
        ResetGame();
        isNewGame = true;
    }
    else
    {
        if (isNewGame)
        {
            totalScore = 0;
            lives = 3;
            isNewGame = false;
        }

        // Update UI
        manager?.UpdateScore(totalScore);
        manager?.UpdateLives(lives);
        //manager?.UpdateLevel(scene.buildIndex);

        // Reset player and camera
        player?.ResetForNewLevel();
        cameraFollow?.ResetCamera();
    }
}

    private void Update()
    {
        if (lives <= 0)
        {
            manager?.ShowGameOver();
        }
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        Debug.Log("Score: " + totalScore);
        manager?.UpdateScore(totalScore);

        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log("🏆 New High Score: " + highScore);
        }
    }

    public int GetScore() => totalScore;
    public int GetHighScore() => highScore;

    // Call this when starting a brand new game from menu
    public void StartNewGame()
    {
        isNewGame = true;
        totalScore = 0;
        lives = 3;
    }

    // Call this when returning to main menu
    public void ResetGame()
    {
        totalScore = 0;
        lives = 3;
        isNewGame = true;
    }

    // Lose a life
    public void LoseLife()
    {
        lives--;
        manager?.UpdateLives(lives);
    }

    // Add a life (for pickups)
    public void AddLife()
    {
        lives++;
        manager?.UpdateLives(lives);
    }
}