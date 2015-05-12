///  Title:  Platform Base Class
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

    public class Platform
    {
        public Vector2 position, velocity;
        public float Length;
        public Texture2D Skin;
        public Rectangle sourceRect, destinationRect;// 
        public int numSprites; 
        public bool visible, wall, platform;
    public Platform()
        {
            
        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault.
      * */
        public Platform(float XDefault, float YDefault)
        {
            
        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault, Skin Sprite Sheet.
      * */
        public Platform(float XDefault, float YDefault, float initLength, Texture2D SkinDefault)
        {  

        }
         /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault, Skin Sprite Sheets, initDiameter, initPointOnDiameter
      * */
        public Platform(float XDefault, float YDefault, float initLength, Texture2D SkinDefault, float initDiameter, float initPointOnDiameter)
        {
            

        }
        public virtual void Update(float backX, float backY, float gameTime)
        {

        }
        //public virtual void Detection(Player player)
        //{

        //}
    }
}