using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------
public class BoardSoldierPlayer : MonoBehaviour
//---------------------------------------------------------------
{
    public GameObject Smoke;
    public BoardBattle boardBattle;
    public GameObject boardShoot;
    public GameObject graveYard;
    public bool AI;

    private GameObject CommandPoint;
    private Animator animator;
    private float ShootCooldown;
    private bool dead;
    private bool fighting;
    private float fightingTime;
    private bool fightingDie;

    //---------------------------------------------------------------
    // Start is called before the first frame update
    //---------------------------------------------------------------
    void Start()
    {
        fighting = false;
        dead = false;
        ShootCooldown = 0.0f;
        fightingTime = 0.0f;
        animator = GetComponent<Animator>();

        //---------------------------------------------------------------
        //Set start animation
        //---------------------------------------------------------------
        AnimationStandingStill();
    }

    //---------------------------------------------------------------
    // Update is called once per frame
    //---------------------------------------------------------------
    void Update()
    {
        //---------------------------------------------------------------
        //Wait for commandpoint
        //---------------------------------------------------------------
        if (CommandPoint == null) return;
        if (dead) return;

        //---------------------------------------------------------------
        //Soldier is fighting
        //---------------------------------------------------------------
        if (fighting)
        {
            if (fightingTime > 0.0f)
            {
                fightingTime -= Time.deltaTime;
                return;
            }
            else
            {
                //---------------------------------------------------------------
                //Kill or survive
                //---------------------------------------------------------------
                if (fightingDie)
                {
                    AnimationDead();
                    return;
                }
                else
                {
                    fighting = false;
                }
            }
        }

        //---------------------------------------------------------------
        //Cooldown when shooting
        //---------------------------------------------------------------
        if (ShootCooldown <= 0.0f)
        {
           
        }
        else
        {
            ShootCooldown -= Time.deltaTime;
            return;
        }

        //---------------------------------------------------------------
        //Animation when moving
        //---------------------------------------------------------------
        if (Vector3.Distance(transform.position, CalculatePosition() ) > 0.0001f)
        {
            AnimationWalk();

            //---------------------------------------------------------------
            //Follow player point (Keep rank position by collection position
            //---------------------------------------------------------------
            float speed = 0.001f;

            if (AI == false && boardBattle.GetPlayerAmmo() <= 0)
            {
                speed = 0.0015f;
            }
            else if (AI == true && boardBattle.GetEnemyAmmo() <= 0)
            {
                speed = 0.0015f;
            }

            transform.position = Vector3.MoveTowards(transform.position, CalculatePosition(), speed);
        }
        else
        {
            AnimationStandingStill();
        }

    }

    //---------------------------------------------------------------
    public void SetFighting(bool die,float time)
    //---------------------------------------------------------------
    {
        //Set animation
        animator.Play("PlayerFight");
        animator.speed = 1.0f;

        //Start timer
        fighting = true;
        fightingTime = time;
        fightingDie = die;
    }

    //---------------------------------------------------------------
    public bool IsFighting()
    //---------------------------------------------------------------
    {
        return fighting;
    }

    //---------------------------------------------------------------
    public void AnimationDead()
    //---------------------------------------------------------------
    {
        if(animator != null)
        {
            animator.Play("PlayerDead");
            animator.speed = 1.0f;
        }
        else
        {
            return; //Wow, you survived a bullet
        }

        dead = true;

        //---------------------------------------------------------------
        //Play hit sound
        //---------------------------------------------------------------
        GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
        GetComponent<AudioSource>().Play();

        //---------------------------------------------------------------
        //Move soldier to graveyeard group
        //---------------------------------------------------------------
        transform.SetParent(graveYard.transform);
    }

    //---------------------------------------------------------------
    public void SetCommandPoint(GameObject commandPoint)
    //---------------------------------------------------------------
    {
        CommandPoint = commandPoint;
    }

    //---------------------------------------------------------------
    private Vector3 CalculatePosition()
    //---------------------------------------------------------------
    {
        //Find soldier position
        int pos = transform.GetSiblingIndex();

        Vector3 result = new Vector3(CommandPoint.transform.position.x, CommandPoint.transform.position.y, CommandPoint.transform.position.z);
        if (pos % 2 == 1)
            result += new Vector3(0.1f,0,0);

        result += new Vector3(0, ((float)(pos / 2)) * 0.1f, 0);

        return result;
    }

    //---------------------------------------------------------------
    public void AnimationShoot()
    //---------------------------------------------------------------
    {
        if (IsFighting()) return;

        animator.Play("PlayerShoot");
        animator.speed = 1.0f;
        ShootCooldown = 1.0f;
        if(AI)
        {
            //---------------------------------------------------------------
            //Spawn FlashEffect
            //---------------------------------------------------------------
            Instantiate(Smoke, transform.position + new Vector3(-0.1f + Random.Range(-0.01f, 0.01f), 0.03f + Random.Range(-0.01f, 0.01f), 0), Quaternion.identity, transform.parent);

            //---------------------------------------------------------------
            //Spawn Bullet
            //---------------------------------------------------------------
            var bullet = Instantiate(boardShoot, transform.position + new Vector3(-0.1f + Random.Range(-0.01f, 0.01f), 0.03f + Random.Range(-0.01f, 0.01f), 0), Quaternion.identity, transform.parent);
            bullet.GetComponent<BoardBullet>().GoingRight = false;
            bullet.GetComponent<BoardBullet>().armyEnemy = boardBattle.armyEnemy;
            bullet.GetComponent<BoardBullet>().armyPlayer = boardBattle.armyPlayer;
        }
        else
        {
            //---------------------------------------------------------------
            //Spawn FlashEffect
            //---------------------------------------------------------------
            Instantiate(Smoke, transform.position + new Vector3(0.1f + Random.Range(-0.01f, 0.01f), 0.03f + Random.Range(-0.01f, 0.01f), 0), Quaternion.identity, transform.parent);

            //---------------------------------------------------------------
            //Spawn Bullet
            //---------------------------------------------------------------
            var bullet = Instantiate(boardShoot, transform.position + new Vector3(0.1f + Random.Range(-0.01f, 0.01f), 0.03f + Random.Range(-0.01f, 0.01f), 0), Quaternion.identity, transform.parent);
            bullet.GetComponent<BoardBullet>().armyEnemy = boardBattle.armyEnemy;
            bullet.GetComponent<BoardBullet>().armyPlayer = boardBattle.armyPlayer;
        }
    }

    //---------------------------------------------------------------
    private void AnimationWalk()
    //---------------------------------------------------------------
    {
        animator.speed = 1.0f;

        if (AI == false && boardBattle.GetPlayerAmmo() <= 0)
        {
            animator.Play("PlayerCharge");
        }
        else if (AI == true && boardBattle.GetEnemyAmmo() <= 0)
        {
            animator.Play("PlayerCharge");
        }
        else
        {
            animator.Play("PlayerWalk");
        }
    }

    //---------------------------------------------------------------
    private void AnimationStandingStill()
    //---------------------------------------------------------------
    {
        animator.Play("PlayerWalk");
        animator.speed = 0f;
    }

}
