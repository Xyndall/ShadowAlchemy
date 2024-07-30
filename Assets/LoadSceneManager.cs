using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(1);
    }

}
