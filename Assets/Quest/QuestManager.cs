using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager inst;

    public int questProgress;
    [SerializeField] string questText="";

    [SerializeField] VillageResource food;
    [SerializeField] VillageResource wood;
    int foodCount;
    int woodCount;
    int unitCount;
    int toolLevel;

    [SerializeField] TextMeshProUGUI UItext;

    private void Awake()
    {
        inst = this;
    }

    private void Update()
    {
        UItext.text = questText;

        switch (questProgress)
        {
            case 0:
                questText = "Gather Food!<br>Goblins need to eat. Go pick some berries and place them in the Storage Hut.";
                VillageResourceManager.inst.villageResources.TryGetValue(food,out foodCount);
                if (foodCount > 0)
                {
                    questProgress++;
                }

            break;
            case 1:
                questText = "More Wood!<br>Gather 5 Wood, and place it in the Storage Hut.";
                VillageResourceManager.inst.villageResources.TryGetValue(wood, out woodCount);
                if (woodCount >= 5)
                {
                    questProgress++;
                }
            break;
            case 2:
                questText = "More Goblins!<br>Build a house and spawn a Goblin friend.";
                unitCount =UnitManager.inst.units.Count;
                if (unitCount > 1)
                {
                    questProgress++;
                }
            break;

            case 3:
                questText = "Tools!<br>Build a Stone Cutter to unlock tools to mine and chop stuff.";
                toolLevel =EquipmentManager.inst.toolLevel;
                if (toolLevel > 0) { questProgress++; }
            break;

            case 4:
                questText = "Get 100 Goblins in your village";
                unitCount = UnitManager.inst.units.Count;
                if (unitCount >= 100)
                {
                    questProgress++;
                }
                break;
            case 5:
                questText = "Congratulations to your 100 Goblins!";
            break;
        }
    }
}
