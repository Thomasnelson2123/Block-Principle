using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    enum GameState
    {
        gameOver,
        normal,
        menu,
        jump
    }

    [SerializeField] private LevelController levelController;

    private GameState state = GameState.normal;

    public void SetGameState(int gameState)
    {
        state = (GameState)gameState;
    }


    public bool isNormal() { return state == GameState.normal; }

    public bool isGameOver() { return state == GameState.gameOver; }

    public bool isMenu() { return state == GameState.menu; }

    public void GetStateOfMenu(bool menuOpen)
    {
        GameState previousState = state;
        if (menuOpen)
        { 
            state = GameState.menu;
        }         
        else
            state = previousState;
    }

    public void SetJump(bool isJumping)
    {
        if (isJumping)
            state = GameState.jump;
        else
            state = GameState.normal;
    }

    public bool InitiateJump { get; set; } = false;

    private void Update()
    {
        Debug.Log(state);
    }

    public void NextLevel()
    {
        levelController.LoadNextLevel();
    }



}
