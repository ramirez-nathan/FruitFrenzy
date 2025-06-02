using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public GameObject InGameCanvas;
    public GameObject MenuCanvas;
    public GameObject GameOverCanvas;
    public GameObject FruitSpawner;

    public int score = 0;
    public int fails = 0;
    public int highScore = 0;
    private void Awake()
    {
        Instance = this;
        SwitchToMenu();
    }

    private void LateUpdate()
    {
        if (State == GameState.Lose)
        {
            GameOver();
        }
    }
    public void SwitchToPlayState()
    {
        State = GameState.Play;
        FruitSpawner.gameObject.SetActive(true);    
        if (score > highScore) highScore = score;
        score = 0;
        fails = 0;
        InGameCanvas.SetActive(true);
        InGameCanvas.GetComponent<Score>().ResetXs();
        MenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
    }
    public void SwitchToMenu() 
    {
        State = GameState.Menu;
        InGameCanvas.SetActive(false);
        MenuCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
    }
    public void GameOver()
    {
        InGameCanvas.SetActive(false);
        MenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        FruitSpawner.gameObject.SetActive(false);
    }
}
public enum GameState
{
    Play,
    Paused,
    Menu, 
    Lose
}