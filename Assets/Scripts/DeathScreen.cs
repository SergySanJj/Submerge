using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public Animator transition;
    public void Start()
    {
        GameEvents.current.onShowDeathScreen += ShowDeathScreen;

        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        // HideDeathScreen();
    }

    private void OnDestroy()
    {
        GameEvents.current.onShowDeathScreen -= ShowDeathScreen;
    }

    public void HideDeathScreen()
    {
        transition.SetTrigger("Disappear");
    }

    public void ShowDeathScreen()
    {
        transition.SetTrigger("Appear");
    }


}
