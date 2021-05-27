using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum DropSides { LEFT, RIGHT, BOTTOM }
public class DropController : MonoBehaviour
{
    public float moveOnOutLength = 100;
    public DropSides dropSide;

    void Start()
    {
        GameEvents.current.onDropCardTriggerEnter += OnDrop;
    }

    private void OnDestroy()
    {
        GameEvents.current.onDropCardTriggerEnter -= OnDrop;
    }

    private void OnDrop(GameObject cardObject, DropSides side)
    {
        if (side.Equals(this.dropSide))
        {
            //Debug.Log("Item " + cardObject.name + " dropped on side " + side.ToString());
            cardObject.GetComponent<CardDisplay>().card.Act(side);
            cardObject.GetComponent<ObjectDrag>().TakeControl();
            AnimateCardExit(cardObject);

            GameEvents.current.CardLeftScreen();
        }
    }

    private void AnimateCardExit(GameObject cardObject)
    {
        cardObject.GetComponent<CanvasGroup>().interactable = false;
        cardObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Vector3 lastPos = cardObject.transform.position;
        if (cardObject.transform.parent.parent != null)
            cardObject.transform.SetParent(cardObject.transform.parent.parent);
        cardObject.transform.position = lastPos;

        Vector3 exitVector = lastPos;
        exitVector.z = -200;
        
        
        switch (dropSide)
        {
            case DropSides.LEFT:
                exitVector.x -= moveOnOutLength;
                break;
            case DropSides.RIGHT:
                exitVector.x += moveOnOutLength;
                break;
            case DropSides.BOTTOM:
                exitVector.y -= moveOnOutLength;
                break;
            default:
                exitVector = Vector3.zero;
                break;
        }
        LeanTween.moveLocal(cardObject, exitVector, 0.2f).setDestroyOnComplete(true);
    }
}
