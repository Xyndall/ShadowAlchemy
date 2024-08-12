using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public bool isAlive;
    public LayerMask Death;
    private void Update()
    {
        _PlayerDeath();
    }
    void _PlayerDeath()
    {
        //when the player is inside a deathzone i.e enemy light, natural light, etc
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, Death))
        {
            Debug.Log("death");
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }


        if (!isAlive)
        {
            Debug.Log("Over");
            this.gameObject.SetActive(false);
            //Play animation for death
            //Pause Game
            //Show death screen
            SceneManager.LoadScene(1);
        }
    }

    // Need to set up a collider for the enemy
    // Put the collider into an enemy layer
    
}
