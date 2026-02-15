using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager inst;

    public int questProgress;
    [SerializeField] string questText="";

    private void Awake()
    {
        inst = this;
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
