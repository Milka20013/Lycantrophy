using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    public NavMeshAgent agent;
    public DropTable dropTable;
    public LayerMask layermask = 1 << 9;

    public Vector3 startPosition;

    public float health = 50f;
    public float rawExperience = 2f;

    public float detectionRange = 40f;
    private double previousTime = 0;

    public float damage = 1f;
    public float attackRange = 5f;
    public float attackSpeed = 0.5f;
    public float attackDelay = 0.5f;

    private GameObject player;
    private Player playerScr;


    public delegate void DieHandler();
    public event DieHandler OnDeath;
    private void Start()
    {
        startPosition = transform.position;
        agent.stoppingDistance = attackRange - 0.1f;
    }
    private void Update()
    {

        
        //detect player
        if (Time.timeAsDouble - previousTime >= 2) 
        {
            previousTime = Time.timeAsDouble;
            if (TryDetectPlayer(out player))
            {
                //become agressive
            }
            else
            {
                
            }
            
        }


        //if it has a player, moves towards it, and attacks if able to
        if (player != null) 
        {
            //move towards the player
            agent.stoppingDistance = attackRange - 0.1f;
            agent.SetDestination(player.transform.position);
            if (PlayerIsInAttackRange())
            {
                //count down attack timer
                if (attackDelay > 0f)
                {
                    attackDelay -= Time.deltaTime;
                }
                //attack if it can
                if (attackDelay <= 0f)
                {
                    if (playerScr != null)
                    {
                        playerScr.TakeDamage(damage);
                    }
                    attackDelay = 1 / attackSpeed;
                }
            }
        }
        //if there is nothing to see
        else
        {
            ReturnToStartPosition();
        }
    }
    // detects the player in a given radius. 
    //If not given, then the mob detection range is used
    public bool TryDetectPlayer(out GameObject player, float detectionRange = 0f) 
    {
        player = null;
        
        if (detectionRange == 0)
        {
            detectionRange = this.detectionRange;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, layermask);
        if (colliders.Length > 0)
        {
            player = colliders[0].gameObject;
            playerScr = player.GetComponentInParent<Player>();
            if (playerScr.isDead)
            {
                ClearTarget();
            }
            return true;
        }
        return false;
    }



    public bool PlayerIsInAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, layermask);
        if (colliders.Length > 0)
        {
            player = colliders[0].gameObject;
            playerScr = player.GetComponentInParent<Player>();
            if (playerScr.isDead)
            {
                ClearTarget();
            }
            return true;
        }
        return false;
    }

    public void ClearTarget()
    {
        player = null;
        playerScr = null;
    }

    public void ReturnToStartPosition()
    {
        agent.stoppingDistance = 0f;
        agent.SetDestination(startPosition);
    }

    public void TakeDamage(float amount, Player playerWhoHit)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die(playerWhoHit);
        }
    }

    public void DropExp(Player playerWhoKilled)
    {
        playerWhoKilled.AddExp(rawExperience);
    }

    public void Die(Player playerWhoKilled)
    {
        DropExp(playerWhoKilled);
        dropTable.DropItems(playerWhoKilled);
        if (OnDeath != null)
        {
            OnDeath();
        }
        Destroy(gameObject);
    }
}
