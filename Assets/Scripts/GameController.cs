using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [Header("References")]
    public UIController uiController;
    public PlayerController playerController;
    public int maxGameTime = 60;
    
    private float gameTimer = 0f;
    
    enum GameStates
    {
        titleScreen,
        playing,
        wonGame, 
        failedGame
    }
    private GameStates currentState;
   
    // Start is called before the first frame update
    void Start()
    {
        currentState = GameStates.titleScreen;
        uiController.ShowTitleScreen();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.titleScreen:
                gameTimer = 0;
                if (playerController.PlayerHasStarted)
                {
                    currentState = GameStates.playing;
                    uiController.ShowGamePlay();
                }

                break;

            case GameStates.playing:
                gameTimer += Time.deltaTime;
                var timeLeft = maxGameTime - (int) gameTimer;
                if (timeLeft <= 0)
                {
                    timeLeft = 0;
                    playerController.PlayerhasDied = true;
                    currentState = GameStates.failedGame;
                    uiController.ShowFailingText();
                }
                
                uiController.SetTimertext(timeLeft);
                
                if (playerController.PlayerHasWon)
                {
                    currentState = GameStates.wonGame;
                    uiController.ShowWinningText();
                }

                if (playerController.PlayerhasDied)
                {
                    currentState = GameStates.failedGame;
                    uiController.ShowFailingText();
                }
                break;

                case GameStates.wonGame:
                if (playerController.PlayerhasReset)
                {
                    currentState = GameStates.titleScreen;
                    uiController.ShowTitleScreen();
                    playerController.ResetGame();
                }
                break;
            
            case GameStates.failedGame:
                if (playerController.PlayerhasReset)
                {
                    currentState = GameStates.titleScreen;
                    uiController.ShowTitleScreen();
                    playerController.ResetGame();
                }
                break;
        }
    }
}
