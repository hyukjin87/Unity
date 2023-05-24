using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire solitaire;
    private UserInput userInput;

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();

        // Loop through the deck of cards
        int i = 0;
        foreach(string card in deck)
        {
            // If the name of this card object matches the current card in the deck
            if (this.name == card)
            {
                // Set the card face sprite to the corresponding sprite in the Solitaire script
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        // Get the sprite renderer component of the card object
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Get the selectable component of the card object
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the card is face up
        if (selectable.faceUp == true) 
        {
            // Set the sprite to the card face sprite
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            // Set the sprite to the card back sprite
            spriteRenderer.sprite = cardBack;
        }

        // If the user has selected a card slot
        if (userInput.slot1)
        {
            // If this card object matches the selected card slot
            if (name == userInput.slot1.name)
            {
                // Change the color of the sprite to yellow
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                // Change the color of the sprite to white
                spriteRenderer.color = Color.white;
            }
        }           
    }
}
