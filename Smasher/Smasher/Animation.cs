using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Smasher
{
    public class Animation 
    {
        private Texture2D texture;
        private Texture2D killTexture;
        private Rectangle rectangle;
        private Vector2 originalPosition;
        private Vector2 position;
        private Vector2 moveVelocity;
        private BoundingBox boundingBox;
      
        private int frameHeight;
        private int frameWidth;
        private int frameHeightKill;
        private int frameWidthKill;
        private int currentFrame;
        private int currentFrameKill;
        private int frameCount;
        private int frameCountKill;
        private int velocity;

        private float timer;
        private float timerKill;
        private int intervalR;
        private int interval;
        private int intervalKill;
        private int killInterval;

        private bool isWalk;

        public bool IsWalk
        {
            get { return isWalk; }
            set { isWalk = value; }
        }

        public BoundingBox Bounding
        {
            get { return boundingBox; }       
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Animation(Texture2D newTexture, Texture2D newKillTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth,int newFrameHeightKill, int newFrameWidthKill, int newInterval, int newVelocity, int newKillInterval)
        {
            texture = newTexture;
            killTexture = newKillTexture;
            position = newPosition; 
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            frameHeightKill = newFrameHeightKill;
            frameWidthKill = newFrameWidthKill;
            interval = newInterval;
            velocity = newVelocity;
            killInterval = newKillInterval;
        }

        public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            originalPosition = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            Right(gameTime, 9, interval);
            Move(gameTime);
            boundingBox.Min = new Vector3(position.X - 35, position.Y - 54, 0);
            boundingBox.Max = new Vector3(position.X + 35, position.Y + 55, 0);            
        }

        public void UpdateKill(GameTime gameTime)
        {
            rectangle = new Rectangle(currentFrameKill * frameWidthKill, 0, frameWidthKill, frameHeightKill);
            originalPosition = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            RightKill(gameTime, 7, killInterval);
        }

        public void Right(GameTime gameTime, int newFrameCount, int newInterval)
        {
            intervalR = newInterval;
            frameCount = newFrameCount;
            timer += (float) gameTime.ElapsedGameTime.TotalMilliseconds/2;
            if (timer > intervalR)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > frameCount)
                {
                    currentFrame = 0;
                }                  
            }
        }

        public void RightKill(GameTime gameTime, int newFrameCountKill, int newIntervalKill)
        {
            intervalKill = newIntervalKill;
            frameCountKill = newFrameCountKill;
            timerKill += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timerKill > intervalKill)
            {
                currentFrameKill++;
                timerKill = 0;
                if (currentFrameKill > frameCountKill)
                {
                    currentFrameKill = 0;
                    isWalk = true;
                }

            }
        }

        public void Move(GameTime gameTime)
        {
            if(position.X < 700)
                position.X += velocity;
            else
                position.X = 100;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, originalPosition, 1.0f, SpriteEffects.None, 0);
        }

        public void DrawKill(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(killTexture, position, rectangle, Color.White, 0f, originalPosition, 1.0f, SpriteEffects.None, 0);
        }
    }
}
