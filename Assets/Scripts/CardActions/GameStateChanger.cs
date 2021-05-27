using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum StateChangerType
{
    SET,
    CHANGE,
    PERCENT
}

[Serializable]
public class GameStateChanger
{
    public StateChangerType changerType;
    public GameStateParameter parameterToChange;
    public int value;

    public void Act()
    {
        var valueBefore = GameState.current.GetValue(parameterToChange);
        switch (changerType)
        {
            case StateChangerType.SET:
                GameState.current.SetValue(parameterToChange, value);
                break;
            case StateChangerType.PERCENT:
                GameState.current.ChangeValueInPersentage(parameterToChange, value);
                break;
            case StateChangerType.CHANGE:
                GameState.current.ChangeValue(parameterToChange, value);
                break;
            default:
                break;
        }
        var valueAfter = GameState.current.GetValue(parameterToChange);
        if (valueAfter < valueBefore)
        {
            GameEvents.current.VisualiseChange(parameterToChange, ChangeDirection.FALL);
        } else
        {
            GameEvents.current.VisualiseChange(parameterToChange, ChangeDirection.RISE);
        }
    }
}

