using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveMob : Mob
{
    private void Start()
    {
        takeDamage.OnHit += OnHit;
    }

    public void OnHit(float amount, GameObject attacker)
    {
        
    }
}
