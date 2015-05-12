///  Title:  PlatformDynamic Class
///  Author: Rob Rivera
///  For: Desu
///  Description: provides a structure enemy design and control in game. To be used for AI design, minimal art needed.
///  Enemies will utilize a class system, no polymorphism will be used for simplifying the main class. 
///  enemy positions will be based on the universal map, not the viewport (like the main character).
///  because each enemy will have unique timing for performance and habits, timing variables will be enemy.class variables...
///  NOTE: consider redesigning character class if enemy class proves to be superior.
///  controls ONLY enemy movement and enemy bullet generation. bullet behavior will be BASED off of enemy action but will be its own class.
///  enemyBullet class will by variable of enemy class but is NOT to be used in main DESU! class.
///  
/// Definition of enemy classes to be done at level load. each instance will NOT carry information about any other enemy class except the 
/// class concerned. 
/// 
/// enemy behaviors to be determined in the update() method. update method will utilize said timings. 
/// 
/// IMPORTANT, to make this effective with memory, each enemy class array must be unallocated memory since they will NEVER be reused after a particular zone.
/// IMPORTANT, can use different sprite count if sprite timer is done in class updater method.
/// 
/// TODO: THIS IS THE VERSION 0 OF ENEMY CLASS, USE AS TEMPLATE FOR FUTURE ENEMIES. MAY MANIPULATE ANYTHING.
/// 
/// 
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
    public class Enemy : Player
    {
        //a few common values
        public Texture2D[] mainSpriteSheet;//should be three, but open. defined in main (size).
        public Vector2 position;//global position.
        public float rotation, gravityAccel, fireTimer, deltaTime, enemyTimer, enemyInterval;//rotion done in radians if i remember.
        public Vector2 center;
        public Vector2 velocity;
        public int pose, numPoses, currentFrame, spriteWidth, spriteHeight, health, healthCapacity, numSprites;//determines which sprite sheet to use based on position. updated via user input.
        //pose guide: right, left,
        public bool alive, fire, airborne, hit, ignoresWalls, visible;
        public bool boss = false;
        public bool bulletReady = false;
        public Rectangle sourceRect, destRect;//determines size of base sprite. should stay consistent but is flexible.
        
        
        public Enemy()
        {

        }

        public virtual void loadSpriteSheet(Texture2D Sprite, int index)
        {
            
        }
        public virtual void Update(float gameTime, float backgroundX, float backgroundY)
        {

        }
        public virtual void loadSpriteSheet(Texture2D Sprite, int index, Texture2D extraSheet)
        {

        }
        public virtual void setBulletReady(bool setBullet)
        {

        }
        public virtual void reverseVelocityX()
        {

        }
        public virtual List<VFX> getListVFX()
        {
            return null;
        }
        public virtual EnemyBullet fireBullet(Vector2 VectorPosition, Vector2 VectorVelocity)
        {
            return null; 
        }
    }
}
