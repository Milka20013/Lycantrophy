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
    public bool lockState { get; private set; }

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
        if (!IsPlayingAnim())
        {
            ChangeAnimationState(idle, true);
        }
    }

    private bool IsPlayingAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.GetCurrentAnimatorStateInfo(0).loop)
        {
            return true;
        }
        return false;
    }

    public bool ChangeAnimationState(string newState, bool forceIdle = false)
    {
        if (lockState)
        {
            return false;
        }
        if (currentState == newState)
        {
            if (IsPlayingAnim())
            {
                return false;

            }
            //reset the animation so it can be replayed
            animator.Play(currentState, -1, 0);
        }
        if (newState == idle && !forceIdle)
        {
            return false;
        }
        animator.Play(newState);

        currentState = newState;
        return true;
    }

    public bool ChangeAnimationState(string newState, float lockTime)
    {
        bool result = ChangeAnimationState(newState);
        if (result)
        {
            SetLock(lockTime);
        }
        return result;
    }

    public void SignalIdle(string state)
    {
        if (lockState)
        {
            return;
        }
        if (currentState == state)
        {
            ChangeAnimationState(idle, true);
        }
    }

    public void SetFloat(string valueName, float value)
    {
        animator.SetFloat(valueName, value);
    }

    public void SetLock(float time)
    {
        if (lockState)
        {
            return;
        }
        lockState = true;
        StartCoroutine(FreeLock(time));
    }

    IEnumerator FreeLock(float time)
    {
        yield return new WaitForSeconds(time);
        lockState = false;
    }
}
