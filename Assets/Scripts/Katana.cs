using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [Header("Katana Identity")]
    public string katanaId = "Left"; // Set to "Left" or "Right" in inspector

    [Header("Velocity Tracking")]
    public float velocityUpdateRate = 0.02f; // How often to update velocity
    private Vector3 lastPosition;
    private float lastUpdateTime;
    private float currentVelocity = 0f;

    [Header("Combo Detection")]
    public float minSlashVelocity = 1.0f; // Minimum velocity to count as slashing

    private void Start()
    {
        lastPosition = transform.position;
        lastUpdateTime = Time.time;

        // Start velocity tracking coroutine
        InvokeRepeating(nameof(UpdateVelocity), 0f, velocityUpdateRate);
    }

    private void UpdateVelocity()
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            float deltaTime = Time.time - lastUpdateTime;
            if (deltaTime > 0)
            {
                Vector3 deltaPosition = transform.position - lastPosition;
                currentVelocity = deltaPosition.magnitude / deltaTime;

                // Inform GameManager about velocity changes with katana ID
                GameManager.Instance.OnSlashVelocityChanged(currentVelocity, katanaId);

                lastPosition = transform.position;
                lastUpdateTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("WholeFruit"))
            {
                Debug.Log($"Sword hit fruit {other.name}");
                var fruitScript = other.gameObject.GetComponent<Fruit>();
                if (!fruitScript.hasBeenSliced)
                {
                    SliceFruit(fruitScript);
                }
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
                var fruitScript = other.gameObject.GetComponent<Fruit>();
                if (!fruitScript.hasBeenSliced)
                {
                    SliceFruit(fruitScript);
                }
            }
        }
    }

    public void SliceFruit(Fruit fruitScript)
    {
        // Get combined velocity from both katanas
        float combinedVelocity = Mathf.Max(GameManager.Instance.leftKatanaVelocity,
                                          GameManager.Instance.rightKatanaVelocity);

        // Start combo window if not already active and either katana is slashing fast enough
        if (!GameManager.Instance.inComboWindow && combinedVelocity >= minSlashVelocity)
        {
            GameManager.Instance.StartComboWindow();
        }

        // Add fruit to current gesture if combo window is active
        if (GameManager.Instance.inComboWindow)
        {
            GameManager.Instance.AddFruitToCurrentGesture(fruitScript);
        }

        // Slice the fruit
        fruitScript.Slice();
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(UpdateVelocity));
    }
}