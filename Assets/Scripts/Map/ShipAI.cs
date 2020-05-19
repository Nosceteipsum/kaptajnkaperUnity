using System;
using UnityEngine;

//---------------------------------------------------------------
public class ShipAI : Ship
//---------------------------------------------------------------
{
    //---------------------------------------------------------------
    //Hidden fields
    //---------------------------------------------------------------

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
    }

    //---------------------------------------------------------------
    public void MoveAI(int playerX,int playerY)
    //---------------------------------------------------------------
    {
        //------------------------
        // Player near, then don't move
        //------------------------
        if (posX == (playerX + 1) && posY == playerY)
            return;
        if (posX == (playerX - 1) && posY == playerY)
            return;
        if (posX == (playerX    ) && posY == playerY + 1)
            return;
        if (posX == (playerX    ) && posY == playerY - 1)
            return;

        if (posX == (playerX + 1) && posY == playerY + 1)
            return;
        if (posX == (playerX - 1) && posY == playerY - 1)
            return;
        if (posX == (playerX + 1) && posY == playerY - 1)
            return;
        if (posX == (playerX - 1) && posY == playerY + 1)
            return;
        if (posX == (playerX    ) && posY == playerY    )
            return;

        //------------------------
        // Move random
        //------------------------
        int way = UnityEngine.Random.Range(0, 8);
        if (way == 0) // North
        {
            int mapCheckResult = gameHandler.MapCheckCollision(posX, posY + 1);
            bool colisionCheck = gameHandler.ShipCollision(posX, posY + 1);
            if (mapCheckResult == 1 && colisionCheck == false)
            {
                posY++;
                SetSprite(8);
            }
        }
        if (way == 1) // South
        {
            int mapCheckResult = gameHandler.MapCheckCollision(posX, posY - 1);
            bool colisionCheck = gameHandler.ShipCollision(posX, posY - 1);
            if (mapCheckResult == 1 && colisionCheck == false)
            {
                posY--;
                SetSprite(2);
            }
        }
        if (way == 2) // South
        {
            int mapCheckResult = gameHandler.MapCheckCollision(posX + 1, posY);
            bool colisionCheck = gameHandler.ShipCollision(posX + 1, posY);
            if (mapCheckResult == 1 && colisionCheck == false)
            {
                posX++;
                SetSprite(6);
            }
        }
        if (way == 3) // West
        {
            int mapCheckResult = gameHandler.MapCheckCollision(posX - 1, posY);
            bool colisionCheck = gameHandler.ShipCollision(posX - 1, posY);
            if (mapCheckResult == 1 && colisionCheck == false)
            {
                posX--;
                SetSprite(4);
            }
        }

    }

}
