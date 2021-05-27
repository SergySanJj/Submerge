using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationSignalReceiver: MonoBehaviour
{
    public string receiverName = "";
    public bool destroyOnAnimationEnd = true;
    private float destroyAfter = 10.0f;
    private Animator animator;

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();

        destroyAfter = CountSumAnimationsTime();

        GameEvents.current.onPlayAnimationSender += PlayAnimationSignalReceiver;
    }

    private void OnDestroy()
    {
        GameEvents.current.onPlayAnimationSender -= PlayAnimationSignalReceiver;
    }

    public void PlayAnimationSignalReceiver(string receiver, string animationName)
    {
        Debug.Log("Animation signal received");
        if (this.receiverName.Equals(receiver))
        {
            PlayAnimation(animationName);
        }
    }

    private void PlayAnimation(string animationTrigger)
    {
        Debug.Log("Playing animation");
        animator.SetTrigger(animationTrigger);
        if (destroyOnAnimationEnd)
            Destroy(gameObject, destroyAfter);
    }

    private float CountSumAnimationsTime()
    {
        float res = 0.0f;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            res += clip.length;
        }
        return res;
    }
}

