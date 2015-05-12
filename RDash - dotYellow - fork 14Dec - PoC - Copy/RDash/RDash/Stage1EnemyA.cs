///  Title:  PlatformDynamic Class
///  Author: Rob Rivera
///  For: Desu
///  Description: provides a structure enemy design and control in game. To be used for AI design, minimal art needed.
///  Enemies will utilize a class system, no polymorphism will be used for simplifying the main class. 
///  enemy positions will be based on the universal map, not the viewport (like the main character).
///  because each enemy will have unique timing for performance and habits, timing variables will be enemy.class variables...
///  NOTE: consider redesigning character class if enemy class proves to be superior.
///  controls ONLY enemy movement and enemy bullet generation. bullet behavior will be BASED off of enemy action but will be its own class.
///  enemyBullet class will by variable of enemy class but is NOT to be used in main DESU! class.
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
    class Stage1EnemyA
    {
        public Texture2D[] mainSpriteSheet;//should be three, but open. defined in main (size).
        public Vector2 position;//global position.
        public float rotation, gravityAccel, deltaTime, enemyTimer, enemyInterval, positionTimer,
                positionInterval;//rotion done in radians if i remember. fireTimer and fireInterval are unnecessary, as it will be the same as position timer.
        public Vector2 center;
        public Vector2 velocity;
        public int pose, numPoses, currentFrame, spriteWidth, health, numSprites;//determines which sprite sheet to use based on position. updated via user input.
        //pose guide: right, left,
        public bool alive, fire, airborne, hit;
        public Rectangle sourceRect;//determines size of base sprite. should stay consistent but is flexible.


        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public Stage1EnemyA(int numposes, float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteSize)
        {
            mainSpriteSheet = new Texture2D[numposes];
            position.X = initX;
            position.Y = initY;
            rotation = initRotation;
            center.X = initXCenter;
            center.Y = initYCenter;
            velocity.X = initXVelocity;
            velocity.Y = initYVelocity;
            pose = 0;
            alive = true;
            sourceRect = new Rectangle(0, 0, SpriteSize, SpriteSize);
            numPoses = numposes;
            airborne = true;
            currentFrame = 0;
            spriteWidth = 64;
            enemyInterval = 3000f / 25f;
            positionInterval = 6400f / 25f;
            positionTimer = 0f;
            gravityAccel = 2f;
            health = 2000;
            numSprites = 5;
            fire = false;
            //manually loads each spritesheet

        }


        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public void loadSpriteSheet(Texture2D Sprite, int index)
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


        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public void Update(float gameTime)
        {
            //fire = false; //inorder to make fire work, fire==true check must be completed immediately after update method in the same game cycle.
            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            deltaTime = gameTime;
            enemyTimer += deltaTime;
            positionTimer += deltaTime;
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
                enemyTimer = 0f;
                //do timing behavior here
                // bulletReady = true; //bulletReady boolean will be used for bulletspacing.
            }
            //following allows the static shooter to alter position.
            if (positionTimer > positionInterval)
            {
                fire = true;
                positionTimer = 0;//highly inefficient method below...but allows specific sprite pose changes...
                if (this.rotation == 1.57f)
                {
                    this.rotation = 1.57f + .75f; //2.32f
                }
                else if (this.rotation == 2.32f)
                {
                    this.rotation = 3.15f; //2.32f + 75f = 3.07f, so a little off, no biggie.
                }
                else if (this.rotation == 3.15f)
                {
                    this.rotation = 3.90f;
                }
                else if (this.rotation == 3.90f)
                {
                    this.rotation = 3.16f;
                }
                else if (this.rotation == 3.16f)
                {
                    this.rotation = 2.33f;
                }
                else if (this.rotation == 2.33f)
                {
                    this.rotation = 1.57f;
                }

            }
            if (this.airborne)
            {
                //apply physics later, use linear decelleration for now.
                if (this.velocity.Y < 10f)
                {
                    this.velocity.Y += gravityAccel;//custom hardcode for enemy gravity.
                }

                position.Y += velocity.Y;

                //Duke.position.Y += Duke.velocity.Y;

                //c
            }
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, this.mainSpriteSheet[this.pose].Height);






            ///
            /// asdf jkl;
            ///
        }
    }
}
