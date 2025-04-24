using System.Collections;
using UnityEngine;

public class EndingStuff : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.EndGame();
    }

}
