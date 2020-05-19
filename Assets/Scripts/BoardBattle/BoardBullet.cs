using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class BoardBullet : MonoBehaviour
//---------------------------------------------------------------
{
    public float speedBall;
    public bool GoingRight;

    public GameObject armyPlayer;
    public GameObject armyEnemy;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {

    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        transform.Translate(speedBall * (GoingRight ? 1f : -1f), 0, 0);

        //---------------------------------------------------------------
        //Bullet hit wall
        //---------------------------------------------------------------
        if (transform.position.x <= -2.0f || transform.position.x >= 2.0f)
        {
            //---------------------------------------------------------------
            //Destroy bullet
            //---------------------------------------------------------------
            Destroy(gameObject);
        }

        //---------------------------------------------------------------
        //Check distance to enemy soldier
        //---------------------------------------------------------------
        if(GoingRight) //Player shoot
        {
            var soldiersEnemy = armyEnemy.GetComponentsInChildren<BoardSoldierPlayer>();
            foreach(var soldier in soldiersEnemy)
            {
                if(Vector3.Distance(soldier.transform.position, transform.position) < 0.1f)
                {
                    //---------------------------------------------------------------
                    //Kill soldier
                    //---------------------------------------------------------------
                    soldier.AnimationDead();

                    //---------------------------------------------------------------
                    //Destroy bullet
                    //---------------------------------------------------------------
                    Destroy(gameObject);
                }
            }

        }
        else //Check distance to player soldiers
        {
            var soldiersPlayer = armyPlayer.GetComponentsInChildren<BoardSoldierPlayer>();
            foreach (var soldier in soldiersPlayer)
            {
                if (Vector3.Distance(soldier.transform.position, transform.position) < 0.1f)
                {
                    //---------------------------------------------------------------
                    //Kill soldier
                    //---------------------------------------------------------------
                    soldier.AnimationDead();

                    //---------------------------------------------------------------
                    //Destroy bullet
                    //---------------------------------------------------------------
                    Destroy(gameObject);
                }
            }
        }

    }
}
