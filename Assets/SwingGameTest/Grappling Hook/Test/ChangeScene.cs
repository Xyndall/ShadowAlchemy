using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadFreeScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadLimitScene()
    {
        SceneManager.LoadScene(1);
    }

}
