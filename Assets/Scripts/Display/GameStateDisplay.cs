using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public enum ChangeDirection
{
    RISE,
    FALL
}

public class GameStateDisplay: MonoBehaviour
{
    public TextMeshProUGUI peopleCountText;
    public TextMeshProUGUI peopleHappinesText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI ArmyText;
    public TextMeshProUGUI DayNumberText;

    private Color startColor;
    public Color riseColor = Color.green;
    public Color fallColor = Color.red;

    public void Start()
    {
        startColor = peopleCountText.color; // Taking any text

        GameEvents.current.onUpdateGameStateDisplay += UpdateDisplay;
        GameEvents.current.onVisualiseChange += VisualiseChange;
    }

    private void OnDestroy()
    {
        GameEvents.current.onUpdateGameStateDisplay -= UpdateDisplay;
        GameEvents.current.onVisualiseChange -= VisualiseChange;
    }

    public void UpdateDisplay()
    {
        GameState gs = GameState.current;
        peopleCountText?.SetText(gs.GetRepresentation(GameStateParameter.PEOPLE_COUNT));
        peopleHappinesText?.SetText(gs.GetRepresentation(GameStateParameter.PEOPLE_HAPPINES));
        MoneyText?.SetText(gs.GetRepresentation(GameStateParameter.MONEY));
        ArmyText?.SetText(gs.GetRepresentation(GameStateParameter.ARMY));
        DayNumberText?.SetText(gs.GetRepresentation(GameStateParameter.DAY_NUMBER));
    }

    public void VisualiseChange(GameStateParameter parameter, ChangeDirection direction)
    {
        Color toColor = startColor;
        switch (direction)
        {
            case ChangeDirection.RISE:
                toColor = riseColor;
                break;
            case ChangeDirection.FALL:
                toColor = fallColor;
                break;
            default:
                break;
        }

        TextMeshProUGUI textMesh = null;
        switch (parameter)
        {
            case GameStateParameter.ARMY:
                textMesh = ArmyText;
                break;
            case GameStateParameter.DAY_NUMBER:
                textMesh = DayNumberText;
                break;
            case GameStateParameter.MONEY:
                textMesh = MoneyText;
                break;
            case GameStateParameter.PEOPLE_COUNT:
                textMesh = peopleCountText;
                break;
            case GameStateParameter.PEOPLE_HAPPINES:
                textMesh = peopleHappinesText;
                break;
            default:
                break;
        }
        TweenChange(textMesh, toColor);
    }

    private void TweenChange(TextMeshProUGUI textMesh, Color toColor)
    {
        if (textMesh != null)
        {
            LeanTween.value(textMesh.gameObject, (col) => { textMesh.color = col; }, startColor, toColor, 0.2f).setEaseInExpo().setOnComplete(() =>
            {
                LeanTween.value(textMesh.gameObject, (col) => { textMesh.color = col; }, toColor, startColor, 0.5f).setEaseInExpo();
            });
        }
       
    }
}

