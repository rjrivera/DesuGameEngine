///  Title:  TronDartGunDR
///  Author: Rob Rivera
///  For: Desu
///  Description: A simple enemy class that contains the characteristics of said enemy. To be used in the Tron level's, loaded via approved enemy generation processes. 
///  
/// Definition of enemy classes to be done at level load. each instance will NOT carry information about any other enemy class except the 
/// class concerned. 
/// 
/// enemy behaviors to be determined in the update() method. update method will utilize said timings. 
/// 
/// IMPORTANT, to make this effective with memory, each enemy class array must be unallocated memory since they will NEVER be reused after a particular zone.
/// IMPORTANT, can use different sprite count if sprite timer is done in class updater method.
/// 
/// TODO: THIS IS THE VERSION 0 OF ENEMY CLASS, USE AS TEMPLATE FOR FUTURE ENEMIES. MAY MANIPULATE ANYTHING.
/// 
/// 
///  
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//using statements copied from the Game1.cs which references the xna framework classes.
namespace RDash
{
    public class TronDartGunDR : Enemy
    {

        public float bulletTimer = 0f;
        public float bulletInterval = 0f;
        public EnemyBullet bullet;
        public Texture2D bulletTexture;
        public float movementTimer = 0f;
        public float movementInterval = 0f;
        public float shotsFired = 0;
        public float travelTimer = 0f; //to be used as countdown for terminating movement. 
        public float travelInterval = 0f;
        public int directionRNG = 0;
        public bool moving = false;
        public float previousBackgroundX;
        public float previousBackgroundY;
        public float currentBackgroundX;
        public float currentBackgroundY;
        public float futureBackgroundX;
        public float futureBackgroundY;

        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public TronDartGunDR(float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, float backgroundX, float backgroundY)
        {

            //===============everything between these lines are default lines for each enemy, but with different values. these initiations are standard.
            numPoses = 1;
            mainSpriteSheet = new Texture2D[numPoses];
            position.X = initX;
            position.Y = initY;
            rotation = initRotation;
            fireTimer = initFireTimer;
            center.X = initXCenter;
            center.Y = initYCenter;
            velocity.X = initXVelocity;
            velocity.Y = initYVelocity;
            pose = 0;
            alive = true;
            spriteWidth = 32; //hardcoded to enforce class uniqueness in design
            spriteHeight = 32;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            //====used for maintaining change in gamemovement, to make it seem like they are matching dukes speed.
            /*previousBackgroundX = backgroundX;
            previousBackgroundY = backgroundY;*/
            currentBackgroundX = backgroundX;
            currentBackgroundY = backgroundY;
            //==================I DO NOT DESIRE THIS ENEMY TO MATCH DUKES SPEED.
            airborne = true;
            currentFrame = 0;
            enemyInterval = 3000f / 25f;
            bulletInterval = 30000f / 25f;
            movementInterval = 9500f / 25f; //with respect to the bullet timer...movements are about 3x's as long as rdash's rate of fire. 
            travelInterval = 4000f / 25f; //for now, I want him to move approximately this speed...
            gravityAccel = 0f; // aliens hover. 
            health = 5;
            numSprites = 6;
            ignoresWalls = true;

            //======================================================================
            //manually loads each spritesheet

        }


        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public override void loadSpriteSheet(Texture2D Sprite, int index, Texture2D bulletSprite)
        {
            if (index >= numPoses)
            {
                //do nothing prevents runtime error.
            }
            else
            {
                mainSpriteSheet[index] = Sprite;
            }

            bulletTexture = bulletSprite;
        }


        public override void Update(float gameTime) { } //no suitable use for this method, may cause problems down the road...consider sending more vital game info. 

        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public override void Update(float gameTime, float backgroundX, float backgroundY)
        {
            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            //updateing positioning to make up for dukes movement. 

            //previousBackgroundX = currentBackgroundX;
            //previousBackgroundY = currentBackgroundY;

            currentBackgroundX = backgroundX;
            currentBackgroundY = backgroundY;

            //this.position.X -= previousBackgroundX;
            //this.position.X += currentBackgroundX;

            // this.position.Y -= previousBackgroundY;
            // this.position.Y += currentBackgroundY; 

            if (position.X - backgroundX < 800 && position.X - backgroundX > -64 &&
                position.Y - backgroundY < 800 && position.Y - backgroundY > -200)
            {
                visible = true;
            }
            else { visible = false; }


            deltaTime = gameTime;
            enemyTimer += deltaTime;
            if (enemyTimer > enemyInterval)//possiblity of updateing the interval itself for advanced behavior control.
            {
                enemyTimer = 0f;
                currentFrame++;
                //following if statement is for clock independent spritesheets
                //if (dead && deathFrame < 7)
                //{
                //    deathFrame++;
                //}
                if (currentFrame > numSprites - 1)
                {
                    currentFrame = 0;
                }

                //do timing behavior here
                // bulletReady = true; //bulletReady boolean will be used for bulletspacing.
            }


            //==================bullet timer===================
            //each enemy must have a different bullet timer offset to prevent enemy synchronization...TODO
            bulletTimer += deltaTime;
            if (bulletTimer > bulletInterval )//possiblity of updateing the interval itself for advanced behavior control.
            {
                bulletTimer = 0f;
                this.setBulletReady(true);
                shotsFired++;
                if (shotsFired == 3)
                {
                    bulletInterval = 40000f / 25f;
                    shotsFired = 0;
                }
                else
                {
                    bulletInterval = 5000f / 25f;
                }
                //following if statement is for clock independent spritesheets
                //if (dead && deathFrame < 7)
                //{
                //    deathFrame++;
                //}

                //BULLET BEHAVIOR IMPLEMENTED IN MAIN CLASS since it spawns a new game object. 
                //do timing behavior here
                // bulletReady = true; //bulletReady boolean will be used for bulletspacing.
            }

            
                

           
            //====================================================
            //==================Traveling timer===================
            if (moving)
            {
                deltaTime = gameTime;
                travelTimer += deltaTime;
                if (travelTimer > travelInterval)//possiblity of updateing the interval itself for advanced behavior control.
                {
                    travelTimer = 0f;
                    moving = false;
                    //=========
                    //redefine velocity vectors to prevent object movement. 
                    //=========
                    this.velocity.X = 0f;
                    this.velocity.Y = 0f;
                }
            }

            //====================================================

            if (this.airborne)//TODO: IMPLEMENT GRAVITY.
            {
                //apply physics later, use linear decelleration for now.
                if (this.velocity.Y < 10f)
                {
                    this.velocity.Y += gravityAccel;//custom hardcode for enemy gravity.
                }

                position.Y += velocity.Y;

            }

            position.X += velocity.X;

            //TODO determine how to base enemy sprite direction...probably shouldnt worry about it for a fast paced game like this tho...

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, this.mainSpriteSheet[this.pose].Height);

            destRect = new Rectangle((int)(position.X), (int)(position.Y), (int)spriteWidth, (int)spriteHeight);




            ///
            /// asdf jkl;
            ///
        }

        /*
         * setBulletReady(bool setBullet)
         * parameters: setBullet
         * Purpose: allows mainclass to set bulletReady variable after spawning a bullet object, used as part of the enemy fire control system. 
         * Enemy bullet depends on enemy class. 
         * */
        public override void setBulletReady(bool setBullet)
        {
            bulletReady = setBullet;
        }


        /*
         * fireBullet(Vector2 dukePosition)
         * parameters: setBullet
         * Purpose: allows mainclass to set bulletReady variable after spawning a bullet object, used as part of the enemy fire control system. 
         * Enemy bullet depends on enemy class. 
         * */
        public override EnemyBullet fireBullet(Vector2 dukePosition, Vector2 dukeVelocity)
        {
            
            bullet = new EnemyBullet(bulletTexture);
            bullet.rotation = (float)(Math.PI * .3f);//lowerRight
            bullet.position = this.position;
            bullet.position.X += spriteWidth/2;
            bullet.position.Y += this.mainSpriteSheet[0].Height / 4;
            bullet.velocity = this.velocity;


            //determining velocity to go from alien to the pony
            bullet.velocity = new Vector2(
                       (float)Math.Cos(bullet.rotation) * 2f,
                       (float)Math.Sin(bullet.rotation)) * 10.0f;
            bullet.visible = true;

            this.setBulletReady(false);
            return bullet;
        }

        //class RNG
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}
