using UnityEngine;
using System;
using System.Collections.Generic;

public enum MaterialType { Raw, Processed }

[CreateAssetMenu(fileName = "NewMaterial", menuName = "Game/Material")]
public class MaterialSO : ScriptableObject
{
    [Header("General Info")]
    public string materialName;
    public MaterialType materialType;
    public Sprite icon;
    public Color iconColor;

    [Header("Pricing")]
    // For raw materials:
    public double buyPrice;
    public double licensePrice;
    // For processed materials:
    public double sellPrice;

    [Header("Unlocking")]
    // For raw materials, this is unlocked by purchasing the license.
    // For processed materials, this is updated automatically based on recipe ingredients.
    public bool unlocked;

    [Header("Recipe (Processed Only)")]
    // Leave this null for raw materials.
    public MaterialRecipe recipe;
}

[Serializable]
public class MaterialRecipe
{
    // List of ingredients required to process this material.
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

    // The amount of processed material produced from one processing action.
    public int outputAmount = 1;

    // Processing time (in seconds) needed to complete the conversion.
    public float processingTime;
}

[Serializable]
public class RecipeIngredient
{
    // The required input material.
    public MaterialSO inputMaterial;
    // The amount required of that input material.
    public int inputAmount = 1;
}
