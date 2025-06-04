using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        bool isGameOver;

     
        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;

       
        Vector2 pl1BatPosition;
        Vector2 pl2BatPosition;
        Vector2 batSize = new Vector2(20, 100);
        Texture2D batTexture;

        private SpriteFont myFont;


        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(
                _graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2
            );

            ballSpeed = 200f;
            ballSpeedVector = new Vector2(1, -1);
            ballSpeedVector.Normalize();

            
            pl1BatPosition = new Vector2(30, _graphics.PreferredBackBufferHeight / 2 - batSize.Y / 2);
            pl2BatPosition = new Vector2(_graphics.PreferredBackBufferWidth - 50, _graphics.PreferredBackBufferHeight / 2 - batSize.Y / 2);

            isGameOver = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");

            
            batTexture = new Texture2D(GraphicsDevice, 1, 1);
            batTexture.SetData(new[] { Color.White });
            myFont = Content.Load<SpriteFont>("MyFont"); 
            

        }

        private void checkBallCollision()
        {
            Rectangle ballRect = new Rectangle(
                (int)(ballPosition.X - ballTexture.Width / 2),
                (int)(ballPosition.Y - ballTexture.Height / 2),
                ballTexture.Width,
                ballTexture.Height
                
            );

            Rectangle bat1Rect = new Rectangle(pl1BatPosition.ToPoint(), batSize.ToPoint());
            Rectangle bat2Rect = new Rectangle(pl2BatPosition.ToPoint(), batSize.ToPoint());

            
            if (ballRect.Intersects(bat1Rect) || ballRect.Intersects(bat2Rect))
            {
                ballSpeedVector.X = -ballSpeedVector.X;
            }

            if (ballPosition.Y < ballTexture.Height / 2 ||
                ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballSpeedVector.Y = -ballSpeedVector.Y;
            }

            
            if (ballPosition.X < 0 || ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                isGameOver = true;
            }
        }

        private void updateBallPosition(float updatedBallSpeed)
        {
            ballPosition += ballSpeedVector * updatedBallSpeed;
        }

        private void updateBatsPositions()
        {
            float batSpeed = 5f;
            var kstate = Keyboard.GetState();

            
            if (kstate.IsKeyDown(Keys.W))
                pl1BatPosition.Y -= batSpeed;
            if (kstate.IsKeyDown(Keys.S))
                pl1BatPosition.Y += batSpeed;

            
            if (kstate.IsKeyDown(Keys.Up))
                pl2BatPosition.Y -= batSpeed;
            if (kstate.IsKeyDown(Keys.Down))
                pl2BatPosition.Y += batSpeed;

            
            pl1BatPosition.Y = Math.Clamp(pl1BatPosition.Y, 0, _graphics.PreferredBackBufferHeight - batSize.Y);
            pl2BatPosition.Y = Math.Clamp(pl2BatPosition.Y, 0, _graphics.PreferredBackBufferHeight - batSize.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isGameOver)
            {
                float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                updateBallPosition(updatedBallSpeed);
                checkBallCollision();
                updateBatsPositions();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            
            _spriteBatch.Draw(batTexture, new Rectangle(pl1BatPosition.ToPoint(), batSize.ToPoint()), Color.Red);
            _spriteBatch.Draw(batTexture, new Rectangle(pl2BatPosition.ToPoint(), batSize.ToPoint()), Color.Blue);
            if (isGameOver )
            {
                

            }
            _spriteBatch.End();

            base.Draw(gameTime);


        }
    }
}
