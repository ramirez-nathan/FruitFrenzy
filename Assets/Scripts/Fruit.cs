using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Fruit Properties")]
    public int points = 10; // Points this fruit is worth
    public bool hasBeenSliced = false;
    public bool hasBeenProcessed = false; // Prevents double-scoring in combos

    [Header("Fruit Visual/Audio")]
    public GameObject slicedVersion; // The sliced version of the fruit
    public AudioClip sliceSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Set initial values
        hasBeenSliced = false;
        hasBeenProcessed = false;
    }

    public void Slice()
    {
        if (hasBeenSliced) return; // Already sliced

        hasBeenSliced = true;

        // Play slice sound
        if (audioSource != null && sliceSound != null)
        {
            audioSource.PlayOneShot(sliceSound);
        }

        // Visual feedback
        ShowSliceEffect();

        // If not in a combo window, score immediately
        if (!GameManager.Instance.inComboWindow)
        {
            GameManager.Instance.AddScore(points);
            hasBeenProcessed = true;
        }

        // Destroy or deactivate after a short delay
        Invoke(nameof(DestroyFruit), 0.5f);
    }

    private void ShowSliceEffect()
    {
        // Activate sliced version if available
        if (slicedVersion != null)
        {
            slicedVersion.SetActive(true);
            // Hide original mesh
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        // Add some visual flair - particles, etc.
        // You can add particle system activation here
    }

    private void DestroyFruit()
    {
        // Remove from current gesture if still there
        if (GameManager.Instance.inComboWindow &&
            GameManager.Instance.currentGestureFruits != null)
        {
            // Note: You'll need to make currentGestureFruits public in GameManager
            // or create a method to remove fruits
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // If fruit falls off screen without being sliced
        if (other.CompareTag("Ground") || other.CompareTag("DestroyZone"))
        {
            if (!hasBeenSliced)
            {
                // This is a miss - add fail and end combo
                GameManager.Instance.AddFail();
                Destroy(gameObject);
            }
        }
    }
}