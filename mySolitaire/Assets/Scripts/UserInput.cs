using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    //private float timer;
    //private float doubleClickTime = 0.3f;
    //private int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (clickCount == 1) 
        //{
        //    timer += Time.deltaTime;
        //}
        //if (clickCount == 3) 
        //{
        //    timer = 0;
        //    clickCount = 1;
        //}
        //if (timer > doubleClickTime)
        //{
        //    timer = 0;
        //    clickCount = 0;
        //}

        GetMouseClick();
    }

    // This method is used to detect mouse clicks and perform actions based on the object clicked.
    void GetMouseClick()
    {
        // Check if the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //clickCount++;

            // Get the position of the mouse click in the world space.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            // Cast a ray from the camera to the clicked position and get the object hit by the ray.
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            // Check if an object is hit by the ray.
            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Deck();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    Top(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    Bottom(hit.collider.gameObject);
                }
            }
        }
    }

    // This method represents the deck of cards.
    void Deck()
    {
        // Deal a card from the deck.
        solitaire.DealFromDeck();
        // Set the current game object to slot1.
        slot1 = this.gameObject;
    }

    // This method represents a card.
    void Card(GameObject seleced)
    {
        //print("click on card");
        //Debug.Log(seleced);

        // If the selected card is face down.
        if (!seleced.GetComponent<Selectable>().faceUp)
        {
            // If the card is not blocked.
            if (!Blocked(seleced))
            {
                // Flip the card face up.
                seleced.GetComponent<Selectable>().faceUp = true;
                // Set the current game object to slot1.
                slot1 = this.gameObject;
            }
        }
        // If the selected card is in the deck pile.
        else if (seleced.GetComponent<Selectable>().inDeckPile)
        {
            // If the card is not blocked.
            if (!Blocked(seleced))
            {
                // If the selected card is the same as slot1.
                if (slot1==seleced)
                {
                    // If the card can be auto stacked on top.
                    if (!AutoStackTop(seleced))
                    {
                        // Auto stack the card on the bottom.
                        AutoStackBottom(seleced);
                    }

                    //if (DoubleClick())
                    //{
                    //    AutoStack(seleced);
                    //}
                }
                else
                {
                    // Set the current game object to slot1.
                    slot1 = seleced;
                    // If the card can be auto stacked on the bottom.
                    if (!AutoStackTop(seleced))
                    {
                        // Auto stack the card on the bottom.
                        AutoStackBottom(seleced);
                    }
                }
            }
        }
        else
        {
            // If slot1 is the current game object.
            if (slot1 == this.gameObject)
            {
                // Set the current game object to slot1.
                slot1 = seleced;
                // Auto stack the card on top.
                AutoStackTop(seleced);
            }
            // If slot1 is not the same as the selected card.
            else if (slot1 != seleced)
            {
                // If the selected card can be stacked on top of slot1.
                if (Stackable(seleced))
                {
                    //Stack(seleced);

                    // If the card can be auto stacked on the bottom.
                    if (!AutoStackTop(seleced))
                    {
                        // Auto stack the card on the bottom.
                        AutoStackBottom(seleced);
                    }
                }
                else
                {
                    // Set the current game object to slot1.
                    slot1 = seleced;
                    // If the card can be auto stacked on the bottom.
                    if (!AutoStackTop(seleced))
                    {
                        // Auto stack the card on the bottom.
                        AutoStackBottom(seleced);
                    }
                }
            }
            // If slot1 is the same as the selected card.
            else if (slot1==seleced)
            {
                // If the card can be auto stacked on the bottom.
                if (!AutoStackTop(seleced))
                {
                    // Auto stack the card on the bottom.
                    AutoStackBottom(seleced);
                }
                //if (DoubleClick())
                //{
                //    AutoStack(seleced);
                //}
            }
        }        
    }

    // This method is called when the top slot is clicked
    void Top(GameObject selected)
    {
        //print("Click on the Top");

        // Check if the slot contains a card
        if (slot1.CompareTag("Card"))
        {
            // Check if the card in the slot is an Ace
            if (slot1.GetComponent<Selectable>().value == 1) 
            {
                //Stack(selected);

                // If the card is an Ace, try to stack the selected card on top of it
                // If it cannot be stacked on top, try to stack it on the bottom
                if (!AutoStackTop(selected))
                {
                    AutoStackBottom(selected);
                }

            }
        }
    }

    // This method is called when the bottom slot is clicked
    void Bottom(GameObject selected)
    {
        //print("Click on the Bottom");

        // Check if the slot contains a card
        if (slot1.CompareTag("Card"))
        {
            // Check if the card in the slot is a King
            if (slot1.GetComponent<Selectable>().value == 13) 
            {
                //Stack(selected);

                // If the card is a King, try to stack the selected card on top of it
                // If it cannot be stacked on top, try to stack it on the bottom
                if (!AutoStackTop(selected))
                {
                    AutoStackBottom(selected);
                }
            }
        }
    }

    // This method checks if a selected game object can be stacked
    // on top of another game object in a slot.
    bool Stackable(GameObject selected)
    {
        // Check if both the selected game object and the first slot game object are not null.
        if (slot1 != null & selected != null)
        {
            // Get the Selectable component of both game objects.
            Selectable s1 = slot1.GetComponent<Selectable>();
            Selectable s2 = selected.GetComponent<Selectable>();

            // Check if the selected game object is not in the deck pile.
            if (!s2.inDeckPile)
            {
                // Check if the selected game object is on top of the stack.
                if (s2.top)
                {
                    // Check if the suit of the first slot game object matches
                    // the suit of the selected game object or if the selected game object is an Ace.
                    if ((s1.suit == s2.suit) || (s1.value == 1 && s2.suit == null))
                    {
                        // Check if the value of the first slot game object is one less than
                        // the value of the selected game object.
                        if (s1.value == s2.value + 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Check if the value of the first slot game object is one more than
                    // the value of the selected game object.
                    if (s1.value == s2.value - 1)
                    {
                        bool card1Red = true;
                        bool card2Red = true;

                        // Check if the suit of the first slot game object is Clubs or Spades.
                        if (s1.suit == "C" || s1.suit == "S")
                        {
                            card1Red = false;
                        }

                        // Check if the suit of the selected game object is Clubs or Spades.
                        if (s2.suit == "C" || s2.suit == "S")
                        {
                            card2Red = false;
                        }

                        // Check if both game objects have the same color.
                        if (card1Red == card2Red)
                        {
                            //print("not stackable");
                            return false;
                        }
                        else
                        {
                            //print("stackable");
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    void Stack(GameObject selected)
    {
        // Get the Selectable component of slot1 and the selected GameObject
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        
        //Debug.Log(s2);
        
        float yOffset = 0.2f;

        // If the selected card is a top card or if it's not a top card
        // but the card being stacked on it is a king (value 13), set the yOffset to 0
        if (s2.top || (!s2.top && s1.value == 13)) 
        {
            yOffset = 0;
        }

        // Set the position of slot1 to be on top of the selected card with the yOffset and a slight z-offset
        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z - 0.01f);
        // Set the parent of slot1 to be the selected card
        slot1.transform.parent = selected.transform;

        // If the card being stacked was in the deck pile, remove it from the tripsOnDisplay list
        if (s1.inDeckPile)
        {
            solitaire.tripsOnDisplay.Remove(slot1.name);
        }
        // If both cards are top cards and the card being stacked on is an ace (value 1),
        // set the value and suit of the top position to null
        else if (s1.top && s2.top && s1.value == 1) 
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = null;
        }
        // If the card being stacked is a top card,
        // set the value of the top position to be one less than the value of the card being stacked
        else if (s1.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value - 1;
        }
        // If the card being stacked is not a top card, remove it from the bottoms list
        else
        {
            solitaire.bottoms[s1.row].Remove(slot1.name);
        }

        // Set the inDeckPile variable of the card being stacked to false
        s1.inDeckPile = false;
        // Set the row of the card being stacked to be the same as the row of the selected card
        s1.row = s2.row;

        // If the selected card is a top card,
        // set the value and suit of the top position to be the same as the card being stacked
        // and set the top variable of the card being stacked to true
        if (s2.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }
        // If the selected card is not a top card,
        // set the top variable of the card being stacked to false
        else
        {
            s1.top = false;
        }
        // Set slot1 to be this GameObject
        slot1 = this.gameObject;
    }

    // This method checks if a given GameObject is blocked or not.
    bool Blocked(GameObject selected)
    {
        // Get the Selectable component of the selected GameObject.
        Selectable s2 = selected.GetComponent<Selectable>();
        // If the selected GameObject is in the deck pile.
        if (s2.inDeckPile==true)
        {
            // If the name of the selected GameObject is the same as the last card
            // on the trips on display.
            if (s2.name==solitaire.tripsOnDisplay.Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // If the selected GameObject is not in the deck pile.
        else
        {
            // If the name of the selected GameObject is the same as the last card
            // on the bottom of the row.
            if (s2.name==solitaire.bottoms[s2.row].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    //bool DoubleClick()
    //{
    //    if(timer<doubleClickTime&&clickCount==2)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    // This method checks if the selected card can be stacked on top of any of the top positions.
    bool AutoStackTop(GameObject selected)
    {
        bool stackCheck = false;
        // Loop through all the top positions.
        for (int i = 0; i < solitaire.topPos.Length; i++)
        {
            // Get the Selectable component of the current top position.
            Selectable stack = solitaire.topPos[i].GetComponent<Selectable>();
            // If the selected card is an Ace and the current top position is empty, stack the card.
            if (selected.GetComponent<Selectable>().value == 1)
            {
                if (solitaire.topPos[i].GetComponent<Selectable>().value == 0)
                {
                    // Set the selected card as the first slot and stack it on the current top position.
                    slot1 = selected;
                    Stack(stack.gameObject);
                    stackCheck = true;
                    break;
                }
            }           
            else
            {
                // If the selected card is not an Ace,
                // check if it can be stacked on top of the current top position.
                if ((solitaire.topPos[i].GetComponent<Selectable>().suit == slot1.GetComponent<Selectable>().suit)
                    && (solitaire.topPos[i].GetComponent<Selectable>().value == slot1.GetComponent<Selectable>().value - 1))
                {
                    if (HasNoChildren(slot1))
                    {
                        // Set the selected card as the first slot
                        // and stack it on top of the current top position.
                        slot1 = selected;
                        // Get the name of the last card in the current top position.
                        string lastCardName = stack.suit + stack.value.ToString();
                        if (stack.value == 1)
                        {
                            lastCardName = stack.suit + "A";
                        }
                        if (stack.value == 11)
                        {
                            lastCardName = stack.suit + "J";
                        }
                        if (stack.value == 12)
                        {
                            lastCardName = stack.suit + "Q";
                        }
                        if (stack.value == 13)
                        {
                            lastCardName = stack.suit + "K";
                        }
                        // Find the last card in the current top position
                        // and stack the selected card on top of it.
                        GameObject lastCard = GameObject.Find(lastCardName);
                        Stack(lastCard);
                        stackCheck = true;
                        break;
                    }
                }                
            }       
        }
        // Return whether the selected card was successfully stacked on top of any of the top positions.
        return stackCheck;
    }

    // This method is used to automatically stack the selected card
    // to the bottom position of the solitaire game.
    void AutoStackBottom(GameObject selected)
    {
        // Check if the selected card is a King.
        if (selected.GetComponent<Selectable>().value == 13)
        {
            // Loop through all the bottom positions.
            for (int j = 0; j < solitaire.bottomPos.Length; j++)
            {
                // Check if the current bottom position is empty.
                if (solitaire.bottomPos[j].transform.childCount == 0)
                {
                    // Set the selected card as the first card in the stack and stack it
                    // to the current bottom position.
                    slot1 = selected;
                    Stack(solitaire.bottomPos[j].gameObject);
                    break;
                }
            }
        }
        else
        {
            // Loop through all the bottom positions.
            for (int k = 0; k < solitaire.bottomPos.Length; k++)
            {
                // Get the top card in the current bottom position.
                Selectable stack = solitaire.bottomPos[k].GetComponentsInChildren<Selectable>().Last();
                // Check if the top card is face up and has a value one less than the selected card.
                if (stack.faceUp == true && stack.GetComponent<Selectable>().value == slot1.GetComponent<Selectable>().value + 1)
                {
                    // Set the selected card as the first card in the stack.
                    slot1 = selected;
                    // Get the name of the last card in the stack.
                    string lastCardName2 = stack.suit + stack.value.ToString();
                    if (stack.value == 1)
                    {
                        lastCardName2 = stack.suit + "A";
                    }
                    if (stack.value == 11)
                    {
                        lastCardName2 = stack.suit + "J";
                    }
                    if (stack.value == 12)
                    {
                        lastCardName2 = stack.suit + "Q";
                    }
                    if (stack.value == 13)
                    {
                        lastCardName2 = stack.suit + "K";
                    }
                    // Find the last card in the stack.
                    GameObject lastCard2 = GameObject.Find(lastCardName2);
                    // Check if the selected card can be stacked on top of the last card in the stack.
                    if (Stackable(lastCard2) == true)
                    {
                        // Check if the last card in the stack is the same suit as the selected card
                        // and has a value one less than the selected card.
                        if (CheckLastCard(lastCard2))
                        {
                            // Stack the selected card on top of the last card in the stack.
                            Stack(lastCard2);
                            break;
                        }
                    }
                }
            }
        }
    }

    // This method checks if a given game object has no children
    bool HasNoChildren(GameObject card)
    {
        int i = 0;
        // Loop through all the children of the card
        foreach (Transform child in card.transform)
        {
            i++;
        }
        // If there are no children, return true
        if (i==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This method checks if a given card is the last card in the deck
    bool CheckLastCard(GameObject card)
    {
        int count = 0;
        // Loop through all the children of the card
        for (int i = 0; i < card.transform.childCount; i++)
        {
            count++;
        }
        // If there are no children, return true
        if (count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
