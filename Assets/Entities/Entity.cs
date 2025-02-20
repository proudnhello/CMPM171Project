using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EntityInflictionEffectHandler;
using Unity.VisualScripting;
using System;
using Infliction = SoupSpoon.SpoonInfliction;

public class Entity : MonoBehaviour
{
    // ~~~ DEFINITIONS ~~~
    [Serializable]
    public struct BaseStats
    {
        public int maxHealth;
        public float baseMoveSpeed;
        public float invincibility;
    }
    public struct CurrentStats
    {
        public int health;
        public float moveSpeed;
    }

    // ~~~ VARIABLES ~~~
    [SerializeField] BaseStats baseStats;
    CurrentStats currentStats;
    internal EntityInflictionEffectHandler inflictionHandler;
    internal EntityRenderer entityRenderer;
    internal Rigidbody2D _rigidbody;

    public void InitEntity()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        inflictionHandler = new(this);
        ResetStats();
    }

    public void ResetStats()
    {
        currentStats.health = baseStats.maxHealth;
        currentStats.moveSpeed = baseStats.baseMoveSpeed;
    }

    public void ApplyInfliction(List<Infliction> spoonInflictions, Transform source)
    {
        inflictionHandler.ApplyInflictions(spoonInflictions, source);
    }

    public int GetHealth()
    {
        return currentStats.health;
    }  
    public virtual void ModifyHealth(int amount)
    {
        currentStats.health += amount;
        currentStats.health = Mathf.Clamp(currentStats.health, 0, baseStats.maxHealth);
    }

    public bool IsDead()
    {
        return currentStats.health <= 0;
    }

    public float GetMoveSpeed()
    {
        return currentStats.moveSpeed;
    }
    public void SetMoveSpeed(float newSpeed)
    {
        currentStats.moveSpeed = newSpeed;
    }

    public void ResetMoveSpeed()
    {
        currentStats.moveSpeed = baseStats.baseMoveSpeed;
    }

    public float GetInvincibility()
    {
        return baseStats.invincibility;
    }

}
