using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Fruit : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 velocity;
    private float apexTimer = 0f;
    bool isAtApex = false;
    public float apexThreshold = 1f;
    [SerializeField] bool isBomba = false; // bomboclatt

    public GameObject halfFruitPrefab;

    public float gravityScale = 0.5f;
    public float maxFallSpeed = -2.5f;

    public float upwardGravityReduction = 0.8f;

    [Tooltip("Duration to hover at apex")]
    public float apexHoverTime = 6f;

    [Tooltip("How much to reduce gravity at apex (0 = no gravity, 1 = full gravity)")]
    [Range(0f, 1f)]
    public float apexGravityMultiplier = 0.15f;

    [Tooltip("Air resistance while moving upward/downward")]
    public float upwardDrag = 0.1f;
    public float downwardDrag = 0.05f;
    public bool hasBeenSliced = false;
    private void Awake()
    {
        Debug.Assert(halfFruitPrefab != null,
                 $"{name} spawned without a Half Fruit reference!");
        if (halfFruitPrefab != null ) Debug.Log($"{name} has correct reference to {halfFruitPrefab.name}"); 
        gameObject.layer = LayerMask.NameToLayer("WholeFruit");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("WholeFruit"), LayerMask.NameToLayer("Bamboo"), true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        if (velocity.y > 0) // Moving upward
        {
            ApplyUpwardPhysics();
        }
        else if (Mathf.Abs(velocity.y) < apexThreshold) // Near apex
        {
            ApplyApexPhysics();
        }
        else // Falling
        {
            ApplyFallingPhysics();
        }

        var clampedVelocity = rb.linearVelocity;
        clampedVelocity.y = Mathf.Max(clampedVelocity.y, maxFallSpeed);
        rb.linearVelocity = clampedVelocity;
    }

    private void ApplyUpwardPhysics()
    {
        isAtApex = false;
        apexTimer = 0f;

        // reduced gravity when moving up for floaty feel
        float reducedGravity = gravityScale * upwardGravityReduction;
        //Debug.Log($"upward gravity: {Physics.gravity * reducedGravity}");
        rb.AddForce(Physics.gravity * reducedGravity, ForceMode.Acceleration);

        // add upward air resistance
        Vector3 dragForce = -velocity.normalized * upwardDrag * velocity.sqrMagnitude;
        rb.AddForce(dragForce, ForceMode.Force);
    }

    private void ApplyApexPhysics()
    {
        if (!isAtApex)
        {
            isAtApex = true;
            apexTimer = 0f;
        }

        apexTimer += Time.fixedDeltaTime;

        if (apexTimer < apexHoverTime)
        {
            //Debug.Log($"apex gravity: {Physics.gravity * gravityScale * apexGravityMultiplier}");
            // reduced gravity at apex
            rb.AddForce(Physics.gravity * gravityScale * apexGravityMultiplier, ForceMode.Acceleration);

            // slight upward force to really suspend the fruit
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Force);
        }
        else
        {
            //Debug.Log("applying falling physics from apex");
            // transition back to normal falling
            ApplyFallingPhysics();
        }
    }

    private void ApplyFallingPhysics()
    {
        isAtApex = false;

        // normal gravity when falling
        rb.AddForce(Physics.gravity * gravityScale * 0.3f, ForceMode.Acceleration);

        // light air resistance when falling
        Vector3 dragForce = -velocity.normalized * downwardDrag * velocity.sqrMagnitude;
        rb.AddForce(dragForce, ForceMode.Force);
    }

    public void Slice()
    {
        hasBeenSliced = true;
        if (isBomba)
        {
            GameManager.Instance.State = GameState.Lose;
            return;
        }
        else
        {
            GameManager.Instance.AddScore(1);
            SpawnHalfFruit(true); // is flipped
            SpawnHalfFruit(false); // isnt flipped
        }
        // spawn 2 half fruits with exit velocity

    }

    public void SpawnHalfFruit(bool isFlipped)
    {
        GameObject halfFruit = Instantiate(halfFruitPrefab, transform.position, transform.rotation);
        Rigidbody rb = halfFruit.GetComponent<Rigidbody>();
        var halfFruitScript = halfFruit.GetComponent<HalfFruit>();
        halfFruitScript.isFlipped = isFlipped;
        if (rb != null)
        {
            rb.angularVelocity = this.rb.angularVelocity;
        }
        halfFruitScript.SetSpawnSpacingandRotation();
        Destroy(gameObject);
    }

    public Vector3 GetRandAngVel()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(-2f, 2f);
        float z = Random.Range(-2f, 2f);

        return new Vector3(x, y, z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{gameObject.name} collision with {collision.gameObject.name} | This layer: {gameObject.layer}, Other layer: {collision.gameObject.layer}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Plane"))
        {
            if (!isBomba) GameManager.Instance.AddFail();
            Destroy(gameObject);
        }
    }
    private void LateUpdate()
    {
        // nothing for now idk
    }
}
