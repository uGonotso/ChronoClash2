using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer rend;

    [SerializeField]
    public Sprite faceSprite, backSprite;

    private bool coroutineAllowed, facedUp;

    public CardManager cardManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = backSprite;
        coroutineAllowed = true;
        facedUp = false;
    }

    private void OnMouseDown()
    {
        // Only allow the card to be flipped if it's not already faced up
        if (!facedUp && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    public void FlipCardDown()
    {
        if (facedUp && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
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
}