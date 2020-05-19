using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class MainMenu : MonoBehaviour
//---------------------------------------------------------------
{
    public Cloud cloud;
    public int amountOfClouds;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        SpawnClouds();
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
    }

    //---------------------------------------------------------------
    public void SpawnClouds()
    //---------------------------------------------------------------
    {
        //---------------------------------------------------------------
        //Spawn random clouds on map
        //---------------------------------------------------------------
        for (int i = 0; i < amountOfClouds; i++)
        {
            var cloudInstance = Instantiate(cloud, gameObject.transform);
            cloudInstance.GetComponent<Cloud>().SetMainMenu();
        }
    }

}
