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

//FUCK YOU TEEMO
namespace RDash
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    enum View { Front, Right, Rear, Left }; 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
      
        //List<int> listint = new List<int>();
        GraphicsDeviceManager graphics;
        //TODO: CREATE TEXTURE MANAGEMENT CONSTRUCT. possibly make textures simple objects...
        Texture2D backgroundTexture, backgroundTextureB, backgroundTextureC, statusTexture, deathTexture, HUDTexture, HUDHitTexture,
            flash, titleScreen;//deathTexture will be replaced by the sprite array.
        //set the 2D model to draw. 
        Model myModel;
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;
        float targetModelRotation = 0.0f;
        int currentView = (int)View.Front; 
        //set the position of the camera in world space, for our view matrix. 
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        // The aspect ratio determines how to scale 3d to 2d.
        float aspectRatio;
        //Enemy[] enemies;//to be renamed after naming scheme is established. 
        List<Enemy> enemies = new List<Enemy>();
        //TODO, DETERMINE  A WAY TO DYNAMICALY DEALLOCATE MEMORY FROM THE FOLLOWIN ARRAYS AFTER THE STAGE IS COMPLETE.
        //Stage1EnemyA[] S1Aenemies;
        List<VFX> specialFX = new List<VFX>(); 
        Rectangle viewportRect;
        SpriteBatch spriteBatch;
        List<Player> players = new List<Player>();
        List<int> expiredFX = new List<int>();
        Character Duke;
        const int maxCannonBalls = 50;
        const int maxEnemyCannonBalls = 200;
        const int maxPlatforms = 50;
        GameObject[] potions;//enemyCannonBalls to be replaced by array in the enemyclass.
        Bullet[] cannonBalls;
        EnemyBullet[] enemyCannonBalls;
        //PlatformStatic[] platforms;
        //PlatformDynamic[] platformsDynamic;
        List<Platform>[] platforms = new List<Platform>[4];
        List<Door>[] doors = new List<Door>[4];
        List<SoundEffect> randomCheer = new List<SoundEffect>(); 
        //List<PlatformDynamic> platformsDynamic = new List<PlatformDynamic>();
        GamePadState previousGamePadState = GamePad.GetState(PlayerIndex.One);
        KeyboardState previousKeyboardState, keyboardState; 
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
        bool GPFlag1, GPFlag2, GPFlag3, GPFlag4, GPFlag5, GPFlag6, GPFlag7, GPFlag8, lockPan, fire, bossDefeated;
        float GPTimer1, GPTimer2, GPTimer3, GPTimer4, GPTimer5, GPTimer6, GPTimer7, GPTimer8;
        float GPInterval1, GPInterval2, GPInterval3, GPInterval4, GPInterval5, GPInterval6, GPInterval7, GPInterval8; 
        bool platforming = false; //used to determine if player is on active platform. 
        bool newPlatform = false;
        int level = 1;
        int listIndex = 0;
        int numEnemies = 0;
        int numFX = 0;
        int hitCounter = 0; 
        SoundEffect enemyFireEffect;
        SoundEffect beamSoundEffect;
        SoundEffect shieldActivation;
        Platform currentPlatform; 
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
        float backgroundX = 0f;
        float backgroundY = 2000f;
        bool loadLevel = true;
        //audio files and necessary helper data types.
        Song levelOneTheme, Monochrome;
        bool songstart = false;
        public Game1()
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
            levelOneTheme = Content.Load<Song>("Audio\\Ikaruga_Chapter1");
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
            backgroundTextureB =
                   Content.Load<Texture2D>("Sprites\\BackgroundB");
            backgroundTextureC =
                   Content.Load<Texture2D>("Sprites\\BackgroundC");
            statusTexture =
                   Content.Load<Texture2D>("Sprites\\Statusbanner");
            titleScreen =
                   Content.Load<Texture2D>("Sprites\\titleScreenBackground");
            myModel = Content.Load<Model>("Model\\untitled");
            modelPosition.Y -= 1500f; //testing models. 
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            //cannonTexture = Content.Load<Texture2D>("Sprites\\Duke");
            enemyFireEffect = Content.Load<SoundEffect>("Audio\\blast");
            beamSoundEffect = Content.Load<SoundEffect>("Audio\\Beam_Sound_Effect");
            shieldActivation = Content.Load<SoundEffect>("Audio\\MLPGurren"); 
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
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeRight"), 3);
            Duke.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\DukeLeft"), 4);
            //
            for (int i = 1; i < 7; i++)
            {
                randomCheer.Add(Content.Load<SoundEffect>("Audio\\pl3a_voi_0000" + i.ToString())); 
            }
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
                    "Sprites\\RBullet"));
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
            Duke.setAllRangeBounds(viewportRect.Width, viewportRect.Height); 
            
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
                specialFX = new List<VFX>();
                numFX = 0;
                enemies = new List<Enemy>();
                numEnemies = 0;
                //during level loads, reload GUI, 
                //dump GUI's after dumps. `
                specialFX.Add(new energy_Gauge_Holder(200f, 1000f, 0f, 0f,
                        0f, 0f, 0f, backgroundX, backgroundY, 1));
                numFX++;
                specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\EnergyGaugeHolder"));

                specialFX.Add(new energy_Gauge(200f, 1000f, 0f, 0f,
                        0f, 0f, 0f, backgroundX, backgroundY, 3));
                numFX++;
                specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\EnergyGauge"));

                specialFX.Add(new health_Gauge_Holder(200f, 1000f, 0f, 0f,
                        0f, 0f, 0f, backgroundX, backgroundY, 1));
                numFX++;
                specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\EnergyGaugeHolder"));

                specialFX.Add(new health_Gauge(200f, 1000f, 0f, 0f,
                        0f, 0f, 0f, backgroundX, backgroundY, 3));
                numFX++;
                specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\HealthGauge"));

                switch (level)
                {
                    case 1:
                        loadLevelOne();
                        break;
                    case 2:
                        loadLevelTwo();
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
                //dump all Duke data cycle resets here. Gather in appropriate Duke.CycleReset method*****
                Duke.openDoor = false;
                //  ****END CYCLE REST FOR DUKE  ****                          

                if (Duke.position.X > (currentPlatform.position.X + currentPlatform.Length -250f) - backgroundX) { newPlatform = true; } //determines the condition to spawn a new platform
                //testbed for logic in creating a new platform.
                if (newPlatform && !Duke.allRangeMode)
                {
                    currentPlatform = new PlatformStatic(currentPlatform.position.X + currentPlatform.Length + randomNumber(100, 200), //x
                        currentPlatform.position.Y + randomNumber(-150, 150), //y
                        320f, //randomNumber(200,320)
                        (Content.Load<Texture2D>("Sprites\\Cloud1")));

                    this.platforms[currentView].Add(currentPlatform);
                    newPlatform = false;
                }

                
                keyboardState = Keyboard.GetState();
                if (life <= 0 || Duke.position.Y + backgroundY > 4550)
                {
                    Duke.alive = false;
                }
                //control if statement handles the world 3-D rotation. 
                if (keyboardState.IsKeyDown(Keys.D)&&previousKeyboardState.IsKeyUp(Keys.D))
                {
                    //TODO: drop backgroundX and backgroundY to the nearest multiple of 64f.
                    for (int i = 0; i < 14; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            //specialFX.Add(new bullet_Decay(ball.position.X, ball.position.Y, 0f, 0f,
                       // 0f, 0f, 16f, backgroundX, backgroundY, 3));
                            
                            specialFX.Add(new rotationSwipeLeft((64f * i) + backgroundX, (64f * j) + backgroundY, 0f, 0f,
                                0f, 0f, 0f, backgroundX, backgroundY, 4));
                            numFX++;
                            specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\RotationSwipe_Left"));
                        }
                    }
                    targetModelRotation = (float)(modelRotation + (Math.PI) * .5);
                    switch (currentView)
                    {
                        case (int)View.Front:
                            currentView = (int)View.Right;
                            break; 
                        case (int)View.Right:
                            currentView = (int)View.Rear;
                            break;
                        case (int)View.Rear:
                            currentView = (int)View.Left;
                            break;
                        case (int)View.Left:
                            currentView = (int)View.Front;
                            break;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                {
                    targetModelRotation = (float)(modelRotation - (Math.PI) * .5);
                    switch (currentView)
                    {
                        case (int)View.Front:
                            currentView = (int)View.Left;
                            break;
                        case (int)View.Left:
                            currentView = (int)View.Rear;
                            break;
                        case (int)View.Rear:
                            currentView = (int)View.Right;
                            break;
                        case (int)View.Right:
                            currentView = (int)View.Front;
                            break;
                    }
                }
                
                float deltaRotationStep = (targetModelRotation - modelRotation)/20;
                if (targetModelRotation != modelRotation)
                {

                    modelRotation += (deltaRotationStep); 
                }
                //captures input for opening a door. 
                if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    Duke.openDoor = true; 
                }
                Timing(gameTime);
                //Updates sprite=================
                
                if (Duke.alive)
                {
                    
                    
                    ////Duke.velocity.X = maxSpeed; 
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
                    Duke.update(gameTime, previousKeyboardState, keyboardState, backgroundX, backgroundY);

                    if (Duke.spawnFlyingNimbus)
                    {
                        Duke.spawnFlyingNimbus = false;
                        specialFX.Add(new flying_Nimbus(Duke.position.X + backgroundX, Duke.position.Y + backgroundY + 50f, 0f, 0f,
                                0f, 0f, 0f, backgroundX, backgroundY, 1));
                        numFX++;
                        specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\flyingNimbus"));
                    }
                    if (Duke.spawnShieldRDash)
                    {
                        Duke.spawnShieldRDash = false;
                        specialFX.Add(new shield_RDash(Duke.position.X + backgroundX + 10f, Duke.position.Y + backgroundY + 10f, 0f, 0f,
                        0f, 0f, 0f, backgroundX, backgroundY, 3));
                        numFX++;
                        specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\shieldRDash"));
                        shieldActivation.Play();
                    }
                    if (Duke.fireCannonBall)
                    {
                        Duke.fireCannonBall = false;
                        FireCannonBall();//this needs to be taken out of the main Game1 class...
                    }

                    if (Duke.craftingWorld)
                    {
                        if (Duke.spawnCursor)
                        {
                            specialFX.Add(new cursorWorldPlacement(backgroundX + 64f -12f, backgroundY + 64f, 0f, 0f,
                            0f, 0f, 16f, backgroundX, backgroundY));
                            numFX++;
                            specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Cursor"));
                            specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;

                        }
                        else
                        {
                            foreach (GameObject FX in specialFX)
                            {
                                if (FX is cursorWorldPlacement)
                                {
                                    FX.move(Duke.cursorDirection);
                                }
                            }

                        }
                    }
                    if (!Duke.craftingWorld)
                    {
                        foreach (GameObject FX in specialFX)
                        {
                            if (FX is cursorWorldPlacement)
                            {
                                FX.ttl = -1; 
                            }
                        }
                    
                    }
                   /* else
                    {
                        foreach (GameObject FX in specialFX)
                        {
                            if (FX is cursorWorldPlacement)
                            {
                                FX.ttl = 0; 
                            }
                        }
                    }*/
                    //if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Left))
                    //{
                    //    Duke.rotation = 3.9f;
                    //    //Duke.velocity.X -= maxSpeed;//moves left at the per/second rate.
                       
                    //}
                    //else if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Right))
                    //{
                    //    Duke.rotation = 5.45f;
                    //    //Duke.velocity.X += maxSpeed;
                    //}
                    //else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Right))
                    //{
                    //    Duke.rotation = Duke.rotation = (float)(Math.PI * .25f) ;
                    //    //Duke.velocity.X += maxSpeed;
                    //}
                    //else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Left))
                    //{
                    //    Duke.rotation = (float)(Math.PI * .75f);
                    //    //Duke.velocity.X += maxSpeed;
                    //}
                    //else if (keyboardState.IsKeyDown(Keys.Down))
                    //{
                    //    Duke.rotation = 1.57f;
                    //}

                    //else if (keyboardState.IsKeyDown(Keys.Left))
                    //{

                    //    Duke.rotation = (float) Math.PI ;
                       
                    //}

                    //else if (keyboardState.IsKeyDown(Keys.Right))
                    //{
                    //    Duke.rotation = 0f;
                    //    //Duke.velocity.X += maxSpeed;
                        
                    //}
                    
                    //else if (keyboardState.IsKeyDown(Keys.Up))
                    //{
                    //    Duke.rotation = 4.71f;//4.7f;
                    //}

                    //else
                    //{
                    //    Duke.rotation = 0f; 
                    //}

                    //if (keyboardState.IsKeyDown(Keys.Space))
                    //{
                    //    FireCannonBall();
                    //    //great test bed
                        
                    //}

                    //if (keyboardState.IsKeyDown(Keys.W) && Duke.energy > 0 && Duke.airborne)
                    //{
                    //    Duke.levitation = true;
                    //    Duke.decrementEnergy(1f) ;
                    //    Duke.velocity.Y = 0;
                    //    if (!previousKeyboardState.IsKeyDown(Keys.W))
                    //    {
                    //        specialFX.Add(new flying_Nimbus(Duke.position.X + backgroundX, Duke.position.Y + backgroundY + 50f, 0f, 0f,
                    //        0f, 0f, 0f, backgroundX, backgroundY, 1));
                    //        numFX++;
                    //        specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\flyingNimbus"));
                    //    }
                    //}
                    //else { Duke.levitation = false; }


                    //if (keyboardState.IsKeyDown(Keys.E) && !Duke.shield && Duke.energy >= 20f)
                    //{
                    //    Duke.shield = true;
                    //    Duke.decrementEnergy(20f);
                    //    specialFX.Add(new shield_RDash(Duke.position.X + backgroundX + 10f, Duke.position.Y + backgroundY + 10f, 0f, 0f,
                    //    0f, 0f, 0f, backgroundX, backgroundY, 3));
                    //    numFX++;
                    //    specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\shieldRDash"));
                    //    shieldActivation.Play();
                        
                    //}
                    ///*/jump loop
                    // if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                    // {
                    //     if (!currentlyJumping)
                    //     {
                    //         jump();
                    //     }

                    // }
                    // */

                    //if (keyboardState.IsKeyDown(Keys.Q) && !Duke.airborne )
                    //{
                    //    Duke.velocity.Y = -20f;
                    //    Duke.airborne = true;
                    //    Duke.levitation = false; 
                    //}

                    
                    ////if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                    ////{
                    ////    Morph();
                    ////}
                }

                ////Duke.rotation = MathHelper.Clamp(Duke.rotation, -MathHelper.PiOver2, 0);
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

                    if (enemy.destRect.Intersects(Duke.destRect))
                    {
                         
                        
                        Duke.hit = true;
                        

                        //inserte health decrement here. -= 1;
                        break;
                    }
                    
                }
                //foreach (Stage1EnemyA enemy in S1Aenemies)
                //{
                //    if (enemy.alive)
                //    {
                //        enemy.hit = false;
                //    }
                //}
                UpdatePlatforming(this.backgroundX, this.backgroundY, (float)gameTime.ElapsedGameTime.TotalMilliseconds); //deals with platforming mechanic, to be developed.
                Updatedoors(this.backgroundX, this.backgroundY, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                UpdatePanning();
                UpdateCannonBalls(gameTime); 
                
                /* Following code snippet will append the current VFX list with a vfx for each item that is to the right and to the left. 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * */
                /*
                switch (currentView)
                {

                    case((int)View.Front):
                        foreach(Platform platform in platforms[(int)View.Right]){
                            specialFX.Add(new bullet_Decay(platform.position.X, platform.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                    numFX++;
                    specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                    specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;


                        }

                        break;
                    case((int)View.Right):
                        foreach(Platform platform in platforms[(int)View.Rear]){
                            specialFX.Add(new bullet_Decay(platform.position.X, platform.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                    numFX++;
                    specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                    specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;

                        }
                        break;

                    case((int)View.Rear):
                        foreach(Platform platform in platforms[(int)View.Left]){
                            specialFX.Add(new bullet_Decay(platform.position.X, platform.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                    numFX++;
                    specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                    specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;
                        }

                        break;
                    case((int)View.Left):
                        foreach(Platform platform in platforms[(int)View.Front]){
                            specialFX.Add(new bullet_Decay(platform.position.X, platform.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                    numFX++;
                    specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                    specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;
                        }
                        break;
                }
                */
                // TODO: Add your update logic here
                //TODO: these lines should be in the duke.update method if such a method exists. 
                //used to configure landing pose. TODO: PUT IN UPDATE DUKE METHOD.
                

            }
            base.Update(gameTime);
            previousKeyboardState = keyboardState; 
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
            
            ////this.Duke.position.X += Duke.velocity.X;
           // Vector2 tempVect = Duke.position;//cheap bandage TROLOLOLOOLOL best practices. 
            if (!Duke.allRangeMode)
            {
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
                else if (!lockPan && backgroundX <= Duke.spriteWidth && Duke.velocity.X < 0 && Duke.position.X <= Duke.spriteWidth / 6)//if passes, the leftrunning Duke will not run off the SCREEN. uses else if because previous if-statement. 
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
                if (Duke.position.X < 400 && !lockPan && Duke.velocity.X > 0) //allows duke to move freely right. 
                {
                    this.Duke.position.X += 0;
                }
                if (Duke.position.X >= 400 && !lockPan && Duke.velocity.X > 0) // allows right panning. 
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
            }
            //upon exiting this method, position does not update. 
        }
        /*==============================================
         * UpdatePlatforming()
         * Purpose: Keeps track of falling for platforming dynamics.
         * 
         * Class variables used: bulletTimer, deltaTime, bulletInterval, 
         * bulletReady, timer, spriteInterval, currentFrame, frameCount
         *============================================== */

        public void UpdatePlatforming(float backX, float backY, float gameTime)
        {
            platforming = false;
            if (Duke.airborne && !Duke.levitation)//TODO: IMPLEMENT GRAVITY.
            {
                //apply physics later, use linear decelleration for now.
                if (Duke.velocity.Y < 10f)
                {
                    Duke.velocity.Y += gravityAccel;
                }
                
                //Duke.position.Y += Duke.velocity.Y;
                
                if (Duke.position.Y > 150 && Duke.velocity.Y <0)
                {
                    Duke.position.Y += Duke.velocity.Y;
                }
                else if (Duke.position.Y <= 150 && backgroundY > 0f && Duke.velocity.Y < 0f && !Duke.allRangeMode)
                {
                    this.backgroundY += Duke.velocity.Y;
                }
                else if (Duke.position.Y + Duke.velocity.Y > 275 && Duke.velocity.Y > 0f &&!Duke.allRangeMode)
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
            foreach (Platform platform in platforms[currentView])
            {
                platform.Update(this.backgroundX, this.backgroundY, gameTime);
            }
            

            //===========================================
            //Duke.falling = true;//might be unnecessary.
            //jumpReady = false;
            //debug This never enters after I push the A button.
            //TODO: DEBUG why player is stuck with half of body in platform.
            
            foreach (Platform platform in platforms[currentView])
            {
                
                foreach (Enemy enemy in enemies)
                {
                    //accounts for platforming
                    if (enemy.position.Y <= (platform.position.Y) &&
                    enemy.position.Y > (platform.position.Y) - enemy.spriteHeight &&
                    enemy.position.X >= (platform.position.X - 25f) &&
                    (float)enemy.position.X < platform.Length + (platform.position.X - 25f)&& enemy.velocity.Y > 0
                    && platform.platform)
                    {
                        enemy.position.Y = platform.position.Y - (float)(enemy.spriteHeight);//16 is for 64x64 sprite. this must use the sprite height. 
                        enemy.airborne = false;
                        enemy.velocity.Y = 0f;
                    }

                    if (platform.wall && enemy.position.X < platform.position.X +platform.Length)
                    {

                        if ((enemy.position.X <= (platform.position.X) &&
                            enemy.position.X > (platform.position.X) - 64f &&
                            enemy.position.Y >= (platform.position.Y - (float)(enemy.spriteHeight / 2)) &&
                        (float)enemy.position.Y < platform.Skin.Height + (platform.position.Y)
                            && enemy.velocity.X > 0) ||
                            (enemy.position.X >= (platform.position.X) &&
                            enemy.position.X < (platform.position.X + platform.Length) && enemy.velocity.X < 0 &&
                            enemy.position.Y >= (platform.position.Y - (float)(enemy.spriteHeight/2)) &&
                        (float)enemy.position.Y < platform.Skin.Height + (platform.position.Y)
                            && enemy.velocity.X < 0)
                            && platform.wall
                            && !enemy.ignoresWalls)
                        {
                            enemy.reverseVelocityX();
                            

                        }
                    }
                }
                if (platform is VertCube)
                {
                    //ew this must be redun andyou know it. 
                    Rectangle tempRect = new Rectangle((int)platform.position.X, (int)platform.position.Y, (int)platform.Skin.Height, (int)(platform.Length/platform.numSprites)); 
                    if (Duke.destRect.Intersects(tempRect) )
                    {
                        if (Duke.velocity.Y > -16f)
                        {
                            Duke.velocity.Y -= 2f; //let him fly. 
                        }
                    }
                }
                
                if (platform is RightCube)
                {
                    //ew this must be redun andyou know it. 

                    Rectangle tempRect = new Rectangle((int)platform.position.X, (int)platform.position.Y, 64, 64);
                    if (Duke.destRect.Intersects(tempRect))
                    {
                        if (Duke.velocity.X < 16f)
                        {
                            Duke.velocity.X += 4f; //let him 'gotta go fast'.   
                        }
                        if (Duke.velocity.X < 0f)
                        {
                            Duke.velocity.X += 6f;
                        }
                        if (Duke.velocity.X < -6f)
                        {
                            Duke.velocity.X += 16f;
                        }
                        

                    }
                }
                

                //determines if duke should land on this platform.
                if (Duke.position.Y <= (platform.destinationRect.Y) &&
                    Duke.position.Y > (platform.destinationRect.Y) - 64f &&
                    Duke.position.X >= (platform.destinationRect.X - 25f) &&
                    (float)Duke.position.X < platform.Length + (platform.destinationRect.X - 25f) && platform.visible && Duke.velocity.Y > 0
                    && platform.platform)
                {
                    //do nothing, do no update the Y component, unless the platform is moving vertically.
                    
                    //jumpReady = true; //use the condition of dukes v.y component == 0.
                    //next line "auto-snaps" player to a platform.
                    Duke.position.Y = platform.destinationRect.Y - Duke.spriteHeight;//16 is for 64x64 sprite.
                    Duke.airborne = false;
                    if (platform.velocity.X != 0f)
                    {
                        platforming = true;//identifies player is on dynamic platform. 
                    }
                    
                    Duke.velocity.Y = 0f;
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

                if (platform.wall)
                {
                    //checks if duke is on the left boundary of a wall. OR right boundary
                    if ((Duke.position.X <= (platform.destinationRect.X) &&
                        Duke.position.X > (platform.destinationRect.X) - 64f &&
                        Duke.position.Y >= (platform.destinationRect.Y) &&
                    (float)Duke.position.Y < platform.Skin.Height - 1f + (platform.destinationRect.Y)
                        && Duke.velocity.X > 0) ||
                        (Duke.position.X >= (platform.destinationRect.X) &&
                        Duke.position.X < (platform.destinationRect.X + platform.Length) && Duke.velocity.X < 0 &&
                        Duke.position.Y >= (platform.destinationRect.Y) - 1f &&
                    (float)Duke.position.Y < platform.Skin.Height + (platform.destinationRect.Y)
                        && Duke.velocity.X < 0)
                        && platform.wall
                        && platform.visible)
                    {
                        //do nothing, do no update the Y component, unless the platform is moving vertically.

                        //jumpReady = true; //use the condition of dukes v.y component == 0.
                        //next line "auto-snaps" player to a platform.
                        if (Duke.velocity.X > 0)
                        {
                            Duke.position.X = platform.destinationRect.X - 64f;
                        }
                        else if (Duke.velocity.X < 0) { Duke.position.X -= Duke.velocity.X; }

                        //update his pose before you update the vector. still have to determine what to do on the zero case. 
                        if (Duke.velocity.X > 0) { Duke.pose = 0; }
                        else if (Duke.velocity.X < 0) { Duke.pose = 1; }






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
                    //determines if duke hit the bottom of this wall and provides appropriate feedback (ie. Duke hit the wallcube's ceiling
                    if (Duke.position.Y >= (platform.destinationRect.Y) &&
                        Duke.position.Y < platform.destinationRect.Y + 65f &&
                        Duke.position.X >= (platform.destinationRect.X - 25f) &&
                        (float)Duke.position.X < platform.Length + (platform.destinationRect.X - 25f) && platform.visible && Duke.velocity.Y < 0
                        && platform.platform
                        && platform.wall)
                    {
                        //do nothing, do no update the Y component, unless the platform is moving vertically.

                        //jumpReady = true; //use the condition of dukes v.y component == 0.
                        //next line "auto-snaps" player to a platform.
                        ////Duke.position.Y = platform.destinationRect.Bottom;//16 is for 64x64 sprite.
                        Duke.airborne = true;


                        Duke.velocity.Y = 0f;





                    }

                }

            }


            //apply the levitation power up here. 
            //if (Duke.levitation)
            //{

            //    Duke.airborne = false;

            //}LETS NOT AND APPLIE IT WHERE GRAVITY IS APPLIED. bc technically he is still airborne, but he is levitating. 
            

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
         * UpdateDoors()
         * Purpose: Keeps track of world navigation through implementing door traversal logic.
         * 
         * Class variables used: 
         * 
         *============================================== */

        public void Updatedoors(float backX, float backY, float gameTime)
        {
            foreach (Door door in doors[currentView])
            {
                door.Update(this.backgroundX, this.backgroundY, gameTime);
            }


            //===========================================
            //Duke.falling = true;//might be unnecessary.
            //jumpReady = false;
            //debug This never enters after I push the A button.
            //TODO: DEBUG why player is stuck with half of body in platform.

            foreach (Door door in doors[currentView])
            {

                if (Duke.position.Y >= (door.destinationRect.Y) &&
                    Duke.position.Y <= (door.destinationRect.Y) + 64f &&
                    Duke.position.X >= (door.destinationRect.X - 25f) &&
                    (float)Duke.position.X < door.Length + (door.destinationRect.X - 25f) &&
                    Duke.openDoor &&
                    door.unlock)
                {

                    // implement level change logic here. After demonstrating you can toggle two levels, start constructing a world, a basic world. 
                    //when building world, mind the door-based traversal. 
                    loadLevel = true;
                    if (level == 1) { level++; }
                    else if (level == 2) { level--; }
                    //warning, this is bad practice, just used to demonstrate that level construction and destruction is mastered. 

                }

            }
        }
        

        /*==============================================
        * updateEnemySpawn(GameTime gameTime)
        * Purpose: Determines spawning logic of varying enemies. 
        * To be used in the Update() method.
        * Class variables used: maxSpeed, Duke.velocity 
        * gameTime
        *============================================== */
        public void updateEnemySpawn(float seconds)
        {
            switch (level)
            {
                case 1:
                    //this.velocity.X = maxSpeed;
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
                    //=======insert overriding spawn logic (phases, boss battles, etc.)====
                    //level 1 GP1 == ALIENS
                    /*===========================================logic for basic temple run gameplay=================
                    if (seconds <= 5) { GPTimer1 = 0F;
                    GPInterval1 = 50000f / 25f;
                    Duke.velocity.X = maxSpeed; 
                    }
                    else if (seconds > 5f && seconds < 6f)
                    {
                        GPInterval1 = 10000f / 25f;
                        GPTimer1 = 0f;
                        Duke.velocity.X = maxSpeed;
                    }
                    else if (seconds > 30f && seconds < 31f && !Duke.allRangeMode)
                    {
                        GPInterval1 = 10000f / 35f;
                        GPTimer1 = 0f;
                        Duke.allRangeMode = true; 
                        //spawn first complex enemy here :3
                       enemies.Add(new un_Dead_Core(viewportRect.Width - 250f + backgroundX, 50f + backgroundY, 0f, 0f,
                       viewportRect.Width - 250f + backgroundX, 50f + backgroundY, 0f, 0f, backgroundX, backgroundY,
                       Content.Load<Texture2D>("Sprites\\ring"),
                       Content.Load<Texture2D>("Sprites\\unDeadCoreSmall"),
                       Content.Load<Texture2D>("Sprites\\unDeadCoreLarge")));

                    }
                    else if (seconds > 60f && seconds < 61f)
                    {
                        GPInterval1 = 10000f / 50f;
                        GPTimer1 = 0f;
                    }
                    else if (seconds > 90f && seconds < 91f)
                    {
                        GPInterval1 = 10000f / 100f;
                        GPTimer1 = 0f;
                    }
                    //=====================================================================
                    //=======enact spawning logic here=====================================
                    GPTimer1++;
                    if (GPTimer1 > GPInterval1)//possiblity of updateing the interval itself for advanced behavior control.
                    {
                        GPTimer1 = 0f;
                        enemies.Add(new Alien(viewportRect.Width - 64f + backgroundX, 100f + backgroundY, 0f, 0f,
                        viewportRect.Width - 64f + backgroundX, 100f + backgroundY, 0f, 0f, backgroundX, backgroundY));
                        numEnemies++;
                        enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\AlienSaucer"), 0, Content.Load<Texture2D>("Sprites\\AlienSaucer"));
                         
                    }
                    */
                    //=====================================================================

                    break;
                default:
                    //do nothing 
                    break;
            }
           

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
            //for updateing Duke's speed in phases. 
            //updateDukeSpeed((float)(timer/ 1000)); //there is only one Duke, A number one.
            updateEnemySpawn((float)(timer / 1000)); 
            //i fucked up animations
            //currentworkaround, call each type of derived class individually and determine how to fix this later. 
            foreach (GameObject FX in specialFX)
            {
                if (FX is energy_Gauge)
                {
                    FX.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY, Duke.energy, Duke.energyCapacity);//I don't want to do this, but I may pass Duke object...I REALLY don't want to do this. 
                }
                else if (FX is health_Gauge)
                {
                    FX.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY, Duke.health, Duke.healthCapacity);//I don't want to do this, but I may pass Duke object...I REALLY don't want to do this. 
                }
                else if (FX is shield_RDash)
                {
                    FX.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY, Duke.position);//I don't want to do this, but I may pass Duke object...I REALLY don't want to do this. 
                    if (Duke.shield == false)
                    {
                        FX.killEffect();
                    }
                    
                }
                else if (FX is flying_Nimbus)
                {
                    FX.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY, Duke.position);//I don't want to do this, but I may pass Duke object...I REALLY don't want to do this. 
                    if (!Duke.levitation)
                    {
                        FX.killEffect();
                    }

                }
                else
                {
                    FX.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY);//I don't want to do this, but I may pass Duke object...I REALLY don't want to do this. 

                }
            }
            foreach (Enemy enemy in enemies)
            {
                
                enemy.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, backgroundX, backgroundY);
                if (enemy.bulletReady)
                {
                    foreach (EnemyBullet ball in enemyCannonBalls)
                    {
                        if (!ball.alive)
                        {
                            enemy.setBulletReady(false);
                            ball.alive = true;
                            EnemyBullet tempBullet = new EnemyBullet();
                            Vector2 tempVector = Duke.position;
                            tempVector.X += backgroundX;
                            tempVector.Y += backgroundY;
                            tempBullet = enemy.fireBullet(tempVector, Duke.velocity);
                            ball.position = tempBullet.position;
                            ball.velocity = tempBullet.velocity;
                            if (enemy.visible)
                            {
                                enemyFireEffect.Play();
                            }
                            break; //use brerak for multiple enemies, vs the return for the main character. 
                        }
                    }
                }
            }
            
            
            if (numFX > 0)
            {
                listIndex = 0;
                expiredFX = new List<int>();
                foreach (GameObject FX in specialFX)
                {

                    
                    if (FX.ttl < 0)
                    {
                        expiredFX.Add(listIndex); 
                        //specialFX.RemoveAt(listIndex);
                        //numFX--;
                    }
                    listIndex++;
                    //break; //I don't want to do this, but in theory...no FX's should expire at the same time doing this...terrible programming 
                    //SCRATCH THAT....I'm a fucking genious.
                }
                listIndex = 0; 
                foreach (int index in expiredFX)
                {
                    specialFX.RemoveAt(index - listIndex);
                    numFX--;
                    listIndex++; 
                    
                         //lol wow, this is needed incase multiple fx's expire at the same time. 
                        //specialFX.ElementAt<GameObject>(numFX - 1).ttl = 1;
                    
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
            
            //reset all VFX-spawning booleans EACH GAME CYCLE
            //then check. 


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
            int fireCounter = 0;
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
                    ball.position.X += spriteWidth/2 + backgroundX;
                    ball.position.Y += Duke.mainSpriteSheet[0].Height/4 + backgroundY;
                    ball.velocity = new Vector2(
                        (float)Math.Cos(Duke.rotation) * 2f,
                        (float)Math.Sin(Duke.rotation)) * 10.0f;
                    beamSoundEffect.Play();


                    //adds the round's decaying energy field, the position of the ball is wrt the viewport, and does not take into account the backgroundX/Y. 
                    specialFX.Add(new bullet_Decay(ball.position.X, ball.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                    numFX++;
                    specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                    specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;
                    
                    return;
                }
            }
        }
        //=====================
        /// <summary>
        /// 
        /// 
        /// </summary>

        public void UpdateCannonBalls(GameTime gameTime)//will keep update method because it depends on the viewport and i do not want to pass it as a variable...am i lazy? 
        {
            foreach (Bullet ball in cannonBalls)
            {
                if (ball.alive)
                {
                    ball.update((float)gameTime.ElapsedGameTime.TotalMilliseconds, (float)this.backgroundX, (float)this.backgroundY); //float gameTime, float backgroundX, float backgroundY)
                    if (ball.bulletDecayReadyCheck())
                    {
                        specialFX.Add(new bullet_Decay(ball.position.X, ball.position.Y, 0f, 0f,
                        0f, 0f, 16f, backgroundX, backgroundY, 3));
                        numFX++;
                        specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\bullet_Decay"));
                        specialFX.ElementAt<VFX>(numFX - 1).ttl = 1;
                        specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;
                        
                    }
                    //ball.position += ball.velocity; done in the update method now. 
                    if (!(ball.position.X - backgroundX < 800 && ball.position.X - backgroundX > -64 &&
                ball.position.Y - backgroundY < 800 && ball.position.Y - backgroundY > -200))
                    {
                        ball.alive = false;
                        continue;
                    }
                    Rectangle cannonBallRect = new Rectangle(
                        (int)ball.position.X - (int)backgroundX,
                        (int)ball.position.Y - (int)backgroundY,
                        ball.sprite.Width,
                        ball.sprite.Height);
                    //ALL FOR EACH LOOPS MUST REFERENCE A LISTINDEX VAR INORDER TO REMOVE ITEMS FROM THE LIST. 
                    listIndex = 0;
                    foreach (Enemy enemy in enemies)
                    {
                        
                            Rectangle enemyRect = new Rectangle(
                                (int)enemy.position.X - (int)backgroundX,
                                (int)enemy.position.Y - (int)backgroundY,
                                enemy.spriteWidth,
                                enemy.mainSpriteSheet[enemy.pose].Height);

                            if (cannonBallRect.Intersects(enemyRect))
                            {

                                //===logic for special vfx===
                                /*public VFX(float initX, float initY, float initRotation, float initXCenter, float initYCenter, 
                float initXVelocity, float initYVelocity, float backgroundX, float backgroundY)
                                 * */
                                specialFX.Add(new Explosion2(ball.position.X, ball.position.Y, 0f, 0f,
                            0f, 0f, 0f, backgroundX, backgroundY, 3));
                                numFX++;
                                specialFX.ElementAt<VFX>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Explosion2"));
                                specialFX.ElementAt<VFX>(numFX - 1).ttl = 1;
                                specialFX.ElementAt<VFX>(numFX - 1).spriteInterval = 2000f / 25f;
                                specialFX.ElementAt<VFX>(numFX - 1).loadSoundEffect(enemyFireEffect); //variable should be a different variable, this is a dummy soundbite. 
                                specialFX.ElementAt<VFX>(numFX - 1).soundFX.Play();

                                //===========================
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
                                    numEnemies--;
                                    specialFX.Add(new Explosion1(enemy.position.X, enemy.position.Y, 0f, 0f,
                            0f, 0f, 0f, backgroundX, backgroundY, 3));
                                    numFX++;
                                    specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Explosion1"));
                                    specialFX.ElementAt<GameObject>(numFX - 1).ttl = 1;
                                    specialFX.ElementAt<GameObject>(numFX - 1).spriteInterval = 2000f / 25f;

                                    specialFX.Add(new killAlert(backgroundX + 100f, backgroundY + 200f, 0f, 0f,
                            0f, 0f, 0f, backgroundX, backgroundY, 1));
                                    randomCheer.ElementAt<SoundEffect>(randomNumber(1, 6)).Play();
                                    numFX++;
                                    specialFX.ElementAt<GameObject>(numFX - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\0Chain"));
                                    specialFX.ElementAt<GameObject>(numFX - 1).ttl = 8;
                                    specialFX.ElementAt<GameObject>(numFX - 1).spriteInterval = 2000f / 25f;

                                    Duke.energy += 20f;
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

            //this should be in an update method...TODO/ 
            foreach (EnemyBullet ball in enemyCannonBalls)
            {

                
                if (ball.alive)
                {
                    ball.update((float)gameTime.ElapsedGameTime.TotalMilliseconds, (float)this.backgroundX, (float)this.backgroundY);
                    // ball.position += ball.velocity; done in the update method now as per good programming practice. 
                    if (!(ball.position.X - backgroundX < 800 && ball.position.X - backgroundX > -64 &&
                ball.position.Y - backgroundY < 800 && ball.position.Y - backgroundY > -200))
                    {
                        ball.alive = false;//DESUDESU
                        continue;
                    }
                    Rectangle cannonBallRect = new Rectangle(
                        (int)ball.position.X - (int)this.backgroundX,
                        (int)ball.position.Y - (int)this.backgroundY,
                        ball.sprite.Width,
                        ball.sprite.Height);

                    Rectangle cannonRect = new Rectangle(
                        (int)Duke.position.X ,
                        (int)Duke.position.Y ,
                        Duke.spriteWidth,
                        Duke.mainSpriteSheet[0].Height);

                    if (cannonBallRect.Intersects(cannonRect))
                    {
                        ball.alive = false;
                        //Duke.alive = false;
                        Duke.hit = true;
                        hitCounter = 0;
                        ball.alive = false; //potentially add some megaman hit sound effect?
                        if (Duke.shield)
                        {
                            Duke.shield = false;
                        }
                        else { Duke.decrementHealth(10f); }

                        //inserte health decrement here. -= 1;
                        break;
                    }
 
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
            //PLATFORMS
            currentPlatform = new PlatformStatic(100f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform")));
            this.platforms[(int)View.Front] = new List<Platform>();
            this.platforms[(int)View.Right] = new List<Platform>();
            this.platforms[(int)View.Rear] = new List<Platform>();
            this.platforms[(int)View.Left] = new List<Platform>();
            this.platforms[0].Add(new PlatformStatic(100f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //TODO: Rearchitect pipeline to use dereferencing, not individually load a new texture, for memory. 
            if (!songstart)//checks if song started.
            {
                //MediaPlayer.Play(levelOneTheme);
                songstart = true;
            }
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            //experiment with wall

            this.platforms[(int)View.Front].Add(new WallCube(164f + 64f, 2036f, 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            enemies.Add(new TronDartGunDR(192f, 2000f, 0f, 0f,
                        192f, 2000f, 0f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\tronDartGunSheet"), 0, Content.Load<Texture2D>("Sprites\\enemyCannonBall"));


            //  *** END EXPERIMENT ***
            //experiment with enemies

            enemies.Add(new TronDart(192f, 2000f, 0f, 0f,
                        192f, 2000f, 0f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\tronDartSheet"), 0, Content.Load<Texture2D>("Sprites\\tronDartSheet"));

            enemies.Add(new TronBikeBlue(100f + (64f * 16f), 2200f - (64f * 4f) - 10f, 0f, 0f,
                        100f + (64f * 15f), 2200f - (64f * 4f) -10f, -5f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\TronBikeBlue"), 0);

            //  *** END EXPERIMENT ***
            this.platforms[(int)View.Front].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f * 2f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));

            this.platforms[(int)View.Front].Add(new WallCube(164f + 64f, 2036f, 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 9f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 10f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 11f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 13f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 14f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 15f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 16f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 17f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 14f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 17f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            //DATGAP

            
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 1f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 31f), 2200f , 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 32f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 33f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f *2f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 2f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 40f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 6f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 6f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            //corridor to the levitation room.
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 37f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 34f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 33f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 32f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 31f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 29f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f + (64f * 24f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            // *** END CORRIDOR ***

            // **Right Cube Corridor east of Fron Wall**


            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));

            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));

            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));


            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Front].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));




            // ** END CORRIDOR **

            //**"the great tron wall**

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 21f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            // ** END WALL **

            // **The great fron vert tunnel**
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Front].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));


            // ** END TUNNEL **

            // ** corridor north of TronWall East of Verttunnel

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 26f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 27f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));


            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 25f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 24f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 23f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 21f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            enemies.Add(new TronDartGunDR(100f + (64f * 25f), 2200f - (64f * 21f), 0f, 0f,
                        100f + (64f * 25f), 2200f - (64f * 21f), 0f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\tronDartGunSheet"), 0, Content.Load<Texture2D>("Sprites\\enemyCannonBall"));




            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 27f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 26f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            enemies.Add(new TronDart(100f + (64f * 32f), 2200f - (64f * 14f), 0f, 0f,
                        100f + (64f * 32f), 2200f - (64f * 14f), 0f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\tronDartSheet"), 0, Content.Load<Texture2D>("Sprites\\enemyCannonBall"));


            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Front].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            enemies.Add(new TronBikeBlue(100f + (64f * 34f), 2200f - (64f * 13f), 0f, 0f,
                        100f + (64f * 15f), 2200f - (64f * 4f) - 10f, -5f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\TronBikeBlue"), 0);
            enemies.Add(new TronBikeBlue(100f + (64f * 38f), 2200f - (64f * 13f), 0f, 0f,
                        100f + (64f * 15f), 2200f - (64f * 4f) - 10f, -5f, 0f, backgroundX, backgroundY));
            numEnemies++;
            enemies.ElementAt<Enemy>(numEnemies - 1).loadSpriteSheet(Content.Load<Texture2D>("Sprites\\TronBikeBlue"), 0);

            // ** END CORRIDOR **

            // ** start here **
            // ** ---------- **
            // ** right facet**

            //  *** END EXPERIMENT ***
            this.platforms[(int)View.Right].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f * 2f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));

            this.platforms[(int)View.Right].Add(new WallCube(164f + 64f, 2036f, 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 9f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 10f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 11f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 13f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 14f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 15f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 16f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 17f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 14f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 17f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            //DATGAP


            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 1f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 31f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 32f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 33f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 2f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 2f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 40f), 2200f - (64f * 4f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 6f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 6f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            //corridor to the levitation room.
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 37f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 34f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 33f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 32f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 31f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 29f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(100f + (64f * 24f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            // *** END CORRIDOR ***

            // **Right Cube Corridor east of Fron Wall**


            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));

            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));

            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));


            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 30f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 29f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 28f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 27f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 26f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));
            this.platforms[(int)View.Right].Add(new RightCube(100f + (64f * 25f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronRightCube"))));




            // ** END CORRIDOR **

            //**"the great tron wall**

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 21f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 24f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            // ** END WALL **

            // **The great fron vert tunnel**
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 8f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 9f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));

            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 23f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 22f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 21f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 20f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 19f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 18f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 17f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));
            this.platforms[(int)View.Right].Add(new VertCube(100f + (64f * 16f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronVertCube"))));


            // ** END TUNNEL **

            // ** corridor north of TronWall East of Verttunnel

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 26f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 27f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));


            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 26f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 25f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 24f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 23f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 22f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 21f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 16f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 14f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 20f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 19f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 17f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 18f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

           




            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 29f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 28f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 27f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 26f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 25f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 30f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 15f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            


            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 39f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 38f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 37f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 36f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 35f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 34f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 32f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 31f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));
            this.platforms[(int)View.Right].Add(new WallCube(100f + (64f * 33f), 2200f - (64f * 13f), 64f, (Content.Load<Texture2D>("Sprites\\TronBlueWall"))));

            

            // ** END CORRIDOR **


            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f * 2f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));

            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 9f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 10f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 11f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 13f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 14f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 15f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 16f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 17f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 21f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 1f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 31f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 32f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 33f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            //corridor to the levitation room.
            
            
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 29f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 24f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            // *** END CORRIDOR ***
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 37f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 40f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 41f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 42f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 43f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 44f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 45f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 46f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 47f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 48f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 49f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));



            this.platforms[(int)View.Left].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f * 2f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformRed"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 9f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 10f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 11f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 13f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 14f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 15f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 16f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 17f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 21f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 3f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 1f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 31f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 32f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f + (64f * 33f), 2200f, 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            //corridor to the levitation room.


            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 30f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 29f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 28f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 27f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 26f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 25f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 24f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            // *** END CORRIDOR ***
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 35f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 36f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 37f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 38f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 39f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 40f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 41f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 42f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 43f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 44f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 45f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 46f), 2200f - (64f * 7f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 47f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 48f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 49f), 2200f - (64f * 11f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 44f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 43f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 42f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 41f), 2200f - (64f * 10f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(100f + (64f * 40f), 2200f - (64f * 12f), 64f, (Content.Load<Texture2D>("Sprites\\TronPlatformBlue"))));
            // **** END PLATFORMS ****


            // BEGIN DOORS 

            this.doors[(int)View.Front] = new List<Door>();
            this.doors[(int)View.Right] = new List<Door>();
            this.doors[(int)View.Rear] = new List<Door>();
            this.doors[(int)View.Left] = new List<Door>();

            this.doors[(int)View.Front].Add(new DoorSmall(100f, 2200f - 64f, 64f, (Content.Load<Texture2D>("Sprites\\DoorSmallTronBlue"))));

            // **** END DOORS ****

            //GPFlag1 = false;
            //GPFlag2 = false;
            //GPFlag3 = false;
            //GPFlag4 = false;
            //GPFlag5 = false;
            //GPFlag6 = false;
            //GPFlag7 = false;
            //GPFlag8 = false;


            ////================================
            ///*
            // * Enemy Placement
            // * 
            // * follow design document, if provided.
            // * 
            // * 
            // * */
            ////===============================
            ////List<int> listint = new List<int>();
            ////TODO: replace hardcode with platform references

            //enemies.Add(new Ninja(5, 360f, 1900f, 0f, 0f, 360f, 1900f, 0f, 1f, 64, 64));
            //enemies.Add( new Ninja(5, 360f*2, 1900f, 0f, 0f, 360f*2, 1900f, 0f, 1f, 64, 64));
            //enemies.Add(new Ninja(5, 360f*3, 1900f, 0f, 0f, 360f*3, 1900f, 0f, 1f, 64, 64));
            //enemies.Add(new Ninja(5, 360f*4, 1900f, 0f, 0f, 360f*4, 1900f, 0f, 1f, 64,64));
            //enemies.Add(new Ninja(5, 360f*5, 1900f, 0f, 0f, 360f*5, 1900f, 0f, 1f, 64, 64));
            ////understand polymorphism more you stupid :D
            
            //foreach (Enemy enemy in enemies)//loading sprite sheets
            //{
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 0, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 1, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 2, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 3, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            //    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 4, Content.Load<Texture2D>("Sprites\\ninjaBullet"));
            //}
            ////S1Aenemies = new Stage1EnemyA[5];

            ////S1Aenemies[0] = new Stage1EnemyA(5, 460f, 1900f, 1.57f, 0f, 360f, 1900f, 0f, 1f, 64);
            ////S1Aenemies[1] = new Stage1EnemyA(5, 460f * 2, 1900f, 1.57f, 0f, 360f * 2, 1900f, 0f, 1f, 64);
            ////S1Aenemies[2] = new Stage1EnemyA(5, 460f * 3, 1900f, 1.57f, 0f, 360f * 3, 1900f, 0f, 1f, 64);
            ////S1Aenemies[3] = new Stage1EnemyA(5, 460f * 4, 1900f, 1.57f, 0f, 360f * 4, 1900f, 0f, 1f, 64);
            ////S1Aenemies[4] = new Stage1EnemyA(5, 460f * 5, 1900f, 1.57f, 0f, 360f * 5, 1900f, 0f, 1f, 64);
            ////foreach (Stage1EnemyA enemy in S1Aenemies)//loading sprite sheets
            ////{
            ////    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 0);
            ////    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 1);
            ////    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 2);
            ////    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 3);
            ////    enemy.loadSpriteSheet(Content.Load<Texture2D>("Sprites\\Enemy"), 4);
            ////}

            

            //// public Enemy(int numSprites, float initX, float initY, float initRotation, float initFireTimer,
            //// float initXCenter, float initYCenter, float initXVelocity, float initYVelocity, int SpriteSize)
            
            
            ////objects are already created when game is loaded, so
            //// first set how many you want alive.
            //numPlatforms = 15;//was 25;
            //if (!songstart)//checks if song started.
            //{
            //    MediaPlayer.Play(MMTheme);
            //    songstart = true;
            //}
            ////next set the other array elements as not alive for later logic.
            //////static platforms.TODO same for dynamic platforms. 
            //////for (int i = 0; i < maxPlatforms; i++)
            //////{
            //////    if (i < numPlatforms)
            //////    {
            //////        this.platforms[i].visible = true;//initializes the platforms
            //////        this.platformsDynamic[i].visible = true;
            //////        this.platforms[i].Skin = (Content.Load<Texture2D>(
            //////        "Sprites\\Platform"));
            //////    }
            //////    else
            //////    {
            //////        this.platforms[i].visible = false;
            //////        this.platformsDynamic[i].visible = false;
            //////    }
            //////}

            ////first, concentrate on only placeing platforms...
            ////currently i have 15 platforms to play with...
            ////position vectors must be consitent throughout gameplay, and all operations
            ////are based off an off set, which is altered by user input/player mobility.
            //this.platforms.Add(new PlatformStatic(0f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f*1, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 2, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 3, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 4, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 5, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 6, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 7, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 8, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 9, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 10, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 11, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 12, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 13, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 14, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 15, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 16.5f, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 8, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 9, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 10, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 11, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //this.platforms.Add(new PlatformStatic(320f * 12, 2160f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //////this.platforms[0].position = new Vector2(0f, 2300f);
            //////this.platforms[1].position = new Vector2(platforms[0].Length * 1, 2300f);
            //////this.platforms[2].position = new Vector2(platforms[0].Length * 2, 2300f);
            //////this.platforms[3].position = new Vector2(platforms[0].Length * 3, 2300f);
            //////this.platforms[4].position = new Vector2(platforms[0].Length * 4, 2300f);
            //////this.platforms[5].position = new Vector2(platforms[0].Length * 4, 2300f);
            //////this.platforms[6].position = new Vector2(platforms[0].Length * 5, 2300f);
            //////this.platforms[7].position = new Vector2(platforms[0].Length * 5, 2300f);
            //////this.platforms[8].position = new Vector2(platforms[0].Length * 6, 2300f);
            //////this.platforms[9].position = new Vector2(platforms[0].Length * 7, 2300f);
            //////this.platforms[10].position = new Vector2(platforms[0].Length * 8, 2300f);
            //////this.platforms[11].position = new Vector2(platforms[0].Length * 9, 2300f);
            //////this.platforms[12].position = new Vector2(platforms[0].Length * 3, 2220f);
            //////this.platforms[13].position = new Vector2(platforms[0].Length * 4, 2220f);
            //////this.platforms[14].position = new Vector2(platforms[0].Length * 5, 2220f);
            ////========================================================================================
            ////initial seed dx dy need to match scale%$^%$#^#%$^%$#^6563

            ////to uncomment next 3 lines after configureing platform mechanics to work with panLock. 
            ////this.platforms.Add(new PlatformDynamic(360f * 13, 2100f, 360f, true, true, 0f, -5f, true, true, 1, 5));
            //////this.platforms.Add(new PlatformDynamic(360f * 14, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            ////this.platforms.Add(new PlatformDynamic(360f * 15, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 1, 1700f, 360f, true, true, 0f, -8f, true, true, 1, 8));
            //this.platforms.Add(new PlatformDynamic(360f * 2, 1500f, 360f, true, true, 0f, 0f, true, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 3, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 4, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 5, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 6, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 7, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 15, 2300f, 360f, true, true, 0f, -2.5f, true, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 16, 2300f, 360f, true, true, -2.5f, -2.5f, true, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 17, 2400f, 360f, true, true, 0f, 0f, true, false, 1, 5));//first two bools is dynamic x/y, and second pair is x+/y+
            //this.platforms.Add(new PlatformDynamic(360f * 18, 2300f, 360f, true, true, -4f, 4f, true, true, 1, 8)); //makes a CW circle
            //this.platforms.Add(new PlatformDynamic(360f * 19, 2300f, 360f, true, true, 4f, -4f, true, true, 1, 8)); //makes CCW circle
            //this.platforms.Add(new PlatformDynamic(360f * 20, 2300f, 360f, true, true, 0f, -1.25f, false, false, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 22, 2300f, 360f, true, true, 0f, -1.25f, true, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 9, 1800f, 360f, true, true, 0f, -5f, false, true, 1, 5));
            //this.platforms.Add(new PlatformDynamic(360f * 3, 1800f, 360f, true, true, 0f, 0f, true, false, 1, 5));
            ////this.platformsDynamic[1] = new PlatformDynamic(platforms[0].Length * 6, 2100f, 360f, true, true, 0f, -5f, true, true, 1, 5);
            ////this.platformsDynamic[2] = new PlatformDynamic(platforms[0].Length * 7, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            ////this.platformsDynamic[3] = new PlatformDynamic(platforms[0].Length * 8, 2100f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            ////this.platformsDynamic[4] = new PlatformDynamic(platforms[0].Length * 1, 1700f, 360f, true, true, 0f, -8f, true, true, 1, 8);
            ////this.platformsDynamic[5] = new PlatformDynamic(platforms[0].Length * 2, 1500f, 360f, true, true, 0f, 0f, true, true, 1, 5);
            ////this.platformsDynamic[6] = new PlatformDynamic(platforms[0].Length * 3, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5);
            ////this.platformsDynamic[7] = new PlatformDynamic(platforms[0].Length * 4, 1900f, 360f, true, true, 0f, 0f, true, false, 1, 5);
            ////this.platformsDynamic[8] = new PlatformDynamic(platforms[0].Length * 5, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5);
            ////this.platformsDynamic[9] = new PlatformDynamic(platforms[0].Length * 6, 1900f, 360f, true, true, 0f, -2.5f, false, false, 1, 5);
            ////this.platformsDynamic[10] = new PlatformDynamic(platforms[0].Length * 7, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5);
            ////this.platformsDynamic[11] = new PlatformDynamic(platforms[0].Length * 8, 1900f, 360f, true, true, 0f, -2.5f, true, true, 1, 5);
            ////this.platformsDynamic[12] = new PlatformDynamic(platforms[0].Length * 9, 2300f, 360f, true, true, 0f, -5f, false, true, 1, 5);
            ////this.platformsDynamic[13] = new PlatformDynamic(platforms[0].Length * 3, 1800f, 360f, true, true, 0f, 0f, true, false, 1, 5);


            //foreach(Platform platform in platforms)
            //{

            //    platform.visible = true;
            //    platform.Skin = (Content.Load<Texture2D>("Sprites\\Platform"));
            //}
            //////for (int i = 0; i < maxPlatforms; i++)
            //////{

            //////    if (i < numPlatforms)
            //////    {
            //////        platformsDynamic[i].visible = true;
            //////        platformsDynamic[i].Skin = (Content.Load<Texture2D>(
            //////        "Sprites\\Platform"));
            //////    }
            //////    else
            //////    {
            //////        platformsDynamic[i].visible = false;
            //////    }


            //    //this.platformsDynamic[0] = new PlatformDynamic(platforms[0].Length * 5, 2220f, 360f, true, true, 0f, -1f, true, true, 10, 200);
            //    //public PlatformDynamic(float XDefault, float YDefault, float initLength, bool initYDynamic, bool initXDynamic,
            //    //float initdx, float initdy, bool initXPositive, bool initYPositive, int initSpeed, int initScale)

            //    //this.platforms[15].position = new Vector2(platforms[0].Length * 1, 200f);
            //    //this.platforms[16].position = new Vector2(platforms[0].Length * 1, 200f);
            //    //this.platforms[17].position = new Vector2(platforms[0].Length * 1, 200f);
            //    //this.platforms[16].position = new Vector2(platforms[0].sprite.Width * 13, 4400);
            //    //this.platforms[16].vertical = true;
            //    //this.platforms[16].horizontal = false;
            //    //this.platforms[16].dy = -4f;
            //    //this.platforms[16].up = false;
            //    //this.platforms[17].position = new Vector2(platforms[0].sprite.Width * 14, 4400);
            //    //this.platforms[17].vertical = true;
            //    //this.platforms[17].horizontal = true;
            //    //this.platforms[17].dy = -4f;
            //    //this.platforms[17].dx = +6f;
            //    //this.platforms[17].up = false;
            //    //this.platforms[17].right = true;
            //    //this.platforms[18].position = new Vector2(platforms[0].sprite.Width * 6, 4300);
            //    //this.platforms[19].position = new Vector2(platforms[0].sprite.Width * 7, 4200);
            //    //this.platforms[20].position = new Vector2(platforms[0].sprite.Width * 5.5f, 4100);
            //    //this.platforms[21].position = new Vector2(platforms[0].sprite.Width * 16 - 20, 4450);
            //    //this.platforms[21].vertical = false;
            //    //this.platforms[21].horizontal = true;
            //    //this.platforms[21].dy = -4f;
            //    //this.platforms[21].dx = 0f;
            //    //this.platforms[21].up = false;
            //    //this.platforms[21].right = true;
            //    //this.platforms[22].position = new Vector2(platforms[0].sprite.Width * 18 - 20, 4450);
            //    //this.platforms[22].vertical = false;
            //    //this.platforms[22].horizontal = true;
            //    //this.platforms[22].dy = +4f;
            //    //this.platforms[22].dx = 0f;
            //    //this.platforms[22].up = false;
            //    //this.platforms[22].right = false;
            //    //this.platforms[23].position = new Vector2(platforms[0].sprite.Width * 4.5f, 4000);
            //    //this.platforms[24].position = new Vector2(platforms[0].sprite.Width * 3.5f, 4100);
            //    //this.platforms[25].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4100);
            //    //this.platforms[25].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4400);
            //    //this.platforms[26].position = new Vector2(platforms[0].sprite.Width * .5f, 4400);
            //    //this.platforms[27].position = new Vector2(platforms[0].sprite.Width * 1.5f, 4350);
            //    //this.platforms[28].position = new Vector2(platforms[0].sprite.Width * 2.5f, 4150);
            //    //this.platforms[29].position = new Vector2(platforms[0].sprite.Width * 1f, 4250);
            

        }
        public void loadLevelTwo()
        {
            //PLATFORMS
            currentPlatform = new PlatformStatic(100f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform")));
            //RESETS ALL PLATFORM LISTS;
            this.platforms[(int)View.Front] = new List<Platform>();
            this.platforms[(int)View.Right] = new List<Platform>();
            this.platforms[(int)View.Rear] = new List<Platform>();
            this.platforms[(int)View.Left] = new List<Platform>();
            //END LIST RESETS. 
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f, 2300f, 320f, (Content.Load<Texture2D>("Sprites\\Platform"))));
            //TODO: Rearchitect pipeline to use dereferencing, not individually load a new texture, for memory. 
            if (!songstart)//checks if song started.
            {
                MediaPlayer.Play(levelOneTheme);
                songstart = true;
            }
            this.platforms[(int)View.Front].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Front].Add(new PlatformStatic(228f + 64f * 2f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));

            this.platforms[(int)View.Right].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f * 3f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Right].Add(new PlatformStatic(228f + 64f * 3f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));

            this.platforms[(int)View.Rear].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f * 4f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Rear].Add(new PlatformStatic(228f + 64f * 4f, 2000f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));

            this.platforms[(int)View.Left].Add(new PlatformStatic(100f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(164f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f * 2f, 2200f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f, 2100f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            this.platforms[(int)View.Left].Add(new PlatformStatic(228f + 64f * 5f, 2000f, 64f, (Content.Load<Texture2D>("Sprites\\Frontcloud"))));
            // **** END PLATFORMS ****


            // BEGIN DOORS 

            this.doors[(int)View.Front] = new List<Door>();
            this.doors[(int)View.Right] = new List<Door>();
            this.doors[(int)View.Rear] = new List<Door>();
            this.doors[(int)View.Left] = new List<Door>();

            this.doors[(int)View.Front].Add(new DoorSmall(100f, 2200f - 64f, 64f, (Content.Load<Texture2D>("Sprites\\DoorSmallTronBlue"))));

            // **** END DOORS ****
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
            /* REMOVES THE CONDITIONALS FROM THE SIDESCROLLING SHOOTER LEGACY CODE.
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
             */
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
            /*
             * Background texture will be based on an offset. Offset must be an infinite loop.
             * 
             * */

            /*
           spriteBatch.Draw(backgroundTexture, 
               new Rectangle(0, 0, viewportRect.Width, viewportRect.Height), //Destination Rectangle (100% of Viewport.
               new Rectangle((int) (backgroundX/500), 0, 
                   (int) ((backgroundTexture.Width*.55) + (backgroundX/500)), 
                   (int) backgroundTexture.Height), //source rectangle (80% of source X, 100%source Y).
               Color.White);
            
           spriteBatch.Draw(backgroundTextureC,
               new Rectangle(0,
                   viewportRect.Height - backgroundTextureC.Height - 300, //flush with bottom of sprite and viewport. 
                   viewportRect.Width, (int)(backgroundTextureC.Height*2)), //Destination Rectangle (100% of Viewport.
               new Rectangle((int)(backgroundX / 250), 0,
                   (int)((backgroundTextureC.Width * .40) + (backgroundX / 250)),
                   (int)backgroundTextureC.Height), //source rectangle (80% of source X, 100%source Y).
               Color.White);

           spriteBatch.Draw(backgroundTextureB,
               new Rectangle(0,
                   viewportRect.Height - backgroundTextureC.Height + (int)(backgroundTextureC.Height*2) - 360, //flush with bottom of sprite and viewport. 
                   viewportRect.Width, backgroundTextureB.Height), //Destination Rectangle (100% of Viewport.
               new Rectangle((int)(backgroundX / 50), 0,
                   (int)((backgroundTextureB.Width * .30) + (backgroundX / 50)),
                   (int)backgroundTextureB.Height), //source rectangle (80% of source X, 100%source Y).
               Color.White);
            
            
              */
            //BEGIN EXPERIMENTING WITH 3D==========================
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Vector3 realPosition = modelPosition;
                    realPosition.X -= backgroundX;
                    realPosition.Y += backgroundY;
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(realPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            //END EXPERIMENTING WITH 3D====================================
           


           // spriteBatch.Draw(backgroundTexture, viewportRect,
             //   Color.White);

            foreach (Bullet ball in cannonBalls)
            {
                if (ball.alive)
                {
                    Vector2 tempVect = new Vector2(ball.position.X - backgroundX, ball.position.Y - backgroundY); 
                    spriteBatch.Draw(ball.sprite,
                        tempVect, Color.White);
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
                        new Vector2(ball.position.X - backgroundX, ball.position.Y - backgroundY), Color.White);
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
            //TODO, design specific instances of the cloud, and do not allow the size to be created too freely. need MANY varieties of clouds
            Vector2 currentPlatform = new Vector2(0f, 0f);
            foreach (Platform platform in platforms[currentView])
            {
               if (platform.visible)
                {
                    currentPlatform = new Vector2(platform.destinationRect.X,
                        platform.destinationRect.Y + 36f);
                    
                    spriteBatch.Draw(platform.Skin, platform.destinationRect,
                     platform.sourceRect, Color.White);
                   //spriteBatch.Draw(platform.Skin,
                   //    currentPlatform, Color.White);

                }
            }
            foreach (Door door in doors[currentView])
            {
                if (door.visible)
                {
                    currentPlatform = new Vector2(door.destinationRect.X,
                        door.destinationRect.Y + 36f);

                    spriteBatch.Draw(door.Skin, door.destinationRect,
                     door.sourceRect, Color.White);
                    //spriteBatch.Draw(platform.Skin,
                    //    currentPlatform, Color.White);

                }
            }


            if (Duke.pose == 0)//faceing right.
            {
                //spriteBatch.Draw(Duke.mainSpriteSheet[0], new Vector2(Duke.position.X + (2 * spriteWidth),
                //    Duke.position.Y), Duke.sourceRect, Color.White,
                //    Duke.rotation,
                //    Duke.center, 1.0f, SpriteEffects.None, 0);
                spriteBatch.Draw(Duke.mainSpriteSheet[0], new Vector2(Duke.position.X, Duke.position.Y), Duke.sourceRect, Color.White);


            }
            else if (Duke.pose == 1)//put conditions for whether he faces left or right.
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
            foreach (VFX FX in specialFX)
            {

                spriteBatch.Draw(FX.mainSpriteSheet, new Rectangle((int)(FX.position.X - backgroundX), (int)(FX.position.Y - backgroundY), FX.spriteWidth, FX.spriteHeight),
                    FX.sourceRect, Color.White);
                if (FX is cursorWorldPlacement)
                {
                    spriteBatch.Draw(FX.mainSpriteSheet, new Rectangle((int)(FX.position.X - backgroundX), (int)(FX.position.Y - backgroundY), FX.spriteWidth, FX.spriteHeight),
                    FX.sourceRect, Color.White);
                }

            }
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


            //*/
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
