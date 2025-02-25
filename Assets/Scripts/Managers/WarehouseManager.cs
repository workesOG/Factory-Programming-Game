using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarehouseManager : MonoBehaviour
{
    private Transform rawMaterialsWindow;
    private Transform processedMaterialsWindow;
    private GameObject warehouseListElementPrefab;
    private Dictionary<string, GameObject> loadedWarehouseListElements = new();

    private static WarehouseManager _instance;
    public static WarehouseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<WarehouseManager>();
            }

            return _instance;
        }
    }

    public void Initialize(Transform windowTransform)
    {
        rawMaterialsWindow = windowTransform.Find("Background/Raw/Main Scroll/Viewport/Content");
        processedMaterialsWindow = windowTransform.Find("Background/Processed/Main Scroll/Viewport/Content");
        warehouseListElementPrefab = Resources.Load<GameObject>("Prefabs/Warehouse List Element");
        loadedWarehouseListElements = new();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (AppManager.Instance.currentAppID == 4)
        {

            foreach (MaterialSO material in MaterialsManager.Instance.materials)
            {
                if (loadedWarehouseListElements.ContainsKey(material.name))
                    return;
                if (material.amount > 0)
                {
                    Debug.Log($"{material.name}: {material.amount}");
                    loadedWarehouseListElements[material.name] = Instantiate(warehouseListElementPrefab, material.materialType == MaterialType.Raw ? rawMaterialsWindow : processedMaterialsWindow);
                    loadedWarehouseListElements[material.name].GetComponent<WarehouseListElement>().Initialize(material);
                }
            }
        }
    }
}
