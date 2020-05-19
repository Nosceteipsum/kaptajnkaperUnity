using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class BoardBattlePlayerCommand : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject armyPlayer;
    public BoardBattle boardBattle;
    public GameObject PlayercommandObject;
    public bool AI;

    private float ShootCooldown;
    private float AIShootXPos;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        ShootCooldown = 0.0f;
        AIShootXPos = 1.517f;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        //Player controls
        //---------------------------------------------------------------
        if (AI == false)
        {
            //---------------------------------------------------------------
            //Check if player shoot
            //---------------------------------------------------------------
            if (boardBattle.GetPrepare() <= 0.0f && boardBattle.GetPlayerAmmo() > 0 && ShootCooldown <= 0.0f && Input.GetButton("Fire1"))
            {
                ShootCooldown = 1.0f;

                //---------------------------------------------------------------
                //Set animation pose on every soldiers and spawn bullet
                //---------------------------------------------------------------
                var army = armyPlayer.GetComponentsInChildren<BoardSoldierPlayer>();
                foreach (var soldier in army)
                {
                    soldier.AnimationShoot();
                }

                //---------------------------------------------------------------
                //Cost 1 bullet
                //---------------------------------------------------------------
                boardBattle.PlayerShoot();

            }
            else if (ShootCooldown >= 0.0f)
            {
                ShootCooldown -= Time.deltaTime;
            }
            else
            {
                //---------------------------------------------------------------
                //Player movement
                //---------------------------------------------------------------
                float xDirection = 0.002f * Input.GetAxis("Horizontal");
                float yDirection = 0.002f * Input.GetAxis("Vertical");

                transform.Translate(xDirection, yDirection, 0);

                //---------------------------------------------------------------
                //Player map limit
                //---------------------------------------------------------------
                if (transform.position.x > 1.5f)
                {
                    transform.position = new Vector3(1.5f, transform.position.y, 0);
                }
                if (transform.position.x < -1.7f)
                {
                    transform.position = new Vector3(-1.7f, transform.position.y, 0);
                }
                if (transform.position.y > 0.25f)
                {
                    transform.position = new Vector3(transform.position.x, 0.25f, 0);
                }
                if (transform.position.y < -0.8f)
                {
                    transform.position = new Vector3(transform.position.x, -0.8f, 0);
                }
            }
        }
        //---------------------------------------------------------------
        //AI
        //---------------------------------------------------------------
        else if(boardBattle.GetPrepare() <= 0.0f && ShootCooldown <= 0.0f)
        {
            //---------------------------------------------------------------
            //Charge when no bullets
            //---------------------------------------------------------------
            if (boardBattle.GetEnemyAmmo() <= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayercommandObject.transform.position, 0.01f);
            }
            else
            {
                //---------------------------------------------------------------
                //Move up/down to make a clear shoot
                //---------------------------------------------------------------
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(AIShootXPos, PlayercommandObject.transform.position.y,transform.position.z), 0.001f);

                //---------------------------------------------------------------
                //Shot if y is aligned
                //---------------------------------------------------------------
                if(Vector2.Distance(transform.position, new Vector2(transform.position.x, PlayercommandObject.transform.position.y)) < 0.05f)
                {
                    int random = Random.Range(0, 200);
                    if(random == 0) //Something just don't fire
                    {
                        ShootCooldown = 1.0f;

                        //---------------------------------------------------------------
                        //Set animation pose on every soldiers and spawn bullet
                        //---------------------------------------------------------------
                        var army = armyPlayer.GetComponentsInChildren<BoardSoldierPlayer>();
                        foreach (var soldier in army)
                        {
                            soldier.AnimationShoot();
                        }

                        //---------------------------------------------------------------
                        //Cost 1 bullet
                        //---------------------------------------------------------------
                        boardBattle.enemyShoot();
                    }
                    else if(random == 1)
                    {
                        var army = armyPlayer.GetComponentsInChildren<BoardSoldierPlayer>();
                        foreach (var soldier in army)
                        {
                            AIShootXPos = Random.Range(0.0f, 1.5f);
                        }
                    }
                }
            }
        }
        else if (ShootCooldown >= 0.0f)
        {
            ShootCooldown -= Time.deltaTime;
        }
    }
}
