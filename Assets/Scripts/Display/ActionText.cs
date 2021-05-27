using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public DropSides side;

    private void Awake()
    {
        
    }

    private void Start()
    {
        textMesh = this.gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.SetText("");
        GameEvents.current.onSetAnswerTextTrigger += SetText;
    }

    private void OnDestroy()
    {
        GameEvents.current.onSetAnswerTextTrigger -= SetText;
    }

    public void SetText(DropSides actionSide, string text)
    {
        if (side.Equals(actionSide))
        {
            var start = 1.0f;
            LeanTween.value(textMesh.gameObject, (float val) => { textMesh.alpha = val; textMesh.SetAllDirty(); }, start, 0.0f, 0.15f).setOnComplete(() =>
            {
                textMesh.SetText(text);
                LeanTween.value(textMesh.gameObject, (float val) => { textMesh.alpha = val; textMesh.SetAllDirty(); }, 0.0f, start, 0.15f);
            }); 
        }
    }


}
