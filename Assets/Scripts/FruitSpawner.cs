using UnityEngine;


public class FruitSpawner : MonoBehaviour
{
    public GameObject Fruit;
    public Vector3 velocityToSet;

    [Header("Spawn Area")]
    public Collider Collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SpawnFruit()
    {
        GameObject fruit = Instantiate(Fruit, GetRandSpawnPos(), transform.rotation);
        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-1f, 1f), 6f, -.3f);

            rb.angularVelocity = GetRandAngVel();
        }
    }

    public Vector3 GetRandAngVel()
    {
        float x = Random.Range(-3f, 3f);
        float y = Random.Range(-3f, 3f);
        float z = Random.Range(-3f, 3f);

        return new Vector3(x, y, z);
    }

    public Vector3 GetRandSpawnPos()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = transform.position.y;
        float z = Random.Range(-1.77f, -1.3f);

        return new Vector3(x, y, z);
    }

    public Quaternion GetRandSpawnRot()
    {
        float x = Random.Range(0f, 360f);
        float y = Random.Range(0f, 360f);
        float z = Random.Range(0f, 360f);

        return Quaternion.Euler(x, y, z);
    }


    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnFruit();
        }
    }
}