using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PauseFreeze : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
