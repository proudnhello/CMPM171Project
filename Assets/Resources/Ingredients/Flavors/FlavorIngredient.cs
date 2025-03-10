using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "New Item", menuName = "Ingredient/New Flavor Ingredient")]
public class FlavorIngredient : Ingredient
{
    public string FlavorProfile;
    
    [Serializable]
    public class BuffFlavor
    {
        public enum BuffType
        {
            SOUR_Duration,
            SALTY_CriticalStrike,
            BITTER_Size,
            SWEET_Speed,
            UMAMI_Vampirism,
            REFRESHING_Cooldown,
        }
        public enum Operation
        {
            Add,
            Multiply
        }
        public BuffType buffType;
        public Operation operation;
        public float amount;
    }
    [Serializable]
    public class InflictionFlavor
    {
        public enum InflictionType
        {
            SPICY_Burn,
            FROSTY_Freeze,
            HEARTY_Health,
            SPIKY_Damage,
            GREASY_Knockback,
            UNAMI_Vampirism
        }
        public enum Operation
        {
            Add,
            Multiply
        }
        public InflictionType inflictionType;
        public Operation operation;
        public int amount;
        public float statusEffectDuration;

        public InflictionFlavor(InflictionFlavor other)
        {
            inflictionType = other.inflictionType;
            operation = other.operation;
            amount = other.amount;
            statusEffectDuration = other.statusEffectDuration;
        }

        public InflictionFlavor() { }
    }
    [Header("Flavors")]
    public List<BuffFlavor> buffFlavors;
    public List<InflictionFlavor> inflictionFlavors;

public static readonly Dictionary<BuffFlavor.BuffType, Color> buffColorMapping = new Dictionary<BuffFlavor.BuffType, Color>
{
    { BuffFlavor.BuffType.SOUR_Duration, Color.yellow },
    { BuffFlavor.BuffType.SALTY_CriticalStrike, new Color(0.65f, 0.16f, 0.16f) }, // Brownish
    { BuffFlavor.BuffType.BITTER_Size, new Color(0f, 1f, 0f) }, // Green
    { BuffFlavor.BuffType.SWEET_Speed, new Color(0.5f, 0f, 0.5f) }, // Purple
    { BuffFlavor.BuffType.UMAMI_Vampirism, new Color(0.5f, 0.25f, 0f) } // Brown
};

public static readonly Dictionary<InflictionFlavor.InflictionType, Color> inflictionColorMapping = new Dictionary<InflictionFlavor.InflictionType, Color>
{
    { InflictionFlavor.InflictionType.SPICY_Burn, Color.red },
    { InflictionFlavor.InflictionType.FROSTY_Freeze, new Color(0f, 1f, 1f) }, // Cyan
    { InflictionFlavor.InflictionType.HEARTY_Health, Color.green },
    { InflictionFlavor.InflictionType.SPIKY_Damage, new Color(1f, 0f, 1f) }, // Magenta
    { InflictionFlavor.InflictionType.GREASY_Knockback, new Color(0.55f, 0.27f, 0.07f) }, // SaddleBrown
    { InflictionFlavor.InflictionType.UNAMI_Vampirism, new Color(0.58f, 0, 0.82f) } // Purple
};

// --------------------- Dictionaries for localization below ------------------------
public static readonly Dictionary<InflictionFlavor.InflictionType, string> EnglishFlavors = new Dictionary<InflictionFlavor.InflictionType, string>{
    {InflictionFlavor.InflictionType.SPICY_Burn, "Burn"},
    {InflictionFlavor.InflictionType.FROSTY_Freeze, "Freeze"},
    {InflictionFlavor.InflictionType.HEARTY_Health, "Health"},
    {InflictionFlavor.InflictionType.SPIKY_Damage, "Damage"},
    {InflictionFlavor.InflictionType.GREASY_Knockback, "Knockback"},
    {InflictionFlavor.InflictionType.UNAMI_Vampirism, "Vampirism"}
};
public static readonly Dictionary<InflictionFlavor.InflictionType, string> PortugueseFlavors = new Dictionary<InflictionFlavor.InflictionType, string>{
    {InflictionFlavor.InflictionType.SPICY_Burn, "Queimadura"},
    {InflictionFlavor.InflictionType.FROSTY_Freeze, "Congelar"},
    {InflictionFlavor.InflictionType.HEARTY_Health, "Saúde"},
    {InflictionFlavor.InflictionType.SPIKY_Damage, "Dano"},
    {InflictionFlavor.InflictionType.GREASY_Knockback, "Recuo"},
    {InflictionFlavor.InflictionType.UNAMI_Vampirism, "Vampirismo"}
};
public static readonly Dictionary<InflictionFlavor.InflictionType, string> SpanishFlavors = new Dictionary<InflictionFlavor.InflictionType, string>{
    {InflictionFlavor.InflictionType.SPICY_Burn, "Quemadura"},
    {InflictionFlavor.InflictionType.FROSTY_Freeze, "Congelar"},
    {InflictionFlavor.InflictionType.HEARTY_Health, "Salud"},
    {InflictionFlavor.InflictionType.SPIKY_Damage, "Daño"},
    {InflictionFlavor.InflictionType.GREASY_Knockback, "Retroceso"},
    {InflictionFlavor.InflictionType.UNAMI_Vampirism, "Vampirismo"}
};

public static Dictionary<InflictionFlavor.InflictionType, string> inflictionTextMapping = new Dictionary<InflictionFlavor.InflictionType, string>{
    {InflictionFlavor.InflictionType.SPICY_Burn, "Burn"},
    {InflictionFlavor.InflictionType.FROSTY_Freeze, "Freeze"},
    {InflictionFlavor.InflictionType.HEARTY_Health, "Health"},
    {InflictionFlavor.InflictionType.SPIKY_Damage, "Damage"},
    {InflictionFlavor.InflictionType.GREASY_Knockback, "Knockback"},
    {InflictionFlavor.InflictionType.UNAMI_Vampirism, "Vampirism"}
};


// 3

// public static void InitializeInflictionTextMapping(UnityEngine.Localization.Locale locale)
// {
//     inflictionTextMapping.Clear();
//     Dictionary<InflictionFlavor.InflictionType, string> selectedFlavors;
//     switch (locale.LocaleName)
//     {
//         case "pt":
//             selectedFlavors = new Dictionary<InflictionFlavor.InflictionType, string>(PortugueseFlavors);
//             // inflictionTextMapping = PortugueseFlavors;
//             break;
//         case "es":
//             selectedFlavors = SpanishFlavors;
//             // inflictionTextMapping = SpanishFlavors;
//             break;
//         default:
//             selectedFlavors = EnglishFlavors;
//             // inflictionTextMapping = EnglishFlavors;
//             break;
//     }
//     foreach (var flavor in selectedFlavors)
//     {
//         Debug.Log(flavor.Key + " " + flavor.Value);
//         inflictionTextMapping.Add(flavor.Key, flavor.Value);
//         Debug.Log(inflictionTextMapping);
//     }
// }

// private void OnEnable()
// {
//     LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
//     if(LocalizationSettings.SelectedLocale != null)
//     {
//         inflictionTextMapping.Clear();
//         Dictionary<InflictionFlavor.InflictionType, string> selectedFlavors;
//         switch (LocalizationSettings.SelectedLocale.LocaleName)
//         {
//             case "pt":
//                 selectedFlavors = PortugueseFlavors;
//                 // inflictionTextMapping = PortugueseFlavors;
//                 break;
//             case "es":
//                 selectedFlavors = SpanishFlavors;
//                 // inflictionTextMapping = SpanishFlavors;
//                 break;
//             default:
//                 selectedFlavors = EnglishFlavors;
//                 // inflictionTextMapping = EnglishFlavors;
//                 break;
//         }
//         foreach (var flavor in selectedFlavors)
//         {
//             Debug.Log(flavor.Key + " " + flavor.Value);
//             inflictionTextMapping.Add(flavor.Key, flavor.Value);
//         }
//     }
//     // InitializeInflictionTextMapping(LocalizationSettings.SelectedLocale);
//     // switch (LocalizationSettings.SelectedLocale.LocaleName)
//     // {
//     //     case "en":
//     //         inflictionTextMapping = EnglishFlavors;
//     //         break;
//     //     case "pt":
//     //         inflictionTextMapping = PortugueseFlavors;
//     //         break;
//     //     case "es":
//     //         inflictionTextMapping = SpanishFlavors;
//     //         break;
//     //     default:
//     //         inflictionTextMapping = EnglishFlavors;
//     //         break;
//     // }

// }

}






