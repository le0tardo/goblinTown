using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTabs : MonoBehaviour
{
    [SerializeField] Button[] tabs;
    Button currentTab;

    [SerializeField] Sprite selected;
    [SerializeField] Sprite unselected;
    [SerializeField] Color unselectedColor;

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
            rt.sizeDelta = new Vector2(110,110);

            Image ico = tab.transform.GetChild(0).GetComponent<Image>();
            ico.color = unselectedColor;
            RectTransform icoRt=ico.GetComponent<RectTransform>();
            icoRt.sizeDelta = new Vector2(45, 45);

            TabMenuToggler menu=tab.GetComponent<TabMenuToggler>();
            if (menu != null)menu.menu.SetActive(false);
        }

        currentTab = tabs[0];

        Image curImg = currentTab.GetComponent<Image>();
        curImg.sprite=selected;
        RectTransform rtc = currentTab.GetComponent<RectTransform>();
        rtc.sizeDelta = new Vector2(125, 125);
       
        Image currIco = currentTab.transform.GetChild(0).GetComponent<Image>();
        currIco.color = Color.white;
        RectTransform currIcoRt=currIco.GetComponent<RectTransform>();
        currIcoRt.sizeDelta = new Vector2(55, 55);

        TabMenuToggler currMenu = currentTab.GetComponent<TabMenuToggler>();
        if (currMenu != null) currMenu.menu.SetActive(true);
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
            rt.sizeDelta = new Vector2(110, 110);

            Image ico = tab.transform.GetChild(0).GetComponent<Image>();
            ico.color = unselectedColor;
            RectTransform icoRt = ico.GetComponent<RectTransform>();
            icoRt.sizeDelta = new Vector2(45, 45);

            TabMenuToggler menu = tab.GetComponent<TabMenuToggler>();
            if (menu != null) menu.menu.SetActive(false);
        }

        currentTab = tabs[n];
        Image curImg = currentTab.GetComponent<Image>();
        curImg.sprite = selected;
        RectTransform rtc = currentTab.GetComponent<RectTransform>();
        rtc.sizeDelta = new Vector2(125, 125);

        Image currIco = currentTab.transform.GetChild(0).GetComponent<Image>();
        currIco.color = Color.white;
        RectTransform currIcoRt = currIco.GetComponent<RectTransform>();
        currIcoRt.sizeDelta = new Vector2(55, 55);

        TabMenuToggler currMenu = currentTab.GetComponent<TabMenuToggler>();
        if (currMenu != null) currMenu.menu.SetActive(true);
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
        if (!shown)
        {
            HideAll();
        }
    }

    void HideAll()
    {
        foreach (var tab in tabs)
        {
            TabMenuToggler tabTog = tab.GetComponent<TabMenuToggler>();
            if(tabTog!=null)tabTog.menu.SetActive(false);

        }
    }
}
