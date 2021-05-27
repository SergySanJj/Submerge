using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DropActions
{
    public int dayAmount;

    public List<CardAction> staticActions;
    public List<ShuffleActivator> shuffleActions;
    public List<GameStateChanger> gameStateChangeActions;
    public List<Checkpoint> passCheckpoints;
    public List<CardTag> removeCardsWithTags;

    public void Act(DropSides dropSide)
    {
        foreach (CardAction action in staticActions)
        {
            action?.Act(dropSide);
        }
        foreach (ShuffleActivator shuffle in shuffleActions)
        {
            shuffle?.Act(dropSide);
        }
        foreach (GameStateChanger stateChange in gameStateChangeActions)
        {
            stateChange?.Act();
        }
        foreach(Checkpoint c in passCheckpoints)
        {
            GameEvents.current.AddPassedCheckpoint(c);
        }
        foreach(CardTag tag in removeCardsWithTags)
        {
            GameEvents.current.RemoveCardsWithTag(tag);
        }
        GameState.current.ChangeValue(GameStateParameter.DAY_NUMBER, dayAmount);
    }
}

