using System.Collections;
using Unity.VisualScripting;
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

    [Header("Group Spawn Settings")]
    public bool useGroupSpawning = true;
    public int minFruitsPerGroup = 2;
    public int maxFruitsPerGroup = 4;
    public float bombProbability = 0.2f; // preset 20% chance of spawning a bomb per group (old) 
    public float groupSpawnDelay = 0.1f; // Delay between spawning fruits in a group

    [Header("Dynamic Difficulty Settings")]
    public bool useDynamicDifficulty = true;
    public int baseMinFruits = 1; // Starting minimum fruits
    public int baseMaxFruits = 2; // Starting maximum fruits
    public int maxMinFruits = 3;  // Maximum minimum fruits (at high scores)
    public int maxMaxFruits = 5;  // Maximum maximum fruits (at high scores)
    public int scoreThreshold = 100; // Score needed to reach max difficulty
    public float baseBombProbability = 0.1f; // Starting bomb probability (10%)
    public float maxBombProbability = 0.25f;  // Maximum bomb probability (30%)

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
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        StartSpawning();
    }

    private IEnumerator SpawnFruitRoutine()
    {
        while (true)
        {
            // wait for a rand interval between min and max
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // Choose between single spawn or group spawn
            if (useGroupSpawning)
            {
                yield return StartCoroutine(SpawnFruitGroup());
            }
            else
            {
                float currentBombProb = GetCurrentBombProbability();
                if (Random.value < currentBombProb)
                {
                    SpawnBomb();
                }
                else SpawnFruit();
            }
        }
    }

    private IEnumerator SpawnFruitGroup()
    {
        // Get dynamic values based on current score from GameManager
        int minFruits, maxFruits;
        float currentBombProb;
        GetDynamicSpawnValues(out minFruits, out maxFruits, out currentBombProb);

        int fruitsToSpawn = Random.Range(minFruits, maxFruits + 1);

        // spawn fruits in the group
        for (int i = 0; i < fruitsToSpawn; i++)
        {
            SpawnFruit();
            if (i < fruitsToSpawn - 1) // Don't wait after the last fruit
            {
                yield return new WaitForSeconds(groupSpawnDelay);
            }
        }

        // Check if we should spawn a bomb
        if (Random.value < currentBombProb)
        {
            yield return new WaitForSeconds(groupSpawnDelay);
            SpawnBomb();
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

        GameObject selectedFruit = WholeFruits[Random.Range(0, WholeFruits.Length-1)]; // not including bomb

        GameObject fruit = Instantiate(selectedFruit, randomSpawnPoint.position, transform.rotation);
        Rigidbody rb = fruit.GetComponent<Rigidbody>();

        // adjust fruits gravity
        if (rb != null)
        {
            rb.angularVelocity = GetRandAngVel();

            rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-0.2f, 0.2f), 3f, 0f);
        }
    }

    public void SpawnBomb()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject selectedBomb = WholeFruits[13];
        GameObject bomb = Instantiate(selectedBomb, randomSpawnPoint.position, transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        // Apply same physics as fruits
        if (rb != null)
        {
            rb.angularVelocity = GetRandAngVel();
            rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-0.2f, 0.2f), 3f, 0f);
        }
    }

    // Method to calculate dynamic spawn values based on current score from GameManager
    private void GetDynamicSpawnValues(out int minFruits, out int maxFruits, out float currentBombProb)
    {
        if (useDynamicDifficulty && GameManager.Instance != null && scoreThreshold > 0)
        {
            // Get current score from GameManager
            int currentScore = GameManager.Instance.score;

            // Calculate difficulty progress (0.0 to 1.0)
            float difficultyProgress = Mathf.Clamp01((float)currentScore / scoreThreshold);

            // Interpolate fruit counts
            minFruits = Mathf.RoundToInt(Mathf.Lerp(baseMinFruits, maxMinFruits, difficultyProgress));
            maxFruits = Mathf.RoundToInt(Mathf.Lerp(baseMaxFruits, maxMaxFruits, difficultyProgress));

            // Interpolate bomb probability
            currentBombProb = Mathf.Lerp(baseBombProbability, maxBombProbability, difficultyProgress);
        }
        else
        {
            // Use static values
            minFruits = minFruitsPerGroup;
            maxFruits = maxFruitsPerGroup;
            currentBombProb = bombProbability;
        }
    }

    // Get current bomb probability for single spawning
    private float GetCurrentBombProbability()
    {
        if (useDynamicDifficulty && GameManager.Instance != null && scoreThreshold > 0)
        {
            int currentScore = GameManager.Instance.score;
            float difficultyProgress = Mathf.Clamp01((float)currentScore / scoreThreshold);
            return Mathf.Lerp(baseBombProbability, maxBombProbability, difficultyProgress);
        }
        return bombProbability;
    }

    // Public method to get current difficulty info (for debugging or UI)
    public string GetDifficultyInfo()
    {
        int minFruits, maxFruits;
        float currentBombProb;
        GetDynamicSpawnValues(out minFruits, out maxFruits, out currentBombProb);

        int currentScore = GameManager.Instance != null ? GameManager.Instance.score : 0;
        float progress = useDynamicDifficulty ? Mathf.Clamp01((float)currentScore / scoreThreshold) : 0f;

        return $"Score: {currentScore} | Difficulty: {progress:P0} | Fruits: {minFruits}-{maxFruits} | Bomb Chance: {currentBombProb:P0}";
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
        /*if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnFruit();
        }*/

    }
}