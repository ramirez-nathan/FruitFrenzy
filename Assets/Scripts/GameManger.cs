using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Score & Combo System")]
    public int score = 0;
    public int fails = 0;
    public int highScore = 0;

    // Combo system variables
    [Header("Combo Settings")]
    public float comboWindowTime = 0.25f; // Time window for gesture detection
    public float velocityThreshold = 0.5f; // Minimum velocity to consider active slashing
    public int minComboSize = 3; // Minimum fruits for a combo

    // Combo tracking
    public bool inComboWindow = false;
    private float comboTimer = 0f;
    public List<Fruit> currentGestureFruits = new List<Fruit>();
    private bool isActiveSlashing = false;
    private float lastSlashTime = 0f;

    // UI Feedback
    [Header("Combo UI")]
    public GameObject comboPopupPrefab; // Assign a UI prefab for "+X COMBO" popup
    public Transform comboUIParent; // Parent transform for combo popups

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
            if (score > highScore) highScore = score;
            UpdateComboWindow();
        }
        if (State == GameState.Lose)
        {
            GameOver();
        }
    }

    private void UpdateComboWindow()
    {
        if (inComboWindow)
        {
            comboTimer += Time.deltaTime;

            // End combo window if time expired
            if (comboTimer >= comboWindowTime)
            {
                EndComboWindow();
            }
        }
    }

    public void StartComboWindow()
    {
        if (!inComboWindow)
        {
            inComboWindow = true;
            comboTimer = 0f;
            currentGestureFruits.Clear();
            isActiveSlashing = true;
            lastSlashTime = Time.time;
        }
    }

    public void ExtendComboWindow()
    {
        if (inComboWindow)
        {
            comboTimer = 0f; // Reset timer to extend window
            lastSlashTime = Time.time;
        }
    }

    public void EndComboWindow()
    {
        if (inComboWindow)
        {
            inComboWindow = false;
            isActiveSlashing = false;

            // Check if we have a combo
            if (currentGestureFruits.Count >= minComboSize)
            {
                ProcessCombo();
            }
            else
            {
                // Process individual fruit scores
                foreach (Fruit fruit in currentGestureFruits)
                {
                    if (fruit != null && !fruit.hasBeenProcessed)
                    {
                        AddScore(fruit.points);
                        fruit.hasBeenProcessed = true;
                    }
                }
            }

            currentGestureFruits.Clear();
        }
    }

    public void AddFruitToCurrentGesture(Fruit fruit)
    {
        if (inComboWindow && !currentGestureFruits.Contains(fruit))
        {
            currentGestureFruits.Add(fruit);
            ExtendComboWindow(); // Extend window when fruit is added
        }
    }

    private void ProcessCombo()
    {
        int comboSize = currentGestureFruits.Count;
        int totalPoints = 0;

        // Calculate combo points (double points for combo fruits)
        foreach (Fruit fruit in currentGestureFruits)
        {
            if (fruit != null && !fruit.hasBeenProcessed)
            {
                totalPoints += fruit.points * 2; // Double points for combo
                fruit.hasBeenProcessed = true;
            }
        }

        // Add combo points to score
        AddScore(totalPoints);

        // Show combo UI feedback
        ShowComboPopup(comboSize, totalPoints);

        Debug.Log($"COMBO! {comboSize} fruits for {totalPoints} points!");
    }

    private void ShowComboPopup(int comboSize, int points)
    {
        if (comboPopupPrefab != null && comboUIParent != null)
        {
            GameObject popup = Instantiate(comboPopupPrefab, comboUIParent);
            ComboPopup popupScript = popup.GetComponent<ComboPopup>();
            if (popupScript != null)
            {
                popupScript.ShowCombo(comboSize, points);
            }
        }
    }

    // Dual katana support
    public float leftKatanaVelocity = 0f;
    public float rightKatanaVelocity = 0f;

    // Called by controller/katana when slash velocity changes
    public void OnSlashVelocityChanged(float velocity, string katanaId = "")
    {
        // Track individual katana velocities
        if (katanaId == "Left")
            leftKatanaVelocity = velocity;
        else if (katanaId == "Right")
            rightKatanaVelocity = velocity;
        else
            // Fallback for single katana or unspecified
            leftKatanaVelocity = velocity;

        // Combined velocity - either katana can trigger slashing state
        float maxVelocity = Mathf.Max(leftKatanaVelocity, rightKatanaVelocity);
        bool wasSlashing = isActiveSlashing;
        isActiveSlashing = maxVelocity > velocityThreshold;

        // If we stopped slashing and have fruits in current gesture, start countdown
        if (wasSlashing && !isActiveSlashing && currentGestureFruits.Count > 0)
        {
            // Don't immediately end - let the timer handle it
            // This gives a small grace period for continuous slashing
        }

        // If we're slashing again, extend the window
        if (isActiveSlashing && inComboWindow)
        {
            ExtendComboWindow();
        }
    }

    public void SwitchToPlayState()
    {
        State = GameState.Play;
        FruitSpawner.gameObject.SetActive(true);
        if (score > highScore) highScore = score;
        score = 0;
        fails = 0;

        // Reset combo system
        inComboWindow = false;
        currentGestureFruits.Clear();
        isActiveSlashing = false;
        leftKatanaVelocity = 0f;
        rightKatanaVelocity = 0f;
        comboTimer = 0f;
        leftKatanaVelocity = 0f;
        rightKatanaVelocity = 0f;

        scoreScript.UpdateScore();
        InGameCanvas.SetActive(true);
        InGameCanvas.GetComponent<Score>().ResetXs();
        MenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
    }

    public void SwitchToMenu()
    {
        State = GameState.Menu;

        // Reset combo system
        inComboWindow = false;
        currentGestureFruits.Clear();
        isActiveSlashing = false;

        InGameCanvas.SetActive(false);
        MenuCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
    }

    public void GameOver()
    {
        // End any active combo
        if (inComboWindow)
        {
            EndComboWindow();
        }

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

        // End combo window when player fails (misses fruit)
        if (inComboWindow)
        {
            EndComboWindow();
        }
    }
}

public enum GameState
{
    Play,
    Paused,
    Menu,
    Lose
}