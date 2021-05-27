using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Action", menuName = "Card Actions/DeathAction")]
class ShowDeathScreen : CardAction
{
    public override void Act(DropSides side)
    {
        GameEvents.current.ShowDeathScreen();
    }
}
