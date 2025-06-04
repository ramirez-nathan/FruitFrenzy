using System.Data;
using UnityEngine;

public class HalfFruit : MonoBehaviour
{
    private float gravityScale = 1f;

    public Rigidbody rb;

    public char axisToManipulate;

    public float spawnSpacingDist;

    public bool isFlipped;

    public float separationForce = 0.2f;

    private ParticleSystem juiceParticleEffect;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("HalfFruit");
        rb = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        if (juiceParticleEffect != null)
        {
            Debug.LogWarning($"{gameObject.name}: Juice Particle Effect not found!");
        }
        juiceParticleEffect.Play();
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Fall();
    }

    private void Fall()
    {
        // normal gravity when falling
        rb.AddForce(Physics.gravity * gravityScale * 0.5f, ForceMode.Acceleration);
    }

    public void SetSpawnSpacingandRotation()
    {
        if (rb != null)
        {
            var ls = transform.localScale;
            var newSpacing = transform.position;
            Vector3 separationDirection = GetSeparationDirection(isFlipped);
            Debug.Log("setting spawn spacing and rotation ");
            switch (axisToManipulate)
            {
                case 'x':
                    if (isFlipped) // flip local scale
                    {
                        transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
                    }

                    break;
                case 'y':
                    if (isFlipped) // flip local scale
                    {
                        transform.localScale = new Vector3(ls.x, -ls.y, ls.z);
                    }

                    break;
                case 'z':
                    if (isFlipped) // flip local scale
                    {
                        transform.localScale = new Vector3(ls.x, ls.y, -ls.z);
                    }

                    break;
            }
            rb.AddForce(separationDirection * separationForce, ForceMode.Impulse);
        }
        else Debug.LogWarning("half fruit rb is null! Something went wrong");
    }

    private Vector3 GetSeparationDirection(bool isFlipped)
    {

        switch (axisToManipulate)
        {
            case 'x':
                return isFlipped ? Vector3.left : Vector3.right;
            case 'y':
                return isFlipped ? Vector3.down : Vector3.up;
            case 'z':
                return isFlipped ? Vector3.back : Vector3.forward;
            default:
                return isFlipped ? Vector3.left : Vector3.right;
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bamboo"))
        {
            Debug.Log($"destroying half fruit {this.gameObject.name}");
            Destroy(gameObject);
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{gameObject.name} collision with {collision.gameObject.name} | This layer: {gameObject.layer}, Other layer: {collision.gameObject.layer}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Plane"))
        {
            Destroy(gameObject);
        }
    }
}
