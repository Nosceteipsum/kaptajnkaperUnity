using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------
public class BoardBattle : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject resourceEnemy;
    public GameObject resourceEnemyCannons;
    public GameObject resourcePlayerCannons;
    public GameObject[] resourceEnemyAmmo;
    public GameObject[] resourcePlayerAmmo;
    public GameObject armyPlayer;
    public GameObject armyEnemy;
    public GameObject SoldierPlayer;
    public GameObject SoldierEnemy;
    public GameObject CommandPointPlayer;
    public GameObject CommandPointEnemy;
    public GameObject graveYard;
    public GameHandler gameHandler;
    public Event eventHandler;
    public Text textEnemyPirates;

    private int enemyPirates;
    private int enemyShipLvl;
    private float prepare;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        //Init(10, 0);
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

        if (eventHandler.EventActive())
        {
            return;
        }

        if (prepare > 0.0f)
        {
            prepare -= Time.deltaTime;
            CommandPointPlayer.transform.position = new Vector3(-1.488f - prepare * 0.2f, -0.128f, 0.0f);
            CommandPointEnemy.transform.position = new Vector3(1.488f + prepare * 0.2f, CommandPointEnemy.transform.position.y, 0.0f);
        }
        else
        {
            //---------------------------------------------------------------
            //Check how many soldiers back (Respawn if all is dead)
            //---------------------------------------------------------------
            if(armyEnemy.transform.childCount <= 0)
            {
                if(enemyPirates <= 0)
                {
                    //Player won

                    //---------------------------------------------------------------
                    //Remove ship from map
                    //---------------------------------------------------------------
                    if(enemyShipLvl != -1)
                        gameHandler.RemoveEnemyShip();

                    //---------------------------------------------------------------
                    //Return remaining units to resourcebar
                    //---------------------------------------------------------------
                    gameHandler.pirates += armyPlayer.transform.childCount;
                    gameHandler.UpdateUI();

                    //---------------------------------------------------------------
                    //Show event box
                    //---------------------------------------------------------------
                    resourceEnemy.SetActive(false);
                    eventHandler.ActivateEventBattleBoardWon(enemyShipLvl);
                }
                else
                {
                    SpawnEnemySoldier(false);
                }
            }
            if (armyPlayer.transform.childCount <= 0)
            {
                if(gameHandler.pirates <= 0)
                {
                    //Game over
                    resourceEnemy.SetActive(false);
                    eventHandler.ActivateEventGameOver();
                }
                else
                {
                    SpawnPlayerSoldier(false);
                }
            }

        }

        //---------------------------------------------------------------
        //Check if soldiers collide (Nearcombat)
        //---------------------------------------------------------------
        var playerSoldiers = armyPlayer.GetComponentsInChildren<BoardSoldierPlayer>();
        var enemySoldiers = armyEnemy.GetComponentsInChildren<BoardSoldierPlayer>();
        foreach (var soldier in playerSoldiers)
        {
            if (soldier.IsFighting())
                continue;

            foreach (var enemySoldier in enemySoldiers)
            {
                if (enemySoldier.IsFighting())
                    continue;

                //---------------------------------------------------------------
                //Check for collision between soldier
                //---------------------------------------------------------------
                if (Vector3.Distance(enemySoldier.transform.position, soldier.transform.position) < 0.1f)
                {
                    //---------------------------------------------------------------
                    //Set fighting stance (with timer)
                    //---------------------------------------------------------------
                    float time = Random.Range(0.5f,1.5f);

                    //---------------------------------------------------------------
                    //choose one win / loose
                    //---------------------------------------------------------------
                    int result = Random.Range(0, 3);
                    if(result == 0) //Both die
                    {
                        soldier.SetFighting(true, time);
                        enemySoldier.SetFighting(true, time);
                    }
                    if (result == 1) //Enemy win
                    {
                        soldier.SetFighting(true, time);
                        enemySoldier.SetFighting(false, time);
                    }
                    if (result == 2) //Player win
                    {
                        soldier.SetFighting(false, time);
                        enemySoldier.SetFighting(true, time);
                    }
                }
            }
        }

    }

    //---------------------------------------------------------------
    public void CleanUp()
    //---------------------------------------------------------------
    {
        foreach (Transform child in armyPlayer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in armyEnemy.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in graveYard.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        resourceEnemyCannons.SetActive(true);
        resourcePlayerCannons.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            resourcePlayerAmmo[i].SetActive(false);
            resourceEnemyAmmo[i].SetActive(false);
        }
    }

    //---------------------------------------------------------------
    public float GetPrepare()
    //---------------------------------------------------------------
    {
        return prepare;
    }

    //---------------------------------------------------------------
    public int GetPlayerAmmo()
    //---------------------------------------------------------------
    {
        int ammo = 0;
        for (int i = 0; i < 5; i++)
        {
            if (resourcePlayerAmmo[i].activeSelf == true)
            {
                ammo++;
            }
        }
        return ammo;
    }

    //---------------------------------------------------------------
    public int GetEnemyAmmo()
    //---------------------------------------------------------------
    {
        int ammo = 0;
        for (int i = 0; i < 5; i++)
        {
            if (resourceEnemyAmmo[i].activeSelf == true)
            {
                ammo++;
            }
        }
        return ammo;
    }

    //---------------------------------------------------------------
    public void PlayerShoot()
    //---------------------------------------------------------------
    {
        for (int i = 0; i < 5; i++)
        {
            if(resourcePlayerAmmo[i].activeSelf == true)
            {
                resourcePlayerAmmo[i].SetActive(false);

                //---------------------------------------------------------------
                //Play hit sound
                //---------------------------------------------------------------
                GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                GetComponent<AudioSource>().Play();

                return;
            }
        }
    }

    //---------------------------------------------------------------
    public void enemyShoot()
    //---------------------------------------------------------------
    {
        for (int i = 0; i < 5; i++)
        {
            if (resourceEnemyAmmo[i].activeSelf == true)
            {
                resourceEnemyAmmo[i].SetActive(false);

                //---------------------------------------------------------------
                //Play hit sound
                //---------------------------------------------------------------
                GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                GetComponent<AudioSource>().Play();

                return;
            }
        }
    }

    //---------------------------------------------------------------
    public void Init(int enemySoldiers,int enemyLvl)
    //---------------------------------------------------------------
    {
        enemyShipLvl = enemyLvl;
        enemyPirates = enemySoldiers;
        textEnemyPirates.text = enemyPirates.ToString("D5");
        prepare = 3.0f;

        //---------------------------------------------------------------
        //Show resources
        //---------------------------------------------------------------
        resourceEnemy.SetActive(true);
        resourceEnemyCannons.SetActive(false);
        resourcePlayerCannons.SetActive(false);
        GiveAmmoPlayer();
        GiveAmmoEnemy();

        //---------------------------------------------------------------
        //Clear previous Clouds and obstacles
        //---------------------------------------------------------------
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("CityObstacle");
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        //---------------------------------------------------------------
        //Spawn player soldiers
        //---------------------------------------------------------------
        SpawnPlayerSoldier(true);

        //---------------------------------------------------------------
        //Spawn enemy soldiers
        //---------------------------------------------------------------
        SpawnEnemySoldier(true);

    }

    //---------------------------------------------------------------
    private void GiveAmmoPlayer()
    //---------------------------------------------------------------
    {
        for (int i = 0; i < 5; i++)
        {
            resourcePlayerAmmo[i].SetActive(true);
        }
    }

    //---------------------------------------------------------------
    private void GiveAmmoEnemy()
    //---------------------------------------------------------------
    {
        for (int i = 0; i < 5; i++)
        {
            resourceEnemyAmmo[i].SetActive(true);
        }
    }

    //---------------------------------------------------------------
    private void SpawnPlayerSoldier(bool init)
    //---------------------------------------------------------------
    {
        CommandPointPlayer.transform.position = new Vector3(-1.488f - prepare * 0.2f, -0.128f, 0.0f);
        Vector3 startPos = new Vector3(CommandPointPlayer.transform.position.x, CommandPointPlayer.transform.position.y, CommandPointPlayer.transform.position.z);
        if(init == false)
        {
            startPos = new Vector3(-1.7f,0,0);
        }

        for (int i = 0; i < 8; i++)
        {
            //---------------------------------------------------------------
            //Subtract interface for every spawn
            //---------------------------------------------------------------
            if (gameHandler.pirates <= 0)
                break;
            gameHandler.pirates--;

            gameHandler.UpdateUI();

            //---------------------------------------------------------------
            //Spawn soldier
            //---------------------------------------------------------------
            var soldier = Instantiate(SoldierPlayer, startPos, Quaternion.identity, armyPlayer.transform);
            soldier.GetComponent<BoardSoldierPlayer>().SetCommandPoint(CommandPointPlayer);
            soldier.GetComponent<BoardSoldierPlayer>().boardBattle = GetComponent<BoardBattle>();
            soldier.GetComponent<BoardSoldierPlayer>().graveYard = graveYard;
        }

        GiveAmmoPlayer();
    }

    //---------------------------------------------------------------
    private void SpawnEnemySoldier(bool init)
    //---------------------------------------------------------------
    {
        CommandPointEnemy.transform.position = new Vector3(1.488f + prepare * 0.2f, Random.Range(-0.8f, 0.25f), 0.0f);
        Vector3 startPos = new Vector3(CommandPointEnemy.transform.position.x, CommandPointEnemy.transform.position.y, CommandPointEnemy.transform.position.z);
        if (init == false)
        {
            startPos = new Vector3( 1.7f, 0, 0);
        }

        for (int i = 0; i < 8; i++)
        {
            if (enemyPirates <= 0)
                break;
            enemyPirates--;

            var soldier = Instantiate(SoldierEnemy, startPos, Quaternion.identity, armyEnemy.transform);
            soldier.GetComponent<BoardSoldierPlayer>().SetCommandPoint(CommandPointEnemy);
            soldier.GetComponent<BoardSoldierPlayer>().boardBattle = GetComponent<BoardBattle>();
            soldier.GetComponent<BoardSoldierPlayer>().graveYard = graveYard;
        }

        GiveAmmoEnemy();

        textEnemyPirates.text = enemyPirates.ToString("D5");
    }

}
