using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ShufflePlace
{
    FIRST,
    LAST,
    BEGIN,
    MIDDLE,
    END,
    DIALOGUE
}

[System.Serializable]
public class ShuffleActivator
{
    public Card cardToShuffle;
    public ShufflePlace shufflePlace;

    public void Act(DropSides side)
    {
        Debug.Log("Shuffle card " + cardToShuffle?.person.personName + " for side " + side.ToString());

        switch (shufflePlace)
        {
            case ShufflePlace.FIRST:
                DeckController.current.ShuffleToFirst(cardToShuffle);
                break;
            case ShufflePlace.LAST:
                DeckController.current.ShuffleToLast(cardToShuffle);
                break;
            case ShufflePlace.BEGIN:
                DeckController.current.ShuffleBegin(cardToShuffle);
                break;
            case ShufflePlace.MIDDLE:
                DeckController.current.ShuffleMiddle(cardToShuffle);
                break;
            case ShufflePlace.END:
                DeckController.current.ShuffleEnd(cardToShuffle);
                break;
            case ShufflePlace.DIALOGUE:
                DeckController.current.ShuffleDialogue(cardToShuffle);
                break;
            default:
                break;
        }
    }
}
