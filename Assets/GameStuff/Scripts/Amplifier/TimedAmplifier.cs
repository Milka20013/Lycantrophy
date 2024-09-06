using System;

[Serializable]
public class TimedAmplifier
{
    public Amplifier amplifier;
    public float duration;

    public TimedAmplifier(Amplifier amplifier)
    {
        this.amplifier = amplifier;
        duration = 0f;
    }
    public TimedAmplifier(TimedAmplifier other)
    {
        this.amplifier = other.amplifier;
        this.duration = other.duration;
    }

    public override string ToString()
    {
        return amplifier.ToString() + " for " + duration + " seconds";
    }

    public string Description()
    {
        return amplifier.Description() + " for " + duration + " seconds";
    }

    public static explicit operator Amplifier(TimedAmplifier amp) => new(amp.amplifier);
    public static implicit operator TimedAmplifier(Amplifier amplifier) => new(amplifier);
}
