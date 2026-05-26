using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] int unitCount;
    [SerializeField]int foodCount;
    [SerializeField] VillageResource foodResource;

    [SerializeField] bool gameOver;

    private void Awake()
    {
        if (gameOverScreen.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        unitCount=UnitManager.inst.units.Count;
        if (VillageResourceManager.inst.villageResources.ContainsKey(foodResource))
        {
            foodCount = VillageResourceManager.inst.villageResources[foodResource];
        }

        if (unitCount == 0 && foodCount < 10)
        {
            if (!gameOver)
            {
                GameOverTrigger();
                gameOver=true;
            }
        }
    }

    void GameOverTrigger()
    {
        gameOverScreen.SetActive(true);
    }
}
