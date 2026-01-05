using System.Collections.Generic;
using UnityEngine;

public class VillageResourceManager : MonoBehaviour
{
    public static VillageResourceManager inst { get; private set; }
    Dictionary<MacroResourceCategory, int> macroResources = new Dictionary<MacroResourceCategory, int>();
    [SerializeField] VillageResourceGUI gui;
    [SerializeField] MacroResourceCategory[] macroResourceList;

    void Awake()
    {
        if (inst != null && inst != this)
        {
            Destroy(gameObject);
            return;
        }
        inst = this;

        foreach(var macResource in macroResourceList)
        {
            macroResources.Add(macResource, 0);
        }
    }

    private void Start()
    {
        gui.Initialize(macroResources);
    }

    public void AddResource(MacroResourceCategory data, int amount)
    {
        macroResources[data] += amount;

        gui.UpdateResource(data, macroResources[data]);
    }
}
