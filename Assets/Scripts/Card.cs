using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer rend;

    [SerializeField]
    private Sprite faceSprite, backSprite;

    private bool coroutineAllowed, facedUp;

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
        if (!facedUp && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    // This function can be called from another script to flip the card to faced down
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
    }
}