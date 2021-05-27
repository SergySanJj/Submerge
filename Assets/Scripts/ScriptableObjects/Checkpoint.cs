using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Checkpoint", menuName = "Checkpoint")]
public class Checkpoint: ScriptableObject
{
    public void SetParams(string checkpointName, bool needsToBeDisplayed, string displayText, Sprite displaySprite)
    {
        this.checkpointName = checkpointName;
        this.needsToBeDisplayed = needsToBeDisplayed;
        this.displayText = displayText;
        this.displaySprite = displaySprite;
    }

    public string checkpointName;

    public bool needsToBeDisplayed = false;

    public string displayText = "";
    public Sprite displaySprite = null;
}

