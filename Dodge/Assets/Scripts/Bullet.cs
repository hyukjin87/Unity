using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float speed = 8f;
    private Rigidbody bulletRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component of the bullet game object
        bulletRigidbody = GetComponent<Rigidbody>();
        // Set the velocity of the Rigidbody to move the bullet forward with the specified speed
        bulletRigidbody.velocity = transform.forward * speed;
        // Destroy the bullet game object after 20 seconds
        Destroy(gameObject, 20f);
    }

    // OnTriggerEnter is called when the bullet collides with another collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "Player" tag
        if (other.tag=="Player")
        {
            // Get the PlayerController component from the collided object
            PlayerController playerController = other.GetComponent<PlayerController>();
            // If the PlayerController component is found
            if (playerController!=null)
            {
                // Call the Die() method of the playerController to eliminate the player
                playerController.Die();
            }
        }
        // Check if the collided object has the "Wall" tag
        else if (other.tag=="Wall")
        {
            // Destroy the bullet game object
            Destroy(gameObject);
        }
    }

    // Function to change the speed of existing bullet instances
    public static void ChangeBulletSpeed(float newSpeed)
    {
        // Update the speed of the bullet
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
