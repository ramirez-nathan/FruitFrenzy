using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;

    public int prevFails = 0;
    public Image redX1;
    public Image redX2;
    public Image redX3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevFails = 0;
    }
    private void OnEnable()
    {
        prevFails = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        //AddScore();
        CheckForFailUpdate();
    }

    public void UpdateScore()
    {
        score.text = "Score: " + GameManager.Instance.score.ToString() + "\nBest: " + GameManager.Instance.highScore.ToString();
        
    }
    public void CheckForFailUpdate()
    {
        if (prevFails < GameManager.Instance.fails)
        {
            Debug.Log("adding fail");
            if (GameManager.Instance.fails == 3)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f);
                redX2.color = new Color(1f, 1f, 1f, 1f);
                redX3.color = new Color(1f, 1f, 1f, 1f);
                GameManager.Instance.State = GameState.Lose;
            }
            else if (GameManager.Instance.fails == 2)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f);
                redX2.color = new Color(1f, 1f, 1f, 1f);
            }
            else if (GameManager.Instance.fails == 1)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        else if (prevFails > GameManager.Instance.fails)
        {
            Debug.Log("removing fail");
            if (GameManager.Instance.fails == 3) // edge case 
            {
                GameManager.Instance.State = GameState.Lose;
            }
            if (GameManager.Instance.fails == 2) // was prev 3 fails
            {
                redX3.color = new Color(0f, 0f, 0f, 1f);
            }
            else if (GameManager.Instance.fails == 1) // was prev 2 fails
            {
                redX2.color = new Color(0f, 0f, 0f, 1f);
            }
            else if (GameManager.Instance.fails == 0) // was prev 1 fail
            {
                redX1.color = new Color(0f, 0f, 0f, 1f);
            }
        }
        prevFails = GameManager.Instance.fails; 
    }
    public void ResetXs()
    {
        redX1.color = new Color(0f, 0f, 0f, 1f);
        redX2.color = new Color(0f, 0f, 0f, 1f);
        redX3.color = new Color(0f, 0f, 0f, 1f);
    }
}
