using UnityEngine;

//---------------------------------------------------------------
public class ShipPlayer : Ship
//---------------------------------------------------------------
{
    public Event eventHandler;

    //---------------------------------------------------------------
    //Hidden fields
    //---------------------------------------------------------------
    private bool buttonDown;
    private Vector2Int handledMessagePos;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    public new void Start()
    {
        //---------------------------------------------------------------
        //Call inheritance
        //---------------------------------------------------------------
        base.Start();

        //---------------------------------------------------------------
        //Default values
        //---------------------------------------------------------------
        buttonDown = false;
        handledMessagePos = new Vector2Int(0, 0);
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    public new void Update()
    {
        //---------------------------------------------------------------
        //Call inheritance
        //---------------------------------------------------------------
        base.Update();

        //---------------------------------------------------------------
        //Movement in progress
        //---------------------------------------------------------------
        Vector2 target = gameHandler.CalculateMapCoordinates(posX, posY);
        float distance = Vector2.Distance(target, gameObject.transform.position);
        if (distance <= 0.02f)
        {
            //---------------------------------------------------------------
            //Check current field for enemy/debris
            //---------------------------------------------------------------
            if (gameHandler.ShipCollision(posX, posY, false) == true && (handledMessagePos.x != posX || handledMessagePos.y != posY))
            {
                handledMessagePos.x = posX;handledMessagePos.y = posY;
                eventHandler.ShipAttack(gameHandler.ShipGetEnemy(posX,posY).level);
            }
            else if (gameHandler.DebrisCollision(posX, posY) == true)
            {
                handledMessagePos.x = posX; handledMessagePos.y = posY;
                gameHandler.RemoveDebris(posX, posY);
                eventHandler.HandleDebris();
            }
            else
            {
                //---------------------------------------------------------------
                //Input Move ship
                //---------------------------------------------------------------
                float xDirection = Input.GetAxis("Horizontal");
                float yDirection = Input.GetAxis("Vertical");
                if (buttonDown == true &&
                    Input.GetAxis("Horizontal") < 0.2f && Input.GetAxis("Horizontal") > -0.2f &&
                    Input.GetAxis("Vertical") < 0.2f && Input.GetAxis("Vertical") > -0.2f
                    )
                {
                    buttonDown = false;
                }

                if (buttonDown == false && eventHandler.EventActive() == false)
                {
                    int mapCheckResult = 0;

                    //---------------------------------------------------------------
                    //Handle player movement
                    //---------------------------------------------------------------
                    if (xDirection > 0.2f)
                    {
                        buttonDown = true;
                        mapCheckResult = gameHandler.MapCheckCollision(posX + 1, posY);
                        if (mapCheckResult > 0)
                        {
                            handledMessagePos.x = posX; handledMessagePos.y = posY;
                            posX++;
                            SetSprite(6);
                            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                            GetComponent<AudioSource>().Play();
                            gameHandler.NewTurn();
                        }
                    }
                    else if (xDirection < -0.2f)
                    {
                        buttonDown = true;
                        mapCheckResult = gameHandler.MapCheckCollision(posX - 1, posY);
                        if (mapCheckResult > 0)
                        {
                            handledMessagePos.x = posX; handledMessagePos.y = posY;
                            posX--;
                            SetSprite(4);
                            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                            GetComponent<AudioSource>().Play();
                            gameHandler.NewTurn();
                        }
                    }
                    else if (yDirection < -0.2f)
                    {
                        buttonDown = true;
                        mapCheckResult = gameHandler.MapCheckCollision(posX, posY + 1);
                        if (mapCheckResult > 0)
                        {
                            handledMessagePos.x = posX; handledMessagePos.y = posY;
                            posY++;
                            SetSprite(8);
                            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                            GetComponent<AudioSource>().Play();
                            gameHandler.NewTurn();
                        }
                    }
                    else if (yDirection > 0.2f)
                    {
                        buttonDown = true;
                        mapCheckResult = gameHandler.MapCheckCollision(posX, posY - 1);
                        if (mapCheckResult > 0)
                        {
                            handledMessagePos.x = posX; handledMessagePos.y = posY;
                            posY--;
                            SetSprite(2);
                            GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                            GetComponent<AudioSource>().Play();
                            gameHandler.NewTurn();
                        }
                    }

                    //---------------------------------------------------------------
                    //Player visit city
                    //---------------------------------------------------------------
                    if (mapCheckResult > 1)
                    {
                        gameHandler.VisitCity(mapCheckResult);
                    }

                }
            }
        }
    }


}
