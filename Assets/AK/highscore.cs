using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;    // Reference to the high score TextMeshPro
    public TextMeshProUGUI currentScoreText; // Reference to the current score TextMeshPro
    public GameObject gameOverGameObject;    // Reference to the Game Over UI GameObject


    private int highScore = 0;               // Variable to track the high score

    void Start()
    {
        // Load the saved high score from PlayerPrefs
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }

        // Update the high score display
        UpdateHighScoreText();
    }

    void Update()
    {
        // Check if the gameOverGameObject is active in the scene
        if (gameOverGameObject != null && gameOverGameObject.activeSelf)
        {
            UpdateHighScore();
        }
    }

    public void UpdateHighScore()
    {
        int currentScore = 0;

        // Convert the current score TextMeshPro text into an integer
        if (currentScoreText != null && int.TryParse(currentScoreText.text, out currentScore))
        {
            // Check if the current score is greater than the saved high score
            if (currentScore > highScore)
            {
                highScore = currentScore;

                // Save the new high score to PlayerPrefs
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }

            // Update the high score display
            UpdateHighScoreText();
        }
        else
        {
            Debug.LogWarning("Unable to parse current score text into an integer.");
        }
    }

    private void UpdateHighScoreText()
    {
        // Update the high score TextMeshPro with the high score
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }
}
