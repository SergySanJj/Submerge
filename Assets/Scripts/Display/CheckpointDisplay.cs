using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Image iconSlot;
    public Animator animator;


    void Start()
    {
        textMesh?.SetText("");

        GameEvents.current.onShowCheckpoint += Display;
    }

    private void OnDestroy()
    {
        GameEvents.current.onShowCheckpoint -= Display;
    }

    public void Display(string text, Sprite sprite)
    {
        textMesh?.SetText(text);
        if (sprite == null)
        {
            var startColor = iconSlot.color;
            startColor.a = 0.0f;
            iconSlot.color = startColor;
        } else
        {
            var startColor = iconSlot.color;
            startColor.a = 1.0f;
            iconSlot.color = startColor;

            iconSlot.sprite = sprite;
            iconSlot.SetAllDirty();
        }
        
        PlayAppeadAndDisappearAnimation();
    }

    private void PlayAppeadAndDisappearAnimation()
    {
        animator.SetTrigger("AppearAndDisappear");
    }
}
