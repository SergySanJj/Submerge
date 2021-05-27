using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "ScriptableObjectsDatabase")]
public class ScriptableObjectsDatabase : ScriptableObject
{
    [SerializeField] private List<Card> cards = new List<Card>();

    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    public ScriptableObject GetById<T>(int id) where T : ScriptableObject
    {
        if (id < 0)
            return null;
        if (typeof(T).IsAssignableFrom(typeof(Card)))
        {
            return cards[id];
        }
        if (typeof(T).IsAssignableFrom(typeof(Checkpoint)))
        {
            return checkpoints[id];
        }

        Debug.LogError("Requested type is not supported by database");
        return null;
    }
    
    public int GetId<T>(T item) where T : ScriptableObject
    {
        if (typeof(T).IsAssignableFrom(typeof(Card)))
        {
            return cards.FindIndex(a => a.Equals(item));

        }
        if (typeof(T).IsAssignableFrom(typeof(Checkpoint)))
        {
            return checkpoints.FindIndex(a => a.Equals(item));

        }
        Debug.LogError("Requested type is not supported by database");
        return -1;
    }

    [ContextMenu("Database/Refresh")]
    public void Refresh()
    {
#if UNITY_EDITOR
        Debug.Log("Refreshing database");
        cards.Clear();
        string[] cardGuids = AssetDatabase.FindAssets("t:Card", null);
        foreach (string id in cardGuids)
        {
            cards.Add(AssetDatabase.LoadAssetAtPath<Card>(AssetDatabase.GUIDToAssetPath(id)));
        }

        checkpoints.Clear();
        string[] checkpointGuids = AssetDatabase.FindAssets("t:Checkpoint", null);
        foreach (string id in checkpointGuids)
        {
            checkpoints.Add(AssetDatabase.LoadAssetAtPath<Checkpoint>(AssetDatabase.GUIDToAssetPath(id)));
        }
#endif
    }
}
