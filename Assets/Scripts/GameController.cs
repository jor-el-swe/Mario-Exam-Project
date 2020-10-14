using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [Header("References")]
    public UIController uiController;
    public PlayerController playerController;
    
    
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
                if (playerController.PlayerHasStarted)
                {
                    currentState = GameStates.playing;
                    uiController.ShowGamePlay();
                }

                break;

            case GameStates.playing:
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
