using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Attacker : MonoBehaviour
{
    private Stats stats;

    private float _damage;
    [SerializeField] private Attribute damageAttribute;
    public float attackRange = 4;

    private float _attackSpeed;
    [SerializeField] private Attribute attackSpeedAttribute;
    private float attackDelay = 0f;


    public LayerMask enemyLayer = 1 << 8;

    [Tooltip("Automatically attacks if its attackDelay is <= 0, and if canAttack is true")]
    public bool attackIfAbleTo = false;
    [HideInInspector] public bool canAttack = false;

    EntityAnimator animator;
    private bool hasAnimator;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        hasAnimator = TryGetComponent(out animator);
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

        if (attackIfAbleTo && canAttack && attackDelay <= 0f)
        {
            TryAttack();
        }
    }
    public void TryAttack()
    {
        if (attackDelay <= 0f)
        {
            if (hasAnimator)
            {
                animator.ChangeAnimationState(animator.attack);
                animator.SetLock(animator.AttackLength / _attackSpeed);
            }
            Attack();
            attackDelay = 1 / _attackSpeed;
        }
    }

    private void Attack()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
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
