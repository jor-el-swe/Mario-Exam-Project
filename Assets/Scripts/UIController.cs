
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text welcomeText;
    public TMP_Text finishText;

    public void showTitleScreen()
    {
        finishText.enabled = false;
        welcomeText.enabled = true;
    }

    public void showGamePlay()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
    }
    
    public void showWinningText()
    {
        finishText.enabled = false;
        welcomeText.enabled = false;
    }
}
