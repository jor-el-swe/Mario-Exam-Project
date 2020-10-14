using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{
    public TMP_Text welcomeText;
    public TMP_Text finishText;
    public TMP_Text failingText;
    
    public void ShowTitleScreen()
    {
        finishText.enabled = false;
        welcomeText.enabled = true;
        failingText.enabled = false;
    }

    public void ShowGamePlay()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
        failingText.enabled = false;
    }
    
    public void ShowWinningText()
    {
        finishText.enabled = true;
        welcomeText.enabled = false;
        failingText.enabled = false;
    }
    
    public void ShowFailingText()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
        failingText.enabled = true;
    }
}
