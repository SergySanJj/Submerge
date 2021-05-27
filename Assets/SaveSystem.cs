using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem current;
    public ScriptableObjectsDatabase Database;

    private void Awake()
    {
        current = this;
    }
    public void DropSaves()
    {
        GameState.current.DropSaves();
        DeckController.deckState.DropSaves();
    }

}
