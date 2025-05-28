using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballSpeedVector;
        float ballSpeed;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                       _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 200f;

            // Начална посока: диагонално нагоре и надясно
            ballSpeedVector = new Vector2(1, -1);
            ballSpeedVector.Normalize();
            ballSpeedVector *= ballSpeed;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Преместваме топката
            ballPosition += ballSpeedVector * deltaTime;

            // Проверка за сблъсък с лявата и дясната страна
            if (ballPosition.X <= ballTexture.Width / 2)
            {
                ballPosition.X = ballTexture.Width / 2;
                ballSpeedVector.X = -ballSpeedVector.X;
            }
            else if (ballPosition.X >= _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
                ballSpeedVector.X = -ballSpeedVector.X;
            }

            // Проверка за сблъсък с горната и долната страна
            if (ballPosition.Y <= ballTexture.Height / 2)
            {
                ballPosition.Y = ballTexture.Height / 2;
                ballSpeedVector.Y = -ballSpeedVector.Y;
            }
            else if (ballPosition.Y >= _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
                ballSpeedVector.Y = -ballSpeedVector.Y;
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
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
