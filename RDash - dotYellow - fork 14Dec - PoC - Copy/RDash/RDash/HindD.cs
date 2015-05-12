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

    public class HindD : Enemy
    {

        
             /*
           * PlatformStatic constructer
           * parameters: None
           * */
            public HindD(int numposes, float initX, float initY, float initRotation, float initFireTimer,
                    float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteWidth, int SpriteHeight, bool bossEnemy)
            {
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
                gravityAccel = 2f;
                health = 20;
                numSprites = 4;
                boss = bossEnemy;
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
                

                sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, this.mainSpriteSheet[this.pose].Height);

            
                
            


                ///
                /// asdf jkl;
                ///
            }
	    }
    }
