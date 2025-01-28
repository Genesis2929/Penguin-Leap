using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
	float recentTime;

	public GameObject play;
	public GameObject tutorial;
	public GameObject tutorialinstruction;

    public TextMeshProUGUI currentScoreText;
	public Text scoreText;

    private void UpdatePauseScoreText()
    {
        // Update the high score TextMeshPro with the high score
        if (currentScoreText != null)
        {
            if(scoreText!= null)
			{
				scoreText.text = currentScoreText.text;	
			}
        }
    }

    public void ToMenu() 
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	}
	public void ToGame() {SceneManager.LoadScene("Game", LoadSceneMode.Single);}
	
	//Save the recent time and pause the game
	public void Pause()
	{
		UpdatePauseScoreText();
		recentTime = Time.timeScale; Time.timeScale = 0;
	}
	//Continue back to the recent time
	public void Continue() {Time.timeScale = recentTime;}


	public void tutorialfunc()
	{
		play.SetActive(false);
		tutorialinstruction.SetActive(true);
		tutorial.SetActive(false);
	}
	public void close()
	{
        play.SetActive(true);
        tutorialinstruction.SetActive(false);
        tutorial.SetActive(true);
    }
}