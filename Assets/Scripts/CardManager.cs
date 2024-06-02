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

    // Start is called before the first frame update
    void Start()
    {
        // Randomize the instantiation of the cards
        for (int i = 0; i < cardPositions.Count; i++)
        {
            // Select a random card prefab
            GameObject cardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Count)];

            // Instantiate the card at the current position
            GameObject cardObject = Instantiate(cardPrefab, cardPositions[i].position, Quaternion.identity);

            // Add the Card component of the instantiated card to the cards list
            cards.Add(cardObject.GetComponent<Card>());
        }

        // Flip all cards face down after a delay
        StartCoroutine(FlipAllCardsDown());
    }

    private IEnumerator FlipAllCardsDown()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Flip all cards face down
        foreach (Card card in cards)
        {
            card.FlipCardDown();
        }
    }
}