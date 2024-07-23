using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
