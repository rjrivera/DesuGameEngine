///  Title:  PlatformDynamic Class
///  Author: Rob Rivera
///  For: Desu
///  Description: provides a structure platforming mechanic in game. To be used for level design, minimal art needed. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RDash
{
    class PlatformDynamic : Platform
    {
        public bool visible;
        public float dx;//Describes from -1.0-1.0 how far o the displacement schedule the platform is. 
        public float dy;//Describes from -1.0-1.0 how far o the displacement schedule the platform is. 
        public bool XPositive;//Describes if the Xvariable is increasing
        public bool YPositive;//Describes if the Yvariable is increasing
        public bool XDynamic, YDynamic;//enables isolation of certain range movement.
        public float Speed;//defines the speed of the platforms movement (pixels per game second).
        public float Scale;//defines the scale of the platform movement.
        
        /*
       * PlatformDynamic constructer
       * parameters: None
       * */
        public PlatformDynamic()
        {
            position.X = 100;
            position.Y = 100;
            Length = 256;
            Skin = null;
        }
        /*
      * PlatformDynamic constructer
      * parameters: XDefault, YDefault.
      * */
        public PlatformDynamic(float XDefault, float YDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = 256;
            Skin = null;
        }
      
        /*
      * PlatformDynamic constructer
      * parameters: XDefault, YDefault, Skin Sprite Sheet, 
         * public int position.X
        public int position.Y
        public int Length
        public Texture2D Skin
        public bool XDynamic
        public bool YDynamic
        public float dx
        public float dy
        public bool XPositive
        public bool YPositive
        public int Speed
        public int Scale
      * */
        public PlatformDynamic(float XDefault, float YDefault, float initLength, bool initYDynamic, bool initXDynamic,
            float initdx, float initdy, bool initXPositive, bool initYPositive, int initSpeed, int initScale)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = initLength;
            XDynamic = initXDynamic;
            YDynamic = initYDynamic;
            dx = initdx;
            dy = initdy;
            XPositive = initXPositive;
            YPositive = initYPositive;
            Speed = initSpeed;
            Scale = initScale;
            visible = true;

        }

        public override void Update(float backX, float backY, float gameTime)
        {
            this.position.X += velocity.X;
            this.position.Y += velocity.Y;
            if (this.visible && this.YDynamic)
            {//update vertical platforming dynamics.
                if (this.dy <= -1f * (float)this.Scale)
                {
                    this.YPositive = true;
                }
                else if (this.dy >= 1f * (float)this.Scale)
                {
                    this.YPositive = false;
                }

                if (this.YPositive)
                {
                    this.dy += .1f;
                }
                else { this.dy -= .1f; }
            }
            if (this.visible && this.XDynamic)
            {//update horizontal platforming dynamics.
                if (this.dx >= 1f * (float)this.Scale)
                {
                    this.XPositive = false;
                }
                else if (this.dx <= -1f * (float)this.Scale)
                {
                    this.XPositive = true;
                }
                if (this.XPositive)
                {
                    this.dx += .1f;
                }
                else { this.dx -= .1f; }
            }
            this.velocity.X = (this.dx * this.Speed);
            this.velocity.Y = (this.dy * this.Speed);
            
            
        }
        /*
    * PlatformDynamic constructer
    * parameters: XDefault, YDefault, Skin Sprite Sheet.
    * */
        public PlatformDynamic(float XDefault, float YDefault, float initLength, Texture2D SkinDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = initLength;
            Skin = SkinDefault;
            visible = true;
        }
        //TODO:create mutator methods and data access methods.
    }
}
