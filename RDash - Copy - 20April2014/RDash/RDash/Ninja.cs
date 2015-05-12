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
    public class Ninja : Enemy
    {

        public float bulletTimer = 0f;
        public float bulletInterval = 0f;
        public EnemyBullet bullet; 
        public Texture2D bulletTexture;
        
        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public Ninja(int numposes, float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteWidth, int SpriteHeight)
        {

            //===============everything between these lines are default lines for each enemy, but with different values. these initiations are standard.
            mainSpriteSheet = new Texture2D[numposes];
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
            sourceRect = new Rectangle(0, 0, SpriteWidth, SpriteHeight);
            numPoses = numposes;
            airborne = true;
            currentFrame = 0;
            spriteWidth = SpriteWidth;
            spriteHeight = SpriteHeight;
            enemyInterval = 3000f / 25f;
            bulletInterval = 4000f / 25f;
            gravityAccel = 2f;
            health = 2000;
            numSprites = 5;
            
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


        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public override void Update(float gameTime)
        {

            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            deltaTime = gameTime;
            enemyTimer += deltaTime;
            if (enemyTimer > enemyInterval)//possiblity of updateing the interval itself for advanced behavior control.
            {
                enemyTimer = 0f;
                bulletReady = true;
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
            deltaTime = gameTime;
            bulletTimer += deltaTime;
            if (bulletTimer > bulletInterval)//possiblity of updateing the interval itself for advanced behavior control.
            {
                bulletTimer = 0f;
                currentFrame++;
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
            if (this.airborne)//TODO: IMPLEMENT GRAVITY.
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
            
            //TODO determine how to base enemy sprite direction...probably shouldnt worry about it for a fast paced game like this tho...

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, this.mainSpriteSheet[this.pose].Height);

            
                
            


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
        public override void fireBullet(Vector2 dukePosition)
        {
            if (dukePosition.X < this.position.X)//determines if character faces left or right
            {
                rotation = 3.9f;
            }
            else { rotation = 5.45f; }
            bullet = new EnemyBullet(bulletTexture);
              
                bullet.position = this.position;
                bullet.position.X += spriteWidth / 2;
                bullet.position.Y += this.mainSpriteSheet[0].Height / 4;
                bullet.velocity = new Vector2(
                    (float)Math.Cos(this.rotation),
                    (float)Math.Sin(this.rotation)) * 10.0f;
                
            
        }
        
    }
}
