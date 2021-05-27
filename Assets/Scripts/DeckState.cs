using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DeckState
{
    [SerializeField] public List<Card> cardList; // current cards in hand
    [SerializeField] public List<Card> tutorialCards;
    [SerializeField] public List<Card> currentDialogue;
    [SerializeField] public List<Card> neededCheckpoints;
    [SerializeField] public List<Card> rareCards;
    [SerializeField] public int cardsPassed = 0;
    
    public static bool CanBeLoaded()
    {
        string[] neededLists = { "cardList", "tutorialCards", "currentDialogue", "neededCheckpoints", "rareCards" };

        foreach(string listName in neededLists)
        {
            if (!StandaloneSaveManager.FileExists(StandaloneSaveManager.GetFileName(listName)))
                return false;
        }
        return true;
    }

    public void Save()
    {
        Debug.Log("Saving Deck State");
        StandaloneSaveManager.SaveList(cardList, "cardList");
        StandaloneSaveManager.SaveList(tutorialCards, "tutorialCards");
        StandaloneSaveManager.SaveList(currentDialogue, "currentDialogue");
        StandaloneSaveManager.SaveList(neededCheckpoints, "neededCheckpoints");
        StandaloneSaveManager.SaveList(rareCards, "rareCards");
    }

    public void Load()
    {
        Debug.Log("Loading Deck State");
        cardList = StandaloneSaveManager.LoadList<Card>("cardList");
        tutorialCards = StandaloneSaveManager.LoadList<Card>("tutorialCards");
        currentDialogue = StandaloneSaveManager.LoadList<Card>("currentDialogue");
        neededCheckpoints = StandaloneSaveManager.LoadList<Card>("neededCheckpoints");
        rareCards = StandaloneSaveManager.LoadList<Card>("rareCards");
    }

    public void DropSaves()
    {
        StandaloneSaveManager.Delete(StandaloneSaveManager.GetFileName("cardList"));
        StandaloneSaveManager.Delete(StandaloneSaveManager.GetFileName("tutorialCards"));
        StandaloneSaveManager.Delete(StandaloneSaveManager.GetFileName("currentDialogue"));
        StandaloneSaveManager.Delete(StandaloneSaveManager.GetFileName("neededCheckpoints"));
        StandaloneSaveManager.Delete(StandaloneSaveManager.GetFileName("rareCards"));
    }

    
}

