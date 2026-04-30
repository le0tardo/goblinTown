using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsBox;
    [SerializeField] GameObject goblinWalker;

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
        SceneManager.LoadScene("rtsScene");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void KillGoblin()
    {
        goblinWalker.SetActive(false);
    }
}
