using Oculus.Interaction;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] public char katanaId;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("WholeFruit"))
            {
                Debug.Log($"Sword hit fruit {other.name}");
                //Debug.Log($"{gameObject.name} collision with {other.gameObject.name} | This layer: {gameObject.layer}, Other layer: {other.gameObject.layer}");
                var fruitScript = other.gameObject.GetComponent<Fruit>();
                if (!fruitScript.hasBeenSliced) SliceFruit(fruitScript);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("WholeFruit"))
            {
                Debug.Log($"Sword hit fruit {other.name}");
                //Debug.Log($"{gameObject.name} collision with {other.gameObject.name} | This layer: {gameObject.layer}, Other layer: {other.gameObject.layer}");
                var fruitScript = other.gameObject.GetComponent<Fruit>();
                if (!fruitScript.hasBeenSliced) SliceFruit(fruitScript);
            }

        }
    }

    public void SliceFruit(Fruit fruitScript)
    {
        if (fruitScript.isBomba) // bigger vibration
        {
            VibrateController(0.8f, fruitScript);
        }
        else // smaller vibration
        {
            VibrateController(0.4f, fruitScript);
        }
        fruitScript.Slice();
    }
    public void VibrateController(float amplitude, Fruit fruitScript)
    {
        if (katanaId == 'r')
        {
            Debug.Log("Right katana hit fruit");
            OVRInput.SetControllerVibration(1, amplitude, OVRInput.Controller.RTouch);
            if (fruitScript.isBomba) OVRInput.SetControllerVibration(1, amplitude * 0.5f, OVRInput.Controller.LTouch);
            if (!fruitScript.isBomba) StartCoroutine(VibrationTime(0.3f, false)); // if fruit then shorter vibration
            else StartCoroutine(VibrationTime(0.6f, false)); // else its bomb so longer vibration
        }
        else if (katanaId == 'l')
        {
            Debug.Log("Left katana hit fruit");
            OVRInput.SetControllerVibration(1, amplitude, OVRInput.Controller.LTouch);
            if (fruitScript.isBomba) OVRInput.SetControllerVibration(1, amplitude * 0.5f, OVRInput.Controller.RTouch); 
            if (!fruitScript.isBomba) StartCoroutine(VibrationTime(0.3f, false));
            else StartCoroutine(VibrationTime(0.6f, false)); 
        }
        else Debug.LogWarning("Katana id is invalid!");
    }
    private IEnumerator VibrationTime(float seconds, bool isBomb)
    {
        yield return new WaitForSeconds(seconds);
        if (katanaId == 'r')
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            if (isBomb) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
        else if (katanaId == 'l')
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            if (isBomb) OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
        else Debug.LogWarning("Katana id is invalid!");
    }
}
