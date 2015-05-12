//PLACEHOLDER CLASS FOR TRANSITIONING SHOOTER INFO TO DESU!
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RDash
{
    class SFX : GameObject
    {

        /*inherited datafields...
        public Texture2D sprite;
        public Vector2 position;
        public float rotation, gravityAccel, ttl;//dx and dy used for varying platform.
        public Vector2 center;
        public Vector2 velocity, acceleration;
        public bool visible, airborne;
        public Rectangle sourceRect;
        */
        public Texture2D mainSpriteSheet;//should be three, but open. defined in main (size).
        public float deltaTime;
        public int spriteWidth, spriteHeight, numSprites, currentFrame; 

        public SFX(Texture2D loadedTexture)//i shouldn't use this constructor...
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            
           
            velocity = Vector2.Zero;
            gravityAccel = 10f;
            airborne = true;
        }

        public SFX(float initX, float initY, float initRotation, float initXCenter, float initYCenter, 
            float initXVelocity, float initYVelocity, float backgroundX, float backgroundY)
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            
            velocity = Vector2.Zero;
            gravityAccel = 10f;
            airborne = true;
            //===============everything between these lines are default lines for each enemy, but with different values. these initiations are standard.
            position.X = initX;
            position.Y = initY;
            rotation = initRotation;
            center.X = initXCenter;
            center.Y = initYCenter;
            velocity.X = initXVelocity;
            velocity.Y = initYVelocity;
            spriteWidth = 64; //hardcoded to enforce class uniqueness in design
            spriteHeight = 64;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            
            airborne = true;
            currentFrame = 0;
            numSprites = 6;
            
            //======================================================================
            //manually loads each spritesheet

        }

        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public override void loadSpriteSheet(Texture2D Sprite)
        {
                mainSpriteSheet= Sprite;

        }

        //the backgroundx, backgroundy supplied will be the new location the object updates to...advanced tweening must be done external to class...and result of tween 
        //fed to this method. 
         public override void Update(float gameTime, float backgroundX, float backgroundY) 
        {
            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            //updateing positioning to make up for dukes movement. 

           

           // this.position.Y -= previousBackgroundY;
           // this.position.Y += currentBackgroundY; 


            deltaTime = gameTime;
            ttl--;


            
            }
    }
}
