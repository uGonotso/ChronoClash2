using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardState
{
    public string cardName;
    public bool isMatched;
    public Vector3 position;
    public bool isInGame;
}

[System.Serializable]
public class GameState
{
    public int turnsLeft;
    public int matchesMade;
    public List<CardState> cards;
}