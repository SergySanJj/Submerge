using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public enum DeckCardType
{
    REGULAR,
    RARE_EVENT,
    //RANDOM_SHUFFLE
}

[System.Serializable]
public class DeckCard
{
    public Card card;
    public DeckCardType deckCardType;
}

