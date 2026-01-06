using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageResourceSlot : MonoBehaviour
{
    [SerializeField]Image img;
    [SerializeField]TextMeshProUGUI txt;
    public void Set(VillageResource resource) //both resource and cap
    {
        var village = VillageResourceManager.inst;

        village.villageResources.TryGetValue(resource, out int amount);
        village.villageCaps.TryGetValue(resource, out int cap);

        img.sprite = resource.resourceIcon;
        txt.text = $"{amount}/{cap}";
    }
}
