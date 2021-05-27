using Assets.Scripts.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public enum GameStateParameter
{
    PEOPLE_COUNT,
    PEOPLE_HAPPINES,
    MONEY,
    ARMY,
    DAY_NUMBER
}

public class GameState : MonoBehaviour
{
    public static GameState current;

    private Dictionary<GameStateParameter, Parameter> parameters;

    private List<Checkpoint> passedCheckpoints;

    private bool deathEventSpawned = false;

    void Awake()
    {
        current = this;

        parameters = new Dictionary<GameStateParameter, Parameter>
        {
            { GameStateParameter.PEOPLE_COUNT, new PeopleCount(10000) },
            { GameStateParameter.PEOPLE_HAPPINES, new PeopleHappines(90) },
            { GameStateParameter.MONEY, new Money(2000000) },
            { GameStateParameter.ARMY, new Army(20) },
            { GameStateParameter.DAY_NUMBER, new DayNumber(0) },
        };

        passedCheckpoints = new List<Checkpoint>();

        //if (canBeLoaded())
            //Load();
    }
    public void Start()
    {
        GameEvents.current.onCardLeftScreen += SpawnNewCardIfNotDead;
        GameEvents.current.onAddPassedCheckpoint += AddPassedCheckpoint;

        UpdateView();
    }

    private void OnDestroy()
    {
        GameEvents.current.onCardLeftScreen -= SpawnNewCardIfNotDead;
        GameEvents.current.onAddPassedCheckpoint -= AddPassedCheckpoint;
    }

    public bool canBeLoaded()
    {
        return 
            StandaloneSaveManager.FileExists("GameState.params") &&
            StandaloneSaveManager.FileExists(StandaloneSaveManager.GetFileName("passedCheckpoints"));
    }

    public void Save()
    {
        StandaloneSaveManager.BinarySerialize("GameState.params", parameters);
        StandaloneSaveManager.SaveList(passedCheckpoints, "passedCheckpoints");
    }

    public void Load()
    {
        parameters = StandaloneSaveManager.BinaryDeserialize<Dictionary<GameStateParameter, Parameter>>("GameState.params");
        passedCheckpoints = StandaloneSaveManager.LoadList<Checkpoint>("passedCheckpoints");
    }

    public void DropSaves()
    {
        StandaloneSaveManager.Delete("GameState.params");
    }

    private void SpawnNewCardIfNotDead()
    {
        if (IsDead())
        {
            if (!deathEventSpawned)
            {
                Debug.Log("Shuffle death");
                GameEvents.current.SpawnDeathCard();
                GameEvents.current.CreateNewCard();
                deathEventSpawned = true;
            }
        } else
        {
            GameEvents.current.CreateNewCard();
        }
    }


    public Parameter GetParameter(GameStateParameter parameter)
    {
        return parameters[parameter];
    }

    public int GetValue(GameStateParameter parameter)
    {
        return GetParameter(parameter).Value();
    }

    public string GetRepresentation(GameStateParameter parameter)
    {
        return GetParameter(parameter).FormText();
    }

    public void ChangeValue(GameStateParameter parameter, int val)
    {
        SetValue(parameter, parameters[parameter].Value() + val);
        UpdateView();
    }

    public void SetValue(GameStateParameter parameter, int val)
    {
        parameters[parameter].SetValue(val);
        UpdateView();
    }

    public void ChangeValueInPersentage(GameStateParameter parameter, int persentage)
    {
        SetValue(parameter, (int)(parameters[parameter].Value() * ((100 + persentage) / 100.0f)));
        UpdateView();
    }

    
    public void AddPassedCheckpoint(Checkpoint checkpoint)
    {
        if (!CheckpointPassed(checkpoint))
        {
            passedCheckpoints.Add(checkpoint);
            if (checkpoint.needsToBeDisplayed)
            {
                GameEvents.current.ShowCheckpoint(checkpoint.displayText, checkpoint.displaySprite);

            }
        }
        GameEvents.current.UpdateCheckpointCards();
    }

    public bool CheckpointPassed(Checkpoint checkpoint)
    {
        foreach(Checkpoint c in passedCheckpoints)
        {
            if (c.checkpointName.Equals(checkpoint.checkpointName))
                return true;
        }
        return false;
    }

    private void UpdateView()
    {
        GameEvents.current.UpdateGameStateDisplay();
    }

    private bool IsDead()
    {
        foreach (KeyValuePair<GameStateParameter, Parameter> keyValuePair in parameters)
        {
            if (keyValuePair.Value.NoResources())
            {
                return true;
            }
        }
        return false;
    }
}
