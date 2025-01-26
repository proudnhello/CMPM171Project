using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject player;


    [Header("Keybinds")]
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode soupKey = KeyCode.Mouse1;
    public KeyCode drinkey = KeyCode.Space;
    [Header("Attack")]
    [SerializeField] private LayerMask enemies;
    [SerializeField] private int playerDamage = 10;
    [SerializeField] private float attackSpeed = 3;
    [SerializeField] private float attackDelay = 0;
    [SerializeField] private float attackRadius = 1.0f;

    [Header("Movement")]
    [SerializeField] float speed = 10.0f;

    [Header("Abilities")]
    [SerializeField] private List<AbilityAbstractClass> abilities;

    [Header("Soup")]
    [SerializeField] private AbilityLookup lookup;
    [SerializeField] private int maxPotSize = 100;
    List<(string, int)> pot = new List<(string, int)>();

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int health;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        health = maxHealth;
    }

    public int GetDamage()
    {
        return instance.playerDamage;
    }

    public void SetDamage(int newDamage)
    {
        instance.playerDamage = newDamage;
    }

    public float GetSpeed()
    {
        return instance.speed;
    }

    public void SetSpeed(float newSpeed)
    {
        instance.speed = newSpeed;
    }

    public float getAttackSpeed(){
        return attackSpeed;
    }

    public void setAttackSpeed(float newAttackSpeed){
        attackSpeed = newAttackSpeed;
    }

    public float getAttackDelay(){
        return attackDelay;
    }

    public void setAttackDelay(float newAttackDelay){
        attackDelay = newAttackDelay;
    }

    public List<AbilityAbstractClass> GetAbilities()
    {
        return instance.abilities;
    }

    public LayerMask GetEnemies()
    {
        return instance.enemies;
    }

    private int potFullness
    {
        get
        {
            int total = 0;
            foreach(var amount in pot)
            {
                total += amount.Item2;
            }
            return total;
        }
    }


    public static event Action<List<(string, int)>> SoupifyEnemy;
    // Add soup to the pot. If the pot is full, the soup will be wasted.
    public void AddToPot((string, int) soupVal)
    {
        print("soupVal: " + soupVal.Item2);
        print("name: " + soupVal.Item1);

        if (potFullness+soupVal.Item2 >= maxPotSize)
        {
            soupVal.Item2 = maxPotSize - potFullness;
        }
        if (soupVal.Item2 == 0)
        {
            return;
        }
        for (int i = 0; i < pot.Count; i++)
        {
            if (pot[i].Item1 == soupVal.Item1)
            {
                int newSoupVal = pot[i].Item2 + soupVal.Item2;
                pot[i] = (soupVal.Item1, newSoupVal);
                SoupifyEnemy?.Invoke(pot);
                return;
            }
        }
        pot.Add(soupVal);
        SoupifyEnemy?.Invoke(pot);
    }

    public static event Action DrinkPot;
    // Drink the soup in the pot and activate the abilities that correspond to the soup.
    public void Drink()
    {
        foreach((string, int) soup in pot)
        {
            print(soup.Item1 + " " + soup.Item2);
        }
        // You can't drink soup if the pot is empty
        if (pot.Count == 0)
        {
            return;
        }

        // First end all active abilities
        foreach (AbilityAbstractClass ability in abilities.ToList())
        {
            ability.End();
        }

        // Then drink the soup
        List<AbilityAbstractClass> drankAbilities = lookup.Drink(pot);
        foreach(AbilityAbstractClass ability in drankAbilities){
            print(ability._abilityName);
        }

        abilities.Clear();
        pot.Clear();
        foreach (AbilityAbstractClass ability in drankAbilities)
        {
            abilities.Add(ability);
        }
        DrinkPot?.Invoke();
    }

    public void RemoveAbility(AbilityAbstractClass ability)
    {
        abilities.Remove(ability);
    }

    public void SetHealth(int newHealth)
    {
        instance.health = (int)newHealth;
    }

    public int GetHealth()
    {
        return instance.health;
    }

    public int GetMaxHealth()
    {
        return instance.maxHealth;
    }

    public void Heal(int healAmount)
    {
        instance.health += healAmount;
        Debug.Log("Healing");
        if (instance.health > maxHealth)
        {
            instance.health = maxHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        instance.health -= damageAmount;
        Debug.Log("Taking damage");
        if (instance.health <= 0)
        {
            instance.health = 0;
            // Game over
            Debug.Log("Game Over womp womp");
        }
    }

    public float GetAttackRadius()
    {
        return instance.attackRadius;
    }
    public float SetAttackRadius(float newRadius)
    {
        instance.attackRadius = newRadius;
        return instance.attackRadius;
    }
}
