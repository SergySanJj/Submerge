using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public void Execute(IEnumerator corutine)
    {
        StartCoroutine(corutine);
    }



    public delegate void CardEntersZone(GameObject cardObject, DropSides side);
    public event CardEntersZone onDropCardTriggerEnter;
    public void DropTriggerEnter(GameObject cardObject, DropSides side)
    {
        onDropCardTriggerEnter?.Invoke(cardObject, side);
    }

    public event Action onCreateNewCardTrigger;
    public void CreateNewCard()
    {
        onCreateNewCardTrigger?.Invoke();
    }

    public delegate void AnswerText(DropSides side, string text);
    public event AnswerText onSetAnswerTextTrigger;
    public void SetAnswerText(DropSides side, string text)
    {
        onSetAnswerTextTrigger?.Invoke(side, text);
    }

    public delegate void DecisionText(string text);
    public event DecisionText onDecisionTextTrigger;
    public void SetDecisionText(string text)
    {
        onDecisionTextTrigger?.Invoke(text);
    }

    public event Action onUpdateGameStateDisplay;
    public void UpdateGameStateDisplay()
    {
        onUpdateGameStateDisplay?.Invoke();
    }

    public delegate void VisualiseChangeTrigger(GameStateParameter parameter, ChangeDirection direction);
    public event VisualiseChangeTrigger onVisualiseChange;
    public void VisualiseChange(GameStateParameter parameter, ChangeDirection direction)
    {
        onVisualiseChange?.Invoke(parameter, direction);
    }

    public event Action onShowDeathScreen;
    public void ShowDeathScreen()
    {
        onShowDeathScreen?.Invoke();
    }

    public event Action onCardLeftScreen;
    public void CardLeftScreen()
    {
        onCardLeftScreen?.Invoke();
    }

    public event Action onSpawnDeathCard;
    public void SpawnDeathCard()
    {
        onSpawnDeathCard?.Invoke();
    }

    public delegate void SetEnableState(bool value);
    public event SetEnableState onSetDragableObjectEnableState;
    public void DragableObjectsState(bool isEnabled)
    {
        onSetDragableObjectEnableState?.Invoke(isEnabled);
    }

    public delegate void AddPassedCheckpointTrigger(Checkpoint checkpoint);
    public event AddPassedCheckpointTrigger onAddPassedCheckpoint;
    public void AddPassedCheckpoint(Checkpoint checkpoint)
    {
        onAddPassedCheckpoint?.Invoke(checkpoint);
    }

    public event Action onUpdateCheckpointCards;
    public void UpdateCheckpointCards()
    {
        onUpdateCheckpointCards?.Invoke();
    }


    public delegate void ShowCheckpointTrigger(string checkpointText, Sprite checkpointSprite);
    public event ShowCheckpointTrigger onShowCheckpoint;
    public void ShowCheckpoint(string checkpointText, Sprite checkpointSprite)
    {
        onShowCheckpoint?.Invoke(checkpointText, checkpointSprite);
    }

    public delegate void RemoveCardsWithTagTrigger(CardTag tag);
    public event RemoveCardsWithTagTrigger onRemoveCardsWithTag;
    public void RemoveCardsWithTag(CardTag tag)
    {
        onRemoveCardsWithTag?.Invoke(tag);
    }

    public delegate void PlayAnimationTrigger(string receiverName, string animationTrigger);
    public event PlayAnimationTrigger onPlayAnimationSender;
    public void PlayAnimation(string receiverName, string animationTrigger)
    {
        onPlayAnimationSender?.Invoke(receiverName, animationTrigger);
    }

    public delegate void SpawnObjectTrigger(GameObject objectPrefab);
    public event SpawnObjectTrigger onSpawnObject;
    public void SpawnObject(GameObject objectPrefab)
    {
        onSpawnObject?.Invoke(objectPrefab);
    }
}

