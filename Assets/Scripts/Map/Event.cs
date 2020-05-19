using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//---------------------------------------------------------------
public class Event : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject eventAttackShip;
    public GameObject eventOK;
    public GameObject eventCityTrade;
    public GameObject gameCity;
    public GameObject gameCanvasResource;
    public GameObject gameMap;
    public GameObject gameShipBattle;
    public GameObject gameBoardBattle;
    public GameHandler gameHandler;
    public new GameObject camera;

    public AudioClip clipEventSound;
    public AudioClip clipBuySound;

    public Text eventOK_title;
    public Text eventOK_description;
    public Text eventAttacShip_title;
    public Text eventAttackShip_description;
    public Text eventCityTrade_Title;
    public Text eventCityTrade_FoodBuy;
    public Text eventCityTrade_FoodSell;
    public Text eventCityTrade_PiratesBuy;
    public Text eventCityTrade_PiratesSell;
    public Text eventCityTrade_CannonsBuy;
    public Text eventCityTrade_CannonsSell;
    public Text eventCityTrade_UpgradeShip;

    private int price_foodbuy;
    private int price_foodsell;
    private int price_piratesbuy;
    private int price_piratessell;
    private int price_cannonsbuy;
    private int price_cannonssell;

    private int shipEnemyLvl;
    private bool gameOver;
    private bool boardStartBattle;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        gameOver = false;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {

    }

    //---------------------------------------------------------------
    private void HideAll()
    //---------------------------------------------------------------
    {
        eventOK.SetActive(false);
        eventCityTrade.SetActive(false);
        gameObject.SetActive(false);
    }

    //---------------------------------------------------------------
    public bool EventActive()
    //---------------------------------------------------------------
    {
        return gameObject.activeSelf;
    }

    //---------------------------------------------------------------
    public void ActivateEvent_CapturedShip()
    //---------------------------------------------------------------
    {
        int gold = 0;

        //----------------//
        //  Captured Ship //
        //----------------//
        if (gameHandler.capturedShip == 0)
        {
            gold = 50;
        }
        if (gameHandler.capturedShip == 1)
        {
            gold = 100;
        }
        if (gameHandler.capturedShip == 2)
        {
            gold = 200;
        }
        if (gameHandler.capturedShip == 3)
        {
            gold = 300;
        }
        if (gameHandler.capturedShip == 4)
        {
            gold = 400;
        }
        if (gameHandler.capturedShip == 5)
        {
            gold = 700;
        }
        if (gameHandler.capturedShip == 6)
        {
            gold = 150;
        }

        //---------------------------------------------------------------
        //Increase resources
        //---------------------------------------------------------------
        gameHandler.gold += gold;
        gameHandler.capturedShip = -1;
        CalculateMaxCapacity();
        gameHandler.UpdateUI();

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);
        boardStartBattle = false;

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = "Captured ship";
        eventOK_description.text = "You sold your captured ship for " + gold + " gold.";
    }

    //---------------------------------------------------------------
    public void ActivateEventBattleShipBoard(int pirates,int lvl)
    //---------------------------------------------------------------
    {
        string title = "Boarding";
        string description = "Prepare for close combat!";
        ClickBoardShip(pirates, lvl);
        boardStartBattle = true;
        gameShipBattle.SetActive(false);

        //---------------------------------------------------------------
        //Show Gui
        //---------------------------------------------------------------
        ActivateEvent_OK(title, description);
    }

    //---------------------------------------------------------------
    public void ActivateEvent_AttackShip(string textTitle, string textDescription)
    //---------------------------------------------------------------
    {
        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventAttackShip.SetActive(true);

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventAttacShip_title.text = textTitle;
        eventAttackShip_description.text = textDescription;
    }

    //---------------------------------------------------------------
    public void ActivateEventBattleShipFlee()
    //---------------------------------------------------------------
    {
        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = "Flee from combat";
        eventOK_description.text = "You escaped from the battle.";
    }

    //---------------------------------------------------------------
    public void ActivateEventBattleBoardWon(int enemyLevel)
    //---------------------------------------------------------------
    {
        int food = 0;
        int gold = 0;
        int cannons = 0;
        bool capturedShip = true;

        ///////////////////////////
        // CLOSE_COMBAT WON
        ///////////////////////////
        if (0 == enemyLevel)
        {
            gold = 20;
            food = 5;
            cannons = 1;
        }
        else if (1 == enemyLevel)
        {
            gold = 30;
            food = 7;
            cannons = 1;
        }
        else if (2 == enemyLevel)
        {
            gold = 40;
            food = 10;
            cannons = 1;
        }
        else if (3 == enemyLevel)
        {
            gold = 50;
            food = 20;
            cannons = 2;
        }
        else if (4 == enemyLevel)
        {
            gold = 100;
            food = 50;
            cannons = 3;
        }
        else if (5 == enemyLevel)
        {
            gold = 300;
            food = 150;
            cannons = 10;
        }
        else if (6 == enemyLevel)
        {
            gold = 200;
            food = 50;
            cannons = 2;
        }
        else // Non-ship attack
        {
            gold = 150;
            food = 50;
            capturedShip = false; // false; Hårde ordre fra Jonas
        }

        //---------------------------------------------------------------
        //Increase resources
        //---------------------------------------------------------------
        gameHandler.corn += food;
        gameHandler.gold += gold;
        gameHandler.cannons += cannons;
        if(capturedShip)
        {
            gameHandler.capturedShip = enemyLevel;
        }

        CalculateMaxCapacity();
        gameHandler.UpdateUI();

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);
        boardStartBattle = false;

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = "You won";

        if(capturedShip)
        {
            eventOK_description.text = "You captured a ship and salvaged resources\nFood: " + food + " Gold: " + gold + " Cannons: " + cannons;
        }
        else
        {
            eventOK_description.text = "You survived and salvaged resources\nFood: " + food + " Gold: " + gold;
        }
    }

    //---------------------------------------------------------------
    public void ActivateEventBattleShipWon(int enemyLevel)
    //---------------------------------------------------------------
    {
        int food = 0;
        int gold = 0;
        int pirates = 0;

        ///////////////////////////
        // RANGE_COMBAT WON
        ///////////////////////////
        if (0 == enemyLevel)
        {
            food = 5;
            gold = 10;
            pirates = 1;
        }
        if (1 == enemyLevel)
        {
            food = 7;
            gold = 15;
            pirates = 2;
        }
        if (2 == enemyLevel)
        {
            food = 10;
            gold = 20;
            pirates = 3;
        }
        if (3 == enemyLevel)
        {
            food = 15;
            gold = 25;
            pirates = 5;
        }
        if (4 == enemyLevel)
        {
            food = 20;
            gold = 50;
            pirates = 7;
        }
        if (5 == enemyLevel)
        {
            food = 30;
            gold = 150;
            pirates = 20;
        }
        if (6 == enemyLevel)
        {
            food = 20;
            gold = 100;
            pirates = 7;
        }

        //---------------------------------------------------------------
        //Increase resources
        //---------------------------------------------------------------
        gameHandler.corn    += food;
        gameHandler.gold    += gold;
        gameHandler.pirates += pirates;

        CalculateMaxCapacity();
        gameHandler.UpdateUI();

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = "You won";
        eventOK_description.text = "You sank the enemy\nand salvage resources\nFood: " + food + " Gold: " + gold + " Crew: " + pirates;
    }

    //---------------------------------------------------------------
    public void ActivateEventGameOver()
    //---------------------------------------------------------------
    {
        gameOver = true;

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = "Game over";
        eventOK_description.text = "Everyone is dead and you ship has sank to the bottom.";
    }

    //---------------------------------------------------------------
    public void ActivateEvent_OK(string textTitle,string textDescription)
    //---------------------------------------------------------------
    {

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameObject.SetActive(true);
        eventOK.SetActive(true);

        //---------------------------------------------------------------
        //Play event sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipEventSound;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Set gui text
        //---------------------------------------------------------------
        eventOK_title.text = textTitle;
        eventOK_description.text = textDescription;
    }

    //---------------------------------------------------------------
    public void ActivateEvent_CityTrade(int cityID)
    //---------------------------------------------------------------
    {

        //---------------------------------------------------------------
        //Show event gui
        //---------------------------------------------------------------
        gameCanvasResource.SetActive(true);
        gameObject.SetActive(true);
        eventCityTrade.SetActive(true);
        gameCity.SetActive(false);

        //---------------------------------------------------------------
        //Set city name and buy prices
        //---------------------------------------------------------------
        if (cityID == 8)
        {
            eventCityTrade_Title.text = "Kalundborg - trade center";
            price_foodbuy = Random.Range(0,  2) + 1;
            price_piratesbuy = Random.Range(0, 10) + 5;
            price_cannonsbuy = Random.Range(0, 10) + 45;
        }
        if (cityID == 4)
        {
            eventCityTrade_Title.text = "Ebeltoft - trade center";
            price_foodbuy = Random.Range(0, 2) + 1;
            price_piratesbuy = Random.Range(0, 10) + 10;
            price_cannonsbuy = Random.Range(0, 10) + 50;
        }
        if (cityID == 2)
        {
            eventCityTrade_Title.text = "Grenaa - trade center";
            price_foodbuy = Random.Range(0, 2) + 1;
            price_piratesbuy = Random.Range(0, 10) + 10;
            price_cannonsbuy = Random.Range(0, 10) + 45;
        }
        if (cityID == 7)
        {
            eventCityTrade_Title.text = "København - trade center";
            price_foodbuy = Random.Range(0, 2) + 3;
            price_piratesbuy = Random.Range(0, 10) + 25;
            price_cannonsbuy = Random.Range(0, 10) + 25;
        }
        if (cityID == 5)
        {
            eventCityTrade_Title.text = "Helsingør - trade center";
            price_foodbuy = Random.Range(0, 2) + 3;
            price_piratesbuy = Random.Range(0, 10) + 20;
            price_cannonsbuy = Random.Range(0, 10) + 50;
        }
        if (cityID == 6)
        {
            eventCityTrade_Title.text = "Hundested - trade center";
            price_foodbuy = Random.Range(0, 2) + 2;
            price_piratesbuy = Random.Range(0, 10) + 10;
            price_cannonsbuy = Random.Range(0, 10) + 50;
        }
        if (cityID == 3)
        {
            eventCityTrade_Title.text = "Helsingborg - trade center";
            price_foodbuy = Random.Range(0, 2) + 4;
            price_piratesbuy = Random.Range(0, 10) + 15;
            price_cannonsbuy = Random.Range(0, 10) + 35;
        }

        //---------------------------------------------------------------
        //Set city sales prices
        //---------------------------------------------------------------
        price_foodsell = 1; //+ ((m_iFoodPriceBuy   * 10)/100);
        price_piratessell = 1 + ((price_piratesbuy * 65) / 100);
        price_cannonssell = 1 + ((price_cannonsbuy * 65) / 100);
        if (cityID == 7) price_foodsell = 2;

        //---------------------------------------------------------------
        //Set sell/buy text
        //---------------------------------------------------------------
        SetStockPrices();

        //---------------------------------------------------------------
        //Set upgrade text
        //---------------------------------------------------------------
        SetUpgradeText();
    }

    //---------------------------------------------------------------
    private void SetStockPrices()
    //---------------------------------------------------------------
    {
        eventCityTrade_FoodBuy.text = "Buy 10 freesh food stock: " + price_foodbuy + " gold (Max capacity: " + GetFoodCapacity() + ")";
        eventCityTrade_FoodSell.text = "Sell 10 rotten food stock: " + price_foodsell + " gold";
        eventCityTrade_PiratesBuy.text = "Buy 2 angry pirates: " + price_piratesbuy + " gold (Max capacity: " + GetPiratesCapacity() + ")";
        eventCityTrade_PiratesSell.text = "Sell 2 tamed pirates: " + price_piratessell + " gold";
        eventCityTrade_CannonsBuy.text = "Buy bloody cannon: " + price_cannonsbuy + " gold (Max capacity: " + GetCannonsCapacity() + ")";
        eventCityTrade_CannonsSell.text = "Sell rusty cannon: " + price_cannonssell + " gold";
    }

    //---------------------------------------------------------------
    public void SetUpgradeText()
    //---------------------------------------------------------------
    {
        if (gameHandler.GetLevel() == 1)
            eventCityTrade_UpgradeShip.text = "Upgrade ship to level 2: " + GetUpgradePrice() + " gold";
        else if (gameHandler.GetLevel() == 2)
            eventCityTrade_UpgradeShip.text = "Upgrade ship to level 3: " + GetUpgradePrice() + " gold";
        else if (gameHandler.GetLevel() == 3)
            eventCityTrade_UpgradeShip.text = "Upgrade ship to level 4: " + GetUpgradePrice() + " gold";
        else if (gameHandler.GetLevel() == 4)
            eventCityTrade_UpgradeShip.text = "Upgrade ship to level 5: " + GetUpgradePrice() + " gold";
        else //if (gameHandler.GetLevel() == 5)
            eventCityTrade_UpgradeShip.text = "Max level reached";
    }

    //---------------------------------------------------------------
    public int GetUpgradePrice()
    //---------------------------------------------------------------
    {
        if (gameHandler.GetLevel() == 1)
            return 150;
        else if (gameHandler.GetLevel() == 2)
            return 200;
        else if (gameHandler.GetLevel() == 3)
            return 400;
        else if (gameHandler.GetLevel() == 4)
            return 600;
        else //if (gameHandler.GetLevel() == 5)
            return 9999;
    }

    //---------------------------------------------------------------
    public int GetFoodCapacity()
    //---------------------------------------------------------------
    {
        if (gameHandler.GetLevel() == 1)
            return 100;
        else if (gameHandler.GetLevel() == 2)
            return 200;
        else if (gameHandler.GetLevel() == 3)
            return 300;
        else if (gameHandler.GetLevel() == 4)
            return 400;
        else //if (gameHandler.GetLevel() == 5)
            return 500;
    }

    //---------------------------------------------------------------
    public int GetPiratesCapacity()
    //---------------------------------------------------------------
    {
        if (gameHandler.GetLevel() == 1)
            return 35;
        else if (gameHandler.GetLevel() == 2)
            return 50;
        else if (gameHandler.GetLevel() == 3)
            return 100;
        else if (gameHandler.GetLevel() == 4)
            return 200;
        else //if (gameHandler.GetLevel() == 5)
            return 350;
    }

    //---------------------------------------------------------------
    public int GetCannonsCapacity()
    //---------------------------------------------------------------
    {
        if (gameHandler.GetLevel() == 1)
            return 7;
        else if (gameHandler.GetLevel() == 2)
            return 15;
        else if (gameHandler.GetLevel() == 3)
            return 30;
        else if (gameHandler.GetLevel() == 4)
            return 60;
        else //if (gameHandler.GetLevel() == 5)
            return 100;
    }

    //---------------------------------------------------------------
    public void ClickAttackShip()
    //---------------------------------------------------------------
    {
        eventAttackShip.SetActive(false);
        gameObject.SetActive(false);
        gameMap.SetActive(false);
        camera.transform.position = new Vector3(0, 0, -10); //Recenter camera
        gameShipBattle.SetActive(true);
        gameShipBattle.GetComponent<ShipBattle>().Init(shipEnemyLvl);
        gameHandler.SpawnClouds(); //Respawn clouds on gamemap
    }

    //---------------------------------------------------------------
    public void ClickBoardShip(int pirates,int enemyLvl)
    //---------------------------------------------------------------
    {
        eventAttackShip.SetActive(false);
        gameObject.SetActive(false);
        gameMap.SetActive(false);
        camera.transform.position = new Vector3(0, 0, -10); //Recenter camera
        gameBoardBattle.SetActive(true);
        gameBoardBattle.GetComponent<BoardBattle>().Init(pirates, enemyLvl);
    }

    //---------------------------------------------------------------
    public void ClickFleeShip()
    //---------------------------------------------------------------
    {
        eventAttackShip.SetActive(false);
        ClickOK();
    }

    //---------------------------------------------------------------
    public void ClickOK()
    //---------------------------------------------------------------
    {
        //Trade, go back to map overview
        if(eventCityTrade.activeSelf)
        {
            eventCityTrade.SetActive(false);
            gameCity.SetActive(false);
            gameMap.SetActive(true);
            gameHandler.SpawnClouds(); //Respawn clouds on gamemap
            gameObject.SetActive(false);
        }
        else if(gameOver)
        {
            //Go to main menu 
            SceneManager.LoadScene("Scenes/Menu"); 
        }
        else //Just hide event box
        {
            gameObject.SetActive(false);

            //Always hide battlescene and activate map
            if(gameShipBattle.activeSelf)
            {
                gameShipBattle.SetActive(false);
                if(boardStartBattle == false)
                    gameMap.SetActive(true);
            }
            if (boardStartBattle == false && gameBoardBattle.activeSelf)
            {
                gameBoardBattle.GetComponent<BoardBattle>().CleanUp();
                gameBoardBattle.SetActive(false);
                gameMap.SetActive(true);
            }

        }
    }

    //---------------------------------------------------------------
    public void ClickUpgradeShip()
    //---------------------------------------------------------------
    {
        if(GetUpgradePrice() <= gameHandler.gold)
        {
            gameHandler.gold -= GetUpgradePrice();
            gameHandler.UpgradeShipLevel();
            gameHandler.UpdateUI();
            SetUpgradeText();
            SetStockPrices();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickBuyFood()
    //---------------------------------------------------------------
    {
        if(price_foodbuy <= gameHandler.gold && gameHandler.corn != GetFoodCapacity())
        {
            gameHandler.gold -= price_foodbuy;
            gameHandler.corn += 10;

            CalculateMaxCapacity();
            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickSellFood()
    //---------------------------------------------------------------
    {
        if (10 <= gameHandler.corn)
        {
            gameHandler.corn -= 10;
            gameHandler.gold += price_foodsell;

            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickBuyPirates()
    //---------------------------------------------------------------
    {
        if (price_piratesbuy <= gameHandler.gold && gameHandler.pirates != GetPiratesCapacity())
        {
            gameHandler.gold -= price_piratesbuy;
            gameHandler.pirates += 2;

            CalculateMaxCapacity();
            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickSellPirates()
    //---------------------------------------------------------------
    {
        if (2 <= gameHandler.pirates)
        {
            gameHandler.pirates -= 2;
            gameHandler.gold += price_piratessell;

            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickBuyCannons()
    //---------------------------------------------------------------
    {
        if (price_cannonsbuy <= gameHandler.gold && gameHandler.cannons != GetCannonsCapacity())
        {
            gameHandler.gold -= price_cannonsbuy;
            gameHandler.cannons += 1;

            CalculateMaxCapacity();
            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void ClickSellCannons()
    //---------------------------------------------------------------
    {
        if (1 <= gameHandler.cannons)
        {
            gameHandler.cannons -= 1;
            gameHandler.gold += price_cannonssell;

            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Play click sound
            //---------------------------------------------------------------
            GetComponent<AudioSource>().clip = clipBuySound;
            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
            GetComponent<AudioSource>().Play();
        }
    }

    //---------------------------------------------------------------
    public void CalculateMaxCapacity()
    //---------------------------------------------------------------
    {
        if (gameHandler.corn > GetFoodCapacity())
        {
            gameHandler.corn = GetFoodCapacity();
        }

        if (gameHandler.pirates > GetPiratesCapacity())
        {
            gameHandler.pirates = GetPiratesCapacity();
        }

        if (gameHandler.cannons > GetCannonsCapacity())
        {
            gameHandler.cannons = GetCannonsCapacity();
        }

    }

    //---------------------------------------------------------------
    public void ShipAttack(int enemyLvl)
    //---------------------------------------------------------------
    {
        shipEnemyLvl = enemyLvl;
        string enemyType = "";
        string title = "";
        string description = "";

        if (enemyLvl == 0)
        {
            enemyType = "English schooner";
        }
        else if (enemyLvl == 1)
        {
            enemyType = "English merchant ship";
        }
        else if (enemyLvl == 2)
        {
            enemyType = "English brig";
        }
        else if (enemyLvl == 3)
        {
            enemyType = "English gunboat";
        }
        else if (enemyLvl == 4)
        {
            enemyType = "English frigate";
        }
        else if (enemyLvl == 5)
        {
            enemyType = "English Man-O-War";
        }
        else
        {
            enemyType = "Pirate ship!";
        }

        title = enemyType + " ship";
        description = "Choose order:";

        //---------------------------------------------------------------
        //Show Gui
        //---------------------------------------------------------------
        ActivateEvent_AttackShip(title, description);
    }

    //---------------------------------------------------------------
    public void HandleDebris()
    //---------------------------------------------------------------
    {
        string title = "";
        string description = "";

        //Random event
        int randomEvent = Random.Range(0, 12);
        if (randomEvent == 0)
        {
            int food = Random.Range(0, 75) + 1 + ((gameHandler.corn * 50) / 100);
            title = "Floating grain barrel";
            description = "You gain " + food + " food.";
            gameHandler.corn += food;
        }
        else if (randomEvent == 1)
        {
            int pirates = Random.Range(0, 20) + 1 + ((gameHandler.pirates * 50) / 100);
            title = "Survived sailors";
            description = "You found " + pirates + " sailors joining your crew.";
            gameHandler.pirates += pirates;
        }
        else if (randomEvent == 2)
        {
            title = "Empty shipwreck";
            description = "Nothing found...";
        }
        else if (randomEvent == 3)
        {
            int pirates = Random.Range(0, 20) + 1 + ((gameHandler.pirates * 30) / 100); // 30%
            if (pirates > gameHandler.pirates) pirates = gameHandler.pirates;
            title = "The plague";
            description = pirates + " Crewmen dies from the deadly plague.";
            gameHandler.pirates -= pirates;
        }
        else if (randomEvent == 4 ||
                 randomEvent == 8)
        {
            title = "Pirates!";
            description = "You ship has been boarded.\nPrepare for battle!";
            int pirates = Random.Range(0, gameHandler.pirates + 1) + 1;
            ClickBoardShip(pirates, -1);
            boardStartBattle = true;
        }
        else if (randomEvent == 5)
        {
            int cannons = Random.Range(0, 9) + 1;
            title = "Floating wreck";
            description = "You found " + cannons + " used cannons.";
            gameHandler.cannons += cannons;
        }
        else if (randomEvent == 6)
        {
            title = "Empty shipwreck";
            description = "Nothing found...";
        }
        else if (randomEvent == 7)
        {
            int gold = Random.Range(0, 250) + 50;
            title = "Floating shipwreck";
            description = "You found " + gold + " gold.";

            gameHandler.gold += gold;
        }
        else if (randomEvent == 9)
        {
            title = "Empty shipwreck";
            description = "Nothing found...";
        }
        else if (randomEvent == 10)
        {
            int food    = 1 + ((gameHandler.corn *    60) / 100); // 60%
            int gold    = 1 + ((gameHandler.gold *     5) / 100); //  5%
            int pirates = 1 + ((gameHandler.pirates * 20) / 100); // 20%
            int cannons = 1 + ((gameHandler.cannons * 10) / 100); // 10%

            title = "Unexpected rock";
            description = "You crashed into a rock and took heavy damage.\nLost " + food + " food, " + gold + " gold, " + pirates + " pirates, " + cannons + " cannons.";

            gameHandler.corn    -= food;
            gameHandler.gold    -= gold;
            gameHandler.pirates -= pirates;
            gameHandler.cannons -= cannons;
        }
        else if (randomEvent == 11)
        {
            int gold = Random.Range(0, 250) + 50;
            title = "Floating shipwreck";
            description = "You found " + gold + " gold.";

            gameHandler.gold += gold;
        }

        //---------------------------------------------------------------
        //Refresh resource
        //---------------------------------------------------------------
        CalculateMaxCapacity();
        gameHandler.UpdateUI();

        //---------------------------------------------------------------
        //Show Gui
        //---------------------------------------------------------------
        ActivateEvent_OK(title,description);

    }

}
