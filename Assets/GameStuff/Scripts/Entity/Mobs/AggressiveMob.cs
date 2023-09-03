using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MobAttack))]
[RequireComponent(typeof(Detector))]
public class AggressiveMob : Mob
{
    protected MobAttack mobAttack;
    protected Detector detector;

    //This field is used VERY CHAOTICLY!!
    protected float stoppingDistance;

    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = mobAttack.attackRange - 0.1f;
        detector.onDetection += HandleDetection;
        stoppingDistance = CalculateStoppingDistance();
    }

    public override void Provoke(Collider provoker)
    {
        HandleDetection(provoker);
    }

    protected override void FindReferences()
    {
        base.FindReferences();
        mobAttack = GetComponent<MobAttack>();
        detector = GetComponent<Detector>();
    }
    public void HandleDetection(Collider collider)
    {
        if (collider == null)
        {
            CalmDown();
            return;
        }
        occupied = true;
        SetDestinationProjected(collider.ClosestPointOnBounds(transform.position), 0f);
        if (detector.TryDetectTargets(mobAttack.AttackOrigin, out GameObject[] targets, mobAttack.attackRange))
        {
            LookAtTarget(targets[0].transform);
            mobAttack.canAttack = true;
        }
        else
        {
            if (agent.remainingDistance <= stoppingDistance)
            {
                LookAtTarget(collider.transform);
            }
            mobAttack.canAttack = false;
        }
    }
    protected float CalculateStoppingDistance(float offset = 0.2f)
    {
        return Vector3.Distance(transform.position, mobAttack.AttackOrigin.position) + mobAttack.attackRange - offset;
    }

    protected IEnumerator RotateToTarget(Transform target, float speed = 0.6f)
    {
        Quaternion previousRotation = transform.rotation;
        for (; ; )
        {
            var rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
            if (previousRotation == transform.rotation)
            {
                break;
            }
            previousRotation = transform.rotation;
            yield return null;
        }
    }

    protected bool IsLookingAtTarget(Transform target)
    {
        Quaternion previousRotation = transform.rotation;
        var rotation = Quaternion.LookRotation(target.position - transform.position);
        if (Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.5f) == previousRotation)
        {
            return true;
        }
        return false;
    }

    protected void LookAtTarget(Transform target)
    {
        if (!IsLookingAtTarget(target))
        {
            IEnumerator coroutine = RotateToTarget(target);
            StartCoroutine(coroutine);
        }
    }
}
