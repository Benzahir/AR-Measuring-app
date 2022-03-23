using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
     [SerializeField] private GameObject panelDelete;

    private void Start()
    {
        panelDelete.gameObject.SetActive(false);
    }

    public void StartApplication(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GoHistory(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GoManual(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DeleteObject()
    {
        panelDelete.gameObject.SetActive(true);
    }

    public void CloseDeleteObject()
    {
        panelDelete.gameObject.SetActive(false);
    }

}
