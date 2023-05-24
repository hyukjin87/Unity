using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;

    public static string[] suit = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<string>[] bottoms;
    public List<string>[] tops;

    public List<string> tripsOnDisplay = new List<string>();
    public List<List<string>> deckTrips = new List<List<string>>();

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    public List<string> deck;
    public List<string> discardPile = new List<string>();

    private int deckLocation;
    private int trips;
    private int tripsRemainder;
    public bool HardOption { get; set; }
    public static string colorRed = "R";
    public static string colorBlack = "B";
    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is responsible for playing the game of Solitaire.
    public void PlayCards()
    {
        // Clear all the cards from the bottoms list.
        foreach (List<string>list in bottoms)
        {
            list.Clear();
        }
        // Generate a new deck of cards.
        deck = GenerateDeck();
        // Shuffle the deck.
        Shuffle(deck);

        // Sort the cards for Solitaire.
        SolitaireSort();
        // Deal the cards for Solitaire.
        StartCoroutine(SolitaireDeal());
        // Sort the deck into trips.
        SortDeckIntoTrips();
    }

    // This method generates a new deck of cards by combining each suit with each value.
    public static List<string> GenerateDeck()
    {
        // Create a new list to hold the cards in the deck.
        List<string> newDeck = new List<string>();
        // Loop through each suit in the suit array.
        foreach (string s in suit)
        {
            // Loop through each value in the values array.
            foreach (string v in values)
            {
                // Combine the suit and value to create a new card and add it to the deck.
                newDeck.Add(s + v);
            }
        }

        //List<string> newDeck = new List<string>();
        //foreach (string s in suit)
        //{
        //    if (s == "C" || s == "S")
        //    {
        //        foreach (string v in values)
        //        {
        //            newDeck.Add(s + v + colorBlack);
        //        }
        //    }
        //    else
        //    {
        //        foreach(string v2 in values)
        //        {
        //            newDeck.Add(s + v2 + colorRed);
        //        }
        //    }
        //}
        
        // Return the completed deck of cards.
        return newDeck;
    }

    // This method shuffles a list of generic type T using the Fisher-Yates shuffle algorithm.
    void Shuffle<T>(List<T> list)
    {
        // Create a new instance of the Random class.
        System.Random random = new System.Random();
        // Get the number of elements in the list.
        int n = list.Count;
        // Loop through the list from the last element to the second element.
        while (n>1)
        {
            // Generate a random integer between 0 and n-1.
            int k = random.Next(n);
            // Decrement n.
            n--;
            // Swap the element at index k with the element at index n.
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    // This IEnumerator function deals the cards for the Solitaire game
    IEnumerator SolitaireDeal()
    {
        // Loop through the bottom positions to deal the cards
        for (int i = 0;i<7;i++)
        {
            // Set the initial y and z offsets for the cards
            float yOffset = 0;
            float zOffset = 0.02f;

            // Loop through the cards in the current bottom position
            foreach (string card in bottoms[i])
            {
                // Wait for a short time before dealing the next card
                yield return new WaitForSeconds(0.01f);

                // Instantiate a new card object at the current bottom position
                GameObject newCard = Instantiate(cardPrefab, 
                    new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), 
                    Quaternion.identity, bottomPos[i].transform);

                // Set the name of the new card object to the current card
                newCard.name = card;
                // Set the row property of the Selectable component
                // on the new card object to the current row
                newCard.GetComponent<Selectable>().row = i;

                // If the current card is the last card in the bottom position, set it to face up
                if (card == bottoms[i][bottoms[i].Count-1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }

                // Increment the y and z offsets for the next card
                yOffset = yOffset + 0.2f;
                zOffset = zOffset + 0.02f;

                // Add the current card to the discard pile
                discardPile.Add(card);
            }
        }

        // Remove any cards from the deck that are in the discard pile
        foreach (string card in discardPile)
        {
            if(deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        // Clear the discard pile
        discardPile.Clear();
    }

    // This method implements the Solitaire sorting algorithm.
    void SolitaireSort()
    {
        // Loop through the first 7 piles.
        for (int i=0;i<7;i++)
        {
            // Loop through the remaining piles.
            for (int j=i;j<7;j++)
            {
                // Add the last card in the deck to the current pile.
                bottoms[j].Add(deck.Last<string>());
                // Remove the last card from the deck.
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    public void SortDeckIntoTrips()
    {
        // If the HardOption is enabled, divide the deck into trips of 3 cards each
        if (HardOption == true)
        {
            // Calculate the number of trips and the remainder of cards
            trips = deck.Count / 3;
            tripsRemainder = deck.Count % 3;
            deckTrips.Clear();

            // Loop through each trip
            int modifier = 0;
            for (int i = 0; i < trips; i++)
            {
                // Create a new list to hold the cards in the trip
                List<string> myTrips = new List<string>();
                // Loop through each card in the trip and add it to the list
                for (int j = 0; j < 3; j++)
                {
                    myTrips.Add(deck[j + modifier]);
                }
                // Add the list of cards to the deckTrips list
                deckTrips.Add(myTrips);
                // Increase the modifier to move to the next set of 3 cards
                modifier = modifier + 3;
            }

            // If there are any remaining cards, add them to a new trip
            if (tripsRemainder != 0)
            {
                // Create a new list to hold the remaining cards
                List<string> myRemainders = new List<string>();
                modifier = 0;
                // Loop through each remaining card and add it to the list
                for (int k = 0; k < tripsRemainder; k++)
                {
                    myRemainders.Add(deck[deck.Count - tripsRemainder + modifier]);
                    modifier++;
                }
                // Add the list of remaining cards to the deckTrips list
                deckTrips.Add(myRemainders);
                // Increase the number of trips to include the remaining cards
                trips++;
            }
            // Reset the deckLocation to 0
            deckLocation = 0;
        }
        // If the HardOption is not enabled, each card is its own trip
        else
        {
            // Set the number of trips to the number of cards in the deck
            trips = deck.Count;
            deckTrips.Clear();

            // Loop through each card in the deck
            int modifier = 0;
            for (int i = 0; i < trips; i++)
            {
                // Create a new list to hold the card
                List<string> myTrips = new List<string>();
                // Add the card to the list
                myTrips.Add(deck[modifier]);
                // Add the list of the card to the deckTrips list
                deckTrips.Add(myTrips);
                // Increase the modifier to move to the next card
                modifier++;
            }
            // Reset the deckLocation to 0
            deckLocation = 0;           
        }       
    }

    public void DealFromDeck()
    {
        // If the game is set to hard mode
        if (HardOption == true)
        {
            // Loop through each child object of the deckButton object
            foreach (Transform child in deckButton.transform)
            {
                // If the child object has the "Card" tag
                if (child.CompareTag("Card"))
                {
                    // Remove the child object's name from the deck list
                    deck.Remove(child.name);
                    // Add the child object's name to the discard pile list
                    discardPile.Add(child.name);
                    // Destroy the child object
                    Destroy(child.gameObject);                    
                }
            }
            // If the deck location is less than the number of trips
            if (deckLocation < trips)
            {
                // Clear the tripsOnDisplay list
                tripsOnDisplay.Clear();
                // Set the initial x and z offsets for the new cards
                float xOffset = -1.1f;
                float zOffset = -0.2f;

                // Loop through each card in the current trip
                foreach (string card in deckTrips[deckLocation])
                {
                    // Instantiate a new card object at the specified position and rotation
                    GameObject newTopCard = Instantiate(cardPrefab,
                        new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset),
                        Quaternion.identity, deckButton.transform);

                    // Update the x and z offsets for the next card
                    xOffset = xOffset + 0.2f;
                    zOffset = zOffset - 0.2f;
                    // Set the name of the new card object
                    newTopCard.name = card;
                    // Add the new card's name to the tripsOnDisplay list
                    tripsOnDisplay.Add(card);
                    // Set the new card to face up
                    newTopCard.GetComponent<Selectable>().faceUp = true;
                    // Set the new card to be in the deck pile
                    newTopCard.GetComponent<Selectable>().inDeckPile = true;
                }
                // Increment the deck location counter
                deckLocation++;
            }
            else
            {
                // If the deck location is greater than or equal to the number of trips,
                // restack the deck
                RestackTopDeck();
            }
        }
        // If the game is not set to hard mode
        else
        {
            // Loop through each child object of the deckButton object
            foreach (Transform child in deckButton.transform)
            {
                // If the child object has the "Card" tag
                if (child.CompareTag("Card"))
                {
                    // Remove the child object's name from the deck list
                    deck.Remove(child.name);
                    // Add the child object's name to the discard pile list
                    discardPile.Add(child.name);
                    // Destroy the child object
                    Destroy(child.gameObject);
                }
            }

            // If the deck location is less than the number of trips
            if (deckLocation < trips)
            {
                // Clear the tripsOnDisplay list
                tripsOnDisplay.Clear();
                // Set the initial x offset for the new cards
                float xOffset = -0.8f;

                // Loop through each card in the current trip
                foreach (string card in deckTrips[deckLocation])
                {
                    // Instantiate a new card object at the specified position and rotation
                    GameObject newTopCard = Instantiate(cardPrefab,
                        new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z),
                        Quaternion.identity, deckButton.transform);

                    // Update the x offset for the next card
                    xOffset = xOffset + 0.2f;
                    // Set the name of the new card object
                    newTopCard.name = card;
                    // Add the new card's name to the tripsOnDisplay list
                    tripsOnDisplay.Add(card);
                    // Set the new card to face up
                    newTopCard.GetComponent<Selectable>().faceUp = true;
                    // Set the new card to be in the deck pile
                    newTopCard.GetComponent<Selectable>().inDeckPile = true;
                }
                // Increment the deck location counter
                deckLocation++;
            }
            else
            {
                // If the deck location is greater than or equal to the number of trips,
                // restack the deck
                RestackTopDeck();
            }
        }
        
    }
    // This method restacks the deck by clearing it,
    // adding all the cards from the discard pile, and then sorting the deck into trips.
    void RestackTopDeck()
    {
        // Clear the deck
        deck.Clear();
        // Loop through each card in the discard pile
        foreach (string card in discardPile)
        {
            // Add the card to the deck
            deck.Add(card);
        }
        // Clear the discard pile
        discardPile.Clear();
        // Sort the deck into trips
        SortDeckIntoTrips();
    }  
}
