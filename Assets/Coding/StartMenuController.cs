using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("Sample Level");
    }

    public void OnOptionsClick()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnHowtoClick()
    {
        SceneManager.LoadScene("HowMenu");
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Start");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}