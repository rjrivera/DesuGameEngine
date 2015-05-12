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

    public class Door
    {
        public Vector2 position, velocity;
        public float Length;
        public Texture2D Skin;
        public Rectangle sourceRect, destinationRect;// 
        public int numSprites;
        public bool visible;
        public bool unlock = true;
        public float destX, destY, destBackgroundX, destBackgroundY, destLevel; //used as primitive level-linker. ought to use a data type. 
        //NOTE: levels are arrays of lists, essentially. Consider making a list of door_node data types for standardization purposes. 
        public Door()
        {

        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault.
      * */
        public Door(float XDefault, float YDefault)
        {

        }
        /*
      * PlatformStatic constructer
      * parameters: XDefault, YDefault, Skin Sprite Sheet.
      * */
        public Door(float XDefault, float YDefault, float initLength, Texture2D SkinDefault)
        {

        }
        /*
     * PlatformStatic constructer
     * parameters: XDefault, YDefault, Skin Sprite Sheets, initDiameter, initPointOnDiameter
     * */
        public Door(float XDefault, float YDefault, float initLength, Texture2D SkinDefault, float initDiameter, float initPointOnDiameter)
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