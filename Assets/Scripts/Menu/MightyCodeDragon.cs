using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class MightyCodeDragon : MonoBehaviour
//---------------------------------------------------------------
{
    public Sprite spriteIconActive;
    public Sprite spriteIconDeActive;

    private bool inside;
    private bool playOnce;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        inside = false;
        playOnce = true;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //Open mightyCodeDragon
        if(inside && Input.GetMouseButtonDown(0))
        {
            inside = false;
            Application.OpenURL("http://mightycodedragon.com");
        }
    }

    //---------------------------------------------------------------
    void OnMouseEnter()
    //---------------------------------------------------------------
    {
        inside = true;
        GetComponent<SpriteRenderer>().sprite = spriteIconActive;

        if(playOnce)
        {
            playOnce = false;
            GetComponent<AudioSource>().Play();

        }
    }

    //---------------------------------------------------------------
    private void OnMouseExit()
    //---------------------------------------------------------------
    {
        inside = false;
        GetComponent<SpriteRenderer>().sprite = spriteIconDeActive;
    }

}
