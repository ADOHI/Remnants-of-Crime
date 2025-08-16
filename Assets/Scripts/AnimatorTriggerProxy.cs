// AnimatorTriggerProxy.cs
using UnityEngine;

public class AnimatorTriggerProxy : MonoBehaviour
{
    public Animator animator;

    public void SetTrigger(string triggerName)
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator != null && !string.IsNullOrEmpty(triggerName))
            animator.SetTrigger(triggerName);
    }
}
