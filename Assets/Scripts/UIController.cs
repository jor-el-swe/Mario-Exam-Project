using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{
    public TMP_Text welcomeText;
    public TMP_Text finishText;
    public TMP_Text failingText;
    public TMP_Text lifeText;
    public TMP_Text timerText;
    
    public void SetLifeText(int noLives)
    {
        lifeText.text = null;
        for (var i = 0; i < noLives; i++)
        {
            lifeText.text += "<3 ";
        }
        
    }

    public void SetTimertext(int secondsLeft)
    {
        timerText.text = "Time left: " + secondsLeft;
    }
    
    public void ShowTitleScreen()
    {
        finishText.enabled = false;
        welcomeText.enabled = true;
        failingText.enabled = false;
        lifeText.enabled = false;
        timerText.enabled = false;
    }

    public void ShowGamePlay()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
        failingText.enabled = false;
        lifeText.enabled = true;
        timerText.enabled = true;
    }
    
    public void ShowWinningText()
    {
        finishText.enabled = true;
        welcomeText.enabled = false;
        failingText.enabled = false;
        lifeText.enabled = false;
        timerText.enabled = true;
    }
    
    public void ShowFailingText()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
        failingText.enabled = true;
        lifeText.enabled = false;
        timerText.enabled = true;
    }
}
