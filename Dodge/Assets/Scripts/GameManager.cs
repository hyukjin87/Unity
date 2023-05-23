using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;

    private float surviveTime;
    private bool isGameover;
    
    void Start()
    {
        // Initialize the survival time to zero
        surviveTime = 0;
        // Initialize the game over state to false
        isGameover = false;
    }

    void Update()
    {
        // If the game is not over
        if (!isGameover)
        {
            // Increment the survival time based on the elapsed time
            surviveTime += Time.deltaTime;
            // Display the updated survival time in the timeText component
            timeText.text = "Time: " + (int)surviveTime;
        }
        // If the game is over
        else
        {
            // If the "R" key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Load the "SampleScene" to restart the game
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
    public void EndGame()
    {
        // Set the game over state to true
        isGameover = true;
        // Enable the game over text game object
        gameoverText.SetActive(true);
        // Retrieve the previous best time record from PlayerPrefs
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        // If the current survival time is greater than the previous best time
        if (surviveTime>bestTime)
        {
            // Update the best time to the current survival time
            bestTime = surviveTime;
            // Save the updated best time in PlayerPrefs
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
        // Display the best time record in the recordText component
        recordText.text = "Best Time: " + (int)bestTime;
    }
}
