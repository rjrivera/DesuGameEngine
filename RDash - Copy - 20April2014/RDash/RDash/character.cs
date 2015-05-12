///  Title:  PlatformDynamic Class
///  Author: Rob Rivera
///  For: Desu
///  Description: provides a structure platforming mechanic in game. To be used for level design, minimal art needed.
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
    class Character : Player
    {
        public Texture2D[] mainSpriteSheet;//should be three, but open. defined in main (size).
        public Vector2 position;//global position.
        public float rotation, fireTimer, spriteTimer, spriteInterval, energy, energyCapacity, health, healthCapacity, maxSpeed, progressionTimer,
            allRangeX, allRangeY;//rotion done in radians if i remember.
        public Vector2 center;
        public Vector2 velocity;
        public int pose, numPoses, currentFrame, numSprites, spriteWidth, spriteHeight;//determines which sprite sheet to use based on position. updated via user input.
        //pose guide: right, left,
        public bool alive, fire, airborne, levitation, hit, shield, spawnShieldRDash, spawnFlyingNimbus, fireCannonBall, allRangeMode;
        public Rectangle sourceRect;//determines size of base sprite. should stay consistent but is flexible.
        
        
        
          /*
         * PlatformStatic constructer
         * parameters: None
         * */
        //TO DO, INCLUDE ANOTHER CONSTRUCTOR VARIABLE FOR numSprites.
        public Character(int numposes, float initX, float initY, float initRotation, float initFireTimer,
                float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteSize)
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
            sourceRect = new Rectangle(0, 0, SpriteSize, SpriteSize);
            numPoses = numposes;
            airborne = true;
            currentFrame = 0;
            spriteInterval = 3000f / 25f;
            spriteTimer = 0f;
            this.numSprites = 1;//CHANGED TO 1 FROM 5 FOR TESTING DOTYELLOW
            spriteWidth = 64;
            spriteHeight = 64;
            levitation = false;
            energy = 200f; //25f is about half a second
            energyCapacity = 600f; 
            //manually loads each spritesheet
            shield = false;
            health = 100f;
            healthCapacity = 100f; 
            maxSpeed = 6f;
            allRangeMode = false;
            
        }

        /*
         * Sprite loader
         * parameters: spritesheet, index
         * */
        public void update(GameTime gameTime, KeyboardState previousKeyboardState, KeyboardState keyboardState)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            progressionTimer += deltaTime;
            //this section updates the character based on the keyboard state. 
            //allRangeMode = true; 
            //===============================================
            if (allRangeMode)
            {
                this.velocity.X = 0f;
                this.velocity.Y = 0f;
                //input allRangeMode behavior here. 
                levitation = false;
                airborne = true;
                position.Y -= 1f;//hardcoding counterbalance of gravityAccel...TODO: rearchitect gravity more. 

                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Left))
                {
                    this.rotation = 3.9f;
                    this.position.X -= 6f;//moves left at the per/second rate.
                    this.position.Y -= 6f; 

                }
                else if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = 5.45f;
                    this.position.X += 6f;
                    this.position.Y -= 6f;
                    //Duke.velocity.X += maxSpeed;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = this.rotation = (float)(Math.PI * .25f);
                    this.position.X += 6f;
                    this.position.Y += 6f;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Left))
                {
                    this.rotation = (float)(Math.PI * .75f);
                    this.position.X -= 6f;//moves left at the per/second rate.
                    this.position.Y += 6f;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    this.rotation = 1.57f;
                    this.position.Y += 6f;
                }

                else if (keyboardState.IsKeyDown(Keys.Left))
                {

                    this.rotation = (float)Math.PI;
                    this.position.X -= 6f;//moves left at the per/second rate.
                    

                }

                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = 0f;
                    this.position.X += 6f;

                }

                else if (keyboardState.IsKeyDown(Keys.Up))
                {
                    this.rotation = 4.71f;//4.7f;
                    this.position.Y -= 6f;
                }

                else
                {
                    this.rotation = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {

                    fireCannonBall = true;
                    //great test bed

                }

                spawnFlyingNimbus = true; //replace the flyin nimbus with a NEW special effect, low priority. TODO. 

                if (keyboardState.IsKeyDown(Keys.E) && !this.shield && this.energy >= 20f)
                {
                    this.shield = true;
                    this.decrementEnergy(20f);
                    spawnShieldRDash = true;
                    //specialFX.Add(new shield_RDash(Duke.position.X + backgroundX + 10f, Duke.position.Y + backgroundY + 10f, 0f, 0f,
                    //0f, 0f, 0f, backgroundX, backgroundY, 3));
                    //numFX++;
                    //specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\shieldRDash"));
                    //shieldActivation.Play();

                }

                //making sure I stay in bounds of allrange mode.

                if (this.position.X > allRangeX - this.spriteWidth)
                {
                    this.position.X = allRangeX - this.spriteWidth;
                }
                if (this.position.Y > allRangeY - this.spriteHeight)
                {
                    this.position.Y = allRangeY - this.spriteHeight;
                }
                if (this.position.X < 0  )
                {
                    this.position.X = 0;
                }
                if (this.position.Y < 0)
                {
                    this.position.Y = 0;
                }
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Left))
                {
                    this.rotation = 3.9f;
                    //Duke.velocity.X -= maxSpeed;//moves left at the per/second rate.

                }
                else if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = 5.45f;
                    //Duke.velocity.X += maxSpeed;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = this.rotation = (float)(Math.PI * .25f);
                    //Duke.velocity.X += maxSpeed;
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Left))
                {
                    this.rotation = (float)(Math.PI * .75f);
                    //Duke.velocity.X += maxSpeed;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    this.rotation = 1.57f;
                }

                else if (keyboardState.IsKeyDown(Keys.Left))
                {

                    this.rotation = (float)Math.PI;

                }

                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    this.rotation = 0f;
                    //Duke.velocity.X += maxSpeed;

                }

                else if (keyboardState.IsKeyDown(Keys.Up))
                {
                    this.rotation = 4.71f;//4.7f;
                }

                else
                {
                    this.rotation = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {

                    fireCannonBall = true;
                    //great test bed

                }

                if (keyboardState.IsKeyDown(Keys.W) && this.energy > 0 && this.airborne)
                {
                    this.levitation = true;
                    this.decrementEnergy(1f);
                    this.velocity.Y = 0;
                    if (!previousKeyboardState.IsKeyDown(Keys.W))
                    {
                        spawnFlyingNimbus = true;
                        //specialFX.Add(new flying_Nimbus(Duke.position.X + backgroundX, Duke.position.Y + backgroundY + 50f, 0f, 0f,
                        //0f, 0f, 0f, backgroundX, backgroundY, 1));
                        //numFX++;
                        //specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\flyingNimbus"));
                    }
                }
                else { this.levitation = false; }


                if (keyboardState.IsKeyDown(Keys.E) && !this.shield && this.energy >= 20f)
                {
                    this.shield = true;
                    this.decrementEnergy(20f);
                    spawnShieldRDash = true;
                    //specialFX.Add(new shield_RDash(Duke.position.X + backgroundX + 10f, Duke.position.Y + backgroundY + 10f, 0f, 0f,
                    //0f, 0f, 0f, backgroundX, backgroundY, 3));
                    //numFX++;
                    //specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\shieldRDash"));
                    //shieldActivation.Play();

                }

                //update duke speed
                /*==============================================
            * updateDukeSpeed(GameTime gameTime)
            * Purpose: Keeps track of duke's speed progression.
            * To be used in the Update() method.
            * Class variables used: maxSpeed, Duke.velocity 
            * gameTime
            *============================================== */
                float seconds = progressionTimer / 1000;
                //this.velocity.X = maxSpeed; THIS SHOULD ALL BE CONTROLLED BY THE GAME, NOT THE CHARACTER CLASS. otherwise, each level MUST adhere to standard construct. 
                //if (seconds > 30f)
                //{
                //    this.velocity.X = maxSpeed * (float)1.2;
                //    allRangeMode = true; //the trigger to transition between stages
                //}
                //else if (seconds > 60f)
                //{
                //    this.velocity.X = maxSpeed * (float)1.4;
                //}
                //else if (seconds > 90f)
                //{
                //    this.velocity.X = maxSpeed * (float)1.6;
                //}
                //if (seconds > 100f)
                //{
                //    allRangeMode = true; //the trigger to transition between stages. 
                //}
                this.position.X += this.velocity.X; 

            }
            /*/jump loop
             if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
             {
                 if (!currentlyJumping)
                 {
                     jump();
                 }

             }
             */

            if (keyboardState.IsKeyDown(Keys.Q) && !this.airborne)
            {
                this.velocity.Y = -20f;
                this.airborne = true;
                this.levitation = false;
            }


            //if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
            //{
            //    Morph();
            //}



            //================================================

        

            spriteTimer += deltaTime;
            if (spriteTimer > spriteInterval)
            {
                currentFrame++;
                if (!alive)
                {
                    currentFrame = 0;
                    //put deathcode here.
                }
                if (currentFrame > numSprites - 1)
                {
                    currentFrame = 0;
                }
                spriteTimer = 0f;
            }
            this.sourceRect = new Rectangle(currentFrame * this.spriteWidth, 
                0, this.spriteWidth, this.mainSpriteSheet[this.pose].Height);
            //can implement jumptimer if double jump is preferable.
            //if (!Duke.airborne) { jumpTimer = 0; }//resets jump timer if you stop falling.
            //else
            //{
            //    jumpTimer += deltaTime;
            //}
            if (energy > energyCapacity) { energy = energyCapacity;  }
            if (health > healthCapacity) { health = healthCapacity;  }
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

        public void increaseEnergy(float increment)
        {
            this.energy += increment; 
        }

        public void decrementEnergy(float decrement)
        {
            this.energy -= decrement; 
        }

        public void decrementHealth(float decrement)
        {
            this.health -= decrement; 
        }

        public void setAllRangeBounds(float maxX, float maxY)
        {
            allRangeX = maxX;
            allRangeY = maxY;

        }
    }
}
