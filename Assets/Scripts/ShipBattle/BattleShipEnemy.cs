using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------
public class BattleShipEnemy : MonoBehaviour
//---------------------------------------------------------------
{

    public GameHandler gameHandler;
    public GameObject battleFlash;
    public GameObject battleCannonBall;
    public GameObject wave;
    public GameObject playerShip;
    public GameObject battleTarget;

    public AudioClip clipBoatShoot;
    public AudioClip clipBoatHit;

    public Text resourcePirates;
    public Text resourceCannons;

    public Sprite smallShip;
    public Sprite MediumShip;
    public Sprite LargeShip;

    private bool preparingShoot;
    private float ShootDistance;
    private float ShootCooldown;
    private float waveSpawnTime = 3f;
    private float spawnMovementWake;
    private bool paused;

    //AI resources
    public int pirates;
    public int cannons;

    //AI brain
    private float brainCountDown;
    private Vector2 brainTarget;
    private bool brainFire;

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
        spawnMovementWake = 0.0f;
        brainCountDown = 0.0f;
        cannons = 5;
        pirates = 30;
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        //Update resources
        //---------------------------------------------------------------
        resourcePirates.text = pirates.ToString("D5");
        resourceCannons.text = cannons.ToString("D5");

        //---------------------------------------------------------------
        //Move pause
        //---------------------------------------------------------------
        if (paused) return;

        //---------------------------------------------------------------
        //Move ship toward target
        //---------------------------------------------------------------
        transform.position = Vector3.MoveTowards(transform.position, brainTarget, 0.002f);

        //---------------------------------------------------------------
        //Movement in progress, spawn waves
        //---------------------------------------------------------------
        if (Vector3.Distance(transform.position, brainTarget) >= 0.05f)
        {
            spawnMovementWake -= Time.deltaTime;
            if (spawnMovementWake <= 0.0f)
            {
                spawnMovementWake = 0.10f;
                StartCoroutine(SpawnWave(false));
            }
        }

        //---------------------------------------------------------------
        //Prepare cannons when in range
        //---------------------------------------------------------------
        if(cannons > 0 && Vector2.Distance(new Vector2(transform.position.x, 0),new Vector2(playerShip.transform.position.x, 0)) < 0.4f)
        {
            brainFire = true;
        }

        //---------------------------------------------------------------
        //Shot cannons when target reach player
        //---------------------------------------------------------------
        if (brainFire == true && battleTarget.activeSelf)
        {
            if(Vector2.Distance(new Vector2(0, battleTarget.transform.position.y),new Vector2(0, playerShip.transform.position.y)) < 0.1f)
            {
                brainFire = false;
            }
        }

        //---------------------------------------------------------------
        //Check for new command and no cannons left (Charge agains player ship)
        //---------------------------------------------------------------
        if (brainCountDown <= 0.0f && cannons <= 0)
        {
            brainTarget = playerShip.transform.position;
        }

        //---------------------------------------------------------------
        //Check for new command and ship have cannons
        //---------------------------------------------------------------
        else if (brainCountDown <= 0.0f)
        {
            //---------------------------------------------------------------
            //Move random around
            //---------------------------------------------------------------
            if (Random.Range(0, 2) == 0)
            {
                brainTarget = new Vector2(Random.Range(-1.2f, 1.2f),
                                          Random.Range(0.0f, 0.8f));
            }
            //---------------------------------------------------------------
            //Move in shooting range 
            //---------------------------------------------------------------
            else
            {
                brainTarget = new Vector2(Random.Range(-0.1f, 0.1f) + playerShip.transform.position.x,
                                          Random.Range(-0.05f, 0.05f) + playerShip.transform.position.y + 0.8f);
            }

            brainCountDown = Random.Range(0.0f, 3.0f) + 5;
        }
        else
        {
            brainCountDown -= Time.deltaTime;
        }

        //---------------------------------------------------------------
        //AI shooting
        //---------------------------------------------------------------
        if (ShootCooldown <= 0.0f)
        {
            if (brainFire && ShootDistance <= 0.98f)
            {
                battleTarget.SetActive(true);
                preparingShoot = true;
                ShootDistance += Time.deltaTime;
                battleTarget.transform.position = new Vector3(transform.position.x, transform.position.y + 0.08f + 0.06f - (ShootDistance * ShootDistance * ShootDistance * 1.0f), 0); // Approximate!!!
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
                    Instantiate(battleFlash, transform.position - new Vector3(0, 0.05f, 0), Quaternion.identity, transform.parent);

                    //---------------------------------------------------------------
                    //Spawn cannonballs
                    //---------------------------------------------------------------
                    for (int i = 0; i < cannons; i++)
                    {
                        float randomX = Random.Range(-0.25f, 0.25f);
                        float randomY = Random.Range(-0.09f, 0.09f);

                        var cannonBall = Instantiate(battleCannonBall, transform.position + new Vector3(0 + randomX, 0.08f + randomY, 0), Quaternion.identity, transform.parent);
                        BattleCannonBall cannonBallScript = cannonBall.GetComponent<BattleCannonBall>();
                        float ballVelocityHeight = 0.05f - (ShootDistance * 0.05f);
                        if (ballVelocityHeight <= 0.006f) ballVelocityHeight = 0.006f;
                        ballVelocityHeight += Random.Range(-0.001f, 0.001f); //Randomized a little
                        //Debug.Log("Ball velocity: " + ballVelocityHeight);
                        cannonBallScript.speedHeightVelocity = ballVelocityHeight;
                        cannonBallScript.shipEnemy = gameObject;
                        cannonBallScript.shipPlayer = playerShip;
                        cannonBallScript.flyingUp = false;
                        cannonBallScript.speedHorizontal = Random.Range(-0.001f, 0.001f);
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
        cannons -= Random.Range(0,3);
        pirates -= 2 + Random.Range(0, 16);

        if (cannons < 0) cannons = 0;
        if (pirates < 0) pirates = 0;

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
