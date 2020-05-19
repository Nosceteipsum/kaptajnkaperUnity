using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class Cloud : MonoBehaviour
//---------------------------------------------------------------
{
    public float moveSpeed = -0.00005f;
    public Sprite[] spriteImages;

    private float moveSpeedXOffset;
    private float moveSpeedYOffset;

    private bool usingWind = false;
    private bool inShipBattle = false;
    private bool inMainMenu = false;
    private float wind;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        Init();

        //Start random around the screen
        //transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 3f), 0);

    }

    //---------------------------------------------------------------
    public void Init()
    //---------------------------------------------------------------
    {
        //Choose random clouds image
        this.GetComponent<SpriteRenderer>().sprite = spriteImages[Random.Range(0, spriteImages.Length)];

        //Random movement bost
        moveSpeedXOffset = Random.Range(0.0f,0.0001f);
        moveSpeedYOffset = Random.Range(0.0f,0.0001f);

        //Waterbattle scene
        if (inShipBattle)
            transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(0f, 3f), 0);

        //Main menu
        else if (inMainMenu)
            transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1.5f), 0);

        //City
        else if (usingWind)
        {
            transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-1f, 1f), 0);
        }

        else //Map overview
            //Move outside screen
            // X -1 to 4
            // Y  2 to 3
            transform.position = new Vector3(Random.Range(-1f, 5f), Random.Range(-1f, 3f), 0);

    }

    //---------------------------------------------------------------
    public void SetMainMenu()
    //---------------------------------------------------------------
    {
        inMainMenu = true;
    }

    //---------------------------------------------------------------
    public void SetWindBattleScene()
    //---------------------------------------------------------------
    {
        inShipBattle = true;
    }

    //---------------------------------------------------------------
    public void SetWind(float windSpeed)
    //---------------------------------------------------------------
    {
        wind = windSpeed;
        usingWind = true;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        if(inShipBattle == true)
        {
            transform.Translate(0.0f, -0.0005f - moveSpeedYOffset, 0);
        }
        else if(usingWind == false)
        {
            transform.Translate(moveSpeed - moveSpeedXOffset, moveSpeed - moveSpeedYOffset, 0);
        }
        else
        {
            //---------------------------------------------------------------
            //Using wind (harbor game)
            //---------------------------------------------------------------
            transform.Translate(wind, moveSpeed - moveSpeedYOffset, 0);
        }

        //---------------------------------------------------------------
        //Check if outside bounds
        //---------------------------------------------------------------
        if(inMainMenu == true && transform.position.y < -1.2f)
        {
            transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(1.0f, 1.5f), 0);
        }
        else if(usingWind && (transform.position.x < -3.0f || transform.position.x >  3.0f))
        {
            if(wind > 0.0f)
            {
                transform.position = new Vector3(Random.Range(-2.9f, -2.9f), Random.Range(-1.0f, 1.0f), 0);
            }
            else
            {
                transform.position = new Vector3(Random.Range( 2.9f, 2.9f), Random.Range(-1.0f, 1.0f), 0);
            }
        }
        else if (transform.position.y < -3)
        {
            if (inShipBattle == true)
                transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(1.5f, 3f), 0);
            else
                Init();
        }
    }
}
