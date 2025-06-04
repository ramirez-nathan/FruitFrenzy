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
    public GameObject ComboUI;
    private FruitSpawner fruitSpawnerScript;

    public int score = 0;
    public int fails = 0;
    public int highScore = 0;

    public bool inComboWindow = false;
    public int comboCount = 0;
    public float comboWindowTime = 0.5f;

    public Transform[] comboTransforms;
    public Quaternion comboRotation = Quaternion.Euler(0f, 0f, 10f);

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

    public void Update()
    {
        comboWindowTime -= Time.deltaTime;
        if (comboWindowTime < 0)
        {
            inComboWindow = false;
            comboWindowTime = 0.5f;
            CacheCombo();
        }
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
            CacheCombo();
            GameOver();
        }
    }

    public void CacheCombo() 
    {
        if (comboCount >= 3)
        {
            var totalScore = comboCount * 2;
            AddScore(totalScore);
            Debug.Log("hit a combo!");
            var comboUI = Instantiate(ComboUI, comboTransforms[0].position, comboRotation);
            comboUI.GetComponent<ComboUI>().SetCombo(comboCount);
            comboCount = 0;
        }
        else
        {
            AddScore(comboCount);
            comboCount = 0;
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
        if (score > highScore) highScore = score;
        if (score >= 100) // I want to do every 100 pts, erase a fail
        UpdateScore(); // This will update both score UI and difficulty

        // Debug log to see difficulty changes
        if (fruitSpawnerScript != null && fruitSpawnerScript.useDynamicDifficulty)
        {
            //Debug.Log(fruitSpawnerScript.GetDifficultyInfo());
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