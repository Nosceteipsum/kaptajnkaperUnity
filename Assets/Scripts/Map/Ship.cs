using System.Collections;
using UnityEngine;

//---------------------------------------------------------------
public class Ship : MonoBehaviour
//---------------------------------------------------------------
{
    public float waveSpawnTime = 3f;
    public GameObject wave;
    public GameHandler gameHandler;
    public int level;

    //Ship position
    public int posX = 0;  // 10;
    public int posY = 0;  // 12;

    //Player sprites
    public Sprite spriteLevel1_6;
    public Sprite spriteLevel1_8;
    public Sprite spriteLevel1_2;
    public Sprite spriteLevel1_4;
    public Sprite spriteLevel2_6;
    public Sprite spriteLevel2_8;
    public Sprite spriteLevel2_2;
    public Sprite spriteLevel2_4;
    public Sprite spriteLevel3_6;
    public Sprite spriteLevel3_8;
    public Sprite spriteLevel3_2;
    public Sprite spriteLevel3_4;

    //Hidden fields
    private float spawnMovementWake;

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
    public void Start()
    {
        spawnMovementWake = 0.0f;
    }

    //---------------------------------------------------------------
    public void SetSprite(int direction)
    //---------------------------------------------------------------
    {
        if (level == 0 || level == 1 || level == 2)
        {
            if(direction == 6) this.GetComponent<SpriteRenderer>().sprite = spriteLevel1_6;
            if(direction == 2) this.GetComponent<SpriteRenderer>().sprite = spriteLevel1_2;
            if(direction == 4) this.GetComponent<SpriteRenderer>().sprite = spriteLevel1_4;
            if(direction == 8) this.GetComponent<SpriteRenderer>().sprite = spriteLevel1_8;
        }
        else if (level == 3 || level == 4)
        {
            if (direction == 6) this.GetComponent<SpriteRenderer>().sprite = spriteLevel2_6;
            if (direction == 2) this.GetComponent<SpriteRenderer>().sprite = spriteLevel2_2;
            if (direction == 4) this.GetComponent<SpriteRenderer>().sprite = spriteLevel2_4;
            if (direction == 8) this.GetComponent<SpriteRenderer>().sprite = spriteLevel2_8;
        }
        else 
        {
            if (direction == 6) this.GetComponent<SpriteRenderer>().sprite = spriteLevel3_6;
            if (direction == 2) this.GetComponent<SpriteRenderer>().sprite = spriteLevel3_2;
            if (direction == 4) this.GetComponent<SpriteRenderer>().sprite = spriteLevel3_4;
            if (direction == 8) this.GetComponent<SpriteRenderer>().sprite = spriteLevel3_8;
        }

    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    public void Update()
    {

        //---------------------------------------------------------------
        //Move towards targer position
        //---------------------------------------------------------------
        Vector2 target = gameHandler.CalculateMapCoordinates(posX, posY);
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 0.005f);

        //---------------------------------------------------------------
        //Movement in progress
        //---------------------------------------------------------------
        float distance = Vector2.Distance(target, gameObject.transform.position);
        if(distance > 0.02f)
        {
            spawnMovementWake -= Time.deltaTime;
            if(spawnMovementWake <= 0.0f)
            {
                spawnMovementWake = 0.10f;
                StartCoroutine(SpawnWave(false));
            }
        }
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
        if(wait)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1.0f));
        }

        //Create wave
        GameObject.Instantiate(wave, gameObject.transform.localPosition, Quaternion.identity, transform.parent);
    }

}
