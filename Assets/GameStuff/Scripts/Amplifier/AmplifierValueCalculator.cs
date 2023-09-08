

using System.Collections.Generic;

public class AmplifierValueCalculator
{
    public float FinalAttributeValue(Dictionary<AmplifierType, float> ampTypeDict, float coreValue, bool invertedCalculation)
    {
        float result = coreValue;
        foreach (KeyValuePair<AmplifierType, float> item in ampTypeDict)
        {
            switch (item.Key)
            {
                case AmplifierType.Plus:
                    result += item.Value;
                    break;
                case AmplifierType.Percentage:
                    if (invertedCalculation)
                    {
                        float baseValue = 1 - result;
                        baseValue *= 1 - item.Value;
                        result = 1 - baseValue;
                    }
                    else
                    {
                        result *= (1 + item.Value);
                    }
                    break;
                case AmplifierType.TruePercentage:
                    if (invertedCalculation)
                    {
                        float baseValue = 1 - result;
                        baseValue *= (1 - item.Value);
                        result = 1 - baseValue;
                    }
                    else if (item.Value != 0)
                    {
                        result *= item.Value;
                    }
                    break;
                default:
                    break;
            }
        }
        return result;
    }

    public void SummarizeAmplifierValues(ref Dictionary<Attribute, Dictionary<AmplifierType, float>> dict, Amplifier amplifier)
    {
        switch (amplifier.amplifierType)
        {
            case AmplifierType.Plus:
                dict[amplifier.attribute][amplifier.amplifierType] += amplifier.value;
                break;
            case AmplifierType.Percentage:
                dict[amplifier.attribute][amplifier.amplifierType] += amplifier.value / 100f;
                break;
            case AmplifierType.TruePercentage:
                if (amplifier.attribute.invertedCalculation)
                {
                    float baseValue = 1 - dict[amplifier.attribute][amplifier.amplifierType];
                    baseValue *= 1 - amplifier.value / 100f;
                    dict[amplifier.attribute][amplifier.amplifierType] = 1 - baseValue;
                }
                else
                {
                    dict[amplifier.attribute][amplifier.amplifierType] *= 1 + amplifier.value / 100f;

                }
                break;
            default:
                break;
        }
    }
}
