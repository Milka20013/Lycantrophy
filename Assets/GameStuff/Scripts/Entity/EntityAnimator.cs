using System.Collections;
using UnityEngine;
public class EntityAnimator : MonoBehaviour
{
    private Animator animator;

    public string attack = "Armature_Attack";
    public string walk = "Armature_Walk";
    public string sprint = "Armature_Sprint";
    public string idle = "Armature_Idle";
    [Tooltip("Prevents stuck animations, but lowers performance")]
    public bool doIdleChecks = false;

    public float AttackLength { get; set; }
    public readonly string attackSpeed = "attackSpeed";


    private string currentState;
    public bool lockState { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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

    /// <summary>
    /// Changes the state then after animationTime seconds changes to idle
    /// if the current animation is still the same as newState
    /// </summary>
    /// <returns>True if the animation state changed</returns>
    public bool ChangeAnimationStateThenIdle(string newState, float animationTime)
    {
        bool result = ChangeAnimationState(newState);
        if (result)
        {
            IEnumerator coroutine = PlayIdleAfterDelay(animationTime);
            StartCoroutine(coroutine);
        }
        return result;
    }

    IEnumerator PlayIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeAnimationState(idle, true);
    }
    public void SignalIdle(string stateToChangeFrom)
    {
        if (lockState)
        {
            return;
        }
        if (currentState == stateToChangeFrom)
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
