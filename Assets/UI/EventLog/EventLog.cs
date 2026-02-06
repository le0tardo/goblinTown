using System.Collections;
using UnityEngine;
using TMPro;

public class EventLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI log;

    RectTransform rt;
    Vector2 showPosition;
    Vector2 hidePosition;

    bool hidden = false;

    Coroutine slide;

    [SerializeField] float slideDuration = 0.3f;

    private void Start()
    {
        rt = GetComponent<RectTransform>();

        showPosition = rt.localPosition;
        hidePosition = new Vector2(showPosition.x, -646f);
        log.text = "Welcome to Goblin Town!";
    }

    public void AddString(string str)
    {
        log.text += "<br>" + str;
    }

    public void ToggleHidden()
    {
        hidden = !hidden;

        // Stop previous slide if one is running
        if (slide != null)
            StopCoroutine(slide);

        slide = StartCoroutine(SlideRoutine(hidden));
    }

    IEnumerator SlideRoutine(bool hide)
    {
        Vector2 startPos = rt.localPosition;
        Vector2 targetPos = hide ? hidePosition : showPosition;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;

            rt.localPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        rt.localPosition = targetPos;

        slide = null;
    }
}
