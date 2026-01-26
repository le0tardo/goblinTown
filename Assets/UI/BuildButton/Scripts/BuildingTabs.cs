using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTabs : MonoBehaviour
{
    [SerializeField] Button[] tabs;
    Button currentTab;

    [SerializeField] Sprite selected;
    [SerializeField] Sprite unselected;

    [SerializeField]float show_y;
    [SerializeField]float hide_y;
    [SerializeField] float slideT=0.25f;

    Coroutine slideRoutine;

    RectTransform tabsRect;
    bool shown = true;

    private void Start()
    {
        tabsRect = GetComponent<RectTransform>();
        foreach (var tab in tabs)
        {
            Image img=tab.GetComponent<Image>();
            img.sprite=unselected;
            RectTransform rt=tab.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100,100);
        }

        currentTab = tabs[0];
        Image curImg = currentTab.GetComponent<Image>();
        curImg.sprite=selected;
        RectTransform rtc = currentTab.GetComponent<RectTransform>();
        rtc.sizeDelta = new Vector2(110, 110);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleTabs(!shown);
            shown = !shown;
        }
    }
    public void SwapTab(int n)
    {
        if (n > tabs.Length) return;

        if (tabs[n] == currentTab && shown)
        {
            ToggleTabs(!shown);
            shown = !shown;
            return;
        }
        else
        {
            if (!shown)
            {
                ToggleTabs(true);
                shown = true;
            }
        }

        foreach (var tab in tabs)
        {
            Image img = tab.GetComponent<Image>();
            img.sprite = unselected;
            RectTransform rt = tab.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
        }

        currentTab = tabs[n];
        Image curImg = currentTab.GetComponent<Image>();
        curImg.sprite = selected;
        RectTransform rtc = currentTab.GetComponent<RectTransform>();
        rtc.sizeDelta = new Vector2(110, 110);
    }
    public void ToggleTabs(bool show)
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        float targetY = show ? show_y : hide_y;
        slideRoutine = StartCoroutine(Slide(targetY));
    }

    IEnumerator Slide(float targetY)
    {
        float startY = tabsRect.anchoredPosition.y;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / slideT;

            float y = Mathf.Lerp(startY, targetY, t);
            tabsRect.anchoredPosition = new Vector2(
                tabsRect.anchoredPosition.x,
                y
            );

            yield return null;
        }

        // snap cleanly at the end
        tabsRect.anchoredPosition = new Vector2(
            tabsRect.anchoredPosition.x,
            targetY
        );
    }
}
