///  Title:  un_Dead_Core.cs
///  Author: Rob Rivera
///  For: rdasu
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
/// TODO: THIS IS THE VERSION 0 OF COMPLEXED ENEMY CLASS, USE AS TEMPLATE FOR FUTURE ENEMIES. MAY MANIPULATE ANYTHING.
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
    public class un_Dead_Core : Enemy
    {
        //NOTE, THE POSITION VECTOR WILL BE BASED OFF OF THE TRUE BACKGROUNDX, AND BACKGROUNDY FOR CODE REUSE PURPOSES. 
        public bool spawnRockets;
        public Texture2D bulletTexture;
        public float phaseTimer = 0f; //to be used as countdown for terminating a certain phase's behavior. 
        public float phaseInterval = 0f;
        public Enemy unDeadCoreA, unDeadCoreB, unDeadCoreMain; //TRUE POSITION WILL BE USED FOR ALL FUCKING ENEMIES AND OBJECTS EXCEPT DUKE!.
        public int phase;
        public List<VFX> connectingRingFX = new List<VFX>();//currently, specialFX will refer to the 10 adjoining rings...rename as such.
        public List<Bullet> missiles= new List<Bullet>(); 
        

        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public un_Dead_Core(float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, Texture2D ringSprite, Texture2D miniCore, Texture2D mainCore)
        {
            unDeadCoreA = new mini_Core(initX, initY, initRotation, initFireTimer, initXCenter, initYCenter, initXVelocity, initYVelocity, backgroundX, backgroundY, miniCore);
            unDeadCoreB = new mini_Core(initX + 10f, initY + 160f, initRotation, initFireTimer, initXCenter, initYCenter, initXVelocity, initYVelocity, backgroundX, backgroundY, miniCore);
            unDeadCoreMain = new main_Core(initX + 20f, initY + 100f, initRotation, initFireTimer, initXCenter, initYCenter, initXVelocity, initYVelocity, backgroundX, backgroundY, mainCore);
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
            spriteWidth = 0; //Not to be used for this multi-hitboxed enemy. 
            spriteHeight = 0;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);//not to be used for this multi-hitboxed enemy. 
            //==================
            airborne = true; //this particular boss will be immune to gravity...TODO properly implement imunity to gravity...levitation?
            currentFrame = 0;
            enemyInterval = 3000f / 25f;
            phaseTimer = 0f;
            gravityAccel = 0f; // aliens hover...is this how I will levitate? make the gravity mechanic internal?...I guess so. 
            health = 200;
            numSprites = 0;

            //======================================================================
            //manually loads each sprite in rings. DONE MANUALLY AND HARDCODED (BAD JUJU). 
            float tempRingX = 0f;
            float tempRingY = 0f; 
            for (int i = 0; i < 5; i++)//rings 0-4 will be for between the main body and the auxillary A, rings 5-9 will be for the main body through auxillary B. 
            {

                //(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
            //float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, int sprites)
                connectingRingFX.Add(new ring(tempRingX, tempRingY, 0f, 0f,
                            0f, 0f, 16f, backgroundX, backgroundY, 3));//backgroundX WILL be that of the hero in cases of special bosses, but it is better to use the TRUE position. 
                connectingRingFX.ElementAt<VFX>(i).loadSpriteSheet(ringSprite);
                connectingRingFX.ElementAt<VFX>(i).ttl = 1;//ttl = 0 when the character DIES. 
                connectingRingFX.ElementAt<VFX>(i).spriteInterval = 2000f / 25f;//I'll leave this hear incase I change my mind about sprites. 
            }
            for (int i = 5; i < 10; i++)//rings 5-9 will be for between the main body and the auxillary A, rings 5-9 will be for the main body through auxillary B. 
            {

                //(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
                //float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, int sprites)
                connectingRingFX.Add(new ring(tempRingX, tempRingY, 0f, 0f,
                            0f, 0f, 16f, backgroundX, backgroundY, 3));//backgroundX WILL be that of the hero in cases of special bosses, but it is better to use the TRUE position. 
                connectingRingFX.ElementAt<VFX>(i).loadSpriteSheet(ringSprite);
                connectingRingFX.ElementAt<VFX>(i).ttl = 1;//ttl = 0 when the character DIES. 
                connectingRingFX.ElementAt<VFX>(i).spriteInterval = 2000f / 25f;//I'll leave this hear incase I change my mind about sprites. 
            }
        }


        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public void loadBulletSpriteSheet(Texture2D bulletSprite)
        {

            bulletTexture = bulletSprite;
        }

        /*
         * Returns List [accessor method]
         * parameters: spritesheet, index
         * */
        public override List<VFX> getListVFX()
        {
            //List<VFX> tempVFX = new List<VFX>();
            //for (int i = 0; i < 10; i++)
            //{
            //    tempVFX.Add(connectingRingFX.ElementAt<VFX>(i));
            //}
            return connectingRingFX; 
        }


        ///*
        // * Sprite loader
        // * parameters: spritesheet, index
        // * */
        //public List<Bullet> getListBullets()
        //{
        //    return missiles;
        //}
        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public void loadSpriteSheet(Texture2D mainUnDeadCoreSprite, Texture2D miniUnDeadCoreSprite)
        {

            this.unDeadCoreA.mainSpriteSheet[0] = miniUnDeadCoreSprite;
            this.unDeadCoreB.mainSpriteSheet[0] = miniUnDeadCoreSprite;
            this.unDeadCoreMain.mainSpriteSheet[0] = mainUnDeadCoreSprite; 

        }


        public override void Update(float gameTime) { } //no suitable use for this method, may cause problems down the road...consider sending more vital game info. 

        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public override void Update(float gameTime, float backgroundX, float backgroundY)
        {
            //Insert updates that always occure here. 
            deltaTime = gameTime; 
            

            //insert updates for switch cases
            switch (phase)
            {
                case 0:
                    phaseTimer += deltaTime;
                    if (phaseTimer >= phaseInterval)//occures upon completion of phase. 
                    {
                        phaseTimer = 0f;
                        phase = this.randomNumber(1, 4);
                        if (phase == 1)
                        {
                            phaseInterval = 20000f / 25f;
                        }
                        else if (phase == 2)
                        {
                            phaseInterval = 10000f / 25f;
                        }
                        else if (phase == 3)
                        {
                            phaseInterval = 15000f / 25f;
                        }
                    }
                    else
                    {
                        //perform update action for the IDLE sequence.

                    }
                    break;
                case 1:
                    phaseTimer += deltaTime;
                    if (phaseTimer >= phaseInterval)//occures upon completion of phase. 
                    {
                        phaseTimer = 0f;
                        phase = 0;
                        phaseInterval = 30000f / 25f;
                    }
                    else
                    {
                        //perform update action for the fist sequence.

                    }
                    break;
                case 2:
                    phaseTimer += deltaTime;
                    if (phaseTimer >= phaseInterval)//occures upon completion of phase. 
                    {
                        phaseTimer = 0f;
                        phase = 0;
                    }
                    else
                    {
                        //perform update action for the rocket sequence.

                    }
                    
                    break;
                case 3:
                    phaseTimer += deltaTime;
                    if (phaseTimer >= phaseInterval)//occures upon completion of phase. 
                    {
                        phaseTimer = 0f;
                        phase = 0;
                    }
                    else
                    {
                        //perform update action for the laser sequence.

                    }
                    break;
                default:
                    //do nothing 
                    break;
            }
            //iterate through each ring to update positions to reflect an equidistant spacing between the minicore and main core. 
            //manually loads each sprite in rings. DONE MANUALLY AND HARDCODED (BAD JUJU). 
            float tempRingX = 0f;
            float tempRingY = 0f;
            float deltaX = 0f;
            float deltaY = 0f;
            for (int i = 0; i < 5; i++)//rings 0-4 will be for between the main body and the auxillary A, rings 5-9 will be for the main body through auxillary B. 
            {
                deltaX = this.unDeadCoreMain.position.X - this.unDeadCoreA.position.X;
                tempRingX = this.unDeadCoreA.position.X + (i) * (deltaX / 5);
                deltaY = this.unDeadCoreMain.position.Y - this.unDeadCoreA.position.Y;
                tempRingY = this.unDeadCoreA.position.Y + (i) * (deltaY / 5);
                //(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
                //float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, int sprites)
                
               
                connectingRingFX.ElementAt<VFX>(i).position.X = tempRingX;//ttl = 0 when the character DIES. 
                connectingRingFX.ElementAt<VFX>(i).position.Y = tempRingY;//I'll leave this hear incase I change my mind about sprites. 
            }
            for (int i = 5; i < 10; i++)//rings 5-9 will be for between the main body and the auxillary A, rings 5-9 will be for the main body through auxillary B. 
            {
                deltaX = this.unDeadCoreMain.position.X - this.unDeadCoreB.position.X;
                tempRingX = this.unDeadCoreB.position.X + (i - 5) * (deltaX / 5);
                deltaY = this.unDeadCoreMain.position.Y - this.unDeadCoreB.position.Y;
                tempRingY = this.unDeadCoreB.position.Y + (i - 5) * (deltaY / 5);
                //(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
                //float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, int sprites)


                connectingRingFX.ElementAt<VFX>(i).position.X = tempRingX;//ttl = 0 when the character DIES. 
                connectingRingFX.ElementAt<VFX>(i).position.Y = tempRingY;//I'll leave this hear incase I change my mind about sprites. 
            }

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
            //TODO: DETERMINE HOW TO HANDLE BULLETS. 
            //rotation = 5.45f;
            //bullet = new EnemyBullet(bulletTexture);

            //bullet.position = this.position;
            //bullet.position.X += spriteWidth * 2;
            //bullet.position.Y += this.mainSpriteSheet[0].Height / 4;
            //bullet.velocity = this.velocity;


            ////determining velocity to go from alien to the pony
            //bullet.velocity.X = (dukePosition.X - this.position.X) / 12;
            //bullet.velocity.Y = (dukePosition.Y - this.position.Y) / 12;




            //this.setBulletReady(false);
            return new EnemyBullet(bulletTexture);
        }

        //class RNG
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max); //min is inclusive, max is exlusive. 
        }

    }

}
