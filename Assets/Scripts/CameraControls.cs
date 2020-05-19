using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

//---------------------------------------------------------------
public class CameraControls : MonoBehaviour
//---------------------------------------------------------------
{
    private Rigidbody2D rbody;
    private Vector2 movement = new Vector2(0, 0);

    public float speed = 5.0f;
    public GameObject player;

    public GameObject GameMap;
    public GameObject GameCity;

    //---------------------------------------------------------------
    public void ToggleSnapping()
    //---------------------------------------------------------------
    {
    }

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        if(GameMap.activeSelf && player != null)
        {
            //---------------------------------------------------------------
            //Always set camera on player
            //---------------------------------------------------------------
            movement = player.transform.localPosition - transform.localPosition;
            rbody.AddForce(movement * speed * Time.deltaTime);
        }
        else //if(GameCity.activeSelf) // battle // Menu
        {
            //---------------------------------------------------------------
            //Always set camera center on background
            //---------------------------------------------------------------
            transform.position = new Vector3(0,0,-10);
        }
    }

    //---------------------------------------------------------------
    public void ShowCredits()
    //---------------------------------------------------------------
    {
        Application.OpenURL("http://mightycodedragon.com/KaptajnKaper/license");
    }

    //---------------------------------------------------------------
    public void ChangeSceneToGame()
    //---------------------------------------------------------------
    {
        SceneManager.LoadScene("Scenes/Ingame");
    }


}
