using System.Collections;
using UnityEngine;

public class GuardSpearScript : MonoBehaviour
{
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void ThrowSpear(Vector3 targetPosition)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ThrowRoutine(targetPosition));
    }

    private IEnumerator ThrowRoutine(Vector3 targetPosition)
    {
        float duration = 1f;

        float elapsed = 0f;
        Vector3 from = startPosition;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        elapsed = 0f;
        from = targetPosition;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, startPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;

        gameObject.SetActive(false);
    }
}