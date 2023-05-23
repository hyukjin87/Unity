using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject bulletPrefab;
    public float spawnTime = 5f;
    public float positionMin = -24f;
    public float positionMax = 24f;
    public Transform levelTransform;

    private float afterSpawnTime;
    private float spawnPositionX;
    private float spawnPositionY = 0.5f;
    private float spawnPositionZ;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the time elapsed after the last spawn to zero
        afterSpawnTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the time elapsed after the last spawn
        afterSpawnTime += Time.deltaTime;

        if(afterSpawnTime > spawnTime)
        {
            // Reset the time elapsed after the last spawn to zero
            afterSpawnTime = 0f;
            // Generate random X and Z coordinates within the specified range
            spawnPositionX = Random.Range(positionMin, positionMax);
            spawnPositionZ = Random.Range(positionMin, positionMax);

            // Instantiate the item prefab at the random position with the default rotation
            GameObject item = Instantiate(itemPrefab, new Vector3(spawnPositionX, spawnPositionY, spawnPositionZ), transform.rotation);
            // Set the level transform as the parent of the spawned item
            item.transform.SetParent(levelTransform);
        }
    }

    public void NewBulletSpawn()
    {
        // Generate random X and Z coordinates within the specified range
        spawnPositionX = Random.Range(positionMin, positionMax);
        spawnPositionZ = Random.Range(positionMin, positionMax);

        // Instantiate the bullet prefab at the random position with the default rotation
        GameObject bulletSpawn = Instantiate(bulletPrefab, new Vector3(spawnPositionX, spawnPositionY, spawnPositionZ), transform.rotation);
        // Set the level transform as the parent of the spawned bullet
        bulletSpawn.transform.SetParent(levelTransform);
    }
}
