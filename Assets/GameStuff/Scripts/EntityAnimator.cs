using System.Collections;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public string attack;
    public string walk;
    public string sprint;
    public string idle;

    public float AttackLength { get; set; }
    public readonly string attackSpeed = "attackSpeed";


    private string currentState;

    private void Awake()
    {
        UpdateAnimClipTimes();
    }

    private void Start()
    {
        IEnumerator CheckIdle = CheckIfIdle();
        StartCoroutine(CheckIdle);
    }
    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == attack)
            {
                AttackLength = clip.length;
            }
        }
    }

    IEnumerator CheckIfIdle()
    {
        for (; ; )
        {
            StartCoroutine(nameof(PlayDelayedIdle));
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator PlayDelayedIdle()
    {
        yield return new WaitForSeconds(0.1f);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.GetCurrentAnimatorStateInfo(0).loop)
        {
            ChangeAnimationState(idle, true);
        }
    }

    public void ChangeAnimationState(string newState, bool forceIdle = false)
    {
        if (currentState == newState)
        {
            return;
        }
        if (newState == idle && !forceIdle)
        {
            return;
        }
        animator.Play(newState);

        currentState = newState;
    }

    public void SignalIdle(string state)
    {
        if (currentState == state)
        {
            ChangeAnimationState(idle, true);
        }
    }

    public void SetFloat(string valueName, float value)
    {
        animator.SetFloat(valueName, value);
    }
}
