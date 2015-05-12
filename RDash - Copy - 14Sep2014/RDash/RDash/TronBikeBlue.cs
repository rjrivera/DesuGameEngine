///  Title:  TronDart
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
    public class TronBikeBlue : Enemy
    {

        public float previousBackgroundX;
        public float previousBackgroundY;
        public float currentBackgroundX;
        public float currentBackgroundY;
        public float futureBackgroundX;
        public float futureBackgroundY;
        
        public bool enemyBulletReady = false; // necessary for game management routines to be generic and simple; treat all enemies as potentially capable of firing. 

        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public TronBikeBlue(float initX, float initY, float initRotation, float initFireTimer,
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
            spriteWidth = 64; //hardcoded to enforce class uniqueness in design
            spriteHeight = 64;
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
            gravityAccel = 1f; // aliens hover. 
            health = 5;
            numSprites = 4;
            ignoresWalls = false; 

            //======================================================================
            //manually loads each spritesheet

        }



        

        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public override void loadSpriteSheet(Texture2D Sprite, int index)
        {
            if (index >= numPoses)
            {
                //do nothing prevents runtime error.
            }
            else
            {
                mainSpriteSheet[index] = Sprite;
            }


        }

        public override void Update(float gameTime) { } //no suitable use for this method, may cause problems down the road...consider sending more vital game info. 

        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public override void Update(float gameTime, float backgroundX, float backgroundY)
        {
            

            currentBackgroundX = backgroundX;
            currentBackgroundY = backgroundY;

            //determins if viewable
            if (position.X - backgroundX < 800 && position.X - backgroundX > 0 &&
                position.Y - backgroundY < 800 && position.Y - backgroundY < 800)
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
                
                if (currentFrame > numSprites - 1)
                {
                    currentFrame = 0;
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





            destRect = new Rectangle((int)(position.X ),(int) (position.Y),(int) spriteWidth,(int) spriteHeight);
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
         * reverseVelocityX(
         * parameters: none
         * Purpose: allows mainclass to reverse the X velocity of the enemy unit. Usually called if enemy runs into an obstacle. 
         * */
        public override void reverseVelocityX()
        {
            velocity.X *= -1; 
        }


        /*
         * fireBullet(Vector2 dukePosition)
         * parameters: setBullet
         * Purpose: allows mainclass to set bulletReady variable after spawning a bullet object, used as part of the enemy fire control system. 
         * Enemy bullet depends on enemy class. 
         * */
        
        //class RNG
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}
