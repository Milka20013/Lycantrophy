using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Attacker : MonoBehaviour
{
    private Stats stats;

    private float _damage;
    public float attackRange = 4;

    private float _attackSpeed;
    private float attackDelay = 0f;

    public LayerMask enemyLayer = 1 << 8;

    [Tooltip("Automatically attacks if its attackDelay is <= 0, and if canAttack is true")]
    public bool attackIfAbleTo = false;
    [HideInInspector] public bool canAttack = false;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }
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

        if (attackIfAbleTo && canAttack  && attackDelay <= 0f)
        {
            OnAttack();
        }
    }
    public void OnAttack() //playerinput also invokes this
    {
        if (attackDelay <= 0f)
        {
            Attack();
            attackDelay = 1 / _attackSpeed;
        }
    }
    
    private void Attack()
    {
        Collider[] enemies =  Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent(out TakeDamage takeDamage))
            {
                if (takeDamage.isDead)
                {
                    return;
                }
                takeDamage.RegisterDamage(_damage, gameObject);
            }
        }
    }
    public void UpdateStats()
    {
        _damage = stats.GetAttributeValue(Attribute.Damage);
        _attackSpeed = stats.GetAttributeValue(Attribute.AttackSpeed);
    }

}
