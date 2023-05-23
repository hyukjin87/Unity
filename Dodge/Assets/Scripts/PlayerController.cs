using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    public float speed = 8f;

    public GameObject itemGet;
    public TextMesh itemGetText;
    void Start()
    {
        // Get the Rigidbody component attached to the player
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detect and store input values of horizontal and vertical axes
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // Determine actual travel speed using input and travel speed
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        // Create a new velocity vector
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // Assign the new velocity to the player's Rigidbody
        playerRigidbody.velocity = newVelocity;

        // The commented code below uses AddForce instead of directly assigning velocity
        // It's an alternative approach for player movement using physics forces

        //if(Input.GetKey(KeyCode.UpArrow)==true)
        //{
        //    playerRigidbody.AddForce(0f, 0f, speed);
        //}
        //if(Input.GetKey(KeyCode.DownArrow)==true)
        //{
        //    playerRigidbody.AddForce(0f, 0f, -speed);
        //}
        //if(Input.GetKey(KeyCode.RightArrow)==true)
        //{
        //    playerRigidbody.AddForce(speed, 0f, 0f);
        //}
        //if (Input.GetKey(KeyCode.LeftArrow) == true)
        //{
        //    playerRigidbody.AddForce(-speed, 0f, 0f);
        //}
    }

    public void Die()
    {
        // Deactivate the player object
        gameObject.SetActive(false);
        // Find the GameManager in the scene
        GameManager gameManager = FindObjectOfType<GameManager>();
        // Call the EndGame method in the GameManager
        gameManager.EndGame();
    }

    public void ShowItemGet(String msg)
    {
        // Activate the item get indicator
        itemGet.SetActive(true);
        // Set the item get message in the TextMesh component
        itemGetText.text = msg;
        // Start a coroutine to deactivate the item get indicator after a delay
        StartCoroutine(DeactivateItemGetAfterDelay());
    }

    private IEnumerator DeactivateItemGetAfterDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        // Deactivate the item get indicator
        itemGet.SetActive(false);
    }
}
