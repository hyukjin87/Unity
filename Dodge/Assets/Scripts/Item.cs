using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemTypeMin = 0;
    public int itemTypeMax = 5;

    private int itemType;
    private ItemSpawn itemSpawn;

    // Start is called before the first frame update
    void Start()
    {
        itemSpawn = FindObjectOfType<ItemSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            // Import PlayerController components from opponent's game object
            PlayerController playerController = other.GetComponent<PlayerController>();
            // If you succeeded in importing the Player Controller component from the other
            if (playerController != null)
            {
                itemType = Random.Range(itemTypeMin, itemTypeMax);
                switch(itemType)
                {
                    case 0:
                        // Show item get UI with message
                        playerController.ShowItemGet("Speed UP!");
                        // Increase player speed
                        playerController.speed += 8f;
                        break;
                    case 1:
                        // Show item get UI with message
                        playerController.ShowItemGet("Scale UP!");
                        // Increase player scale
                        playerController.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
                        // Increase player speed
                        playerController.speed += 8f;
                        break;
                    case 2:
                        // Show item get UI with message
                        playerController.ShowItemGet("Rotate Ground!");
                        // Find and get the Rotator component on the "Level" object
                        Rotator rotator = GameObject.Find("Level").GetComponent<Rotator>();
                        // Enable ground rotation
                        rotator.isRotate = true;
                        // Increase ground rotation speed
                        rotator.rotationSpeed += 30f;
                        break;
                    case 3:
                        // Show item get UI with message
                        playerController.ShowItemGet("Bullet Speed UP!");
                        // Increase bullet speed
                        Bullet.ChangeBulletSpeed(Bullet.speed + 8f);
                        break;
                    case 4:
                        // Show item get UI with message
                        playerController.ShowItemGet("Spawn New Bullet!");
                        // Spawn a new bullet
                        itemSpawn.NewBulletSpawn();
                        break;
                    case 5:
                        // Show item get UI with message
                        playerController.ShowItemGet("Scale Down!");
                        // If player scale is greater than 1.0, Decrease player scale and speed
                        if (playerController.transform.localScale.x > 1.0f)
                        {
                            playerController.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f);
                            playerController.speed -= 8f;
                        }                        
                        break;

                }
                // Destroy the item object
                Destroy(gameObject);
            }
        }
    }
}
