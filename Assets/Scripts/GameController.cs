using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    enum GameState
    {
        gameOver,
        normal,
        menu
    }

    private GameState state = GameState.normal;

    public void SetGameState(int gameState)
    {
        state = (GameState)gameState;
    }

    public int GetGameState()
    {
        return (int)state;
    }
}
