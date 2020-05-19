using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class BattleCannonBall : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject battleSplash;
    public GameObject battleExplosion;

    public GameObject shipEnemy;
    public GameObject shipPlayer;

    public float speedHeightVelocity;
    public float speedBall;
    public bool flyingUp;
    public float speedHorizontal;

    private float height;
    private bool increaseHeight;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        height = 1.0f;
        increaseHeight = true;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        transform.Translate(speedHorizontal, speedBall * (flyingUp ? 1 : -1), 0);
        transform.localScale = new Vector2(height, height);

        if(increaseHeight == true)
        {
            height += speedHeightVelocity; //* 0.5f;//0.01f;
            if (height > 1.5f)
            {
                increaseHeight = false;
            }
        }
        else
        {
            height -= speedHeightVelocity; // 0.01f;

            //---------------------------------------------------------------
            //Cannonball hit water/target
            //---------------------------------------------------------------
            if (height <= 0.01f)
            {
                //---------------------------------------------------------------
                //Check for ship collision
                //---------------------------------------------------------------
                if(flyingUp && Vector3.Distance(shipEnemy.transform.position,transform.position) < 0.1f)
                {
                    Instantiate(battleExplosion, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity, transform.parent);
                    shipEnemy.GetComponent<BattleShipEnemy>().Hit();
                }
                else if (!flyingUp && Vector3.Distance(shipPlayer.transform.position, transform.position) < 0.1f)
                {
                    Instantiate(battleExplosion, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity, transform.parent);
                    shipPlayer.GetComponent<BattleShipPlayer>().Hit();
                }
                //---------------------------------------------------------------
                //Spawn Water splash
                //---------------------------------------------------------------
                else
                {
                    Instantiate(battleSplash, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity, transform.parent);
                }

                //---------------------------------------------------------------
                //Destroy ball
                //---------------------------------------------------------------
                Destroy(gameObject);
            }
        }
    }
}
