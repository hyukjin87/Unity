using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Selectable[] topStacks;
    public GameObject highScorePanel;
    public float playTime;
    public Text textTime;
    public Text recordTime;
    private float bestTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's play time and best time to 0.
        playTime = 0;
        bestTime = 0;
    }


    // Update is called once per frame
    void Update()
    {
        // If the player has won the game, call the Win() method.
        if (HasWon())
        {
            Win();
        }
        else
        {
            // Increment the player's play time by the amount of time that has passed since the last frame.
            playTime += Time.deltaTime;
            // Update the text that displays the player's current time.
            textTime.text = "Time : " + Mathf.Round(playTime);
        }
    }

    // Determines if the player has won the game.
    public bool HasWon()
    {
        int i = 0;
        // Loop through each top stack of cards.
        foreach (Selectable topStack in topStacks)
        {
            // Add the value of the top card in each stack to the counter variable.
            i += topStack.value;
        }
        // If the counter variable is greater than or equal to 52
        // (the total number of cards in the deck), the player has won the game.
        if (i>=52)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Called when the player has won the game.
    public void Win()
    {
        // If the player's play time is greater than their best time,
        // update their best time and update the text that displays the player's best time.
        if (playTime>bestTime)
        {
            bestTime = playTime;
            recordTime.text = "Best Record: "+ Mathf.Round(bestTime);
        }
        // Activate the high score panel.
        highScorePanel.SetActive(true);
    }
}
