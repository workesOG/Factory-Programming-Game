using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> gameMaterials;
    private Transform materialWindow;
    private Dictionary<Material.Tier, Button> buttonMap;

    private List<GameObject> loadedMaterialShopElements = new();

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
        materials = gameMaterials;
    }

    public void Initialize(Transform windowTransform)
    {
        materialWindow = windowTransform.Find("Materials Window/Main Scroll/Viewport/Content");
        buttonMap = new Dictionary<Material.Tier, Button>
        {
            { Material.Tier.Tier1, windowTransform.Find("Categories/Tier 1").GetComponent<Button>()},
            { Material.Tier.Tier2, windowTransform.Find("Categories/Tier 2").GetComponent<Button>()},
            { Material.Tier.Tier3, windowTransform.Find("Categories/Tier 3").GetComponent<Button>()},
        };
        foreach (var kvp in buttonMap)
        {
            kvp.Value.onClick.AddListener(() => { LoadMaterials(kvp.Key); });
        }
        LoadMaterials(Material.Tier.Tier1);
    }

    private void LoadMaterials(Material.Tier category)
    {
        foreach (GameObject go in loadedMaterialShopElements)
            Destroy(go);

        loadedMaterialShopElements.Clear();

        List<Material> materialsToDisplay = materials.Where(x => x.tier == category).ToList();
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Material Shop Element");
        foreach (Material material in materialsToDisplay)
        {
            GameObject go = Instantiate(prefab, materialWindow);
            MaterialShopElement component = go.GetComponent<MaterialShopElement>();
            component.Initialize(material);
            loadedMaterialShopElements.Add(go);
        }

        foreach (var kvp in buttonMap)
        {
            if (kvp.Key == category)
                kvp.Value.interactable = false;
            else
                kvp.Value.interactable = true;
        }
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
    public double buyPrice;
    public double sellPrice;
    public bool unlocked;
    public double licensePrice;
    public Tier tier;
    public Sprite icon;
    public Color iconColor;

    public int amount;
}