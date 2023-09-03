using UnityEngine;

[CreateAssetMenu(fileName = "ProvokerItem", menuName = "ItemBlueprint/Consumable/Provoker")]
public class ProvokerItem : ConsumableBlueprint
{
    public LayerMask enemyLayer = 1 << 8;
    public float radius = 10f;
    public int maxNumberOfEnemies = 20;
    public override void ConsumeItem(Player player)
    {
        Collider[] colliders = new Collider[maxNumberOfEnemies];
        int numColliders = Physics.OverlapSphereNonAlloc(player.transform.position, radius, colliders, enemyLayer);
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i].TryGetComponent(out IProvokable provokable))
            {
                provokable.Provoke(player.playerHitbox);
            }
        }
    }
}
