using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs;
    public List<Transform> cardPositions;
    public Text matchesText, turnsText;
    public int matchesRequired, turnsLeft;
    private List<Card> cards = new List<Card>();
    private List<Card> flippedCards = new List<Card>();
    private int matchesMade = 0;
    public AudioSource matchSound;
    public AudioSource noMatchSound;

    void Start()
    {
        GameState gameState = SaveSystem.LoadGame(SceneManager.GetActiveScene().name);
        if (gameState != null)
        {
            // Load the game state
            turnsLeft = gameState.turnsLeft;
            matchesMade = gameState.matchesMade;
            matchesText.text = "Matches: " + matchesMade;
            turnsText.text = "Turns Left: " + turnsLeft;

            // Load the state of each card
            foreach (CardState cardState in gameState.cards)
            {
                // Only instantiate the card if it is not matched
                if (!cardState.isMatched)
                {
                    // Find the card prefab with the same name as the card state
                    foreach (GameObject cardPrefab in cardPrefabs)
                    {
                        Card cardPrefabComponent = cardPrefab.GetComponent<Card>();
                        if (cardPrefabComponent.faceSprite.name == cardState.cardName)
                        {
                            // Instantiate the card at the saved position
                            GameObject cardObject = Instantiate(cardPrefab, cardState.position, Quaternion.identity);
                            Card card = cardObject.GetComponent<Card>();
                            card.cardManager = this;
                            card.SetState(cardState); // Add this line
                            cards.Add(card);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            matchesText.text = "Matches: " + matchesMade;
            turnsText.text = "Turns Left: " + turnsLeft;

            List<GameObject> tempPrefabs = new List<GameObject>(cardPrefabs);
            int numPairs = cardPositions.Count / 2;

            for (int i = 0; i < numPairs; i++)
            {
                int randomIndex = Random.Range(0, tempPrefabs.Count);
                GameObject cardPrefab = tempPrefabs[randomIndex];
                tempPrefabs.RemoveAt(randomIndex);

                for (int j = 0; j < 2; j++)
                {
                    randomIndex = Random.Range(0, cardPositions.Count);
                    Transform randomPosition = cardPositions[randomIndex];
                    cardPositions.RemoveAt(randomIndex);

                    GameObject cardObject = Instantiate(cardPrefab, randomPosition.position, Quaternion.identity);
                    Card card = cardObject.GetComponent<Card>();
                    card.cardManager = this;
                    cards.Add(card);
                }
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
        yield return new WaitForSeconds(1f);

        Card card1 = flippedCards[0];
        Card card2 = flippedCards[1];

        if (card1.faceSprite == card2.faceSprite)
        {
            card1.isMatched = true; // Add this line
            card2.isMatched = true; // Add this line

            Destroy(card1.gameObject);
            Destroy(card2.gameObject);
            matchSound.Play();

            matchesMade++;
            matchesText.text = "Matches: " + matchesMade;

            if (matchesMade == matchesRequired)
            {
                SaveSystem.DeleteSaveFile(SceneManager.GetActiveScene().name); // Delete save file here
                yield return new WaitForSeconds(1.5f); // Wait for a sec and a second
                SceneManager.LoadScene("LevelComplete");
            }
            else
            {
                // Save the game state after every card match and mismatch
                turnsLeft--;
                turnsText.text = "Turns Left: " + turnsLeft;
                SaveGameState();
            }
        }
        else
        {
            card1.FlipCardDown();
            card2.FlipCardDown();
            noMatchSound.Play();

            // Save the game state after every card match and mismatch
            turnsLeft--;
            turnsText.text = "Turns Left: " + turnsLeft;
            SaveGameState();
        }

        if (turnsLeft == 0 && matchesMade != matchesRequired)
        {
            SaveSystem.DeleteSaveFile(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("GameOver");
        }

        flippedCards.Remove(card1);
        flippedCards.Remove(card2);
    }


    private void SaveGameState()
    {
        GameState gameState = new GameState();
        gameState.turnsLeft = turnsLeft;
        gameState.matchesMade = matchesMade;
        gameState.cards = new List<CardState>();
        foreach (Card card in cards)
        {
            if (card != null)
            {
                gameState.cards.Add(card.GetState());
            }
        }
        SaveSystem.SaveGame(gameState, SceneManager.GetActiveScene().name);
    }
}