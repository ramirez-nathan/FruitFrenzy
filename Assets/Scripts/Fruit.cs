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

    public float gravityScale = 0.5f;
    public float maxFallSpeed = -2.5f;

    public float upwardGravityReduction = 0.8f;

    [Tooltip("Duration to hover at apex")]
    public float apexHoverTime = 4f;

    [Tooltip("How much to reduce gravity at apex (0 = no gravity, 1 = full gravity)")]
    [Range(0f, 1f)]
    public float apexGravityMultiplier = 0.15f;

    [Tooltip("Air resistance while moving upward/downward")]
    public float upwardDrag = 0.1f;
    public float downwardDrag = 0.05f;

    
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
            rb.AddForce(Vector3.up * 0.02f, ForceMode.Force);
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
        rb.AddForce(Physics.gravity * gravityScale * 0.5f, ForceMode.Acceleration);

        // light air resistance when falling
        Vector3 dragForce = -velocity.normalized * downwardDrag * velocity.sqrMagnitude;
        rb.AddForce(dragForce, ForceMode.Force);
    }

    public void Slice()
    {
        if (isBomba)
        {
            GameManager.Instance.State = GameState.Lose;
            return;
        }
        // spawn 2 half fruits with exit velocity

    }

    private void LateUpdate()
    {
        // nothing for now idk
    }
}
