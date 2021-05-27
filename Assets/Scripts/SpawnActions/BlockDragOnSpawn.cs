using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Drag", menuName = "Card Spawn Actions/BlockDrag")]
public class BlockDragOnSpawn : CardActionOnSpawn
{
    public float duration = 1.0f;
    public override void Act()
    {
        GameEvents.current.DragableObjectsState(false);
        GameEvents.current.Execute(ResumeAfter(duration));
    }

    private IEnumerator ResumeAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameEvents.current.DragableObjectsState(true);
    }

}
