using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//---------------------------------------------------------------
public class CityHandler : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject player;
    public GameObject obstacles;
    public GameObject cloud;
    public GameHandler gameHandler;
    public Event eventHandler;
    public int amountOfObstacles;

    private List<Vector2Int> listObstacles;
    private float countDown;
    private float wind;
    private int startCityEvent;
    private int cityID;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        Init();
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        // Quit game
        //---------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!eventHandler.EventActive())
        {
            //---------------------------------------------------------------
            //Countdown before game start
            //---------------------------------------------------------------
            if(countDown > 0)
            {
                countDown -= Time.deltaTime;
            }

            //---------------------------------------------------------------
            //Start trade event
            //---------------------------------------------------------------
            if (startCityEvent != 0)
            {
                eventHandler.ActivateEvent_CityTrade(startCityEvent);
            }
        }
    }

    //---------------------------------------------------------------
    public void SetCity(int id)
    //---------------------------------------------------------------
    {
        cityID = id;
    }

    //---------------------------------------------------------------
    public void Init()
    //---------------------------------------------------------------
    {
        startCityEvent = 0;

        //---------------------------------------------------------------
        //Countdown before game start
        //---------------------------------------------------------------
        countDown = 3.0f;

        //---------------------------------------------------------------
        //Randomize wind
        //---------------------------------------------------------------
        wind = Random.Range(-0.0008f, 0.0008f);

        //---------------------------------------------------------------
        //Show event box
        //---------------------------------------------------------------
        eventHandler.ActivateEvent_OK("Harbor", "Get your ship to the port. \n Strong wind from: " + ((wind > 0.0f) ? "west" : "east"));

        //---------------------------------------------------------------
        //Player start position
        //---------------------------------------------------------------
        player.transform.position = new Vector3(-0.039f, 0.843f, 0f);

        //---------------------------------------------------------------
        //Clear previous Clouds and obstacles
        //---------------------------------------------------------------
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("CityObstacle");
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        //---------------------------------------------------------------
        //Spawn clouds (to show wind)
        //---------------------------------------------------------------
        for (int i = 0; i < amountOfObstacles; i++)
        {
            SpawnClouds();
        }

        //---------------------------------------------------------------
        //Clear/Init obstacles
        //---------------------------------------------------------------
        if (listObstacles == null)
        {
            listObstacles = new List<Vector2Int>();
        }
        else
        {
            listObstacles.Clear();
        }

        //---------------------------------------------------------------
        //Spawn obstacles
        //---------------------------------------------------------------
        for (int i = 0; i < amountOfObstacles; i++)
        {
            SpawnObstacles();
        }

    }

    //------------------------------------------------------
    public void ShowTrade(bool crashed)
    //------------------------------------------------------
    {
        if(crashed)
        {
            //Crash punish
            int lostPirates = (gameHandler.pirates / 2);
            gameHandler.cannons--;
            gameHandler.pirates -= lostPirates;
            gameHandler.UpdateUI();

            //Show message
            eventHandler.ActivateEvent_OK("Crashed", "You hit an obstacle on your way and lost some resources.\nPirates died: " + lostPirates + "\nCannon destroyed: 1");
        }
        else if(gameHandler.capturedShip != -1)
        {
            eventHandler.ActivateEvent_CapturedShip();
        }

        startCityEvent = cityID;
    }

    //------------------------------------------------------
    public float GetWind()
    //------------------------------------------------------
    {
        return wind;
    }

    //------------------------------------------------------
    public float GetCountdown()
    //------------------------------------------------------
    {
        return countDown;
    }

    //------------------------------------------------------
    public bool Paused()
    //------------------------------------------------------
    {
        return eventHandler.EventActive() || countDown > 0.0f;
    }

    //------------------------------------------------------
    private void SpawnClouds()
    //------------------------------------------------------
    {
        var gameObject = Instantiate(cloud, transform.parent);
        Cloud cloudInstance = gameObject.GetComponent<Cloud>();
        cloudInstance.SetWind(wind);
    }

    //------------------------------------------------------
    private void SpawnObstacles()
    //------------------------------------------------------
    {
        int randomX;
        int randomY;

        //Find spawn place
        do
        {
            randomX = Random.Range(0, 36);
            randomY = Random.Range(4, 17);
        }
        while (listObstacles.Any(o => o.x == randomX && o.y == randomY) == true);

        var obstacleObject = Instantiate(obstacles, transform.parent);
        obstacleObject.transform.position = CalculateCityCoordinates(randomX, randomY);
        listObstacles.Add(new Vector2Int(randomX,randomY));
    }

    //---------------------------------------------------------------
    private Vector2 CalculateCityCoordinates(int x, int y)
    //---------------------------------------------------------------
    {
        int MapTileSize = 9;
        // MapPixelSize:         320.0   x   180
        // MapPixelHalfSize:     160.0   x    90

        Vector2 result = new Vector2(-160.0f * 0.01f, 90.0f * 0.01f);
        result += new Vector2(4 * 0.01f, 4 * 0.01f); // Ship offset (9x9)
        result += new Vector2(x * MapTileSize * 0.01f, -y * MapTileSize * 0.01f);
        return result;
    }

    //---------------------------------------------------------------
    public Vector2Int CalculateTileCoordinates(Vector2 pos)
    //---------------------------------------------------------------
    {
        int MapTileSize = 9;
        // MapPixelSize:         320.0   x   180
        // MapPixelHalfSize:     160.0   x    90

        Vector2 result = new Vector2(160.0f * 0.01f, 90.0f * 0.01f);
        result += new Vector2(4.5f * 0.01f,4.5f * 0.01f); //Ship Offset
        result += new Vector2(pos.x,-pos.y);
        result *= 100f;
        result /= MapTileSize;
        Vector2Int resultInt = new Vector2Int((int)result.x, (int)result.y);

        //Debug draw
        /*
        if ((((int)(Time.deltaTime*100)) % 100) == 0)
        {
            var obstacleObject = Instantiate(obstacles, transform.parent);
            obstacleObject.transform.position = CalculateCityCoordinates(resultInt.x, resultInt.y);
        }
        */

        return resultInt;
    }

    //------------------------------------------------------
    public bool ObstaclesCollision(int iX, int iY)
    //------------------------------------------------------
    {
        // Enemy ships
        foreach (var obstacle in listObstacles)
        {
            if (obstacle.x == iX)
            {
                if (obstacle.y == iY)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
