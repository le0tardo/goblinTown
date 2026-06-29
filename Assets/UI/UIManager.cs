using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private void Awake()
    {
        if (pauseMenu.activeInHierarchy)pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
