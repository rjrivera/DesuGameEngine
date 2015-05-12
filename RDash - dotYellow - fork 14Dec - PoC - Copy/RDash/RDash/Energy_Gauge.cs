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
    class energy_Gauge : VFX
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



        public energy_Gauge(Texture2D loadedTexture)//i shouldn't use this constructor...
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            mainSpriteSheet = loadedTexture;
            center = new Vector2(mainSpriteSheet.Width / 2, mainSpriteSheet.Height / 2);
            velocity = Vector2.Zero;
            gravityAccel = 10f;
            airborne = true;
        }

        public energy_Gauge(float initX, float initY, float initRotation, float initXCenter, float initYCenter,
            float initXVelocity, float initYVelocity, float backgroundX, float backgroundY, int sprites)
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
            spriteWidth = 188; //hardcoded to enforce class uniqueness in design
            spriteHeight = 15;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            ttl = 1;

            //==================
            airborne = true;
            currentFrame = 0;
            spriteTimer = 2000f;
            spriteInterval = 3000f / 25f;
            movementInterval = 9500f / 25f; //with respect to the bullet timer...movements are about 3x's as long as rdash's rate of fire. 
            travelInterval = 3000f / 50f; //for now, I want him to move approximately this speed...
            gravityAccel = 0f; // aliens hover. 
            numSprites = sprites;

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
        public override void Update(float gameTime, float backgroundX, float backgroundY, float energy, float energyCapacity)//need's a unique update since it changes size based on amount of Duke energy. 
        {
            //following segment updates the bulletReady boolean value.
            //consider a seperate timer for bullet timing. 
            //updateing positioning to make up for dukes movement. 

            //previousBackgroundX = currentBackgroundX;
            //previousBackgroundY = currentBackgroundY;

            //currentBackgroundX = backgroundX;
            //currentBackgroundY = backgroundY;

            //this.position.X -= previousBackgroundX;
            //this.position.X += currentBackgroundX;


            deltaTime = gameTime;
            spriteTimer += deltaTime;


            if (spriteTimer > spriteInterval)//possiblity of updateing the interval itself for advanced behavior control.
            {
                spriteTimer = 0f;

                currentFrame++;
                //following if statement is for clock independent spritesheets
                //if (dead && deathFrame < 7)
                //{
                //    deathFrame++;
                //}
                if (currentFrame > numSprites - 1)
                {
                    currentFrame = 0;
                    ////ttl--; no ttl because it is a status bar, i'll need to set off of screen using a different trigger than 'time'. 
                }
                spriteWidth = 188; 
                sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, this.mainSpriteSheet.Height);
            }
            spriteWidth = (int)(188 * (energy/energyCapacity));//this provides the scaling to energy levels for the power bar. 
            this.position.X = backgroundX + 102f;//+2 offset from energygaugeholder. 
            this.position.Y = backgroundY + 404f;//+4 offset from energygaugeholder. 




        }
    }
}
