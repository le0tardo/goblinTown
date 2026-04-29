using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsBox;

    private void Start()
    {
        settingsBox.SetActive(false);
    }

    public void ToggleSettingsBox()
    {
        settingsBox.SetActive(!settingsBox.activeInHierarchy);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("rtsScene");
    }
}
