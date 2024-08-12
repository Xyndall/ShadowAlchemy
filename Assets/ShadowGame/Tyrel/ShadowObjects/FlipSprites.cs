using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprites : MonoBehaviour
{

    public GameObject OutlineSprite;

    public void OutlineOn()
    {
        OutlineSprite.SetActive(true);
    }
    public void OutlineOff()
    {
        OutlineSprite.SetActive(false);
    }

}
