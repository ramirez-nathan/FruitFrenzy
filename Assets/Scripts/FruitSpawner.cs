using System.Collections;
using UnityEngine;


public class FruitSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    public GameObject[] WholeFruits;
    // spawn fruit object with wholelayer


    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public Vector3 velocityToSet;
    public float minSpawnInterval = 0.2f; // Minimum spawn interval
    public float maxSpawnInterval = 2f;   // Maximum spawn interval

    [Header("Spawn Area")]
    public Collider Collider; // unused

    private Coroutine spawnCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnFruitRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        
    }

    private IEnumerator SpawnFruitRoutine()
    {
        while (true)
        {
            // wait for a rand interval between min and max
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // spawn a fruit
            SpawnFruit();
        }
    }

    public void SpawnFruit()
    {
        // Check if we have fruit prefabs
        if (WholeFruits == null || WholeFruits.Length == 0)
        {
            Debug.LogWarning("No fruit prefabs assigned to FruitSpawner!");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject selectedFruit = WholeFruits[Random.Range(0, WholeFruits.Length)];

        GameObject fruit = Instantiate(selectedFruit, randomSpawnPoint.position, transform.rotation);
        Rigidbody rb = fruit.GetComponent<Rigidbody>();

        // adjust fruits gravity
        if (rb != null)
        {
            rb.angularVelocity = GetRandAngVel();

            rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-0.2f, 0.2f), 3f, 0f);
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