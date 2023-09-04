using UnityEngine;

[CreateAssetMenu(fileName = "ProvokerItem", menuName = "ItemBlueprint/Consumable/Provoker")]
public class ProvokerItem : ConsumableBlueprint
{
    public LayerMask enemyLayer = 1 << 8;
    public float radius = 15f;
    public int maxNumberOfEnemies = 20;
    [Tooltip("How much time added on occupation, the enemy resets the wander cooldown and this is added to it")]
    public float occupationTime = 3f;
    public override void ConsumeItem(Player player)
    {
        Collider[] colliders = new Collider[maxNumberOfEnemies];
        int numColliders = Physics.OverlapSphereNonAlloc(player.transform.position, radius, colliders, enemyLayer);
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i].TryGetComponent(out IProvokable provokable))
            {
                provokable.Provoke(player.playerHitbox, occupationTime);
            }
        }
    }
}
