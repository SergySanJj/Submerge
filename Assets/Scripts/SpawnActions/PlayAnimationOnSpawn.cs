using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[UnityEngine.CreateAssetMenu(fileName = "New Play Animation", menuName = "Card Spawn Actions/PlayAnimation")]
class PlayAnimationOnSpawn : CardActionOnSpawn
{
    public string receiverName = "";
    public string triggerName = "Play";
    public override void Act()
    {
        GameEvents.current.PlayAnimation(receiverName, triggerName);
    }
}

