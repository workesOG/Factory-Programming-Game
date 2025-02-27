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
    public double buyPrice;
    public double licensePrice;
    public double sellPrice;

    [Header("Unlocking")]
    public bool unlocked;

    [Header("Recipe (Processed Only)")]
    public MaterialRecipe recipe;

    [HideInInspector]
    public int amount = 0;

    [Header("Order ID (Lowest displayed first)")]
    public int sortingID;

    public void Add(int number)
    {
        amount += number;
    }

    public void Remove(int number)
    {
        amount = Math.Max(amount - number, 0);
    }

    public void Set(int number)
    {
        amount = number;
    }
}

[Serializable]
public class MaterialRecipe
{
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
    public int outputAmount = 1;
    public float processingTime;
}

[Serializable]
public class RecipeIngredient
{
    public MaterialSO inputMaterial;
    public int inputAmount = 1;
}
