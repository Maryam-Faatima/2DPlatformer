using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    public GameObject HighPanel;

    public TextMeshProUGUI HighScoreText;
    public void switchScene(int sceneNo)
    {
        Time.timeScale = 1f;
       
        SceneManager.LoadScene(sceneNo);
        
    }

    public void onHighButton()
    {
        HighPanel.SetActive(true);
    }
    public void closeHighButton()
    {
        HighPanel.SetActive(false);
    }
    private void Update()
    {
        if (HighScoreText != null)
        {
            HighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");

        }

    }

}
