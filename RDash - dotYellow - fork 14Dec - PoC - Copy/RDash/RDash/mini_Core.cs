///  Title:  mini_Core.cs
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
    public class mini_Core : Enemy
    {
        //NOTE, THE POSITION VECTOR WILL BE BASED OFF OF THE TRUE BACKGROUNDX, AND BACKGROUNDY FOR CODE REUSE PURPOSES. 

        
   


        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public mini_Core(float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, Texture2D mainSprite)
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
            spriteWidth = mainSprite.Width; //Not to be used for this multi-hitboxed enemy. 
            spriteHeight = mainSprite.Height;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);//not to be used for this multi-hitboxed enemy. 
            //==================
            airborne = true; //this particular boss will be immune to gravity...TODO properly implement imunity to gravity...levitation?
            currentFrame = 0;
            enemyInterval = 3000f / 25f;
        
            gravityAccel = 0f; // aliens hover...is this how I will levitate? make the gravity mechanic internal?...I guess so. 
            health = 200;
            numSprites = 0;

            
        }

        public override void Update(float gameTime) { } //no suitable use for this method, may cause problems down the road...consider sending more vital game info. 

        /*
         * Update()
         * parameters: spritesheet, index
         * */
        public void Update(float gameTime, float backgroundX, float backgroundY)
        {
            //Insert updates that always occure here. 
            deltaTime = gameTime;

            //this enemy is currently puppeteered by a more complexed enemy. 
           
        }

        

        

        //class RNG
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max); //min is inclusive, max is exlusive. 
        }

    }

}
