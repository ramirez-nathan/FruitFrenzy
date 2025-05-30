// using UnityEngine;

// public class ClaudeFruitSpawner : MonoBehaviour
// {
//     public GameObject Fruit;
//     public Vector3 velocityToSet;

//     [Header("Multiple Spawn Areas")]
//     public Collider[] spawnAreaColliders; // Array of spawn area colliders
//     public float[] spawnAreaWeights; // Optional: weights for each spawn area (higher = more likely)
//     public int maxSpawnAttempts = 30; // Safety limit for spawn attempts

//     [Header("Semi-Ring Settings (for CapsuleCollider)")]
//     public float innerRadius = 1f; // Inner radius to create the "ring" effect
//     public bool useSemiRingLogic = true; // Enable semi-ring spawning

//     // Current selected spawn area (for debugging/visualization)
//     private Collider currentSpawnArea;

//     void Start()
//     {
//         // If no spawn areas are assigned, try to find them by tag
//         if (spawnAreaColliders == null || spawnAreaColliders.Length == 0)
//         {
//             GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag("SpawnArea");
//             spawnAreaColliders = new Collider[spawnAreas.Length];

//             for (int i = 0; i < spawnAreas.Length; i++)
//             {
//                 spawnAreaColliders[i] = spawnAreas[i].GetComponent<Collider>();
//             }
//         }
//     }

//     public void SpawnFruit()
//     {
//         Vector3 spawnPos = GetRandSpawnPos();
//         Quaternion spawnRot = GetRandSpawnRot();

//         GameObject fruit = Instantiate(Fruit, spawnPos, spawnRot);
//         Rigidbody rb = fruit.GetComponent<Rigidbody>();
//         if (rb != null)
//         {
//             rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-1f, 1f), 6f, -.3f);
//             rb.angularVelocity = GetRandAngVel();
//         }
//     }

//     public Vector3 GetRandAngVel()
//     {
//         float x = Random.Range(-3f, 3f);
//         float y = Random.Range(-3f, 3f);
//         float z = Random.Range(-3f, 3f);
//         return new Vector3(x, y, z);
//     }

//     public Vector3 GetRandSpawnPos()
//     {
//         // Check if we have any spawn areas defined
//         if (spawnAreaColliders == null || spawnAreaColliders.Length == 0)
//         {
//             // Use semi-circle spawn pattern as fallback
//             return GetSemiCircleSpawnPos();
//         }

//         // Randomly select a spawn area
//         Collider selectedSpawnArea = GetRandomSpawnArea();
//         currentSpawnArea = selectedSpawnArea; // Store for debugging/visualization

//         if (selectedSpawnArea == null)
//         {
//             return GetSemiCircleSpawnPos();
//         }

//         // Get random point within the selected spawn area
//         return GetRandomPointInSpawnArea(selectedSpawnArea);
//     }

//     // Randomly select one of the available spawn areas
//     private Collider GetRandomSpawnArea()
//     {
//         // Filter out null colliders
//         Collider[] validColliders = System.Array.FindAll(spawnAreaColliders, collider => collider != null);

//         if (validColliders.Length == 0)
//             return null;

//         // If we have weights defined, use weighted selection
//         if (spawnAreaWeights != null && spawnAreaWeights.Length >= validColliders.Length)
//         {
//             return GetWeightedRandomSpawnArea(validColliders);
//         }
//         else
//         {
//             // Standard random selection
//             int randomIndex = Random.Range(0, validColliders.Length);
//             return validColliders[randomIndex];
//         }
//     }

//     // Select spawn area based on weights
//     private Collider GetWeightedRandomSpawnArea(Collider[] validColliders)
//     {
//         float totalWeight = 0f;

//         // Calculate total weight
//         for (int i = 0; i < validColliders.Length; i++)
//         {
//             // Find the original index of this collider
//             int originalIndex = System.Array.IndexOf(spawnAreaColliders, validColliders[i]);
//             if (originalIndex >= 0 && originalIndex < spawnAreaWeights.Length)
//             {
//                 totalWeight += spawnAreaWeights[originalIndex];
//             }
//         }

//         // Generate random value
//         float randomValue = Random.Range(0f, totalWeight);
//         float currentWeight = 0f;

//         // Select based on weight
//         for (int i = 0; i < validColliders.Length; i++)
//         {
//             int originalIndex = System.Array.IndexOf(spawnAreaColliders, validColliders[i]);
//             if (originalIndex >= 0 && originalIndex < spawnAreaWeights.Length)
//             {
//                 currentWeight += spawnAreaWeights[originalIndex];
//                 if (randomValue <= currentWeight)
//                 {
//                     return validColliders[i];
//                 }
//             }
//         }

//         // Fallback to last valid collider
//         return validColliders[validColliders.Length - 1];
//     }

//     // Public method to spawn fruit in a specific area (optional feature)
//     public void SpawnFruitInArea(int areaIndex)
//     {
//         if (spawnAreaColliders != null && areaIndex >= 0 && areaIndex < spawnAreaColliders.Length && spawnAreaColliders[areaIndex] != null)
//         {
//             Vector3 spawnPos = GetRandomPointInSpawnArea(spawnAreaColliders[areaIndex]);
//             Quaternion spawnRot = GetRandSpawnRot();

//             GameObject fruit = Instantiate(Fruit, spawnPos, spawnRot);
//             Rigidbody rb = fruit.GetComponent<Rigidbody>();
//             if (rb != null)
//             {
//                 rb.linearVelocity = velocityToSet.magnitude > 0 ? velocityToSet : new Vector3(Random.Range(-1f, 1f), 6f, -.3f);
//                 rb.angularVelocity = GetRandAngVel();
//             }
//         }
//     }

//     // Get a random point within a specific spawn area
//     private Vector3 GetRandomPointInSpawnArea(Collider spawnArea)
//     {
//         Bounds bounds = spawnArea.bounds;
//         Vector3 randomPoint = Vector3.zero;
//         Vector3 centerPoint = new Vector3(bounds.center.x, bounds.center.y, bounds.center.z);

//         for (int i = 0; i < maxSpawnAttempts; i++)
//         {
//             // Generate random point within bounds
//             randomPoint = new Vector3(
//                 Random.Range(bounds.min.x, bounds.max.x),
//                 bounds.center.y, // Keep Y at the center of the spawn area
//                 Random.Range(bounds.min.z, bounds.max.z)
//             );

//             // Check if the point is actually inside the collider
//             if (spawnArea.bounds.Contains(randomPoint))
//             {
//                 // For semi-ring logic with CapsuleCollider
//                 if (useSemiRingLogic && spawnArea is CapsuleCollider)
//                 {
//                     // Calculate distance from center (ignoring Y axis for 2D distance)
//                     Vector3 flatRandomPoint = new Vector3(randomPoint.x, centerPoint.y, randomPoint.z);
//                     Vector3 flatCenterPoint = new Vector3(centerPoint.x, centerPoint.y, centerPoint.z);
//                     float distanceFromCenter = Vector3.Distance(flatRandomPoint, flatCenterPoint);

//                     // Only spawn if outside the inner radius (creating the ring effect)
//                     if (distanceFromCenter >= innerRadius)
//                     {
//                         // Additional check for semi-circle (only spawn in front)
//                         Vector3 directionFromCenter = (flatRandomPoint - flatCenterPoint).normalized;
//                         float dotProduct = Vector3.Dot(directionFromCenter, transform.forward);

//                         // Spawn only if the point is in front (dot product > 0)
//                         if (dotProduct > -0.3f) // Slightly behind to in front
//                         {
//                             return randomPoint;
//                         }
//                     }
//                 }
//                 else
//                 {
//                     // Standard collider bounds check without semi-ring logic
//                     return randomPoint;
//                 }
//             }
//         }

//         // If we couldn't find a valid point, return a fallback position
//         return GetSemiCircleSpawnPos();
//     }

//     // Alternative: Semi-circle spawn pattern
//     public Vector3 GetSemiCircleSpawnPos()
//     {
//         // Semi-circle parameters
//         float minRadius = 1f;
//         float maxRadius = 3f;
//         float minAngle = -90f; // Left side
//         float maxAngle = 90f;  // Right side

//         // Get random angle and radius
//         float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
//         float radius = Random.Range(minRadius, maxRadius);

//         // Calculate position relative to this spawner
//         float x = Mathf.Sin(angle) * radius;
//         float z = Mathf.Cos(angle) * radius; // Positive Z is "forward"

//         return transform.position + new Vector3(x, 0, z);
//     }

//     public Quaternion GetRandSpawnRot()
//     {
//         float x = Random.Range(0f, 360f);
//         float y = Random.Range(0f, 360f);
//         float z = Random.Range(0f, 360f);
//         return Quaternion.Euler(x, y, z);
//     }

//     void Update()
//     {
//         if (OVRInput.GetDown(OVRInput.Button.One))
//         {
//             SpawnFruit();
//         }
//     }

//     // Visualize the spawn areas in the Scene view
//     void OnDrawGizmosSelected()
//     {
//         // Draw all spawn areas
//         if (spawnAreaColliders != null)
//         {
//             for (int i = 0; i < spawnAreaColliders.Length; i++)
//             {
//                 if (spawnAreaColliders[i] != null)
//                 {
//                     Gizmos.color = Color.cyan;
//                     Vector3 center = spawnAreaColliders[i].bounds.center;
//                     Vector3 size = spawnAreaColliders[i].bounds.size;

//                     // Draw spawn area bounds
//                     Gizmos.DrawWireCube(center, size);

//                     // Draw area number
//                     UnityEditor.Handles.Label(center + Vector3.up * 0.5f, $"Area {i + 1}");

//                     // If using semi-ring logic, draw additional visualization
//                     if (useSemiRingLogic && spawnAreaColliders[i] is CapsuleCollider)
//                     {
//                         // Draw the outer boundary
//                         Gizmos.color = Color.green;
//                         Gizmos.DrawWireSphere(center, spawnAreaColliders[i].bounds.size.x * 0.5f);

//                         // Draw the inner boundary (exclusion zone)
//                         Gizmos.color = Color.red;
//                         Gizmos.DrawWireSphere(center, innerRadius);
//                     }
//                 }
//             }
//         }

//         // Highlight the currently selected spawn area during runtime
//         if (Application.isPlaying && currentSpawnArea != null)
//         {
//             Gizmos.color = Color.yellow;
//             Vector3 center = currentSpawnArea.bounds.center;
//             Vector3 size = currentSpawnArea.bounds.size;
//             Gizmos.DrawWireCube(center, size);
//         }

//         // Draw forward direction
//         Gizmos.color = Color.blue;
//         Gizmos.DrawRay(transform.position, transform.forward * 2f);
//     }
// }
