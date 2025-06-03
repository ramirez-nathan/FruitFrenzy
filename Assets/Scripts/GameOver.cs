using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI gameOver;
    public UnityEngine.UI.Button restart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
        gameOver.text = "GAME OVER!\n" + "Score: " + GameManager.Instance.score.ToString();
        restart.interactable = false;
        restart.interactable = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
