using UnityEngine;

public class TreeSway : MonoBehaviour
{
[Header("Sway Settings")]
    public float swayAngle = 5f;         // Maximum rotation in degrees
    public float swaySpeed = 1f;         // Speed of sway
    public Vector3 swayAxis = Vector3.forward; // Axis of rotation

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAngle;
        transform.localRotation = initialRotation * Quaternion.AngleAxis(sway, swayAxis);
    }
}
