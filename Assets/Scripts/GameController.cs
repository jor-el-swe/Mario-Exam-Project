using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [Header("References")]
    public UIController uiController;
    
    
    enum GameStates
    {
        titleScreen,
        playing,
        wonGame
    }
    private GameStates currentState;
   
    // Start is called before the first frame update
    void Start()
    {
        currentState = GameStates.titleScreen;
        uiController.showTitleScreen();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.titleScreen:
                if (PlayerController.PlayerHasStarted)
                {
                    currentState = GameStates.playing;
                    uiController.showGamePlay();
                }

                break;

            case GameStates.playing:
                if (PlayerController.PlayerHasWon)
                {
                    currentState = GameStates.wonGame;
                    uiController.showWinningText();
                }
                break;

            case GameStates.wonGame:
                if (false)
                {
                    //restart game
                }
                break;
        }
    }
}
