using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static SoupSpoon;
using static FlavorIngredient;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public GameObject draggableItem;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public string ingredientType;

    [Header("Do Not Edit, Ingredient is Set In CookingUI's Enable()")]
    public Ingredient ingredient = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        // save the original parent
        parentAfterDrag = transform.parent;

        // bring the ingredient to the front of the scene while dragging
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // set raycast off so that when you drop on the slot
        // the drop system doesn't think you dropped it on itself
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // map item position to mouse position
        transform.position = Input.mousePosition;
        //Debug.Log(transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // set the parent to the parent after drag
        transform.SetParent(parentAfterDrag);

        // return raycast to true
        image.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"Mouse entered UI element {ingredient.ingredientName}!");

        CookingManager.Singleton.DisplayItemStats();
        GameObject itemStatsScreen = CookingManager.Singleton.itemStatsScreen;

        Transform background = itemStatsScreen.transform.Find("Background");
        Transform header = background.transform.Find("Header");
        Transform body = background.transform.Find("Body");

        // move the screen
        itemStatsScreen.transform.SetParent(this.transform);
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 actualSize = new Vector2(rt.rect.width * rt.lossyScale.x, rt.rect.height * rt.lossyScale.y);
        itemStatsScreen.transform.position = new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(actualSize.x / 2, -actualSize.y / 2);
        //Debug.Log(itemStatsScreen.transform.position);
        //Debug.Log(rt.rect.size);
        //Debug.Log(this.transform.position);

        // bring to the front
        itemStatsScreen.transform.SetParent(transform.root);
        itemStatsScreen.transform.SetAsLastSibling();


        // set text
        TextMeshProUGUI headerText = header.GetComponent<TextMeshProUGUI>();
        headerText.text = ingredient.IngredientName;  

        TextMeshProUGUI bodyText = body.GetComponent<TextMeshProUGUI>();

        if (ingredient.GetType() == typeof(AbilityIngredient))
        {
            AbilityIngredient abilityIngredient = ingredient as AbilityIngredient;
            bodyText.text = $"<color=purple>Ability Ingredient</color>\nType: {abilityIngredient.abilityType._abilityName}\n\n";

            foreach (InflictionFlavor inflictionFlavor in abilityIngredient.inherentInflictionFlavors)
            {
                string color = ColorUtility.ToHtmlStringRGB(FlavorIngredient.inflictionColorMapping[inflictionFlavor.inflictionType]);
                switch (inflictionFlavor.inflictionType)
                {
                    case FlavorIngredient.InflictionFlavor.InflictionType.SPICY_Burn:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0) {
                                bodyText.text += $"<color=#{color}>Spicy:</color> + {inflictionFlavor.amount}\n";
                            }
                        }
                        else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount > 1) {
                                bodyText.text += $"<color=#{color}>Spicy:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.FROSTY_Freeze:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0) {
                                bodyText.text += $"<color=#{color}>Frosty:</color> + {inflictionFlavor.amount}\n";
                            }
                        }
                        else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount > 1) {
                                bodyText.text += $"<color=#{color}>Frosty:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.HEARTY_Health:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0) {
                                bodyText.text += $"<color=#{color}>Hearty:</color> + {inflictionFlavor.amount}\n";
                            }
                        }
                        else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount > 1) {
                                bodyText.text += $"<color=#{color}>Hearty:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.SPIKY_Damage:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0) {
                                bodyText.text += $"<color=#{color}>Spiky:</color> + {inflictionFlavor.amount}\n";
                            }
                        }
                        else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount > 1) {
                                bodyText.text += $"<color=#{color}>Spiky:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.GREASY_Knockback:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0) {
                                bodyText.text += $"<color=#{color}>Greasy:</color> + {inflictionFlavor.amount}\n";
                            }
                        }
                        else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount > 1) {
                                bodyText.text += $"<color=#{color}>Greasy:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                }
            }

            bodyText.text += "\nBase Stats:\n";
            if (abilityIngredient.baseStats.duration > 0)
                bodyText.text += $"<color=#{ColorUtility.ToHtmlStringRGB(FlavorIngredient.buffColorMapping[FlavorIngredient.BuffFlavor.BuffType.SOUR_Duration])}>Sour (Duration):</color> {abilityIngredient.baseStats.duration}\n";
            if (abilityIngredient.baseStats.size > 0)
                bodyText.text += $"<color=#{ColorUtility.ToHtmlStringRGB(FlavorIngredient.buffColorMapping[FlavorIngredient.BuffFlavor.BuffType.BITTER_Size])}>Bitter (Size):</color> {abilityIngredient.baseStats.size}\n";
            if (abilityIngredient.baseStats.crit > 0)
                bodyText.text += $"<color=#{ColorUtility.ToHtmlStringRGB(FlavorIngredient.buffColorMapping[FlavorIngredient.BuffFlavor.BuffType.SALTY_CriticalStrike])}>Salty (Critical Strike):</color> {abilityIngredient.baseStats.crit}\n";
            if (abilityIngredient.baseStats.speed > 0)
                bodyText.text += $"<color=#{ColorUtility.ToHtmlStringRGB(FlavorIngredient.buffColorMapping[FlavorIngredient.BuffFlavor.BuffType.SWEET_Speed])}>Sweet (Speed):</color> {abilityIngredient.baseStats.speed}\n";
            bodyText.text += $"<color=blue>Cooldown:</color> {abilityIngredient.baseStats.cooldown}\n";

        } else if (ingredient.GetType() == typeof(FlavorIngredient))
        {
            FlavorIngredient flavorIngredient = ingredient as FlavorIngredient;
            bodyText.text = "<color=yellow>Flavor Ingredient</color>\n\n";

            foreach (InflictionFlavor inflictionFlavor in flavorIngredient.inflictionFlavors)
            {
                string color = ColorUtility.ToHtmlStringRGB(FlavorIngredient.inflictionColorMapping[inflictionFlavor.inflictionType]);
                switch (inflictionFlavor.inflictionType)
                {
                    case FlavorIngredient.InflictionFlavor.InflictionType.SPICY_Burn:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Spicy:</color> + {inflictionFlavor.amount}\n";
                            }
                        } else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount != 0 && inflictionFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Spicy:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.FROSTY_Freeze:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Frosty:</color> + {inflictionFlavor.amount}\n";
                            }
                        } else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount != 0 && inflictionFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Frosty:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.HEARTY_Health:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Hearty:</color> + {inflictionFlavor.amount}\n";
                            }
                        } else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount != 0 && inflictionFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Hearty:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.SPIKY_Damage:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Spiky:</color> + {inflictionFlavor.amount}\n";
                            }
                        } else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount != 0 && inflictionFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Spiky:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.InflictionFlavor.InflictionType.GREASY_Knockback:
                        if (inflictionFlavor.operation == InflictionFlavor.Operation.Add)
                        {
                            if (inflictionFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Greasy:</color> + {inflictionFlavor.amount}\n";
                            }
                        } else if (inflictionFlavor.operation == InflictionFlavor.Operation.Multiply)
                        {
                            if (inflictionFlavor.amount != 0 && inflictionFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Greasy:</color> x {inflictionFlavor.amount}\n";
                            }
                        }
                        break;
                }
            }

            foreach (BuffFlavor buffFlavor in flavorIngredient.buffFlavors)
            {
                string color = ColorUtility.ToHtmlStringRGB(FlavorIngredient.buffColorMapping[buffFlavor.buffType]);
                switch (buffFlavor.buffType)
                {
                    case FlavorIngredient.BuffFlavor.BuffType.BITTER_Size:
                        if (buffFlavor.operation == BuffFlavor.Operation.Add)
                        {
                            if (buffFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Bitter:</color> + {buffFlavor.amount}\n";
                            }
                        }
                        else if (buffFlavor.operation == BuffFlavor.Operation.Multiply)
                        {
                            if (buffFlavor.amount != 0 && buffFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Bitter:</color> x {buffFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.BuffFlavor.BuffType.SALTY_CriticalStrike:
                        if (buffFlavor.operation == BuffFlavor.Operation.Add)
                        {
                            if (buffFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Salty:</color> + {buffFlavor.amount}\n";
                            }
                        }
                        else if (buffFlavor.operation == BuffFlavor.Operation.Multiply)
                        {
                            if (buffFlavor.amount != 0 && buffFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Salty:</color> x {buffFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.BuffFlavor.BuffType.SOUR_Duration:
                        if (buffFlavor.operation == BuffFlavor.Operation.Add)
                        {
                            if (buffFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Sour:</color> + {buffFlavor.amount}\n";
                            }
                        }
                        else if (buffFlavor.operation == BuffFlavor.Operation.Multiply)
                        {
                            if (buffFlavor.amount != 0 && buffFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Sour:</color> x {buffFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.BuffFlavor.BuffType.UMAMI_Vampirism:
                        if (buffFlavor.operation == BuffFlavor.Operation.Add)
                        {
                            if (buffFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Cooldown:</color> + {buffFlavor.amount}\n";
                            }
                        }
                        else if (buffFlavor.operation == BuffFlavor.Operation.Multiply)
                        {
                            if (buffFlavor.amount != 0 && buffFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Cooldown:</color> x {buffFlavor.amount}\n";
                            }
                        }
                        break;
                    case FlavorIngredient.BuffFlavor.BuffType.SWEET_Speed:
                        if (buffFlavor.operation == BuffFlavor.Operation.Add)
                        {
                            if (buffFlavor.amount > 0){
                                bodyText.text += $"<color=#{color}>Sweet:</color> + {buffFlavor.amount}\n";
                            }
                        }
                        else if (buffFlavor.operation == BuffFlavor.Operation.Multiply)
                        {
                            if (buffFlavor.amount != 0 && buffFlavor.amount != 1){
                                bodyText.text += $"<color=#{color}>Sweet:</color> x {buffFlavor.amount}\n";
                            }
                        }
                        break;
                }
            }
        } else
        {
            Debug.LogError("Invalid Ingredient Type");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Mouse exited UI element {ingredient.ingredientName}!");
        CookingManager.Singleton.HideItemStats();

        GameObject itemStatsScreen = CookingManager.Singleton.itemStatsScreen;
        GameObject CookingCanvas = CookingManager.Singleton.CookingCanvas;
        itemStatsScreen.transform.SetParent(CookingManager.Singleton.CookingCanvas.transform);

    }

    public void OnDestroy()
    {
        Transform itemStatsScreenTransform = this.transform.Find("IngredientStats");
        if (itemStatsScreenTransform != null)
        {
            CookingManager.Singleton.HideItemStats();
            GameObject CookingCanvas = CookingManager.Singleton.CookingCanvas;
            itemStatsScreenTransform.SetParent(CookingManager.Singleton.CookingCanvas.transform);
        } 
    }

    public void OnDisable()
    {
        Transform itemStatsScreenTransform = this.transform.Find("IngredientStats");
        if (itemStatsScreenTransform != null)
        {
            CookingManager.Singleton.HideItemStats();
            GameObject CookingCanvas = CookingManager.Singleton.CookingCanvas;
            itemStatsScreenTransform.SetParent(CookingManager.Singleton.CookingCanvas.transform);
        }
    }

}
