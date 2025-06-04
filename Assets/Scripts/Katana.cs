using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
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

        fruitScript.Slice();
    }
}
