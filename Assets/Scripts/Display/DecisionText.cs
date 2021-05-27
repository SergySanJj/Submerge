using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DecisionText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = this.gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.SetText("");

        GameEvents.current.onDecisionTextTrigger += SetText;
    }

    private void OnDestroy()
    {
        GameEvents.current.onDecisionTextTrigger -= SetText;
    }

    public void SetText(string text)
    {
        var start = 1.0f;
        LeanTween.value(textMesh.gameObject, (float val) => { textMesh.alpha = val; textMesh.SetAllDirty(); }, start, 0.0f, 0.15f).setOnComplete(() =>
        {
            textMesh.SetText(text);
            LeanTween.value(textMesh.gameObject, (float val) => { textMesh.alpha = val; textMesh.SetAllDirty(); }, 0.0f, start, 0.15f);
        });
    }
}
