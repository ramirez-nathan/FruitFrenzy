using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;
    public int currFails = 0;
    public Image redX1;
    public Image redX2;
    public Image redX3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currFails = 0;
    }
    private void OnEnable()
    {
        currFails = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.text = "Score: " + GameManager.Instance.score.ToString();
        CheckForFailUpdate();
        
    }
    public void CheckForFailUpdate()
    {
        if (currFails < GameManager.Instance.fails)
        {
            Debug.Log("failed, adding x");
            if (GameManager.Instance.fails > 2)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f);
                redX2.color = new Color(1f, 1f, 1f, 1f);
                redX3.color = new Color(1f, 1f, 1f, 1f);
                GameManager.Instance.State = GameState.Lose;   
            }
            else if (GameManager.Instance.fails > 1)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f); 
                redX2.color = new Color(1f, 1f, 1f, 1f);
            }
            else if (GameManager.Instance.fails > 0)
            {
                redX1.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        currFails = GameManager.Instance.fails;
    }
    public void ResetXs()
    {
        redX1.color = new Color(0f, 0f, 0f, 1f);
        redX2.color = new Color(0f, 0f, 0f, 1f);
        redX3.color = new Color(0f, 0f, 0f, 1f);
    }
}
