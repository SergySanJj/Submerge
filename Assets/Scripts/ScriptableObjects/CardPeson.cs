using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Person", menuName = "CardPeson")]
public class CardPeson : ScriptableObject
{
    public string personName = "";
    public string description = "";
    public Sprite artwork;
}

