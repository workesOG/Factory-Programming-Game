using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialsManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> gameMaterials;

    [HideInInspector]
    public List<Material> materials;

    private static MaterialsManager _instance;
    public static MaterialsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MaterialsManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

[Serializable]
public class Material
{
    public enum Tier
    {
        Tier1,
        Tier2,
        Tier3
    }

    public string name;
    public double purchasePrice;
    public double sellPrice;
    public bool unlocked;
    public double licensePrice;
    public Tier tier;
    public Sprite icon;
    public Color iconColor;

    public int amount;
}

