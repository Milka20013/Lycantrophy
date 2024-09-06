using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DogTreat", menuName = "ItemBlueprint/Consumable/DogTreat")]
public class DogTreat : ConsumableBlueprint
{
    [Serializable]
    private struct AmplifierWithIncrement
    {
        public Amplifier amplifier;
        public float increment;

        public void IncreaseAmplifier()
        {
            amplifier.value += increment;
        }
    }
    [SerializeField] private AmplifierWithIncrement treatAmplifier;
    [SerializeField] private Amplifier tempAmplifier;

    public override void ConsumeItem(Player player)
    {
        treatAmplifier.IncreaseAmplifier();
        player.GetComponent<Stats>().RegisterAmplifiers(treatAmplifier.amplifier);
        TempAmplifierManager.instance.RegisterAmplifier(tempAmplifier, 10, player.GetComponent<Stats>());
    }
}
