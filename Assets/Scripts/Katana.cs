using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WholeFruit"))
        {
            Debug.Log("Sword hit fruit");
            other.gameObject.GetComponent<Fruit>().Slice();
        }
    }
}
