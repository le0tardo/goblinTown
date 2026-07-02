using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsBox;
    [SerializeField] GameObject goblinWalker;

    [SerializeField] GameObject loadingScreen;

    private void Start()
    {
        settingsBox.SetActive(false);
        Invoke("KillGoblin",3f);
    }

    public void ToggleSettingsBox()
    {
        settingsBox.SetActive(!settingsBox.activeInHierarchy);
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("rtsScene");
        StartCoroutine(LoadSceneAsyncCoroutine("rtsScene"));
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    void KillGoblin()
    {
        goblinWalker.SetActive(false);
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // 1. Show the loading screen with your animated wheel
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 2. Start loading the scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 3. Keep pausing this function until the scene is fully loaded
        while (!operation.isDone)
        {
            // Optional: If you want a loading bar instead of just a wheel, 
            // operation.progress gives you a float from 0 to 1.
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null; // Wait for the next frame, letting the loading wheel animate
        }
    }
}
