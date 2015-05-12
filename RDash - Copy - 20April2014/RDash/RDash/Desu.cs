//DESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESUdesuDESU
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

using System.ComponentModel;
using System.Text;
using System.IO;

namespace RDash
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Desu : Microsoft.Xna.Framework.Game
    {

        //List<int> listint = new List<int>();
        GraphicsDeviceManager graphics;
        Texture2D backgroundTexture, statusTexture, deathTexture, HUDTexture, HUDHitTexture,
            flash, titleScreen;//deathTexture will be replaced by the sprite array.
        //Enemy[] enemies;//to be renamed after naming scheme is established. 
        List<Enemy> enemies = new List<Enemy>();
        //TODO, DETERMINE  A WAY TO DYNAMICALY DEALLOCATE MEMORY FROM THE FOLLOWIN ARRAYS AFTER THE STAGE IS COMPLETE.
        Stage1EnemyA[] S1Aenemies;
        Rectangle viewportRect;
        SpriteBatch spriteBatch;
        List<Player> players = new List<Player>();
        Character Duke;
        const int maxCannonBalls = 50;
        const int maxEnemyCannonBalls = 200;
        const int maxPlatforms = 50;
        GameObject[] potions;//enemyCannonBalls to be replaced by array in the enemyclass.
        Bullet[] cannonBalls;
        EnemyBullet[] enemyCannonBalls;
        //PlatformStatic[] platforms;
        //PlatformDynamic[] platformsDynamic;
        List<Platform> platforms = new List<Platform>();
        //List<PlatformDynamic> platformsDynamic = new List<PlatformDynamic>();
        GamePadState previousGamePadState = GamePad.GetState(PlayerIndex.One);
        KeyboardState previousKeyboardState = Keyboard.GetState();
        const int maxEnemies = 5;
        const int maxPotions = 10;
        //const float maxEnemyHeight = 0.3f;
        //const float minEnemyHeight = .5f;
        //const float maxEnemyVelocity = 5.0f;
        //const float minEnemyVelocity = 1.0f;
        const float maxPlatformHeight = 0.5f;
        const float minPlatformHeight = 0.7f;
        const int spriteWidth = 64;
        const int enemyClasses = 2;
        const float maxSpeed = 6f;
        const float gravityAccel = 1f;
        bool GPFlag1, GPFlag2, GPFlag3, GPFlag4, GPFlag5, GPFlag6, GPFlag7, GPFlag8;
        bool bossDefeated = false;
        bool lockPan = false;
        bool morph = false;
        bool dead = false;
        bool hit = false;
        bool fire = false;
        bool inTitleScreen = true;
        bool platforming = false; //used to determine if player is on active platform. 
        int fireCounter = 0;
        int hitCounter = 0;
        int deathFrame = 0;
        int level = 1;
        int listIndex = 0;
        Random random = new Random();
        //GameObject[] enemies;
        int score;
        int life = 100;
        SpriteFont font;
        Vector2 scoreDrawPoint = new Vector2(0.1f, 0.1f);
        float bulletTimer = 0f;
        //float deltaTime = 0f;
        float bulletInterval = 3200f / 25f;
        bool bulletReady = true;
        float timer = 0f;
        //float spriteInterval = 3000f / 25f;
        float jumpTimer = 0f;
        float jumpInterval = 3200f / 6f;
        bool jumpReady = true;
        //bool currentlyJumping = false;
        int currentFrame = 0;
        int frameCount = 5;
        float backgroundX = 0f;
        float backgroundY = 2000f;
        bool loadLevel = true;
        int lives = 3;
        bool gameOver = false;
        int numPlatforms;//to be altered at every new level.
        //bool currentlyJumping = false;
        int maxKeys = 10;
        //audio files and necessary helper data types.
        Song MMTheme, Monochrome;
        bool songstart = false;
        public Desu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            MMTheme = Content.Load<Song>("Audio\\MetalMM");
            Monochrome = Content.Load<Song>("Audio\\Monochrome");
            HUDHitTexture =
                   Content.Load<Texture2D>("Sprites\\HUD");
            HUDTexture =
                   Content.Load<Texture2D>("Sprites\\HUDHit");
            flash =
                    Content.Load<Texture2D>("Sprites\\flash");
            deathTexture =
                    Content.Load<Texture2D>("Sprites\\death");
            backgroundTexture =
                   Content.Load<Texture2D>("Sprites\\Background");
            statusTexture =
                   Content.Load<Texture2D>("Sprites\\Statusbanner");
            titleScreen =
                   Content.Load<Texture2D>("Sprites\\titleScreenBackground");
            //cannonTexture = Content.Load<Texture2D>("Sprites\\Duke");
            potions = new GameObject[maxPotions];//TODO: replace with item class.
            Duke = new Character(5, 200f, 100f, 0f, 0f, 200f, 200f, 0f, 0f, 64);
            //Duke.position = new Vector2(120, graphics.GraphicsDevice.Viewport.Height - 280);
            cannonBalls = new Bullet[maxCannonBalls];//TODO, replace with bullets class.
            //keys = new GameObject[maxKeys]; 
            enemyCannonBalls = new EnemyBullet[maxEnemyCannonBalls];
            
            // loading duke sprites.
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeRight"), 0);
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeLeft"), 1);
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeUp"), 2);
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeJumpRight"), 3);
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeJumpLeft"), 4);
            //

                        //cannon.morphSprite = Content.Load<Texture2D>("Sprites\\morphRunner");
                        //cannon.jumper = Content.Load<Texture2D>("Sprites\\jumper");
                        //cannon.right = true;
            //HUDRect = new Rectangle((int)scoreDrawPoint.X * viewportRect.Width + 300,
            //   (int)(scoreDrawPoint.Y * viewportRect.Height + 15), 90, 90);
            
            ////platforms = new PlatformStatic[maxPlatforms];
            ////platformsDynamic = new PlatformDynamic[maxPlatforms];
            //S1Aenemies = new Stage1EnemyA[5];
            for (int i = 0; i < maxCannonBalls; i++)
            {
                cannonBalls[i] = new Bullet(Content.Load<Texture2D>(
                    "Sprites\\cannonball"));
            }
            for (int i = 0; i < maxEnemyCannonBalls; i++)
            {
                enemyCannonBalls[i] = new EnemyBullet(Content.Load<Texture2D>(
                    "Sprites\\enemyCannonBall"));
            }
            //initiates the platform array. 
            ////for (int i = 0; i < maxPlatforms; i++)
            ////{
            ////    platforms[i] = new PlatformStatic(0f, 0f, 320f, (Content.Load<Texture2D>(
            ////        "Sprites\\Platform")));
            ////    platformsDynamic[i] = new PlatformDynamic(0f, 320f, 320, (Content.Load<Texture2D>(
            ////        "Sprites\\Platform")));
            ////    platforms[i].visible = false;
            ////    platformsDynamic[i].visible = false;
            ////    //if (i == 0)//sets the first "island" platform. for debug purposes.
            ////    //{
            ////    //    platforms[i].visible = true;
            ////    //    platforms[i].position = Duke.position;
            ////    //}
            ////}
                        //for (int i = 0; i < maxKeys; i++)
                        //{
                        //    keys[i] = new GameObject(Content.Load<Texture2D>(
                        //        "Sprites\\key"));
                        //    keys[i].visible = false;
                        //}
                        //for (int i = 0; i < maxPotions; i++)
                        //{
                        //    potions[i] = new GameObject(Content.Load<Texture2D>(
                        //        "Sprites\\redCoin"));
                        //    potions[i].visible = false;
                        //    potions[i].alive = false;
                        //}
            //enemySprites = new Texture2D[enemyClasses];
            //for (int i = 0; i < enemyClasses; i++)//loads all possible enemy class images.
            //{
            //    temp = i.ToString(); //converts i to a string to aid in automated image loading.
            //    enemySprites[i] = Content.Load<Texture2D>("Sprites\\enemy" + temp);//naming convention: enemy0
            //}
            //enemies = new GameObject[maxEnemies];
            //for (int i = 0; i < maxEnemies; i++)
            //{
            //    enemies[i] = new GameObject(
            //        Content.Load<Texture2D>("Sprites\\enemy0"));
            //}

            //font = Content.Load<SpriteFont>("Fonts\\GameFont");
            //drawable area of the game screen.
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (loadLevel)
            {
                switch (level)
                {
                    case 1:
                        loadLevelOne();
                        break;
                    default:
                        //do nothing 
                        break;
                }

                //load the start of the current level design.
                loadLevel = false;//alternate this value when level completion logic is reached.

            }
            else
            {

                KeyboardState keyboardState = Keyboard.GetState();
                if (life <= 0 || Duke.position.Y + backgroundY > 4550)
                {
                    Duke.alive = false;
                }
                Timing(gameTime);
                //Updates sprite=================
                
                if (Duke.alive)
                {
                    Duke.velocity.X = 0f; 
                    //================================
                   
                    /* used since next loop is buggy.
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        Duke.rotation = 3.9f;
                        if (Duke.position.X > 200)
                        {
                            cannon.position.X -= maxSpeed;
                        }
                        else if (cannon.position.X <= 200 && backgroundX > maxSpeed)
                        {
                            this.backgroundX -= maxSpeed;
                        }
                        Duke.right = false;
                    }*/
                    //This condition NEVER ENTERS DEBUG
                    //TODO: apply the same panning rules and mechanics to the platforming mechanics which may alter Duke's movements. 
                    if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Left))
                    {
                        Duke.rotation = 3.9f;
                        Duke.velocity.X -= maxSpeed;//moves left at the per/second rate.
                        /*if (Duke.position.X > 200 && !lockPan)
                        {
                            Duke.position.X -= maxSpeed;//moves left at the per/second rate.
                        }

                        
                        else if (Duke.position.X <= 200 && backgroundX > maxSpeed && !lockPan)
                        {
                            this.backgroundX -= maxSpeed;
                        }
                        else if (Duke.position.X > maxSpeed && lockPan)
                        {
                            Duke.position.X -= maxSpeed;
                        }
                        Duke.pose = 1;
                        Duke.velocity.X = -1;//indicator of left movement, status purposes only.
                         */
                    }
                    if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Right))
                    {
                        //if (previousKeyboardState.IsKeyDown(Keys.Right)) unnecessary acceleration mechanic :'(
                        //{
                        //    if (cannon.accelMax > cannon.acceleration.X && cannon.accelCount == 6
                        //        && (!cannon.currentlyJumping && !cannon.falling))
                        //    {
                        //        cannon.acceleration.X += 1f;
                        //        cannon.accelCount = 0;
                        //    }
                        //    cannon.accelCount++;

                        //}
                        //else
                        //{
                        //    cannon.acceleration.X = 0f;
                        //    cannon.accelCount = 0;
                        //}

                        Duke.rotation = 5.45f;
                        Duke.velocity.X += maxSpeed;
                        /*
                        if (Duke.position.X < 600 && !lockPan)
                        {
                            Duke.position.X += maxSpeed;
                        }
                        else if (Duke.position.X >= 600 && !lockPan)
                        {
                            this.backgroundX += maxSpeed;
                        }
                        else if (viewportRect.Width - Duke.position.X - Duke.spriteWidth > maxSpeed && lockPan)
                        {
                            Duke.position.X += maxSpeed;
                        }//TOVERIFY: determine if viewport.Rect.Width is infact the pixel width of the screen. 

                        Duke.pose = 0;//pose index for the right.
                        Duke.velocity.X = 1;//indicator of right movement, status purposes only.
                         */
                    }
                    if (keyboardState.IsKeyDown(Keys.Down))
                    {
                        Duke.rotation = 1.57f;
                    }

                    if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Up))
                    {

                        Duke.rotation = 3.9f;
                        Duke.velocity.X -= maxSpeed;
                        /*
                        if (Duke.position.X > 200 && !lockPan)
                        {
                            Duke.position.X -= maxSpeed;//moves left at the per/second rate.
                        }
                        else if (Duke.position.X <= 200 && backgroundX > maxSpeed && !lockPan)
                        {
                            this.backgroundX -= maxSpeed;
                        }
                        else if (Duke.position.X > maxSpeed && lockPan)
                        {
                            Duke.position.X -= maxSpeed;
                        }
                        Duke.pose = 1;
                        Duke.velocity.X = -1;//indicator of left movement, status purposes only.
                         */
                    }

                    if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Up))
                    {
                        Duke.rotation = 5.45f;
                        Duke.velocity.X += maxSpeed;
                        /*if (Duke.position.X < 600 && !lockPan)
                        {
                            Duke.position.X += maxSpeed;
                        }
                        else if (Duke.position.X >= 600 && !lockPan)
                        {
                            this.backgroundX += maxSpeed;
                        }
                        else if (viewportRect.Width - Duke.position.X - Duke.spriteWidth> maxSpeed && lockPan)
                        {
                            Duke.position.X += maxSpeed;
                        }//TOVERIFY: determine if viewport.Rect.Width is infact the pixel width of the screen. 

                        Duke.pose = 0;//pose index for the right.
                        Duke.velocity.X = 1;//indicator of right movement, status purposes only.
                        */
                    }
                    
                    else if (keyboardState.IsKeyDown(Keys.Up))
                    {
                        Duke.rotation = 4.71f;//4.7f;
                    }

                    else
                    {
                        if (Duke.pose == 0) { Duke.rotation = 0f; }
                        else { Duke.rotation = (float)Math.PI; }
                    }

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        FireCannonBall();
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
                    //if (keyboardState.IsKeyDown(Keys.Space))
                    //{
                    //    FireCannonBall();
                    //}

                    if (keyboardState.IsKeyDown(Keys.A) && !Duke.airborne)
                    {
                        Duke.velocity.Y = -20f;
                        Duke.airborne = true;

                    }

                    
                    //if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                    //{
                    //    Morph();
                    //}
                }

                Duke.rotation = MathHelper.Clamp(Duke.rotation, -MathHelper.PiOver2, 0);
                //UpdatePotions();//great for placeing items, refer to for a template.
                //END USER INPUT ==============================================================================
                
                switch (level)
                {
                    case 1:
                        loadLevelOneConditionals();
                        break;
                    default:
                        //do nothing 
                        break;
                }

                //UpdateEnemies();//these methods are for random generation of enemies, good test bed code.
                //UpdatePlatforms();//ditto
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.alive)
                    {
                        enemy.hit = false;
                    }
                }
                //foreach (Stage1EnemyA enemy in S1Aenemies)
                //{
                //    if (enemy.alive)
                //    {
                //        enemy.hit = false;
                //    }
                //}
                UpdatePlatforming(); //deals with platforming mechanic, to be developed.
                UpdatePanning();
                UpdateCannonBalls(); 
                // TODO: Add your update logic here
                //TODO: these lines should be in the duke.update method if such a method exists. 
                //used to configure landing pose. TODO: PUT IN UPDATE DUKE METHOD.
                if (!Duke.airborne && Duke.velocity.X > 0 && !platforming)
                {
                    Duke.pose = 0;
                }
                else if (!Duke.airborne && Duke.velocity.X < 0 && !platforming)
                {
                    Duke.pose = 1;
                }
                if (Duke.airborne && Duke.velocity.X > 0)//TODO: redefine duke's velocity and adjust this to read <0 || > 0
                {
                    Duke.pose = 3;
                }
                else if (Duke.airborne && Duke.velocity.X < 0)//note velocity.X == 0 should NOT be a trigger for pose change, as Duke may face left or right when he stops moving. 
                {
                    Duke.pose = 4;
                }
                

            }
            base.Update(gameTime);
        }
        
        /*==============================================
         * UpdatePanning()
         * Purpose: Keeps track of Screenpanning with respect to
         * character movement, platforming, and arena modes. 
         * Class variables used: 
         *
         *============================================== */

        public void UpdatePanning()
        { //first X panning is analyzed/established, then Y panning. TODO: annotate right and left in each if statement with && vel.x>/<0
          //ALMOST DONE: need to fix arena mode and left map boundary detection, as well as reiterate for the y axis. 
            
            this.Duke.position.X += Duke.velocity.X;
            Vector2 tempVect = Duke.position;//cheap bandage
                if (Duke.position.X > 200 && !lockPan && Duke.velocity.X < 0)//allows duke to move freely left. 
                {
                    //Duke.position.X -= maxSpeed;//moves left at the per/second rate.
                }                   
                if (Duke.position.X <= 200 && backgroundX > Math.Abs(Duke.velocity.X) && !lockPan
                    && Duke.velocity.X < 0)//allows duke to ride up against the left map boundary. DOES NOT prevent. 
                {
                    this.backgroundX += Duke.velocity.X; //remember, velocity can be a negative value. 
                    this.Duke.position.X -= Duke.velocity.X; //undos dukes position vector update so the panning gets updated instead.
                }
                else if (!lockPan && backgroundX <= Duke.spriteWidth && Duke.velocity.X < 0 && Duke.position.X <= Duke.spriteWidth/6)//if passes, the leftrunning Duke will not run off the SCREEN. uses else if because previous if-statement. 
                {
                    
                        this.backgroundX = 0f;
                        Duke.position.X = 0f;
                   
                }
                 
                if (lockPan && Duke.velocity.X < 0 && Duke.position.X <= Duke.spriteWidth / 6)//if passes, the leftrunning Duke will not run off the SCREEN. uses else if because previous if-statement. 
                {

                    //this.backgroundX = 0f;
                    Duke.position.X -= Duke.velocity.X;//undo any velocity vector affecting duke 

                }
                if (lockPan && Duke.velocity.X > 0 && Duke.position.X > viewportRect.Width - Duke.spriteWidth)
                {

                    //this.backgroundX = 0f;
                    Duke.position.X -= Duke.velocity.X;//undo any velocity vector affecting duke 

                }
                 

            //
            //    this.Duke.position.X -= Duke.velocity.X; //undos dukes position vector update so the panning gets updated instead.
            //        
                if (Duke.position.X < 600 && !lockPan && Duke.velocity.X > 0) //allows duke to move freely right. 
                {
                    this.Duke.position.X += 0;
                }
                if (Duke.position.X >= 600 && !lockPan && Duke.velocity.X > 0) // allows right panning. 
                {
                    this.backgroundX += Duke.velocity.X; //remember, velocity can be a negative value. 
                    this.Duke.position.X -= Duke.velocity.X;// undos duke's position vector update at start of method. 
                }
                //if (viewportRect.Width - (Duke.position.X - Duke.velocity.X) - Duke.spriteWidth > Duke.velocity.X && lockPan && 
                //    Duke.velocity.X > 0)
                //{
                //    //Duke.position.X += maxSpeed; do nothing since he was updated at the start of the method. 
                //}
                
            //comeback to this. 
                //if ((Duke.position.X -= Duke.velocity.X) > Duke.velocity.X && lockPan && Duke.velocity.X < 0)//determines if left movement is allowed for lockPan arena mode. 
                //{
                //    //Duke.position.X = maxSpeed; do nothing, update. 
                //}

                this.Duke.position.X += 0;
            //upon exiting this method, position does not update. 
        }
        /*==============================================
         * UpdatePlatforming()
         * Purpose: Keeps track of falling for platforming dynamics.
         * 
         * Class variables used: bulletTimer, deltaTime, bulletInterval, 
         * bulletReady, timer, spriteInterval, currentFrame, frameCount
         *============================================== */

        public void UpdatePlatforming()
        {
            platforming = false;
            if (Duke.airborne)//TODO: IMPLEMENT GRAVITY.
            {
                //apply physics later, use linear decelleration for now.
                if (Duke.velocity.Y < 10f)
                {
                    Duke.velocity.Y += gravityAccel;
                }
                
                //Duke.position.Y += Duke.velocity.Y;
                
                if (Duke.position.Y > 100 && Duke.velocity.Y <0)
                {
                    Duke.position.Y += Duke.velocity.Y;
                }
                else if (Duke.position.Y <= 100 && backgroundY > 0f && Duke.velocity.Y < 0f)
                {
                    this.backgroundY += Duke.velocity.Y;
                }
                else if (Duke.position.Y + Duke.velocity.Y > 350 && Duke.velocity.Y > 0f)
                {
                    this.backgroundY += Duke.velocity.Y;
                }
                else { Duke.position.Y += Duke.velocity.Y; }

                //Duke.position.Y += Duke.velocity.Y;

                //c
            }
            Duke.airborne = true;
            foreach (Enemy enemy in enemies)
            {
                enemy.airborne = true;
            }
            //foreach (Stage1EnemyA enemy in S1Aenemies)//TO DO, ABSTRACT ALL ENEMIES INTO THE ENEMY CLASS...NOT EFFICIENT BUT BETTER THAN THIS.
            //{
            //    enemy.airborne = true;
            //}
           // update the platforms before the platforming.
            //========================================== todo, handle dynamic infrastructurs.
            foreach (Platform platform in platforms)
            {
                platform.Update();
            }

            //===========================================
            //Duke.falling = true;//might be unnecessary.
            //jumpReady = false;
            //debug This never enters after I push the A button.
            //TODO: DEBUG why player is stuck with half of body in platform.
            
            foreach (Platform platform in platforms)
            {
                
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.position.Y <= (platform.position.Y) &&
                    enemy.position.Y > (platform.position.Y) - (float)(enemy.spriteHeight - 46) &&
                    enemy.position.X >= (platform.position.X - 25f) &&
                    (float)enemy.position.X < platform.Length + (platform.position.X
                     - 25f ) && platform.visible && enemy.velocity.Y > 0)
                    {
                        enemy.position.Y = platform.position.Y - (float)(enemy.spriteHeight - 44);//16 is for 64x64 sprite. this must use the sprite height. 
                        enemy.airborne = false;
                        enemy.velocity.Y = 1f;
                    }


                }
                
                
                if (Duke.position.Y <= (platform.position.Y - backgroundY) &&
                    Duke.position.Y > (platform.position.Y - backgroundY) - 22f &&
                    Duke.position.X >= (platform.position.X - backgroundX - 25f) &&
                    (float)Duke.position.X < platform.Length + (platform.position.X
                    - backgroundX - 25f) && platform.visible && Duke.velocity.Y > 0)
                {
                    //do nothing, do no update the Y component, unless the platform is moving vertically.
                    
                    //jumpReady = true; //use the condition of dukes v.y component == 0.
                    //next line "auto-snaps" player to a platform.
                    Duke.position.Y = platform.position.Y  - backgroundY -16;//16 is for 64x64 sprite.
                    Duke.airborne = false;
                    if (platform.velocity.X != 0f)
                    {
                        platforming = true;//identifies player is on dynamic platform. 
                    }
                   //update his pose before you update the vector. still have to determine what to do on the zero case. 
                    if (Duke.velocity.X > 0) { Duke.pose = 0; }
                    else if (Duke.velocity.X < 0) { Duke.pose = 1; }
                    Duke.velocity.Y = 1f;
                    Duke.velocity.X += platform.velocity.X;  //use position because you should not be fighting platform speed when running. 
                    //if (platform.vertical && platform.up)//logic for glueing character to moving platform. ^^
                    //{
                    //    cannon.position.Y -= 2f;
                    //}
                    //else if (platform.vertical)
                    //{
                    //    cannon.position.Y += 2f;
                    //}

                   

                    
                    if ((platform.position.Y - backgroundY) <= 100 && backgroundY > 0f && platform.velocity.Y < 0f) //if on platform and moving out of sight, updates panning
                    {
                        this.backgroundY += platform.velocity.Y;
                        this.backgroundX += platform.velocity.X;
                        Duke.velocity.X -= platform.velocity.X;
                    }
                    else if ((platform.position.Y - backgroundY) + platform.velocity.Y > 350 && platform.velocity.Y > 0f)//see above comment.
                    {
                        this.backgroundY += platform.velocity.Y;
                        this.backgroundX += platform.velocity.X;
                        Duke.velocity.X -= platform.velocity.X;
                    }
                    

                }
            }

            ////foreach (PlatformDynamic platform in platforms)
            ////{
            ////    foreach (Enemy enemy in enemies)
            ////    {
            ////        if (enemy.position.Y <= (platform.position.Y) &&
            ////        enemy.position.Y > (platform.position.Y ) - 18 - platform.Scale &&
            ////        enemy.position.X >= (platform.position.X - 25f - platform.Speed) &&
            ////        (float)enemy.position.X < platform.Length + (platform.position.X
            ////         - 40f - (platform.dx * platform.Speed)) && platform.visible && enemy.velocity.Y > 0)
            ////        {
            ////            enemy.position.Y += (platform.dy * platform.Speed);
            ////            enemy.airborne = false;
            ////            enemy.position.X += (platform.dx * platform.Speed);
            ////        }


            ////    }
                
            ////    if (Duke.position.Y <= (platform.position.Y - backgroundY) &&
            ////        Duke.position.Y > (platform.position.Y - backgroundY) - 18 - platform.Scale &&
            ////        Duke.position.X >= (platform.position.X - backgroundX - 25f - platform.Speed) &&
            ////        (float)Duke.position.X < platform.Length + (platform.position.X
            ////        - backgroundX - 40f - (platform.dx * platform.Speed)) && platform.visible && Duke.velocity.Y > 0)
            ////    {
            ////        //do nothing, do no update the Y component, unless the platform is moving vertically.

            ////        //jumpReady = true; //use the condition of dukes v.y component == 0.
            ////        //next line "auto-snaps" player to a platform.
            ////        //DESUDESU
                    
            ////        //=================TODO, ALSO AIRBORNE DYNAMICS


            ////        //TO DO, CREATE MORE ELSE IFS FOR WHEN DY IS IN NEG RANGES
            ////        if (Duke.position.Y + (platform.dy * platform.Speed) > 100 && !platform.YPositive)
            ////        {
            ////            Duke.position.Y += (platform.dy * platform.Speed);
            ////        }
            ////        else if (Duke.position.Y + (platform.dy * platform.Speed) <= 100 && backgroundY > 0f && !platform.YPositive)
            ////        {
            ////            this.backgroundY += (platform.dy * platform.Speed);
            ////        }
            ////        else if (Duke.position.Y + (platform.dy * platform.Speed) > 450)
            ////        {
            ////            this.backgroundY += (platform.dy * platform.Speed);
            ////        }
            ////        else { Duke.position.Y = platform.position.Y - backgroundY - 16; }
            ////        Duke.position.X += (platform.dx * platform.Speed); //- xxf causes a reverse sliding effect...
            ////        //if (platform.xdynamic)
            ////        //{
            ////        //    if (platform.xpositive)
            ////        //    {
            ////        //        duke.position.x += platform.speed;
            ////        //    }
            ////        //    else
            ////        //    {
            ////        //        duke.position.x -= platform.speed;
            ////        //    }
            ////        //}
            ////        Duke.airborne = false;
            ////        Duke.velocity.Y = 1f;
            ////        //if (platform.vertical && platform.up)//logic for glueing character to moving platform. ^^
            ////        //{
            ////        //    cannon.position.Y -= 2f;
            ////        //}
            ////        //else if (platform.vertical)
            ////        //{
            ////        //    cannon.position.Y += 2f;
            ////        //}
            ////    }
            ////}

            //if (duke.position.y < 500)
            //{
            //    duke.position.y += 8f;
            //}
            //else if (duke.position.y >= 500 && backgroundy < 4100f)
            //{
            //    this.backgroundy += 8f;
            //}
            //if (duke.position.y > 200)
            //{
            //    duke.position.y -= 5f;
            //}
            //else if (duke.position.y <= 200 && backgroundy > 0f)
            //{
            //    this.backgroundy -= 5f;
            //}
            
        }

        /*==============================================
        * Timing()
        * Purpose: Keeps track of timing for sprite animation,
        * bullet timing, and other time-sensitive dynamics.
        * To be used in the Update() method.
        * Class variables used: bulletTimer, deltaTime, bulletInterval, 
        * bulletReady, timer, spriteInterval, currentFrame, frameCount
        *============================================== */

        public void Timing(GameTime gameTime)
        {
            //i fucked up animations
            //currentworkaround, call each type of derived class individually and determine how to fix this later. 
            foreach (Alien enemy in enemies)
            {
                enemy.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
                if (enemy.bulletReady)
                {
                    foreach(EnemyBullet ball in enemyCannonBalls){
                        if (!ball.alive)
                        {
                            enemy.setBulletReady(false); 
                            ball.alive = true;
                            EnemyBullet tempBullet = new EnemyBullet();
                            tempBullet = enemy.fireBullet(Duke.position, Duke.velocity);
                            ball.position = tempBullet.position;
                            ball.velocity = tempBullet.velocity;
                            ball.alive = true;
                            break; //use brerak for multiple enemies, vs the return for the main character. 
                        }
                    }
                }
            }
            //.//copypaste this code again for each enemy type...i just want to see basic functionality. 
            //following segment updates the bulletReady boolean value.
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            bulletTimer += deltaTime;
            if (bulletTimer > bulletInterval)
            {
                bulletTimer = 0f;
                bulletReady = true; //bulletReady boolean will be used for bulletspacing.
            }
            timer += deltaTime;
            ////Duke.update(deltaTime);
            //following segment updates the frame for sprites.//spriteInterval MUST be class independent
            
            //following handles jumping
            //if statement handles duration of overall jumptime
            //if (jumpTimer > jumpInterval)
            //{
            //    jumpTimer = 0f;
            //    cannon.currentlyJumping = false; //bulletReady boolean will be used for bulletspacing.
            //}
            //if (cannon.currentlyJumping)
            //{
            //    if (cannon.position.Y > 200)
            //    {
            //        cannon.position.Y -= 5f;
            //    }
            //    else if (cannon.position.Y <= 200 && backgroundY > 0f)
            //    {
            //        this.backgroundY -= 5f;
            //    }
            //    //cannon.position.Y -= 5f;
            //}

            //following will keep track of enemy firing patterns.
            //foreach (GameObject enemy in enemies)
            //{
            //    enemy.fireTimer -= deltaTime; //reduces time left to fire...reverse convention of other timing
            //    //which count up, not down.
            //    if (enemy.fireTimer <= 0)
            //    {
            //        enemy.fireTimer = 1000f;//reset the fireTimer to true.
            //        //consider rng here.DESU
            //        enemy.fire = true;
            //    }
            //}

        }


        //====================================
        /// <summary>
        /// 
        /// </summary>
        
        public void FireCannonBall()
        {
            foreach (Bullet ball in cannonBalls)
            {
                if (!ball.alive && bulletReady)
                {
                    bulletReady = false;
                    bulletTimer = 0f;
                    ball.alive = true;
                    fire = true;
                    fireCounter = 3;
                    ball.position = Duke.position;
                    ball.position.X += spriteWidth/2;
                    ball.position.Y += Duke.mainSpriteSheet[0].Height/4;
                    ball.velocity = new Vector2(
                        (float)Math.Cos(Duke.rotation),
                        (float)Math.Sin(Duke.rotation)) * 10.0f;
                    return;
                }
            }
        }
        //=====================
        /// <summary>
        /// 
        /// 
        /// </summary>

        public void UpdateCannonBalls()
        {
            foreach (Bullet ball in cannonBalls)
            {
                if (ball.alive)
                {
                    ball.position += ball.velocity;
                    if (!viewportRect.Contains(new Point(
                        (int)ball.position.X,
                        (int)ball.position.Y)))
                    {
                        ball.alive = false;
                        continue;
                    }
                    Rectangle cannonBallRect = new Rectangle(
                        (int)ball.position.X,
                        (int)ball.position.Y,
                        ball.sprite.Width,
                        ball.sprite.Height);
                    //ALL FOR EACH LOOPS MUST REFERENCE A LISTINDEX VAR INORDER TO REMOVE ITEMS FROM THE LIST. 
                    listIndex = 0;
                    foreach (Enemy enemy in enemies)
                    {
                        
                        Rectangle enemyRect = new Rectangle(
                            (int)enemy.position.X - (int) backgroundX,
                            (int)enemy.position.Y - (int) backgroundY,
                            enemy.spriteWidth,
                            enemy.mainSpriteSheet[enemy.pose].Height);

                        if (cannonBallRect.Intersects(enemyRect))
                        {
                            ball.alive = false;
                            ////enemy.alive = false; =============not so fast, still want to see sprites for now.
                            score += 1;
                            enemy.health--;
                            enemy.hit = true;
                            
                            if (enemy.boss && enemy.health < 0)
                            {
                                bossDefeated = true;
                            }
                            //THIS IS THE FINAL CASE STATEMENT. THE ITEM IS REMOVED FROM THE LIST AFTERIT ENTERS THIS STATEMENT. 
                            if (enemy.health < 0)
                            {
                                enemies.RemoveAt(listIndex);
                            }
                            //bring item drops alive here.
                            //if (randomNumber(0, 20) == 5)
                            //{
                            //    for (int i = 0; i < maxPotions; i++)
                            //    {
                            //        if (!potions[i].alive)
                            //        {

                            //            potions[i].alive = true;
                            //            potions[i].position = new Vector2(enemy.position.X + backgroundX, enemy.position.Y + backgroundY);
                            //            potions[i].ttl = 25;
                            //            potions[i].sourceRect = new Rectangle(0, 0, spriteWidth, potions[i].sprite.Height);
                            //            potions[i].enemyClass = 1;//1 will be the potion.
                            //            i = 10;
                            //        }
                            //    }
                            //}//this implies only one ball can hit an enemy at a time.....could  be pa bug in future.
                            break;
                        }
                        listIndex++;
                        
                    }
                }
            }
            foreach (EnemyBullet ball in enemyCannonBalls)
            {

                
                if (ball.alive)
                {
                    ball.position += ball.velocity;
                    if (!viewportRect.Contains(new Point(
                        (int)ball.position.X,
                        (int)ball.position.Y)))
                    {
                        ball.alive = false;
                        continue;
                    }
                    Rectangle cannonBallRect = new Rectangle(
                        (int)ball.position.X,
                        (int)ball.position.Y,
                        ball.sprite.Width,
                        ball.sprite.Height);

                    Rectangle cannonRect = new Rectangle(
                        (int)Duke.position.X,
                        (int)Duke.position.Y,
                        Duke.spriteWidth,
                        Duke.mainSpriteSheet[0].Height);
/*
                    if (cannonBallRect.Intersects(cannonRect))
                    {
                        ball.alive = false;
                        Duke.alive = false;
                        hit = true;
                        hitCounter = 0;
                        //inserte health decrement here. -= 1;
                        break;
                    }
 * */
                }
            }
        }


        /*==============================================
         * loadLevelOne() -- each level will have its own method
         * Purpose: Loads a singular level design to include
         * enemy placement, platform placement, player start, and starts level music.
         * To be used in the Update() method.
         * Class variables used: platform and enemy arrays
         *============================================== */

        public void loadLevelOne()
        {

            GPFlag1 = false;
            GPFlag2 = false;
            GPFlag3 = false;
            GPFlag4 = false;
            GPFlag5 = false;
            GPFlag6 = false;
            GPFlag7 = false;
            GPFlag8 = false;


            //================================
            /*
             * Enemy Placement
             * 
             * follow design document, if provided.
             * 
             * 
             * */
            //===============================
            //List<int> listint = new List<int>();
            //TODO: replace hardcode with platform references

            //enemies.Add(new Ninja(5, 360f, 1900f, 0f, 0f, 360f, 1900f, 0f, 1f, 64, 64));
            //enemies.Add( new Ninja(5, 360f*2, 1900f, 0f, 0f, 360f*2, 1900f, 0f, 1f, 64, 64));
            //enemies.Add(new Ninja(5, 360f*3, 1900f, 0f, 0f, 360f*3, 1900f, 0f, 1f, 64, 64));
            //enemies.Add(new Ninja(5, 360f*4, 1900f, 0f, 0f, 360f*4, 1900f, 0f, 1f, 64,64));
            //enemies.Add(new Ninja(5, 360f*5, 1900f, 0f, 0f, 360f*5, 1900f, 0f, 1f, 64, 64));
            //understand polymorphism more you stupid :D
            
            
            
            //enemies[0] = new Enemy(5, 360f, 1900f, 0f, 0f, 360f, 1900f, 0f, 1f, 64);
            //enemies[1] = new Enemy(5, 360f*2, 1900f, 0f, 0f, 360f*2, 1900f, 0f, 1f, 64);
            //enemies[2] = new Enemy(5, 360f*3, 1900f, 0f, 0f, 360f*3, 1900f, 0f, 1f, 64);
            //enemies[3] = new Enemy(5, 360f*4, 1900f, 0f, 0f, 360f*4, 1900f, 0f, 1f, 64);
            //enemies[4] = new Enemy(5, 360f*5, 1900f, 0f, 0f, 360f*5, 1900f, 0f, 1f, 64);
            foreach (Enemy enemy in enemies)//loading sprite sheets
            {
                enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 0, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
                enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 1, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
                enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 2, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
                enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 3, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
                enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 4, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            }
            //S1Aenemies = new Stage1EnemyA[5];

            //S1Aenemies[0] = new Stage1EnemyA(5, 460f, 1900f, 1.57f, 0f, 360f, 1900f, 0f, 1f, 64);
            //S1Aenemies[1] = new Stage1EnemyA(5, 460f * 2, 1900f, 1.57f, 0f, 360f * 2, 1900f, 0f, 1f, 64);
            //S1Aenemies[2] = new Stage1EnemyA(5, 460f * 3, 1900f, 1.57f, 0f, 360f * 3, 1900f, 0f, 1f, 64);
            //S1Aenemies[3] = new Stage1EnemyA(5, 460f * 4, 1900f, 1.57f, 0f, 360f * 4, 1900f, 0f, 1f, 64);
            //S1Aenemies[4] = new Stage1EnemyA(5, 460f * 5, 1900f, 1.57f, 0f, 360f * 5, 1900f, 0f, 1f, 64);
            //foreach (Stage1EnemyA enemy in S1Aenemies)//loading sprite sheets
            //{
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 0);
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 1);
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 2);
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 3);
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 4);
            //}

            

            // public Enemy(int numSprites, float initX, float initY, float initRotation, float initFireTimer,
            // float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteSize)
            
            
            //objects are already created when game is loaded, so
            // first set how many you want alive.
            numPlatforms = 15;//was 25;
            if (!songstart)//checks if song started.
            {
                MediaPlayer.Play(MMTheme);
                songstart = true;
            }
            //next set the other array elements as not alive for later logic.
            ////static platforms.TODO same for dynamic platforms. 
            ////for (int i = 0; i < maxPlatforms; i++)
            ////{
            ////    if (i < numPlatforms)
            ////    {
            ////        this.platforms[i].visible = true;//initializes the platforms
            ////        this.platformsDynamic[i].visible = true;
            ////        this.platforms[i].Skin = (Content.Load<Texture2D>(
            ////        "Sprites\\Platform"));
            ////    }
            ////    else
            ////    {
            ////        this.platforms[i].visible = false;
            ////        this.platformsDynamic[i].visible = false;
            ////    }
            ////}

            //first, concentrate on only placeing platforms...
            //currently i have 15 platforms to play with...
            //position vectors must be consitent throughout gameplay, and all operations
            //are based off an off set, which is altered by user input/player mobility.
            this.platforms.Add(new PlatformStatic(0f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f*1, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 2, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 3, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 4, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 5, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 6, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 7, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 8, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 9, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 10, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 11, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 12, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 13, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 14, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 15, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 16.5f, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 8, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 9, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 10, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 11, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            this.platforms.Add(new PlatformStatic(320f * 12, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            ////this.platforms[0].position = new Vector2(0f, 2300f);
            ////this.platforms[1].position = new Vector2(platforms[0].Length * 1, 2300f);
            ////this.platforms[2].position = new Vector2(platforms[0].Length * 2, 2300f);
            ////this.platforms[3].position = new Vector2(platforms[0].Length * 3, 2300f);
            ////this.platforms[4].position = new Vector2(platforms[0].Length * 4, 2300f);
            ////this.platforms[5].position = new Vector2(platforms[0].Length * 4, 2300f);
            ////this.platforms[6].position = new Vector2(platforms[0].Length * 5, 2300f);
            ////this.platforms[7].position = new Vector2(platforms[0].Length * 5, 2300f);
            ////this.platforms[8].position = new Vector2(platforms[0].Length * 6, 2300f);
            ////this.platforms[9].position = new Vector2(platforms[0].Length * 7, 2300f);
            ////this.platforms[10].position = new Vector2(platforms[0].Length * 8, 2300f);
            ////this.platforms[11].position = new Vector2(platforms[0].Length * 9, 2300f);
            ////this.platforms[12].position = new Vector2(platforms[0].Length * 3, 2220f);
            ////this.platforms[13].position = new Vector2(platforms[0].Length * 4, 2220f);
            ////this.platforms[14].position = new Vector2(platforms[0].Length * 5, 2220f);
            //========================================================================================
            //initial seed dx dy need to match scale%$^%$#^#%$^%$#^6563

            //to uncomment next 3 lines after configureing platform mechanics to work with panLock. 
            //this.platforms.Add(new PlatformDynamic(360f * 13, 2100f, 360f, true, true, 0f, -5f, true, true, 1, 5));
            ////this.platforms.Add(new PlatformDynamic(360f * 14, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 15, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 1, 1700f, 360f, true, true, 0f, -8f, true, true, 1, 8));
            this.platforms.Add(new PlatformDynamic(360f * 2, 1500f, 360f, true, true, 0f, 0f, true, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 3, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 4, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 5, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 6, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 7, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 15, 2300f, 360f, true, true, 0f, -2.5f, true, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 16, 2300f, 360f, true, true, -2.5f, -2.5f, true, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 17, 2400f, 360f, true, true, 0f, 0f, true, false, 1, 5));//first two bools is dynamic x/y, and second pair is x+/y+
            this.platforms.Add(new PlatformDynamic(360f * 18, 2300f, 360f, true, true, -4f, 4f, true, true, 1, 8)); //makes a CW circle
            this.platforms.Add(new PlatformDynamic(360f * 19, 2300f, 360f, true, true, 4f, -4f, true, true, 1, 8)); //makes CCW circle
            this.platforms.Add(new PlatformDynamic(360f * 20, 2300f, 360f, true, true, 0f, -1.25f, false, false, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 22, 2300f, 360f, true, true, 0f, -1.25f, true, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 9, 1800f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            this.platforms.Add(new PlatformDynamic(360f * 3, 1800f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            //this.platformsDynamic[1] = new PlatformDynamic(platforms[0].Length * 6, 2100f, 360f, true, true, 0f, -5f, true, true, 1, 5);
            //this.platformsDynamic[2] = new PlatformDynamic(platforms[0].Length * 7, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            //this.platformsDynamic[3] = new PlatformDynamic(platforms[0].Length * 8, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            //this.platformsDynamic[4] = new PlatformDynamic(platforms[0].Length * 1, 1700f, 360f, true, true, 0f, -8f, true, true, 1, 8);
            //this.platformsDynamic[5] = new PlatformDynamic(platforms[0].Length * 2, 1500f, 360f, true, true, 0f, 0f, true, true, 1, 5);
            //this.platformsDynamic[6] = new PlatformDynamic(platforms[0].Length * 3, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5);
            //this.platformsDynamic[7] = new PlatformDynamic(platforms[0].Length * 4, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5);
            //this.platformsDynamic[8] = new PlatformDynamic(platforms[0].Length * 5, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5);
            //this.platformsDynamic[9] = new PlatformDynamic(platforms[0].Length * 6, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5);
            //this.platformsDynamic[10] = new PlatformDynamic(platforms[0].Length * 7, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5);
            //this.platformsDynamic[11] = new PlatformDynamic(platforms[0].Length * 8, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5);
            //this.platformsDynamic[12] = new PlatformDynamic(platforms[0].Length * 9, 2300f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            //this.platformsDynamic[13] = new PlatformDynamic(platforms[0].Length * 3, 1800f, 360f, true, true, 0f, 0f, true, false, 1, 5);


            foreach(Platform platform in platforms)
            {

                platform.visible = true;
                platform.Skin = (Content.Load<Texture2D>("Sprites\\Platform"));
            }
            ////for (int i = 0; i < maxPlatforms; i++)
            ////{

            ////    if (i < numPlatforms)
            ////    {
            ////        platformsDynamic[i].visible = true;
            ////        platformsDynamic[i].Skin = (Content.Load<Texture2D>(
            ////        "Sprites\\Platform"));
            ////    }
            ////    else
            ////    {
            ////        platformsDynamic[i].visible = false;
            ////    }


                //this.platformsDynamic[0] = new PlatformDynamic(platforms[0].Length * 5, 2220f, 360f, true, true, 0f, -1f, true, true, 10, 200);
                //public PlatformDynamic(float XDefault, float YDefault, float initLength, bool initYDynamic, bool initXDynamic,
                //float initdx, float initdy, bool initXPositive, bool initYPositive, int initSpeed, int initScale)

                //this.platforms[15].position = new Vector2(platforms[0].Length * 1, 200f);
                //this.platforms[16].position = new Vector2(platforms[0].Length * 1, 200f);
                //this.platforms[17].position = new Vector2(platforms[0].Length * 1, 200f);
                //this.platforms[16].position = new Vector2(platforms[0].sprite.Width * 13, 4400);
                //this.platforms[16].vertical = true;
                //this.platforms[16].horizontal = false;
                //this.platforms[16].dy = -4f;
                //this.platforms[16].up = false;
                //this.platforms[17].position = new Vector2(platforms[0].sprite.Width * 14, 4400);
                //this.platforms[17].vertical = true;
                //this.platforms[17].horizontal = true;
                //this.platforms[17].dy = -4f;
                //this.platforms[17].dx = +6f;
                //this.platforms[17].up = false;
                //this.platforms[17].right = true;
                //this.platforms[18].position = new Vector2(platforms[0].sprite.Width * 6, 4300);
                //this.platforms[19].position = new Vector2(platforms[0].sprite.Width * 7, 4200);
                //this.platforms[20].position = new Vector2(platforms[0].sprite.Width * 5.5f, 4100);
                //this.platforms[21].position = new Vector2(platforms[0].sprite.Width * 16 - 20, 4450);
                //this.platforms[21].vertical = false;
                //this.platforms[21].horizontal = true;
                //this.platforms[21].dy = -4f;
                //this.platforms[21].dx = 0f;
                //this.platforms[21].up = false;
                //this.platforms[21].right = true;
                //this.platforms[22].position = new Vector2(platforms[0].sprite.Width * 18 - 20, 4450);
                //this.platforms[22].vertical = false;
                //this.platforms[22].horizontal = true;
                //this.platforms[22].dy = +4f;
                //this.platforms[22].dx = 0f;
                //this.platforms[22].up = false;
                //this.platforms[22].right = false;
                //this.platforms[23].position = new Vector2(platforms[0].sprite.Width * 4.5f, 4000);
                //this.platforms[24].position = new Vector2(platforms[0].sprite.Width * 3.5f, 4100);
                //this.platforms[25].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4100);
                //this.platforms[25].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4400);
                //this.platforms[26].position = new Vector2(platforms[0].sprite.Width * .5f, 4400);
                //this.platforms[27].position = new Vector2(platforms[0].sprite.Width * 1.5f, 4350);
                //this.platforms[28].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4150);
                //this.platforms[29].position = new Vector2(platforms[0].sprite.Width * 1f, 4250);
            

        }

        /// <summary>
        /// This is called in the update method after the user input but before any game updates are performed.
        /// designed to allow level specific limitations occure, such as manageing screenscroll limits or boss battles, etc. 
        /// all levels will use general purpose flags, which will be global variables. All GPFlags are individually defined in each conditional method.
        /// GPFlag uses:
        /// GPFlag1 - identifies if engaged in the Hind D boss battle, which will alter screen panning abilities and alter music values.
        /// GPFlag2 - identifies if HindD is destroyed. 
        /// GPFlag3 - identifies if engaged in Final boss battle, which will lock the panning abilities, and alther music values. 
        /// GPFlag4 - identifies if final boss is destroyed. 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void loadLevelOneConditionals()
        {

            if (this.backgroundX > (320f * 15) && !GPFlag2 && !GPFlag1)
            {
                GPFlag1 = true;
                this.lockPan = true;

                enemies.Add(spawnHindD(5, 360f * 15, 1900f, 0f, 0f, 360f * 5, 1900f, 0f, 1f, 128, 192, true));
                //enemies.DESUDESU
            }
            if (GPFlag1 && bossDefeated)
            {
                GPFlag2 = true;
                bossDefeated = false;
                this.lockPan = false;
            }
             
        }
        //EACH ENEMY NEEDS A SPAWN METHOD :D this should be done differently. 
        public HindD spawnHindD(int numposes, float initX, float initY, float initRotation, float initFireTimer,
                    float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteSize, int SpriteHeight, bool bossEnemy)
        {
            HindD miniBoss = new HindD(numposes, initX, initY, initRotation, initFireTimer, initXCenter, initYCenter, initXVelocity, initYVelocity, SpriteSize, SpriteHeight, bossEnemy);
            for (int i = 0; i < numposes; i++)
            {
                miniBoss.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\HindD"), i);//TODO all spritesheets must be appended by a number, and the file name is appended
                //by a number which is converted to a string, "Sprites\\Enemy" + Int.toString(i) [something like that].
            }
            return miniBoss;

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draw the backgroundTexture sized to the width
            //and height of the screen.
            spriteBatch.Draw(backgroundTexture, viewportRect,
                Color.White);

            foreach (Bullet ball in cannonBalls)
            {
                if (ball.alive)
                {
                    spriteBatch.Draw(ball.sprite,
                        ball.position, Color.White);
                }
            }
            foreach (Enemy enemy in enemies)
            {
             //   if (ball.alive) use qualifier for (if the sprite is in viewport) TODO
             //   {
                if (!enemy.hit)
                {
                    spriteBatch.Draw(enemy.mainSpriteSheet[enemy.pose], new Vector2(enemy.position.X - backgroundX, enemy.position.Y - backgroundY),
                        enemy.sourceRect, Color.White);//can create phantom units here.......
                }
                else
                {
                    spriteBatch.Draw(enemy.mainSpriteSheet[enemy.pose], new Vector2(enemy.position.X - backgroundX, enemy.position.Y - backgroundY),
                     enemy.sourceRect, Color.Crimson);
                }//can create phantom units here.......
             //   }
            }
            ////foreach (Stage1EnemyA enemy in S1Aenemies)
            ////{
            ////    //   if (ball.alive) use qualifier for (if the sprite is in viewport) TODO
            ////    //   {
            ////    if (!enemy.hit)
            ////    {
            ////        spriteBatch.Draw(enemy.mainSpriteSheet[enemy.pose], new Vector2(enemy.position.X - backgroundX, enemy.position.Y - backgroundY),
            ////            enemy.sourceRect, Color.White);//can create phantom units here.......
            ////    }
            ////    else
            ////    {
            ////        spriteBatch.Draw(enemy.mainSpriteSheet[enemy.pose], new Vector2(enemy.position.X - backgroundX, enemy.position.Y - backgroundY),
            ////         enemy.sourceRect, Color.Crimson);
            ////    }//can create phantom units here.......
            ////    //   }
            ////}
            //Vector2 currentPotion = new Vector2(0f, 0f);
            //foreach (GameObject potion in potions)
            //{
            //    if (potion.alive)
            //    {
            //        currentPotion = new Vector2(potion.position.X - backgroundX, potion.position.Y - backgroundY);
            //        spriteBatch.Draw(potion.sprite, currentPotion, potions[0].sourceRect, Color.White);
            //    }
            //}
            foreach (EnemyBullet ball in enemyCannonBalls)
            {
                if (ball.alive)
                {
                    spriteBatch.Draw(ball.sprite,
                        ball.position, Color.White);
                }
            }
            //main character logic
            if (Duke.alive)
            {
                //if (!morph)
                //{
                //    if (jumpReady)
                //    {
                //        if (cannon.right)
                //        {
                //            if (!hit)
                //            {
                //                spriteBatch.Draw(cannon.sprite, new Vector2(cannon.position.X + (2 * spriteWidth),
                //                    cannon.position.Y), cannon.sourceRect, Color.White,
                //                    cannon.rotation,
                //                    cannon.center, 1.0f, SpriteEffects.None, 0);
                //            }
                //            else
                //            {
                //                spriteBatch.Draw(cannon.sprite, new Vector2(cannon.position.X + (2 * spriteWidth),
                //                    cannon.position.Y), cannon.sourceRect, Color.Chocolate,
                //                    cannon.rotation,
                //                    cannon.center, 1.0f, SpriteEffects.None, 0);
                //            }
                //        }
                //        else
                //        {
                //            if (!hit)
                //            {
                //                spriteBatch.Draw(cannon.sprite, new Vector2(cannon.position.X + (2 * spriteWidth),
                //                    cannon.position.Y), cannon.sourceRect, Color.White,
                //                    cannon.rotation,
                //                    cannon.center, 1.0f, SpriteEffects.FlipHorizontally, 0);
                //            }
                //            else
                //            {
                //                spriteBatch.Draw(cannon.sprite, new Vector2(cannon.position.X + (2 * spriteWidth),
                //                    cannon.position.Y), cannon.sourceRect, Color.Chocolate,
                //                    cannon.rotation,
                //                    cannon.center, 1.0f, SpriteEffects.FlipHorizontally, 0);
                            
                //            }
                //        }
                //    }
                //    else
                //    {
                //        spriteBatch.Draw(cannon.jumper, new Vector2(cannon.position.X + (2 * spriteWidth),
                //                cannon.position.Y), cannon.sourceRect, Color.White,
                //                cannon.rotation,
                //                cannon.center, 1.0f, SpriteEffects.None, 0);
                //    }
                //}
                
                if (Duke.pose == 0)//faceing right.
                {
                    //spriteBatch.Draw(Duke.mainSpriteSheet[0], new Vector2(Duke.position.X + (2 * spriteWidth),
                    //    Duke.position.Y), Duke.sourceRect, Color.White,
                    //    Duke.rotation,
                    //    Duke.center, 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(Duke.mainSpriteSheet[0], new Vector2(Duke.position.X, Duke.position.Y), Duke.sourceRect, Color.White);
                   

                }
                else if(Duke.pose == 1)//put conditions for whether he faces left or right.
                {
                    //spriteBatch.Draw(Duke.mainSpriteSheet[1], new Vector2(Duke.position.X  + (2 * spriteWidth),
                    //    Duke.position.Y), Duke.sourceRect, Color.White,
                    //    Duke.rotation,
                    //    Duke.center, 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(Duke.mainSpriteSheet[1], new Vector2(Duke.position.X, Duke.position.Y), Duke.sourceRect, Color.White);
                }
                else if (Duke.pose == 3)//put conditions for whether he faces left or right.
                {
                    //spriteBatch.Draw(Duke.mainSpriteSheet[1], new Vector2(Duke.position.X  + (2 * spriteWidth),
                    //    Duke.position.Y), Duke.sourceRect, Color.White,
                    //    Duke.rotation,
                    //    Duke.center, 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(Duke.mainSpriteSheet[3], new Vector2(Duke.position.X, Duke.position.Y), Duke.sourceRect, Color.White);
                }
                else if (Duke.pose == 4)//put conditions for whether he faces left or right.
                {
                    //spriteBatch.Draw(Duke.mainSpriteSheet[1], new Vector2(Duke.position.X  + (2 * spriteWidth),
                    //    Duke.position.Y), Duke.sourceRect, Color.White,
                    //    Duke.rotation,
                    //    Duke.center, 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(Duke.mainSpriteSheet[4], new Vector2(Duke.position.X, Duke.position.Y), Duke.sourceRect, Color.White);
                }

               
            }
            //else//dead animation possily goes here.
            //{
            //    spriteBatch.Draw(cannon.sprite, new Vector2(cannon.position.X + (2 * spriteWidth),
            //                cannon.position.Y), cannon.sourceRect, Color.White,
            //                cannon.rotation,
            //                cannon.center, 1.0f, SpriteEffects.FlipHorizontally, 0);

            //}
            //muzzle flash
            //if (fire)
            //{
            //    spriteBatch.Draw(flash, flashDestRect, flashRect, Color.WhiteSmoke, 0f, cannon.center,
            //           SpriteEffects.None, 0);
            //    fireCounter--;
            //    if (fireCounter == 0)
            //    {
            //        fire = false;
            //    }
            //}
            //spriteBatch.Draw(
            //foreach (GameObject enemy in enemies)
            //{
            //    if (enemy.alive)
            //    {
            //        spriteBatch.Draw(enemy.sprite,
            //            enemy.position, Color.White);
            //    }
            //}
            //Vector2 currentKey = new Vector2(0f, 0f);
            //foreach (GameObject key in keys)
            //{
            //    if (key.alive)
            //    {

            //        currentKey = new Vector2(key.position.X - backgroundX,
            //            key.position.Y - backgroundY);
            //        spriteBatch.Draw(key.sprite, currentKey, cannon.sourceRect, Color.White);
            //    }
            //}
            Vector2 currentPlatform = new Vector2(0f, 0f);
            foreach (Platform platform in platforms)
            {
                if (platform.visible)
                {
                    currentPlatform = new Vector2(platform.position.X - backgroundX,
                        platform.position.Y - backgroundY + 36f);
                    spriteBatch.Draw(platform.Skin,
                        currentPlatform, Color.White);
                }
            }
            ////foreach (PlatformDynamic platform in platforms)
            ////{
            ////    if (platform.visible)
            ////    {
            ////        currentPlatform = new Vector2(platform.position.X - backgroundX,
            ////            platform.position.Y - backgroundY + 36f);
            ////        spriteBatch.Draw(platform.Skin,
            ////            currentPlatform, Color.White);
            ////    }
            ////}

            //spriteBatch.Draw(statusTexture, new Rectangle(0, 0, viewportRect.Width, 100), Color.White);       

            //spriteBatch.DrawString(font,
            //    "Score: " + score.ToString(),
            //    new Vector2(scoreDrawPoint.X * viewportRect.Width + 100,
            //    scoreDrawPoint.Y * viewportRect.Height - 20f) ,
            //    Color.Yellow);

            //spriteBatch.DrawString(font,
            //    "Health: " + life.ToString(),
            //    new Vector2(scoreDrawPoint.X * viewportRect.Width + 350,
            //    scoreDrawPoint.Y * viewportRect.Height - 20f),
            //    Color.Yellow);

            //spriteBatch.DrawString(font,
            //    "x " + lives.ToString(),
            //    new Vector2(scoreDrawPoint.X * viewportRect.Width + 300,
            //    scoreDrawPoint.Y * viewportRect.Height),
            //    Color.Yellow);

            //if (!hit)
            //{
            //    spriteBatch.Draw(HUDHitTexture, HUDRect, Color.White);
            //}
            //else
            //{
            //    spriteBatch.Draw(HUDTexture, HUDRect, Color.Chocolate);
            //}
            //if (hit)
            //{
            //    hitCounter++;
            //    if(hitCounter == 5){
            //        hit = false;
            //        hitCounter = 0;
            //    }
            //}
            spriteBatch.End();

            base.Draw(gameTime);
        }



            //====================================================
            //general purpose Random number generator
            //====================================================
            public int randomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }
        
            
                        //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
                        //spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                        //spriteBatch.Begin();
                        ////if int title menu--------------------------------------------------------------------
                        //if(inTitleScreen)
                        //{
                        //    //draw the background, play the music, display options for quiting or playing
                

                        //    graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

                        //    viewportRect = new Rectangle(0, 0,
                        //       graphics.GraphicsDevice.Viewport.Width,
                        //       graphics.GraphicsDevice.Viewport.Height);

                
                        //    spriteBatch.Draw(titleScreen, viewportRect, Color.White);

                        //    //spriteBatch.DrawString(font,
                        //    //    "Press 'Esc' to Exit",
                        //    //    new Vector2(viewportRect.Width - 750,    //X coordinate
                        //    //        viewportRect.Height - 300),          //Y coordinate
                        //    //        Color.Yellow);


       
















        
                        //    //spriteBatch.DrawString(font,
                        //    //    "Press 'P' to play!",
                        //    //    new Vector2(viewportRect.Width - 750,
                        //    //        viewportRect.Height - 450),
                        //    //        Color.Yellow);

                        //    spriteBatch.End();
                        //}


                        //// TODO: Add your drawing code here

                        //base.Draw(gameTime);
        }


     
        


        
    }

