using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [Header("References")]
    public UIController uiController;
    public PlayerController playerController;
    public Enemy[] enemies;
    public AudioHandler audioHandler;
    
    public int maxGameTime = 60;
    private float gameTimer = 0f;
    private int currentLevel = 1;
    
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
                
                audioHandler.PlaySong(currentLevel);
                
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
                    audioHandler.FadeOutMusic(currentLevel);
                }

                if (playerController.PlayerhasDied)
                {
                    currentState = GameStates.failedGame;
                    uiController.ShowFailingText();
                    audioHandler.FadeOutMusic(currentLevel);
                }
                break;

                case GameStates.wonGame:
                if (playerController.PlayerhasReset)
                {
                    currentState = GameStates.titleScreen;
                    uiController.ShowTitleScreen();
                    playerController.ResetGame();
                    audioHandler.FadeInMusic(currentLevel);
                    foreach (var enemy in enemies)
                    {
                        enemy.ResetEnemy();
                    }

                }
                break;
            
            case GameStates.failedGame:
                if (playerController.PlayerhasReset)
                {
                    currentState = GameStates.titleScreen;
                    uiController.ShowTitleScreen();
                    playerController.ResetGame();
                    audioHandler.FadeInMusic(currentLevel);
                    foreach (var enemy in enemies)
                    {
                        enemy.ResetEnemy();
                    }
                }
                break;
        }
    }
}
