using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{

    public event UnityAction OnDead;
    public event UnityAction OnDamaged;

    [SerializeField] private int health = 100;

    private int healthMAX;

    private void Awake()
    {
        healthMAX = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke();

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke();
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMAX;
    }
}
