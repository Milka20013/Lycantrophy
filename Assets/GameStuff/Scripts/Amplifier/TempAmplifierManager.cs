using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAmplifierManager : MonoBehaviour
{
    public static TempAmplifierManager instance;

    public class TimedRoutine
    {
        public TimedRoutine(IEnumerator coroutine, float duration)
        {
            this.coroutine = coroutine;
            remainingTime = duration;
            checkingTime = duration <= 10f ? duration / 10 : 2.5f;
        }
        public IEnumerator coroutine;
        public float remainingTime;
        public float checkingTime;
    }
    Dictionary<Amplifier, TimedRoutine> coroutines = new();
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of TempAmplifierManager");
        }
        instance = this;
    }

    public bool RegisterAmplifier(Amplifier amplifier, float duration, Stats stats)
    {
        bool result = stats.RegisterAmplifiers(amplifier);
        var routine = UnregisterAmplifier(amplifier, stats);
        if (result)
        {
            coroutines.Add(amplifier, new(routine, duration));
            StartCoroutine(routine);
        }
        else
        {
            if (coroutines.TryGetValue(amplifier, out TimedRoutine timedRoutine))
            {
                timedRoutine.remainingTime = duration;
            }
        }
        return result;
    }

    public bool RegisterAmplifier(TimedAmplifier TAmplifier, Stats stats)
    {
        return RegisterAmplifier(TAmplifier.amplifier, TAmplifier.duration, stats);
    }
    public bool RegisterAmplifiers(TimedAmplifier[] TAmplifiers, Stats stats)
    {
        bool result = true;
        for (int i = 0; i < TAmplifiers.Length; i++)
        {
            result = RegisterAmplifier(TAmplifiers[i].amplifier, TAmplifiers[i].duration, stats) && result;
        }
        return result;

    }

    /*IEnumerator UnregisterAmplifier(Amplifier amplifier, float duration, Stats stats)
    {
        yield return new WaitForSeconds(duration);
        coroutines.Remove(amplifier);
        if (stats != null)
        {
            stats.UnRegisterAmplifiers(amplifier);
        }
    }*/
    IEnumerator UnregisterAmplifier(Amplifier amplifier, Stats stats)
    {
        while (coroutines[amplifier].remainingTime >= 0f)
        {
            if (coroutines.TryGetValue(amplifier, out TimedRoutine timedRoutine))
            {
                timedRoutine.remainingTime -= timedRoutine.checkingTime;
                Debug.Log(timedRoutine.remainingTime);
                yield return new WaitForSeconds(timedRoutine.checkingTime);
            }
            else
            {
                yield break;
            }
        }
        coroutines.Remove(amplifier);
        if (stats != null)
        {
            stats.UnRegisterAmplifiers(amplifier);
        }
    }
}
