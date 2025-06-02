//using UnityEngine;
//using UnityEngine.SceneManagement;
//public class SceneLoader : MonoBehaviour
//{
//    public void LoadSceneByName(string sceneName)
//    {
//        Debug.Log("Loading scene: " + sceneName);
//        SceneManager.LoadScene(sceneName);
//    }

//    public void QuitGame()
//    {
//        Debug.Log("Quit button pressed!");
//        Application.Quit();
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    //public static SceneLoader Instance; 
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log("Start loading scene: " + sceneName);
        StartCoroutine(LoadSceneWithXR(sceneName));
    }

    IEnumerator LoadSceneWithXR(string sceneName)
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("XR loader failed to initialize!");
            yield break;
        }

        XRGeneralSettings.Instance.Manager.StartSubsystems();
        Debug.Log("XR initialized. Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
        GameManager.Instance.State = GameState.Play;
        GameManager.Instance.SwitchToPlayState();
    }

    public void PlayGame()
    {
        GameManager.Instance.SwitchToPlayState();
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");
        Application.Quit();
    }
}