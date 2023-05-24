using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public GameObject highScorePanel;
    public ScoreKeeper gamePlayTime;
    public Text hardText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called when the play again button is clicked.
    public void PlayAgain()
    {
        // Deactivate the high score panel.
        highScorePanel.SetActive(false);
        // Reset the game scene.
        ResetScene();
    }

    // This method resets the game scene.
    public void ResetScene()
    {
        // Reset the game play time.
        gamePlayTime.playTime = 0;
        // Destroy all the cards in the scene.
        UpdateSprite[] cards = FindObjectsOfType<UpdateSprite>();
        foreach (UpdateSprite card in cards)
        {
            Destroy(card.gameObject);
        }
        // Clear the top value of all the selectables in the scene.
        ClearTopValue();
        // Start playing the cards again.
        FindObjectOfType<Solitaire>().PlayCards();
    }

    // This method clears the top value of all the selectables in the scene.
    void ClearTopValue()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        foreach (Selectable selectable in selectables)
        {
            if (selectable.CompareTag("Top"))
            {
                selectable.suit = null;
                selectable.value = 0;
            }
        }
    }

    // This method is called when the hard mode option is clicked.
    public void isHardOption()
    {
        // Get the Solitaire script instance.
        Solitaire option = FindObjectOfType<Solitaire>();
        // If the hard option is false, set it to true and update the text.
        if (option.HardOption==false)
        {
            hardText.text = "NORMAL MODE";
            option.HardOption = true;
        }
        // If the hard option is true, set it to false and update the text.
        else
        {
            hardText.text = "HARD MODE";
            option.HardOption = false;
        }
        // Reset the game scene.
        ResetScene();
    }
}
