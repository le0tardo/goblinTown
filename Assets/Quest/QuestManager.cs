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
                questText = "Gather Food! Go pick some berries and place them in the Storage Hut.";
                VillageResourceManager.inst.villageResources.TryGetValue(food,out foodCount);
                if (foodCount > 0)
                {
                    questProgress++;
                }

            break;
            case 1:
                questText = "Gather 5 Wood, and place it in the Storage Hut";
                VillageResourceManager.inst.villageResources.TryGetValue(wood, out woodCount);
                if (woodCount >= 5)
                {
                    questProgress++;
                }
            break;
            case 2:
                questText = "Build a house and spawn a Goblin friend";
                unitCount=UnitManager.inst.units.Count;
                if (unitCount > 1)
                {
                    questProgress++;
                }
            break;

            case 3:
                questText = "Get 100 Goblins in your village";
                unitCount = UnitManager.inst.units.Count;
                if (unitCount >= 100)
                {
                    questProgress++;
                }
                break;
            case 4:
                questText = "Congratulations to your 100 Goblins!";
            break;
        }
    }
    void UpdateQuestProgress()
    {
        switch (questProgress)
        {
            case 0:
                questText = "Gather 5 wood from the ground";
            break;
            case 1:
                questText = "Build a Small Stick Hut, to deposit your foraged materials in";
            break;
            case 2:
                questText = "Gather 10 food items. Goblins are very stupid and can only carry 1 kind of resource at a time.";
            break;
            case 3:
                questText = "Build a Stick Tent and spawn a new Goblin.";
            break;
        }
    }
}
