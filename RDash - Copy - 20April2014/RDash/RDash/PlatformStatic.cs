///  Title:  PlatformDynamic Class
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
    class PlatformStatic : Platform
    {
        
        
          /*
         * PlatformStatic constructer
         * parameters: None
         * */
        public PlatformStatic()
        {
            position.X = 0;
            position.Y = 0;
            Length = 360;
            Skin = null;
            visible = false;
        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault.
      * */
        public  PlatformStatic(float XDefault, float YDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = 360f;
            Skin = null;
            visible = true;
        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault, Skin Sprite Sheet.
      * */
        public PlatformStatic(float XDefault, float YDefault, float initLength, Texture2D SkinDefault)
        {
            position.X = XDefault;
            position.Y = YDefault;
            Length = initLength;
            Skin = SkinDefault;
            visible = true;

        }
        //TODO:create mutator methods and data access methods.
        public override void Update()
        {

        }
    }
}
