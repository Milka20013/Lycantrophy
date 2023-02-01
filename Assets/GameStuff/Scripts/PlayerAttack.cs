using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerStats stats;

    private float _damage;

    private float _attackSpeed;
    private float attackDelay = 0f;

    public LayerMask enemyLayer = 1 << 8;
    private void Start()
    {
        stats.OnStatChange += UpdateStats;
        UpdateStats();
    }
    private void Update()
    {
        if (attackDelay > 0f)
        {
            attackDelay -= Time.deltaTime;
        }
    }
    private void OnAttack() //if mouse is pressed (new input system les go) 
    {
        if (attackDelay <= 0f)
        {
            Attack();
            attackDelay = 1 / _attackSpeed;
        }
    }
    
    private void Attack()
    {
        Collider[] enemies =  Physics.OverlapSphere(transform.position, 5f, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent(out Mob mob))
            {
                mob.TakeDamage(_damage, this.gameObject.GetComponent<Player>());
            }
        }
    }
    public void UpdateStats()
    {
        _damage = stats.GetAttributeValue(Attribute.Damage);
        _attackSpeed = stats.GetAttributeValue(Attribute.AttackSpeed);
    }

}
