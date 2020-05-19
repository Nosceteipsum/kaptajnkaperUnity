using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------
public class GameHandler : MonoBehaviour
//---------------------------------------------------------------
{
    public int amountOfEnemyShips;
    public int amountOfClouds;
    public int amountOfDebris;
    public GameObject shipEnemy;
    public ShipPlayer shipPlayer;
    public GameObject cloud;
    public GameObject debris;
    public GameObject gameCity;
    public CityHandler gameCityHandler;
    public GameObject gameMap;
    public GameObject gameCanvasResource;
    public Event eventHandler;

    //Player start resource
    public int gold;
    public int corn;
    public int pirates;
    public int cannons;
    public int capturedShip;
    public int turn;

    //Player interface
    public Text uiCorn;
    public Text uiGold;
    public Text uiPirates;
    public Text uiCannons;
    public Text uiLevel;
    public Text uiTurn;
    public Image uiCapturesShip;

    //Hidden fields
    private int MapTileSize = 25;
    private List<ShipAI> enemies;
    private List<Debris> debrisList;

    //---------------------------------------------------------------
    // Map collision 
    //---------------------------------------------------------------
    private int[,] collisionMap = new int[,]
        {

        //          5        10        15        20        25    28
	    { 1,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1 }, //  0
	    { 1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1 }, //
	    { 1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,0,1,1,1 }, //
	    { 1,1,1,0,1,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1 }, //
	    { 1,1,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1 }, //
	    { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,1 }, //  5
	    { 1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,5,0,1,1,1 }, //
	    { 1,1,1,0,0,1,0,0,0,0,0,0,1,1,0,0,0,6,1,1,1,1,1,1,0,0,1,1,1 }, //
	    { 1,1,0,0,0,1,0,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,1,0,1,0,1,1 }, //
	    { 0,1,0,0,0,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1 }, //
	    { 1,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1 }, // 10
	    { 1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,7,0,0,0,0 }, //
	    { 0,0,0,0,0,0,0,0,0,0,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0 }, //
	    { 0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 }, // 13
		};

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        enemies = new List<ShipAI>();
        debrisList = new List<Debris>();

        //---------------------------------------------------------------
        //Spawn random enemy ships
        //---------------------------------------------------------------
        for (int i=0;i < amountOfEnemyShips;i++)
        {
            SpawnEnemy();
        }

        //---------------------------------------------------------------
        //Spawn clouds on map
        //---------------------------------------------------------------
        SpawnClouds();

        //---------------------------------------------------------------
        //Spawn debris on map
        //---------------------------------------------------------------
        for (int i = 0; i < amountOfDebris; i++)
        {
            SpawnDebris();
        }

    }

    //---------------------------------------------------------------
    public void SpawnClouds()
    //---------------------------------------------------------------
    {
        //---------------------------------------------------------------
        //Spawn random clouds on map
        //---------------------------------------------------------------
        for (int i = 0; i < amountOfClouds; i++)
        {
            Instantiate(cloud, gameMap.transform);
        }
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
        //Update UI
        //---------------------------------------------------------------
        UpdateUI();
    }

    //------------------------------------------------------
    public void SpawnDebris()
    //------------------------------------------------------
    {
        int randomX;
        int randomY;

        //---------------------------------------------------------------
        //Find spawn place
        //---------------------------------------------------------------
        do
        {
            randomX = Random.Range(0, 29);
            randomY = Random.Range(0, 14);


        }
        while (MapCheckCollision(randomX, randomY) == 0 || ShipCollision(randomX, randomY) == true || DebrisCollision(randomX, randomY) == true);
        var debrisObject = GameObject.Instantiate(debris, gameObject.transform.localPosition, Quaternion.identity, gameMap.transform);
        debrisObject.transform.position = CalculateMapCoordinates(randomX, randomY);
        Debris debrisInstance = debrisObject.GetComponent<Debris>();
        debrisInstance.posX = randomX;
        debrisInstance.posY = randomY;
        debrisList.Add(debrisInstance);
    }

    //------------------------------------------------------
    public void SpawnEnemy()
    //------------------------------------------------------
    {
        int randomX;
        int randomY;

        //Find spawn place
        do
        {
            randomX = Random.Range(0, 29);
            randomY = Random.Range(0, 14);
        }
        while (MapCheckCollision(randomX, randomY) == 0 || ShipCollision(randomX,randomY) == true);

        var enemy = GameObject.Instantiate(shipEnemy, gameObject.transform.localPosition, Quaternion.identity, gameMap.transform);
        enemy.transform.position = CalculateMapCoordinates(randomX,randomY);
        ShipAI shipAI = enemy.GetComponent<ShipAI>();
        shipAI.gameHandler = this;
        shipAI.posX = randomX;
        shipAI.posY = randomY;
        shipAI.level = Random.Range(0, (GetLevel() + 2)); //0 - (OwnLevel + 1)
        shipAI.SetSprite(4); //Set start sprite
        enemies.Add(shipAI);
    }

    //------------------------------------------------------
    public void VisitCity(int city)
    //------------------------------------------------------
    {
        gameCity.SetActive(true);
        gameCanvasResource.SetActive(false);
        gameCityHandler.SetCity(city);
        gameCityHandler.Init();
        gameMap.SetActive(false);
    }

    //------------------------------------------------------
    public void NewTurn()
    //------------------------------------------------------
    {
        //------------------------------------------------------
        //Increase turn
        //------------------------------------------------------
        turn++;

        //------------------------------------------------------
        // Starving
        //------------------------------------------------------
        if (corn <= 0)
        {
            pirates--;
        }

        //------------------------------------------------------
        // GameOver
        //------------------------------------------------------
        if (pirates <= 0)
        {
            eventHandler.ActivateEventGameOver();
        }

        //------------------------------------------------------
        // Calculate Food
        //------------------------------------------------------
        if (corn > 0)
        {
            int iFoodUse = (pirates / 10) + 1;
            corn -= Random.Range(0, iFoodUse + 1);
            if (corn < 0) corn = 0;
        }

        //------------------------------------------------------
        // Move AI
        //------------------------------------------------------
        foreach(var ai in enemies)
        {
            ai.MoveAI(shipPlayer.posX, shipPlayer.posY);
        }

    }

    //---------------------------------------------------------------
    public Vector2 CalculateMapCoordinates(int x, int y)
    //---------------------------------------------------------------
    {
        // MapPixelSize:         725.0   x   350
        // MapPixelHalfSize:     362.5   x   175
        // Unity UI leftbutton:   -3.622 x    -1.7261

        Vector2 result = new Vector2(-362.5f * 0.01f, 175.0f * 0.01f);
        result += new Vector2(10 * 0.01f, 10 * 0.01f); // Ship offset (20x20)
        result += new Vector2(-1 * 0.01f, (-MapTileSize + 3) * 0.01f); // map offset
        result += new Vector2(x * MapTileSize * 0.01f, -y * MapTileSize * 0.01f);
        return result;
    }

    //---------------------------------------------------------------
    public int GetLevel()
    //---------------------------------------------------------------
    {
        return shipPlayer.level;
    }

    //---------------------------------------------------------------
    public void UpgradeShipLevel()
    //---------------------------------------------------------------
    {
        shipPlayer.level++;
    }

    //---------------------------------------------------------------
    public void UpdateUI()
    //---------------------------------------------------------------
    {
        uiCorn.text = corn.ToString("D5");
        uiGold.text = gold.ToString("D5");
        uiPirates.text = pirates.ToString("D5");
        uiCannons.text = cannons.ToString("D5");
        uiLevel.text = shipPlayer.level.ToString();
        uiTurn.text = turn.ToString("D3");

        if(capturedShip != -1)
        {
            uiCapturesShip.enabled = true;
        }
        else
        {
            uiCapturesShip.enabled = false;
        }

    }

    //------------------------------------------------------
    public int MapCheckCollision(int iX, int iY)
    //------------------------------------------------------
    {
        if (iX < 0 || iY < 0) return 0;
        if (iX > 28 || iY > 13) return 0;

        if (0 == collisionMap[iY, iX]) return 1;
        if (1 < collisionMap[iY, iX]) return collisionMap[iY, iX];

        return 0;
    }

    //------------------------------------------------------
    public bool DebrisCollision(int iX, int iY)
    //------------------------------------------------------
    {
        foreach (var debris in debrisList)
        {
            if (debris.posX == iX)
            {
                if (debris.posY == iY)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //------------------------------------------------------
    public void RemoveEnemyShip()
    //------------------------------------------------------
    {
        //------------------------------------------------------
        //Remove enemy ship at player location
        //------------------------------------------------------
        var enemyShip = enemies.Where(d => d.posX == shipPlayer.posX && d.posY == shipPlayer.posY).FirstOrDefault();
        if(enemyShip != null)
        {
            enemies.Remove(enemyShip);
            Destroy(enemyShip.gameObject);

            //------------------------------------------------------
            //Spawn a new enemy
            //------------------------------------------------------
            SpawnEnemy();
        }
        else
        {
            Debug.LogError("No enemy found on player pos");
        }
    }

    //------------------------------------------------------
    public void RemoveDebris(int iX, int iY)
    //------------------------------------------------------
    {
        //Remove at position
        var debris = debrisList.Where(d => d.posX == iX && d.posY == iY).FirstOrDefault();
        debrisList.Remove(debris);
        Destroy(debris.gameObject);

        //------------------------------------------------------
        //Spawn a new debris
        //------------------------------------------------------
        SpawnDebris();
    }

    //------------------------------------------------------
    public ShipAI ShipGetEnemy(int iX, int iY)
    //------------------------------------------------------
    {
        foreach (var ai in enemies)
        {
            if (ai.posX == iX)
            {
                if (ai.posY == iY)
                {
                    return ai;
                }
            }
        }

        return null;
    }

    //------------------------------------------------------
    public bool ShipCollision(int iX, int iY, bool includePlayer = true)
    //------------------------------------------------------
    {
        // Enemy ships
        foreach (var ai in enemies)
        {
            if (ai.posX == iX)
            {
                if (ai.posY == iY)
                {
                    return true;
                }
            }
        }

        if(includePlayer)
        {
            // Kaper ships
            if (shipPlayer.posX == iX)
            {
                if (shipPlayer.posY == iY)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
