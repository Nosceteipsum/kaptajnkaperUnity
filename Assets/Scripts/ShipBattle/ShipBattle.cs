using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class ShipBattle : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject background1;
    public GameObject background2;
    public GameObject cloud;
    public GameObject resourceEnemy;
    public BattleShipEnemy shipEnemy;
    public BattleShipPlayer shipPlayer;
    public GameHandler gameHandler;
    public Event eventHandler;

    public int amountOfClouds;

    private float wind;
    private int enemyLevel;
    private float prepare;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        Init(0);
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

        //---------------------------------------------------------------
        //Prepare ships
        //---------------------------------------------------------------
        if (prepare > 0.0f)
        {
            prepare -= Time.deltaTime;
            shipPlayer.transform.position = new Vector3(0.0f, -0.56f - prepare * 0.2f, 0.0f);
            shipPlayer.SetPause(true);

            shipEnemy.transform.position = new Vector3(0.0f, 0.7f + prepare * 0.2f, 0.0f);
            shipEnemy.SetPause(true);
        }
        else if(eventHandler.EventActive())
        {
            //Do nothing when event active
            shipEnemy.SetPause(true);
            shipPlayer.SetPause(true);
        }
        else
        {
            shipPlayer.SetPause(false);
            shipEnemy.SetPause(false);

            //---------------------------------------------------------------
            //Check if enemy is dead
            //---------------------------------------------------------------
            if (shipEnemy.pirates <= 0)
            {
                //---------------------------------------------------------------
                //Remove ship from map
                //---------------------------------------------------------------
                gameHandler.RemoveEnemyShip();

                //---------------------------------------------------------------
                //Show event box
                //---------------------------------------------------------------
                resourceEnemy.SetActive(false);
                eventHandler.ActivateEventBattleShipWon(enemyLevel);
            }

            //---------------------------------------------------------------
            //Check if player is dead
            //---------------------------------------------------------------
            if (gameHandler.pirates <= 0)
            {
                resourceEnemy.SetActive(false);
                eventHandler.ActivateEventGameOver();
            }

            //---------------------------------------------------------------
            //Check if player is fleeing
            //---------------------------------------------------------------
            if (shipPlayer.transform.position.y < -0.8f)
            {
                //---------------------------------------------------------------
                //Show event box
                //---------------------------------------------------------------
                resourceEnemy.SetActive(false);
                eventHandler.ActivateEventBattleShipFlee();
            }

            //---------------------------------------------------------------
            //Check if a ship is boarding
            //---------------------------------------------------------------
            if (Vector3.Distance(shipPlayer.transform.position, shipEnemy.transform.position) <= 0.15)
            {
                //---------------------------------------------------------------
                //Show event box
                //---------------------------------------------------------------
                resourceEnemy.SetActive(false);
                eventHandler.ActivateEventBattleShipBoard(shipEnemy.pirates, enemyLevel);
            }
        }

        //---------------------------------------------------------------
        //Move background
        //---------------------------------------------------------------
        background1.transform.Translate(0, -0.0005f, 0);
        background2.transform.Translate(0, -0.0005f, 0);

        //---------------------------------------------------------------
        //Background refresh position
        //---------------------------------------------------------------
        if(background2.transform.position.y <= 0.0f)
        {
            background1.transform.position = new Vector3(0, 0, 0);
            background2.transform.position = new Vector3(0, 1.751f, 0);
        }

    }

    //---------------------------------------------------------------
    public void Init(int shipEnemyLevel)
    //---------------------------------------------------------------
    {
        prepare = 3.0f;
        enemyLevel = shipEnemyLevel;

        //---------------------------------------------------------------
        //Show enemy resources
        //---------------------------------------------------------------
        resourceEnemy.SetActive(true);

        //---------------------------------------------------------------
        //Set enemy resources
        //---------------------------------------------------------------
        int resourceCannon  = (gameHandler.cannons * 30) / 100;
        int resourcePirates = (gameHandler.pirates * 30) / 100;

        if (shipEnemyLevel == 0)
        {
            shipEnemy.cannons = Random.Range(0,  3) + 1 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 10) + 5 + resourcePirates;
        }

        else if (shipEnemyLevel == 1)
        {
            shipEnemy.cannons = Random.Range(0,  5) +  2 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 21) + 15 + resourcePirates;
        }
        else if (shipEnemyLevel == 2)
        {
            shipEnemy.cannons = Random.Range(0, 14) + 13 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 26) + 35 + resourcePirates;
        }
        else if (shipEnemyLevel == 3)
        {
            shipEnemy.cannons = Random.Range(0, 26) + 25 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 71) + 40 + resourcePirates;
        }
        else if (shipEnemyLevel == 4)
        {
            shipEnemy.cannons = Random.Range(0,  51) +  30 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 351) + 100 + resourcePirates;
        }
        else if (shipEnemyLevel == 5)
        {
            shipEnemy.cannons = Random.Range(0, 101) + 100 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 501) + 500 + resourcePirates;
        }
        else //(shipEnemyLevel == 6)
        {
            shipEnemy.cannons = Random.Range(0, 101) + 50 + resourceCannon;
            shipEnemy.pirates = Random.Range(0, 501) + 50 + resourcePirates;
        }

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
        wind = Random.Range(-0.001f,0.001f);
        for (int i = 0; i < amountOfClouds; i++)
        {
            SpawnClouds();
        }
    }

    //------------------------------------------------------
    private void SpawnClouds()
    //------------------------------------------------------
    {
        var gameObject = Instantiate(cloud, transform.parent);
        Cloud cloudInstance = gameObject.GetComponent<Cloud>();
        cloudInstance.SetWindBattleScene();
        cloudInstance.Init();
    }

}
