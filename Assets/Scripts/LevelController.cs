using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameController gameState;
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        // will do more later than just restart level
        // i.e kick out to menu, or display death animation, reduce lives, etc
        if(gameState.isGameOver())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
