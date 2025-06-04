using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;
    public Score scoreScript;
    public GameObject InGameCanvas;
    public GameObject MenuCanvas;
    public GameObject GameOverCanvas;
    public GameObject FruitSpawner;
    private FruitSpawner fruitSpawnerScript;

    public int score = 0;
    public int fails = 0;
    public int highScore = 0;

    public bool inComboWindow = false;
    public float comboWindowTime = 0f;

    private void Awake()
    {
        Instance = this;
        // Get reference to FruitSpawner script
        if (FruitSpawner != null)
        {
            fruitSpawnerScript = FruitSpawner.GetComponent<FruitSpawner>();
        }
        SwitchToMenu();
    }

    private void LateUpdate()
    {
        if (State == GameState.Play)
        {
            if (score > highScore)
            {
                highScore = score;
                UpdateScore();
            }
        }
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
        scoreScript.UpdateScore();
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
    public void UpdateScore()
    {
        scoreScript.UpdateScore();
        
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScore(); // This will update both score UI and difficulty

        // Debug log to see difficulty changes
        if (fruitSpawnerScript != null && fruitSpawnerScript.useDynamicDifficulty)
        {
            Debug.Log(fruitSpawnerScript.GetDifficultyInfo());
        }
    }
    public void AddFail()
    {
        fails++;
    }
    
}
public enum GameState
{
    Play,
    Paused,
    Menu, 
    Lose
}