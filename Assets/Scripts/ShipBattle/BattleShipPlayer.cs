using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class BattleShipPlayer : MonoBehaviour
//---------------------------------------------------------------
{
    public GameHandler gameHandler;
    public GameObject battleFlash;
    public GameObject battleCannonBall;
    public GameObject battleTarget;
    public GameObject wave;
    public GameObject shipEnemy;

    public AudioClip clipBoatShoot;
    public AudioClip clipBoatHit;

    public Sprite smallShip;
    public Sprite MediumShip;
    public Sprite LargeShip;

    //Hidden fields
    private bool preparingShoot;
    private float ShootDistance;
    private float ShootCooldown;
    private float waveSpawnTime = 3f;
    private float spawnMovementWake;
    private bool paused;

    //---------------------------------------------------------------
    // Called when script deactivate (changing scene)
    //---------------------------------------------------------------
    void OnDisable()
    {
        //---------------------------------------------------------------
        //Stop boat water animation
        //---------------------------------------------------------------
        CancelInvoke();
    }

    //---------------------------------------------------------------
    // Called when script activate
    //---------------------------------------------------------------
    void OnEnable()
    {
        //---------------------------------------------------------------
        //Boat water animation
        //---------------------------------------------------------------
        InvokeRepeating("SpawnWaveCreator", waveSpawnTime, waveSpawnTime);
    }

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        preparingShoot = false;
        ShootDistance = 0.0f;
        ShootCooldown = 0.0f;
        spawnMovementWake = 0.0f;

        //---------------------------------------------------------------
        //Set player sprite
        //---------------------------------------------------------------
        if (gameHandler.GetLevel() <= 1)
            GetComponent<SpriteRenderer>().sprite = smallShip;
        else if (gameHandler.GetLevel() <= 2)
            GetComponent<SpriteRenderer>().sprite = MediumShip;
        else if (gameHandler.GetLevel() >= 4)
            GetComponent<SpriteRenderer>().sprite = LargeShip;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        //Player movement
        //---------------------------------------------------------------
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        //---------------------------------------------------------------
        //Movement in progress, spawn waves
        //---------------------------------------------------------------
        if (Mathf.Abs(xDirection) >= 0.2f || Mathf.Abs(yDirection) >= 0.2f)
        {
            spawnMovementWake -= Time.deltaTime;
            if (spawnMovementWake <= 0.0f)
            {
                spawnMovementWake = 0.10f;
                StartCoroutine(SpawnWave(false));
            }
        }

        //---------------------------------------------------------------
        //Move pause
        //---------------------------------------------------------------
        if (paused)return;

        //---------------------------------------------------------------
        //Move ship
        //---------------------------------------------------------------
        xDirection *= 0.0025f;
        yDirection *= 0.0025f;
        transform.Translate(xDirection, yDirection, 0);

        //---------------------------------------------------------------
        //Player map limit
        //---------------------------------------------------------------
        if (transform.position.x > 1.5f)
        {
            transform.position = new Vector3(1.5f, transform.position.y,0);
        }
        if (transform.position.x < -1.5f)
        {
            transform.position = new Vector3(-1.5f, transform.position.y, 0);
        }
        if (transform.position.y > 0.8f)
        {
            transform.position = new Vector3(transform.position.x, 0.8f, 0);
        }
        /*
        if (transform.position.y < -0.8f) //No limit, player flee when reached
        {
            transform.position = new Vector3(transform.position.x, -0.8f, 0);
        }*/

        //---------------------------------------------------------------
        //Player shooting
        //---------------------------------------------------------------
        if(ShootCooldown <= 0.0f)
        {
            if (Input.GetButton("Fire1"))
            {
                battleTarget.SetActive(true);
                preparingShoot = true;
                ShootDistance += Time.deltaTime;
                battleTarget.transform.position = new Vector3(transform.position.x, transform.position.y + 0.08f + 0.06f + (ShootDistance * ShootDistance * ShootDistance * 1.0f), 0); // Approximate!!!
                if (ShootDistance >= 1.0f)
                {
                    ShootDistance = 1.0f;
                }
            }
            else
            {
                if (preparingShoot == true)
                {
                    //---------------------------------------------------------------
                    //Cooldown before next shoot
                    //---------------------------------------------------------------
                    ShootCooldown = 1.5f;

                    //---------------------------------------------------------------
                    //Play shoot sound
                    //---------------------------------------------------------------
                    GetComponent<AudioSource>().clip = clipBoatShoot;
                    GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
                    GetComponent<AudioSource>().Play();

                    //---------------------------------------------------------------
                    //Remove target sprite
                    //---------------------------------------------------------------
                    battleTarget.SetActive(false);

                    //---------------------------------------------------------------
                    //Spawn FlashEffect
                    //---------------------------------------------------------------
                    Instantiate(battleFlash, transform.position + new Vector3(0, 0.06f, 0), Quaternion.identity, transform.parent);

                    //---------------------------------------------------------------
                    //Spawn cannonballs
                    //---------------------------------------------------------------
                    for (int i = 0; i < ((gameHandler.cannons+1)/2); i++)
                    {
                        float randomX = Random.Range(-0.1f, 0.1f);
                        float randomY = Random.Range(-0.05f, 0.05f);

                        var cannonBall = Instantiate(battleCannonBall, transform.position + new Vector3(0 + randomX, 0.08f + randomY, 0), Quaternion.identity, transform.parent);
                        BattleCannonBall cannonBallScript = cannonBall.GetComponent<BattleCannonBall>();
                        float ballVelocityHeight = 0.05f - (ShootDistance * 0.05f);
                        if (ballVelocityHeight <= 0.006f) ballVelocityHeight = 0.006f;
                        ballVelocityHeight += Random.Range(-0.001f, 0.001f); //Randomized a little
                        //Debug.Log("Ball velocity: " + ballVelocityHeight);
                        cannonBallScript.speedHeightVelocity = ballVelocityHeight;
                        cannonBallScript.shipEnemy  = shipEnemy;
                        cannonBallScript.shipPlayer = gameObject;
                        cannonBallScript.flyingUp = true;
                        cannonBallScript.speedHorizontal = 0.0f;
                    }

                    //---------------------------------------------------------------
                    //Reset values
                    //---------------------------------------------------------------
                    preparingShoot = false;
                    ShootDistance = 0.0f;

                }
            }
        }
        else
        {
            ShootCooldown -= Time.deltaTime;
        }


    }

    //---------------------------------------------------------------
    public void Hit()
    //---------------------------------------------------------------
    {
        gameHandler.cannons -= Random.Range(0, 3);
        gameHandler.pirates -= 2 + Random.Range(0, 16);

        if (gameHandler.cannons < 0) gameHandler.cannons = 0;
        if (gameHandler.pirates < 0) gameHandler.pirates = 0;

        //---------------------------------------------------------------
        //Play hit sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().clip = clipBoatHit;
        GetComponent<AudioSource>().pitch = (Random.Range(0.6f, .9f));
        GetComponent<AudioSource>().Play();
    }

    //---------------------------------------------------------------
    private void SpawnWaveCreator()
    //---------------------------------------------------------------
    {
        StartCoroutine(SpawnWave());
    }

    //---------------------------------------------------------------
    private IEnumerator SpawnWave(bool wait = true)
    //---------------------------------------------------------------
    {
        //Wait random amount (prevent synched waves)
        if (wait)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1.0f));
        }

        //Create wave
        GameObject.Instantiate(wave, gameObject.transform.localPosition, Quaternion.identity, transform.parent);
    }

    //---------------------------------------------------------------
    public void SetPause(bool pause)
    //---------------------------------------------------------------
    {
        paused = pause;
    }

}
