using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WholeFruit"))
        {
            //other.gameObject.GetComponent<Fruit>().ther
        }
    }
}
