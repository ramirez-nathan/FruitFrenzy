using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Fruit : MonoBehaviour
{
    public float gravityScale = 0.1f;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

}
