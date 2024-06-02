using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    // List of card prefabs
    public List<GameObject> cardPrefabs;

    // List of card positions
    public List<Transform> cardPositions;

    // Text objects to display the number of matches and turns remaining
    public Text matchesText, turnsText;

    // Number of matches required to win and turns left to play
    public int matchesRequired, turnsLeft;

    // List to store the instantiated cards
    private List<Card> cards = new List<Card>();

    // Cards that are currently flipped up
    private List<Card> flippedCards = new List<Card>();

    // Number of matches made
    private int matchesMade = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Update the text objects
        matchesText.text = "Matches: " + matchesMade;
        turnsText.text = "Turns Left: " + turnsLeft;

        // Create a temporary list of card prefabs
        List<GameObject> tempPrefabs = new List<GameObject>(cardPrefabs);

        // Determine the number of pairs to instantiate based on the number of positions
        int numPairs = cardPositions.Count / 2;

        // Randomize the instantiation of the cards
        for (int i = 0; i < numPairs; i++)
        {
            // Select a random card prefab
            int randomIndex = Random.Range(0, tempPrefabs.Count);
            GameObject cardPrefab = tempPrefabs[randomIndex];

            // Remove the selected prefab from the temporary list
            tempPrefabs.RemoveAt(randomIndex);

            for (int j = 0; j < 2; j++) // Place each card twice
            {
                // Select a random position
                randomIndex = Random.Range(0, cardPositions.Count);
                Transform randomPosition = cardPositions[randomIndex];

                // Remove the selected position from the list
                cardPositions.RemoveAt(randomIndex);

                // Instantiate the card at the selected position
                GameObject cardObject = Instantiate(cardPrefab, randomPosition.position, Quaternion.identity);

                // Get the Card component of the instantiated card
                Card card = cardObject.GetComponent<Card>();

                // Set the CardManager of the Card
                card.cardManager = this;

                // Add the Card component of the instantiated card to the cards list
                cards.Add(card);
            }
        }
    }

    public void CardFlipped(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        Card card1 = flippedCards[0];
        Card card2 = flippedCards[1];

        if (card1.faceSprite == card2.faceSprite)
        {
            // It's a match! Destroy both cards
            Destroy(card1.gameObject);
            Destroy(card2.gameObject);

            // Increment the number of matches made
            matchesMade++;
            matchesText.text = "Matches: " + matchesMade;

            // Check if the required number of matches has been made
            if (matchesMade == matchesRequired)
            {
                Debug.Log("Level won!");
            }
        }
        else
        {
            // Not a match, flip both cards back down
            card1.FlipCardDown();
            card2.FlipCardDown();
        }

        // Decrement the number of turns left
        turnsLeft--;
        turnsText.text = "Turns Left: " + turnsLeft;

        // Check if there are no more turns left
        if (turnsLeft == 0)
        {
            Debug.Log("Level lost!");
        }

        // Remove the cards from the flippedCards list
        flippedCards.Remove(card1);
        flippedCards.Remove(card2);
    }
}