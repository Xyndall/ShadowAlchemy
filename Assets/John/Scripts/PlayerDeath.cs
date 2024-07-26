using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool isAlive;

    void _PlayerDeath()
    {
        if (isAlive)
        {
            this.gameObject.SetActive(false);
            //Play animation for death
            //Pause Game
            //Show death screen
        }
    }
}
