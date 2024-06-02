using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // List of card prefabs
    public List<GameObject> cardPrefabs;

    // List of card positions
    public List<Transform> cardPositions;

    // List to store the instantiated cards
    private List<Card> cards = new List<Card>();

    // Cards that are currently flipped up
    private List<Card> flippedCards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
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

        if (flippedCards.Count >= 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        while (flippedCards.Count >= 2)
        {
            Card card1 = flippedCards[0];
            Card card2 = flippedCards[1];

            if (card1.faceSprite == card2.faceSprite)
            {
                // It's a match! Destroy both cards
                Destroy(card1.gameObject);
                Destroy(card2.gameObject);
            }
            else
            {
                // Not a match, flip both cards back down
                card1.FlipCardDown();
                card2.FlipCardDown();
            }

            // Remove the cards from the flippedCards list
            flippedCards.Remove(card1);
            flippedCards.Remove(card2);
        }
    }
}