using UnityEngine;

public class WheatGrowth : MonoBehaviour
{
    [SerializeField] int foodToAdd;
    [SerializeField] VillageResource food;

    [SerializeField] float startPosY;
    [SerializeField] float endPosY;

    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale=Vector3.one;

    [SerializeField] float seasonProg;
    [SerializeField] float growEnd = 0.66f;

    WorkHouseBehaviour wh;
    bool harvested=false;

    private void Start()
    {
        float randomY = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, randomY, 0f);

        transform.localScale=Vector3.zero;

        wh=GetComponentInParent<WorkHouseBehaviour>();
        if (wh == null) print("no workhouse found!");
    }
    private void Update()
    {
        if (wh.needsWorker) return;

        seasonProg = SeasonManager.inst.seasonT;

        float growth = Mathf.Clamp01(seasonProg / growEnd);

        // When fully grown and not harvested yet
        if (growth >= 1f && !harvested)
        {
            harvested = true;
            AddFood();

            // Hide/reset wheat
            transform.localScale = Vector3.zero;
            return;
        }

        // Reset when new season starts
        if (seasonProg <= 0.01f && harvested)
        {
            harvested = false;
        }

        // Grow only if not harvested
        if (!harvested)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, growth);

            Vector3 pos = transform.localPosition;
            pos.y = Mathf.Lerp(startPosY, endPosY, growth);
            transform.localPosition = pos;
        }
    }

    void AddFood()
    {
        VillageResourceManager.inst.AddResource(food,foodToAdd);
    }
}
