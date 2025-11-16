
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class UIManager : MonoBehaviour
//{
//    [Header("Panels")]
//    public GameObject pausePanel;
//    public GameObject gameOverPanel;

//    [Header("Text")]
//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI livesText;
//    public TextMeshProUGUI levelText;

//    [Header("Player Reference")]
//    public playerMovement player;
//    public playerAttack attack;
//    public FlyKickScript flyKick;
//    public CrouchScript crouch;

//    private bool isPaused = false;

//    void Start()
//    {
//        if (pausePanel != null)
//            pausePanel.SetActive(false);

//        if (gameOverPanel != null)
//            gameOverPanel.SetActive(false);

//        Time.timeScale = 1f; // Ensure game starts unpaused
//    }

//    void Update()
//    {
//        // ✅ Keyboard pause toggle
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            if (!isPaused)
//                PauseGame();
//            else
//                ResumeGame();
//        }
//    }

//    // ------------------- Pause Controls -------------------
//    public void PauseGame()
//    {
//        if (pausePanel == null) return;

//        pausePanel.SetActive(true);
//        Time.timeScale = 0f;
//        isPaused = true;

//        // Disable player controls
//        if (player != null) player.enabled = false;
//        if (attack != null) attack.enabled = false;
//        if (flyKick != null) flyKick.enabled = false;
//        if (crouch != null) crouch.enabled = false;
//    }

//    public void ResumeGame()
//    {
//        if (pausePanel == null) return;

//        pausePanel.SetActive(false);
//        Time.timeScale = 1f;
//        isPaused = false;

//        // Re-enable player controls
//        if (player != null) player.enabled = true;
//        if (attack != null) attack.enabled = true;
//        if (flyKick != null) flyKick.enabled = true;
//        if (crouch != null) crouch.enabled = true;
//    }

//    // ------------------- Game Over -------------------
//    public void ShowGameOver()
//    {
//        if (gameOverPanel == null) return;
//        gameOverPanel.SetActive(true);
//        Time.timeScale = 0f;
//    }

//    // ------------------- Restart & Main Menu -------------------
//    //public void OnRestartButton()
//    //{
//    //    // Resume time first (so game restarts properly)
//    //    Time.timeScale = 1f;
//    //    Scene currentScene = SceneManager.GetActiveScene();
//    //    SceneManager.LoadScene(currentScene.buildIndex);
//    //}

//    public void OnRestartButton()
//    {
//        gameOverPanel.SetActive(false);
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


//        //LevelManager levelManager = FindObjectOfType<LevelManager>();
//        //if (levelManager != null)
//        //    levelManager.ResetLevel();
//    }



//    public void OnMainMenuButton()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(0); // assuming 0 = Main Menu scene
//    }

//    public void switchScene(int sceneNo)
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(sceneNo);
//    }

//    // ------------------- UI Updates -------------------
//    public void UpdateScore(int score)
//    {
//        if (scoreText != null)
//            scoreText.text = "Score: " + score;
//    }

//    public void UpdateLives(int lives)
//    {
//        if (livesText != null)
//            livesText.text = "Lives: " + lives;
//    }

//    public void UpdateLevel(int level)
//    {
//        if (levelText != null)
//            levelText.text = "Level: " + level;
//    }

//    // ------------------- Mobile Button Handlers -------------------
//    public void OnJumpButton() => player?.Jump();
//    public void OnAttackButton() => attack?.PerformAttack();
//    public void OnFlyKickButton() => flyKick?.TryFlyKick();
//    public void OnCrouchButton() => crouch?.TryToggleCrouch();
//    public void OnPauseButton() => PauseGame();
//    public void OnResumeButton() => ResumeGame();
//}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;

    [Header("Player Reference")]
    public playerMovement player;
    public playerAttack attack;
    public FlyKickScript flyKick;
    public CrouchScript crouch;

    private bool isPaused = false;
    private CharacterSelection characterSelection;
    public GameObject[] characters;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;

        // Auto-find player references if not set
        if (player == null)
            player = FindObjectOfType<playerMovement>();
        if (attack == null)
            attack = FindObjectOfType<playerAttack>();
        if (flyKick == null)
            flyKick = FindObjectOfType<FlyKickScript>();
        if (crouch == null)
            crouch = FindObjectOfType<CrouchScript>();

        // Run only if current scene number is 2 or higher
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            characters[CharacterSelection.selectedCharacterIndex].SetActive(true);
        }
        
            if (GameManager.instance != null)
                GameManager.instance.manager = this;
        

    }

    void Update()
    {
        // Keyboard pause toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // ------------------- Pause Controls -------------------
    public void PauseGame()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Disable player controls
        if (player != null) player.enabled = false;
        if (attack != null) attack.enabled = false;
        if (flyKick != null) flyKick.enabled = false;
        if (crouch != null) crouch.enabled = false;
    }

    public void ResumeGame()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Re-enable player controls
        if (player != null) player.enabled = true;
        if (attack != null) attack.enabled = true;
        if (flyKick != null) flyKick.enabled = true;
        if (crouch != null) crouch.enabled = true;
    }

    // ------------------- Game Over -------------------
    public void ShowGameOver()
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ------------------- Restart & Main Menu -------------------
    public void OnRestartButton()
    {
        Time.timeScale = 1f;

        // Restart current level (resets everything via OnSceneLoaded)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;

        // Reset game state when going to main menu
        if (GameManager.instance != null)
            GameManager.instance.ResetGame();

        SceneManager.LoadScene(0); // Main menu scene
    }

    public void OnSelectButton()
    {
        characterSelection.selectCharacter();
    }

    // Called from Main Menu to start game
    public void OnStartGameButton()
    {
        if (GameManager.instance != null)
            GameManager.instance.StartNewGame();

        SceneManager.LoadScene(1); // First level
    }

    public void switchScene(int sceneNo)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNo);
    }

    // ------------------- UI Updates -------------------
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    //public void UpdateLevel(int level)
    //{
    //    if (levelText != null)
    //        levelText.text = "Level: " + level;
    //}

    // ------------------- Mobile Button Handlers -------------------
    public void OnJumpButton() => player?.Jump();
    public void OnAttackButton() => attack?.PerformAttack();
    public void OnFlyKickButton() => flyKick?.TryFlyKick();
    public void OnCrouchButton() => crouch?.TryToggleCrouch();
    public void OnPauseButton() => PauseGame();
    public void OnResumeButton() => ResumeGame();
}