using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class Debris : MonoBehaviour
//---------------------------------------------------------------
{
    public float waveSpawnTime = 3f;
    public GameObject wave;
    public int posX;
    public int posY;

    //Hidden fields
    private bool moveUp;
    private Vector3 targetPos;
    private Vector3 startPos;

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
        //Debris water animation
        //---------------------------------------------------------------
        InvokeRepeating("SpawnWaveCreator", waveSpawnTime, waveSpawnTime);
    }

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        moveUp = (Random.Range(0,1) == 0) ? true : false;
        startPos = transform.position;
        targetPos = startPos + new Vector3(0, Random.Range(-0.01f, 0.01f), 0);
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.00002f);

        if (moveUp)
        {
            float distance = Vector2.Distance(targetPos, transform.position);
            if (distance < 0.005f)
            {
                moveUp = false;
                targetPos = startPos + new Vector3(0, -0.01f, 0);
            }
        }
        else
        {
            float distance = Vector2.Distance(targetPos, transform.position);
            if (distance < 0.005f)
            {
                moveUp = true;
                targetPos = startPos + new Vector3(0,  0.01f, 0);
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
        if (wait)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1.0f));
        }

        //Create wave
        GameObject.Instantiate(wave, gameObject.transform.localPosition, Quaternion.identity, transform.parent);
    }

}
