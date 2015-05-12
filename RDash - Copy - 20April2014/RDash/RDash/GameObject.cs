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
    public class GameObject
    {
        public Vector2 position;
        public float rotation, gravityAccel, ttl, spriteInterval;//dx and dy used for varying platform.
        public Vector2 center;
        public Vector2 velocity, acceleration;
        public bool visible, airborne;
        public Rectangle sourceRect;

        public GameObject()
        {

        }

        public GameObject(Texture2D loadedTexture)
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            gravityAccel = 10f;
            airborne = true;
        }

        public virtual void loadSpriteSheet(Texture2D Sprite)
        {

        }
        public virtual void loadSpriteSheet(Texture2D Sprite, int index, Texture2D extraSheet)
        {

        }


        public virtual void Update(float gameTime) { }
        public virtual void Update(float gameTime, float backgroundX, float backgroundY) { }
        public virtual void Update(float gameTime, float backgroundX, float backgroundY, float energy, float energyCapacity) { }
        public virtual void Update(float gameTime, float backgroundX, float backgroundY, Vector2 dukePosition) { }

        public virtual void killEffect() { }

        
    }
}
