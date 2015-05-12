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
    class cursorWorldPlacement : VFX
    {

        /*inherited datafields...
        public Texture2D sprite;
        public Vector2 position;
        public float rotation, gravityAccel, ttl, spriteInterval;//dx and dy used for varying platform. ttl is indicative to numSpriteCycles.
        public Vector2 center;
        public Vector2 velocity, acceleration;
        public bool visible, airborne;
        public Rectangle sourceRect;
        */
        /*
        public Texture2D mainSpriteSheet;//should be three, but open. defined in main (size).
        public float previousBackgroundX, previousBackgroundY, currentBackgroundX, currentBackgroundY, futureBackgroundX, futureBackgroundY,
            spriteTimer, movementInterval, travelInterval, deltaTime;
        public int spriteWidth, spriteHeight, numSprites, currentFrame;
        */




        public cursorWorldPlacement(Texture2D loadedTexture)//i shouldn't use this constructor...
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            mainSpriteSheet = loadedTexture;
            center = new Vector2(mainSpriteSheet.Width / 2, mainSpriteSheet.Height / 2);
            velocity = Vector2.Zero;
            gravityAccel = 10f;
            airborne = true;
        }

        public cursorWorldPlacement(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
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
            spriteWidth = 76; //hardcoded to enforce class uniqueness in design
            spriteHeight = 76;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            //====used for maintaining change in gamemovement, to make it seem like they are matching dukes speed.
            previousBackgroundX = backgroundX;
            previousBackgroundY = backgroundY;
            currentBackgroundX = backgroundX;
            currentBackgroundY = backgroundY;
            //==================
            airborne = true;
            currentFrame = 0;
            spriteTimer = 2000f;
            spriteInterval = 3000f / 25f;
            movementInterval = 9500f / 25f; //with respect to the bullet timer...movements are about 3x's as long as rdash's rate of fire. 
            travelInterval = 3000f / 25f; //for now, I want him to move approximately this speed...
            gravityAccel = 0f; // aliens hover. 
            numSprites = 1;
            spawnBackgroundX = 0;
            spawnBackgroundY = 0;
            ttl = 100; 
            //======================================================================
            //manually loads each spritesheet

        }

        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public override void loadSpriteSheet(Texture2D Sprite)
        {
            mainSpriteSheet = Sprite;
            spriteWidth = mainSpriteSheet.Width / numSprites;
            spriteHeight = mainSpriteSheet.Height;
            center = new Vector2(mainSpriteSheet.Width / 2, mainSpriteSheet.Height / 2);

        }



        //the backgroundx, backgroundy supplied will be the new location the object updates to...advanced tweening must be done external to class...and result of tween 
        //fed to this method. 
        public override void Update(float gameTime, float backgroundX, float backgroundY)
        {
            ttl = 100;// this will never die until forced to through the game1 class calling it to die. 
            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            //updateing positioning to make up for dukes movement. 

            //previousBackgroundX = currentBackgroundX;
            //previousBackgroundY = currentBackgroundY;

            //currentBackgroundX = backgroundX;
            //currentBackgroundY = backgroundY;

            //this.position.X -= previousBackgroundX;
            //this.position.X += currentBackgroundX;

           
            


        }

        /*
         * helper function aids in controlling the targeting reticule for the block placement ability. 
         * 0 = right 
         * 1 = down
         * 2 = left 
         * 3 = up
         * */

        public void move(int direction)
        {
            switch (direction)
            {
                case 0:
                    if (position.X + spriteWidth - currentBackgroundX < (16 * 64))
                    {
                        position.X += spriteWidth;
                    }
                    break;
                case 1:
                    if (position.Y + spriteHeight - currentBackgroundY < 13 * 64)
                    {
                        position.Y += spriteHeight;
                    }
                    break;
                case 2:
                    if (position.X - spriteHeight - currentBackgroundY > 0)
                    {
                        position.X -= spriteHeight;
                    }
                    break;
                case 3:
                    if (position.Y - spriteHeight - currentBackgroundY > 0)
                    {
                        position.Y -= spriteHeight;
                    }
                    break;
                case 4:
                    break; 
            }


        }
    }
}
