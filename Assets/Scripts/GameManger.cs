using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    private void Awake()
    {
        Instance = this;
        State = GameState.Play;
    }


}
public enum GameState
{
    Play,
    Paused,
    Menu, 
    Lose
}