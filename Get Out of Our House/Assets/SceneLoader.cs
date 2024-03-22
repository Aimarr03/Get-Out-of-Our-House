using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
