using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MachinesManager : MonoBehaviour
{
    private Transform machinesWindow;
    private List<GameObject> loadedMachineShopElements = new();

    public List<MachineSO> machines = new List<MachineSO>();

    private static MachinesManager _instance;
    public static MachinesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MachinesManager>();
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        machines = Resources.LoadAll<MachineSO>("SO/Machines").ToList();
        machines = machines.OrderBy(x => x.sortingID).ToList();
    }

    public void Initialize(Transform windowTransform)
    {
        machinesWindow = windowTransform.Find("Materials Window/Main Scroll/Viewport/Content");

        LoadMachines();
    }

    public MachineSO GetMachine(string name)
    {
        return machines.Find(x => x.machineName == name);
    }

    private void LoadMachines()
    {
        foreach (GameObject go in loadedMachineShopElements)
            Destroy(go);
        loadedMachineShopElements.Clear();

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Machine Shop Element");
        foreach (MachineSO machine in machines)
        {
            GameObject go = Instantiate(prefab, machinesWindow);
            MachineShopElement component = go.GetComponent<MachineShopElement>();
            component.Initialize(machine);
            loadedMachineShopElements.Add(go);
        }
    }
}
