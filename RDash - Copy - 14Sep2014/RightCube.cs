///  Title:  RightCube Class
///  Author: Rob Rivera
///  For: Desu
///  Description: provides a structure platforming mechanic in game. To be used for level design, minimal art needed.
///  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace RDash
{
    class RightCube : Platform
    {

        float spriteTimer;
        float spriteInterval;
        int currentFrame;

        /*
       * PlatformStatic constructer
       * parameters: None
       * */
        public RightCube()
        {
            position.X = 0;
            position.Y = 0;
            Length = 360;
            Skin = null;
            visible = false;
            currentFrame = 0;
            spriteTimer = 0f;
            platform = false;
            wall = false;
        }
        /*
      * RightCube constructer
      * parameters: XDefault, YDefault.
      * */
        public RightCube(float XDefault, float YDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = 360f;
            Skin = null;
            visible = true;
            spriteTimer = 0f;
            currentFrame = 0;
            platform = false;
            wall = false;
        }

        /*
       * RightCube constructer
       * parameters: XDefault, YDefault, Skin Sprite Sheet.
       * */
        public RightCube(float XDefault, float YDefault, float initLength, Texture2D SkinDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = initLength;
            Skin = SkinDefault;
            visible = true;
            spriteTimer = 0f;
            currentFrame = 0;
            spriteInterval = 2000f / 25f;
            numSprites = Skin.Width / (int)initLength;
            platform = false;
            wall = false;
        }
        //TODO:create mutator methods and data access methods.
        public override void Update(float backX, float backY, float gameTime)
        {
            //update the destination Rectangle here for all platforms. 
            this.destinationRect = new Rectangle((int)(position.X - backX), (int)(position.Y - backY), (int)Length, (int)(Skin.Height));

            //update any sprites here. 
            float deltaTime = gameTime;
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

                }
                sourceRect = new Rectangle(currentFrame * (int)Length, 0, (int)Length, this.Skin.Height);
            }


        }

        public bool setNumSprites(int numberSprites)
        {
            numSprites = numberSprites;

            return true;
        }

        public bool setSpriteInterval(float newSpriteInterval)
        {
            spriteInterval = newSpriteInterval;

            return true;
        }
    }
}
