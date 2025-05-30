using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    private void Awake()
    {
        Instance = this;
        State = GameState.Play;
    }

    private void LateUpdate()
    {
        if (State == GameState.Play)
        {

        }
        else if (State == GameState.Lose)
        {

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