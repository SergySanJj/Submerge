using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Color", menuName = "Card Color")]
public class CardColor: ScriptableObject
{
    public static string changableShaderName = "CardBotSignal";
    public static string parameterName = "_SignalColor";
    public Color cardColor = new Color(0.0f, 0.4f, 0.97f);
}

