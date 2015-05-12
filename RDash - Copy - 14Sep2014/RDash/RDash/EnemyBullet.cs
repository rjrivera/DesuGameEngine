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
    public class EnemyBullet
    {
        public Texture2D sprite, morphSprite, jumper;
        public Vector2 position;
        public float rotation, fireTimer, dx, dy, accelMax, spawnBackgroundX, spawnBackgroundY, deltaTime, decayTimer, decayTimerInterval;//dx and dy used for varying platform.
        public Vector2 center;
        public Vector2 velocity, acceleration;
        public bool alive, visible, bulletDecayReady;
        public Rectangle sourceRect;
        public int accelCount;//Time TO LIVE concerned with items which could expire.
        public EnemyBullet(Texture2D loadedTexture)
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
            accelMax = 10f;
            accelCount = 0;
            bulletDecayReady = false;
            decayTimerInterval = 4000f / 25f;
            decayTimer = 0f;
            
        }
        public EnemyBullet()
        {
            rotation = 0.0f;
            position = Vector2.Zero;
            
            //center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
            accelMax = 10f;
            accelCount = 0;
        }
        public void update(float gameTime, float backgroundX, float backgroundY)
        {
            if (spawnBackgroundX == 0)
            {

                spawnBackgroundX = backgroundX;
                spawnBackgroundY = backgroundY;

            }
            else { }
            if (this.bulletDecayReady) { this.bulletDecayReady = false; } //resets bulletDecay boolean after one game cycle check. 
            deltaTime = gameTime;
            decayTimer += deltaTime;
            if (decayTimer > decayTimerInterval)//possiblity of updateing the interval itself for advanced behavior control.
            {
                decayTimer = 0f;

                //=========
                //redefine decaySpawnStatus. 
                //=========
                this.bulletDecayReady = true;
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public Boolean bulletDecayReadyCheck()
        {

            return this.bulletDecayReady;
        }
    }
}