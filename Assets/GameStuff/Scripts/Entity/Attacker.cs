using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Attacker : MonoBehaviour
{
    protected Stats stats;

    protected float _damage;
    [SerializeField] private Attribute damageAttribute;
    public float attackRange = 4;
    [Tooltip("Where the attack origins from. If not specified, the center point is used")]


    [field: SerializeField]
    protected Transform _attackOrigin;
    public Transform AttackOrigin
    {
        get => _attackOrigin == null ? transform : _attackOrigin;
        set => _attackOrigin = value;
    }

    protected float _attackSpeed;
    [SerializeField] protected Attribute attackSpeedAttribute;
    protected float attackDelay = 0f;


    public LayerMask enemyLayer = 1 << 12;

    [HideInInspector] public bool canAttack = false;

    EntityAnimator animator;
    protected bool hasAnimator;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }
    private void Start()
    {
        animator = GetComponentInChildren<EntityAnimator>();
        if (animator != null)
        {
            hasAnimator = true;
        }
        stats.OnStatChange += UpdateStats;
        UpdateStats();
    }
    protected virtual void Update()
    {
        if (attackDelay > 0f)
        {
            attackDelay -= Time.deltaTime;
        }


    }
    public void TryAttack()
    {
        if (attackDelay <= 0f)
        {
            if (hasAnimator)
            {
                if (!animator.ChangeAnimationState(animator.attack, animator.AttackLength / _attackSpeed))
                {
                    return;
                }
            }
            Attack();
            attackDelay = 1 / _attackSpeed;
        }
    }

    protected void Attack()
    {
        Collider[] enemies = Physics.OverlapSphere(AttackOrigin.position, attackRange, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent(out IDamageable damageable))
            {
                if (damageable.IsDead())
                {
                    return;
                }
                damageable.RegisterDamage(_damage, gameObject);
            }
        }
    }
    public void UpdateStats()
    {
        _damage = stats.GetAttributeValue(damageAttribute);
        _attackSpeed = stats.GetAttributeValue(attackSpeedAttribute);
        if (!hasAnimator)
        {
            return;
        }
        if (1 / _attackSpeed < animator.AttackLength)
        {
            animator.SetFloat(animator.attackSpeed, animator.AttackLength * _attackSpeed);
        }
    }

}
