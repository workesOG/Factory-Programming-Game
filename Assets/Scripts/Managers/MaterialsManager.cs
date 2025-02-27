using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsManager : MonoBehaviour
{
    private Transform materialWindow;
    private Dictionary<MaterialType, Button> buttonMap;
    private List<GameObject> loadedMaterialShopElements = new List<GameObject>();

    public List<MaterialSO> materials = new List<MaterialSO>();

    private static MaterialsManager _instance;
    public static MaterialsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MaterialsManager>();
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        materials = Resources.LoadAll<MaterialSO>("SO/Materials").ToList();
        materials = materials.OrderBy(x => x.sortingID).ToList();
        ResetMaterials();
    }

    public void Initialize(Transform windowTransform)
    {
        materialWindow = windowTransform.Find("Materials Window/Main Scroll/Viewport/Content");

        buttonMap = new Dictionary<MaterialType, Button>
        {
            { MaterialType.Raw, windowTransform.Find("Categories/Raw").GetComponent<Button>() },
            { MaterialType.Processed, windowTransform.Find("Categories/Processed").GetComponent<Button>() }
        };

        foreach (var kvp in buttonMap)
        {
            MaterialType type = kvp.Key;
            kvp.Value.onClick.AddListener(() => { LoadMaterials(type); });
        }

        LoadMaterials(MaterialType.Raw);
    }

    public MaterialSO GetMaterial(string name)
    {
        return materials.Find(x => x.materialName == name);
    }

    private void LoadMaterials(MaterialType type)
    {
        foreach (GameObject go in loadedMaterialShopElements)
            Destroy(go);
        loadedMaterialShopElements.Clear();

        List<MaterialSO> materialsToDisplay = materials.Where(x => x.materialType == type).ToList();

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Material Shop Element");
        foreach (MaterialSO mat in materialsToDisplay)
        {
            GameObject go = Instantiate(prefab, materialWindow);
            MaterialShopElement component = go.GetComponent<MaterialShopElement>();
            component.Initialize(mat);
            loadedMaterialShopElements.Add(go);
        }

        foreach (var kvp in buttonMap)
            kvp.Value.interactable = kvp.Key != type;
    }

    private void ResetMaterials()
    {
        foreach (MaterialSO mat in materials)
        {
            mat.amount = 0;
            if (mat.name != "Pine Logs")
                mat.unlocked = false;

            UpdateProcessedMaterialsUnlockStatus();
        }
    }

    public void UpdateProcessedMaterialsUnlockStatus()
    {
        foreach (MaterialSO mat in materials)
        {
            if (mat.materialType == MaterialType.Processed && mat.recipe != null)
            {
                bool allIngredientsUnlocked = true;
                foreach (RecipeIngredient ingredient in mat.recipe.ingredients)
                {
                    if (ingredient.inputMaterial == null || !ingredient.inputMaterial.unlocked)
                    {
                        allIngredientsUnlocked = false;
                        break;
                    }
                }
                mat.unlocked = allIngredientsUnlocked;
            }
        }
    }
}
