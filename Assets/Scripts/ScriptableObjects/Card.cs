using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class Card : ScriptableObject
{
    [SerializeField] public List<CardTag> cardTags;
    [SerializeField] public List<Checkpoint> neededCheckpointsToSpawn;
    [SerializeField] public CardPeson person;
    [SerializeField] public CardEmotion cardEmotion;

    [SerializeField][TextArea] public string decisionQuestion;

    [SerializeField] public string[] answers = { "no", "yes"};

    [SerializeField] public List<CardActionOnSpawn> actionsOnCardSpawn;

    [SerializeField] public DropActions leftSideActions;
    [SerializeField] public DropActions rightSideActions;

    [SerializeField] public bool isTutorial;

    [SerializeField] public CardColor cardInfoColor;

    public void ExecuteSpawnActions()
    {
        foreach(CardActionOnSpawn actionOnSpawn in actionsOnCardSpawn)
        {
            actionOnSpawn?.Act();
        }
    }

    public void Act(DropSides side)
    {
        if (side.Equals(DropSides.LEFT))
        {
            leftSideActions.Act(side);
        } else if (side.Equals(DropSides.RIGHT))
        {
            rightSideActions.Act(side);
        }
    }

    public bool HasTag(CardTag tag)
    {
        foreach(CardTag thisTag in cardTags)
        {
            if (tag.tagName.Equals(thisTag.tagName))
                return true;
        }
        return false;
    }
}
