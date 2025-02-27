using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMachine", menuName = "Game/Machine")]
public class MachineSO : ScriptableObject
{
    [Header("General Info")]
    public string machineName;
    public Color iconColor;

    [Header("Pricing")]
    public double price;

    [Header("Unlocking")]
    public bool unlocked;

    [Header("Order ID (Lowest displayed first)")]
    public int sortingID;
}