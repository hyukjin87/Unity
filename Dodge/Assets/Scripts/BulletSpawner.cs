using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the time since the last spawn to zero
        timeAfterSpawn = 0f;
        // Randomize the initial spawn rate
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // Find the transform of the object with the PlayerController component as the target
        target = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        // Increment the time since the last spawn by the elapsed time in the frame
        timeAfterSpawn += Time.deltaTime;
        // If the time since the last spawn is greater than or equal to the spawn rate
        if (timeAfterSpawn>=spawnRate)
        {
            // Reset the time since the last spawn to zero
            timeAfterSpawn = 0f;
            // Instantiate a bullet prefab at the spawner's position and rotation
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            // Rotate the spawned bullet to face the target
            bullet.transform.LookAt(target);
            // Randomize the next spawn rate for the bullets
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
