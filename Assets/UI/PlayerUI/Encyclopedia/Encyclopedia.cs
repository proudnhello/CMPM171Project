using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] LayerMask ClickableLayers;
    [SerializeField] GameObject RenderedObject;
    [SerializeField] TMP_Text Title;
    [SerializeField] Image EntryImage;
    [SerializeField] TMP_Text SourceText;
    [SerializeField] TMP_Text FlavorEntry;
    [SerializeField] TMP_Text AbilityEntry;
    [SerializeField] TextMeshPro FlavorTooltip;


    List<Ingredient> collectedEntries;
    void Start()
    {
        collectedEntries = new();
        PlayerEntityManager.Singleton.input.Player.Encyclopedia.started += PressEncyclopediaButton;
        RenderedObject.SetActive(false);
        FlavorTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckTextHover();
    }

    private void OnDisable()
    {
        PlayerEntityManager.Singleton.input.Player.Encyclopedia.started -= PressEncyclopediaButton;
    }

    void PressEncyclopediaButton(InputAction.CallbackContext ctx)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0, ClickableLayers);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Ingredient"))
            {
                var ing = hit.collider.gameObject.GetComponent<CollectableUI>();
                if (ing) 
                {
                    PullUpEntry(ing.GetCollectable().ingredient);
                    return;
                }
            }
        }

        RenderedObject.SetActive(false);
    }

    void PullUpEntry(Ingredient ing)
    {
        if (!collectedEntries.Contains(ing)) collectedEntries.Add(ing);
        Title.text = ing.IngredientName;
        EntryImage.sprite = ing.EncyclopediaImage;
        SourceText.text = ing.Source;
        if (ing.GetType() == typeof(FlavorIngredient))
        {
            FlavorEntry.text = ((FlavorIngredient)ing).FlavorProfile;
            FlavorEntry.gameObject.SetActive(true);
            AbilityEntry.gameObject.SetActive(false);
        } 
        else // is AbilityIngredient
        {
            AbilityEntry.text = ((AbilityIngredient)ing).AbilityDescription;
            AbilityEntry.gameObject.SetActive(true);
            FlavorEntry.gameObject.SetActive(false);
        }

        RenderedObject.SetActive(true);


    }

    private void CheckTextHover()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isHovering = TMP_TextUtilities.IsIntersectingRectTransform(FlavorEntry.rectTransform, mousePos, Camera.main);
        if (isHovering)
        {
            DisplayFlavorTooltip();
        }
        else
        {
            Debug.Log("Not displaying tooltip");
            //FlavorTooltip.gameObject.SetActive(false);
        }
        
    }
    public void DisplayFlavorTooltip()
    {
        //FlavorTooltip.gameObject.SetActive(true);
        Debug.Log("Displaying tooltip");
        // FlavorTooltip.text = flavor;
    }
}
