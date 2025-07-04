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
    [SerializeField] public bool isBomba = false; // bomboclatt

    // Fruit particles
    private ParticleSystem juiceParticleEffect;
    public ParticleSystem sparkParticleEffect;
    public GameObject explosionEffect;
    public GameObject spawnSmokeEffect;

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
    public bool isDead = false;
    private void Awake()
    {
        //Debug.Assert(halfFruitPrefab != null,
                 //$"{name} spawned without a Half Fruit reference!");
        rb = GetComponent<Rigidbody>();
        if (halfFruitPrefab != null) //Debug.Log($"{name} has correct reference to {halfFruitPrefab.name}");
        gameObject.layer = LayerMask.NameToLayer("WholeFruit");
        if (isBomba) sparkParticleEffect.Play();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("WholeFruit"), LayerMask.NameToLayer("Bamboo"), true);
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        var smoke = Instantiate(spawnSmokeEffect, transform.position, Quaternion.identity);
        smoke.gameObject.SetActive(true);
        smoke.GetComponent<ParticleSystem>().Play();
        smoke.GetComponentInChildren<ParticleSystem>().Play();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = rb.linearVelocity;
        if (GameManager.Instance.State == GameState.Lose) { isDead = true; }    
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
            var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            explosion.GetComponent<ParticleSystem>().Play();
            explosion.GetComponentInChildren<ParticleSystem>().Play(); 
            GameManager.Instance.State = GameState.Lose;
            AudioManager.instance.PlaySoundByName("BombExplode", false); // play bomb sound
            Destroy(gameObject);
            return;
        }
        else
        {

            if (GameManager.Instance.inComboWindow)
            {
                GameManager.Instance.comboCount++;
                Debug.Log("resetting Combo window");
                GameManager.Instance.comboWindowTime = 0.5f; // reset the window
            }
            else
            {
                GameManager.Instance.comboCount++;
                Debug.Log("Starting Combo window");
                GameManager.Instance.inComboWindow = true; // enter combo window
                GameManager.Instance.comboWindowTime = 0.5f; // reset the window
            }
            AudioManager.instance.PlaySoundByName("FruitSlice", true); // play fruit slice sound
            SpawnHalfFruit(true); // is flipped
            SpawnHalfFruit(false); // isnt flipped
            //juiceParticleEffect.Play();
            Destroy(gameObject); // destroy self
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
            if (!isBomba && GameManager.Instance.State == GameState.Play && !isDead)
            {
                GameManager.Instance.AddFail();
                AudioManager.instance.PlaySoundByName("Fail", false); // play fail sound
            }
            Destroy(gameObject);
        }
    }
    private void LateUpdate()
    {
        // nothing for now idk
    }
}
