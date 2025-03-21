using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;
using static SoupSpoon;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Singleton { get; private set; }
    public static event Action UsedSpoon;
    public static event Action<int> ChangedSpoon;
    public static event Action<int> AddSpoon;
    public static event Action<int> RemoveSpoon;
    public int maxSpoons = 4;

    public List<Ingredient> defaultSpoonIngredients;

    //[SerializeField]
    //internal List<Ingredient> ingredientsHeld;

    [SerializeField]
    internal List<Collectable> collectablesHeld;

    [SerializeField]
    List<SoupSpoon> spoons;

    int currentSpoon = 0;

    void Awake()
    {
        if (Singleton == null) Singleton = this;
        spoons = new()
        {
            new SoupSpoon(defaultSpoonIngredients)
        };
        collectablesHeld = new();
    }

    public List<SoupSpoon> GetSpoons()
    {
        return spoons;
    }

    public int GetCurrentSpoon()
    {
        return currentSpoon;
    }

    private void Start()
    {
        PlayerEntityManager.Singleton.input.Player.UseSpoon.started += UseSpoon;
        PlayerEntityManager.Singleton.input.Player.CycleSpoon.started += CycleSpoons;
    }

    private void OnDisable()
    {
        PlayerEntityManager.Singleton.input.Player.UseSpoon.started -= UseSpoon;
        PlayerEntityManager.Singleton.input.Player.CycleSpoon.started -= CycleSpoons;
    }

    public void CollectIngredientCollectable(Collectable collectable)
    {
        collectablesHeld.Add(collectable);
        MetricsManager.Singleton.RecordIngredientCollected();
        BasketUI.Singleton.AddIngredient(collectable, true);
    }


    // Removes ingredient from the player inventory
    // By default it removes the first insance of an ingredient if there are multiple
    // set reverse to true to remove the last instance of the ingredient
    // (The collider under the basket calls it in reverse, the cook button calls it forward)
    public void RemoveIngredientCollectable(Collectable collectable, bool needsDestroy)
    {
        collectablesHeld.Remove(collectable);
        BasketUI.Singleton.RemoveIngredient(collectable, needsDestroy);
    }

    public bool CookSoup(List<Ingredient> ingredients)
    {
        if (spoons.Count == maxSpoons) return false;

        spoons.Add(new SoupSpoon(ingredients));
        currentSpoon = spoons.Count - 1;
        AddSpoon?.Invoke(currentSpoon);
        ChangedSpoon?.Invoke(currentSpoon);

        MetricsManager.Singleton.RecordSoupsCooked();

        return true;
    }

    void CycleSpoons(InputAction.CallbackContext ctx)
    {
        if (spoons.Count <= 1) return;

        if (ctx.ReadValue<float>() < 0)
        {
            currentSpoon--;
            currentSpoon = currentSpoon < 0 ? spoons.Count - 1 : currentSpoon;
        }
        else if(ctx.ReadValue<float>() > 4) //4 is the number of hotkeys
        {
            currentSpoon++;
            currentSpoon = currentSpoon >= spoons.Count ? currentSpoon = 0 : currentSpoon;
        } 
        else
        {
            //TODO: Add check for count
            currentSpoon = (int)ctx.ReadValue<float>() - 1 >= spoons.Count ? currentSpoon = currentSpoon : (int)ctx.ReadValue<float>() - 1;
        }
        ChangedSpoon?.Invoke(currentSpoon);
    }

    void UseSpoon(InputAction.CallbackContext ctx)
    {
        // Don't Use Spoon if In Cooking Screen
        if (CookingManager.Singleton.IsCooking())
        {
            return;
        }

        // Index into current spoon
        SoupSpoon spoon = spoons[currentSpoon];
        
        // See if spoon is on cooldown
        bool notOnCD = spoon.UseSpoon();

        if (!notOnCD)
        {
            return;
        }

        // Invoke That You are using a spoon
        // Audio and Animation are currently subscribed
        UsedSpoon?.Invoke();

        // check if any of the abilities have uses left
        bool noUsesLeft = true;
        foreach (SpoonAbility ability in spoon.spoonAbilities)
        {
            if (ability.GetUses() > 0 || ability.GetUses() == -1)
            {
                noUsesLeft = false;
            }
        }


        // remove spoon if no uses left
        if (noUsesLeft)
        {
            spoons.RemoveAt(currentSpoon);
            RemoveSpoon?.Invoke(currentSpoon);
            currentSpoon--;
            currentSpoon = currentSpoon < 0 ? spoons.Count - 1 : currentSpoon;
        }

        // Invoke the changed spoon event to indicate it has changed
        ChangedSpoon?.Invoke(currentSpoon);
    }
}