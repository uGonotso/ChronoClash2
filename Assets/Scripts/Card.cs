using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer rend;
    [SerializeField]
    public Sprite faceSprite, backSprite;
    private bool coroutineAllowed, facedUp;
    public AudioSource cardFlipSound;
    public CardManager cardManager;

    public bool isMatched = false;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = backSprite;
        coroutineAllowed = true;
        facedUp = false;
    }

    private void OnMouseDown()
    {
        if (!facedUp && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
            cardFlipSound.Play();
        }
    }

    public void FlipCardDown()
    {
        if (facedUp && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
            cardFlipSound.Play();
        }
    }

    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        for (float i = facedUp ? 180f : 0f; facedUp ? i >= 0f : i <= 180f; i += facedUp ? -10f : 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                rend.sprite = facedUp ? backSprite : faceSprite;
            }
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;
        facedUp = !facedUp;

        if (facedUp)
        {
            cardManager.CardFlipped(this);
        }
    }

    public void SetState(CardState state)
    {
        // Set the state of the card based on the CardState object
        transform.position = state.position;
        gameObject.SetActive(state.isInGame);
        isMatched = state.isMatched; 
    }

    public CardState GetState()
    {
        CardState state = new CardState();
        // Populate the CardState object based on the current state of the card
        state.cardName = faceSprite != null ? faceSprite.name : ""; // Null check
        state.position = transform.position;
        state.isInGame = gameObject.activeSelf;
        state.isMatched = isMatched; 
        return state;
    }
}