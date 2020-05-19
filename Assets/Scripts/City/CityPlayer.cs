using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class CityPlayer : MonoBehaviour
//---------------------------------------------------------------
{
    public CityHandler cityHandler;

    private float speedBooster;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        speedBooster = 2.0f;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        //Game is paused
        //---------------------------------------------------------------
        if (cityHandler.Paused())
        {
            //---------------------------------------------------------------
            //Player start position, glide in when countdown
            //---------------------------------------------------------------
            transform.position = new Vector3(-0.039f, 0.843f + (cityHandler.GetCountdown() * 0.03f), 0f);
            return;
        }

        //---------------------------------------------------------------
        //Check for city collision
        //---------------------------------------------------------------
        if (transform.position.y < -0.779f)
        {
            //---------------------------------------------------------------
            //Check for wall collision
            //---------------------------------------------------------------
            if (transform.position.x < -0.251 ||
               transform.position.x > 0.198)
            {
                Debug.Log("Player hit the wall");
                cityHandler.ShowTrade(true);
            }

            //---------------------------------------------------------------
            //Check for inside city
            //---------------------------------------------------------------
            if (transform.position.y < -0.946f)
            {
                cityHandler.ShowTrade(false);
            }
        }

        //---------------------------------------------------------------
        //Check for ship collision
        //---------------------------------------------------------------
        else
        {
            Vector2Int playerPos = cityHandler.CalculateTileCoordinates(transform.position);

            if (cityHandler.ObstaclesCollision(playerPos.x, playerPos.y))
            {
                Debug.Log("Player hit an obstacle at position: " + playerPos.x + "," + playerPos.y);
                cityHandler.ShowTrade(true);
            }
        }

        //---------------------------------------------------------------
        //Always move ship down
        //---------------------------------------------------------------
        transform.Translate(0, speedBooster * -0.00075f, 0);

        //---------------------------------------------------------------
        //Player input
        //---------------------------------------------------------------
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");
        if (xDirection > 0.1f)
        {
            transform.Translate(speedBooster * 0.001f, 0, 0);
        }
        if (xDirection < -0.1f)
        {
            transform.Translate(speedBooster * -0.001f, 0, 0);
        }

        if (yDirection > 0.1f)
        {
            transform.Translate(0, speedBooster * 0.0005f, 0);
        }
        if (yDirection < -0.1f)
        {
            transform.Translate(0, speedBooster * -0.0005f, 0);
        }

        //---------------------------------------------------------------
        //Wind effect
        //---------------------------------------------------------------
        transform.Translate(speedBooster * cityHandler.GetWind(), 0, 0);

    }

}
