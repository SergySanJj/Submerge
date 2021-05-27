using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Action", menuName = "Card Actions/InfoAction")]
class InfoAction : CardAction
{
    public override void Act(DropSides side)
    {
        Debug.Log("Action called for " + side.ToString());
    }
}

