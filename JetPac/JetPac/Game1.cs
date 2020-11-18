/////////////////////////////////////////////////////////
//         JetPac - Written by Ian Wigley              //
//           Monogame re-write Nov 2020                //
//    Static values removed & general code clean up    //
//       Project migrated to VSCode & .NETCore         //
/////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace JetPac
{
    enum GameState
    {
        gameStart,
        gameOn,
        gameOver,
        takeOff,
        nextLevel
    }

    public class Game1 : Game
    {
        private int m_rocket1X = 422;
        private int m_rocket1Y = 443;
        private int m_rocket2X = 110;
        private int m_rocket2Y = 139;
        private int m_rocket3X = 510;
        private int m_rocket3Y = 75;
        private int m_score = 0;
        private int m_lives = 3;
        private int m_level = 0;
        private int m_fuelLevel = 0;
        private int m_currentFrame = 0;
        private int x = 150;
        private int y = 300;
        private const int rocketLowerPosition = 422;
        private const int fuelLowerPosition = 430;

        private GameState gameState = GameState.gameOver;
        private readonly GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private SpriteFont m_font;
        private Texture2D m_menuTexture;
        private Texture2D m_jetmanTexture;
        private Texture2D m_rocketTexture;
        private Texture2D m_floorTexture;
        private Texture2D m_ledge1Texture;
        private Texture2D m_ledge2Texture;
        private Texture2D m_explosionTexture;
        private Texture2D m_particleTexture;
        private Texture2D m_starTexture;
        private Texture2D m_enemyTexture;
        private Texture2D m_bulletTexture;
        private Texture2D m_fuelTexture;
        private Texture2D m_bonusTexture;

        private SoundEffect died;
        private SoundEffect fire;
        private SoundEffect hit;

        private Vector2 scoreLocation = new Vector2(11.0f, 11.0f);
        private Vector2 rocketLocation1;
        private Vector2 rocketLocation2;
        private Vector2 rocketLocation3;

        private SpriteEffects flip = SpriteEffects.None;

        private bool m_onGround = false;
        private bool m_fuelAdded = false;
        private bool m_fuelLowering = false;
        private bool m_walking = false;

        private bool m_firstSectionLowering = false;
        private bool m_firstSectionComplete = false;
        private bool m_secondSectionLowering = false;
        private bool m_secondSectionComplete = false;

        private double m_animationTimer = 0;
        private double m_gameStartingDelay = 0;
        private double m_delayCounter = 0;
        private const double m_elapsedCounter = 0.1;

        private List<Enemy> m_enemies = new List<Enemy>();
        private List<Star> m_stars = new List<Star>();
        private List<Particle> m_particles = new List<Particle>();
        private List<Bullet> m_bullets = new List<Bullet>();
        private List<Rocket> m_rockets = new List<Rocket>();
        private List<Fuel> m_fuel = new List<Fuel>();
        private List<Ledge> m_ledge = new List<Ledge>();
        private List<Bonus> m_bonus = new List<Bonus>();
        private List<Explosion> m_explosion = new List<Explosion>();

        private Jetman m_jetman;

        public Game1()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 600;
            m_graphics.ApplyChanges();

            rocketLocation1 = new Vector2(m_rocket1X, m_rocket1Y);
            rocketLocation2 = new Vector2(m_rocket2X, m_rocket2Y);
            rocketLocation3 = new Vector2(m_rocket3X, m_rocket3Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            m_font = Content.Load<SpriteFont>("font");
            m_bonusTexture = Content.Load<Texture2D>("bonus");
            m_bulletTexture = Content.Load<Texture2D>("bullet");
            m_enemyTexture = Content.Load<Texture2D>("enemies");
            m_explosionTexture = Content.Load<Texture2D>("explosion");
            m_floorTexture = Content.Load<Texture2D>("floor");
            m_fuelTexture = Content.Load<Texture2D>("fuel");
            m_jetmanTexture = Content.Load<Texture2D>("jetman");
            m_ledge1Texture = Content.Load<Texture2D>("ledge1");
            m_ledge2Texture = Content.Load<Texture2D>("ledge2");
            m_menuTexture = Content.Load<Texture2D>("loading");
            m_particleTexture = Content.Load<Texture2D>("particle");
            m_rocketTexture = Content.Load<Texture2D>("rocket");
            m_starTexture = Content.Load<Texture2D>("star");

            died = Content.Load<SoundEffect>("died");
            fire = Content.Load<SoundEffect>("fire");
            hit = Content.Load<SoundEffect>("hit");

            m_jetman = new Jetman(150, 300, m_jetmanTexture);

            m_rockets.Add(new Rocket((int)rocketLocation1.X, (int)rocketLocation1.Y, m_rocketTexture, 0, 75, 61));
            m_rockets.Add(new Rocket((int)rocketLocation2.X, (int)rocketLocation2.Y, m_rocketTexture, 4, 75, 61));
            m_rockets.Add(new Rocket((int)rocketLocation3.X, (int)rocketLocation3.Y, m_rocketTexture, 8, 75, 61));

            m_ledge.Add(new Ledge(60, 200, m_ledge1Texture));
            m_ledge.Add(new Ledge(310, 265, m_ledge2Texture));
            m_ledge.Add(new Ledge(490, 136, m_ledge1Texture));
            m_ledge.Add(new Ledge(0, 500, m_floorTexture));

            m_bonus.Add(new Bonus(m_bonusTexture));

            for (int i = 0; i < 10; i++)
            {
                m_enemies.Add(new Enemy(m_enemyTexture));
            }

            for (int i = 0; i < 40; i++)
            {
                m_stars.Add(new Star(m_starTexture));
                m_particles.Add(new Particle(m_particleTexture));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            CheckKeyboard();

            if (gameState == GameState.gameStart)
            {
                m_gameStartingDelay += m_elapsedCounter;
                if (m_gameStartingDelay > 10)
                {
                    gameState = GameState.gameOn;
                }
            }

            y += 1;

            foreach (Star star in m_stars)
            {
                star.Update();
            }

            foreach (Enemy alien in m_enemies)
            {
                alien.Update(m_level);
            }

            var tempBullet = new List<Bullet>();
            foreach (Bullet bullet in m_bullets)
            {
                if (!bullet.Offscreen)
                {
                    bullet.Update();
                    tempBullet.Add(bullet);
                }
            }
            m_bullets = tempBullet;

            var tempExplosion = new List<Explosion>();
            foreach (Explosion explosion in m_explosion)
            {
                if (!explosion.AnimationComplete)
                {
                    explosion.Update();
                    tempExplosion.Add(explosion);
                }
            }
            m_explosion = tempExplosion;

            foreach (Bonus bonus in m_bonus)
            {
                bonus.Update();
            }

            CheckRocketCollsions();
            LowerRocketSections();
            CheckBulletCollisions();
            CheckEnemyCollisions();
            CheckBonusCollisions();
            CheckJetManLedgeCollsions();

            foreach (Particle particle in m_particles)
            {
                if (m_fuelLevel < 100)
                {
                    particle.Update(x, y, flip != SpriteEffects.None, !m_onGround);
                }
                else
                {
                    particle.Update(rocketLowerPosition + 36, m_rockets[0].RocketRect.Y + 36, false, true);
                }
            }

            if (m_secondSectionComplete && !m_fuelAdded)
            {
                AddFuel();
                m_fuelAdded = true;
            }

            bool nextFuel = false;
            foreach (Fuel fuel in m_fuel)
            {
                fuel.Update(m_ledge);
                CheckFuelCollisions(fuel);
                if (m_fuelLowering)
                {
                    if (fuel.LowerFuel())
                    {
                        if (m_fuelLevel < 100)
                        {
                            m_fuelLevel += 25;
                            m_fuelLowering = false;
                            nextFuel = true;
                        }
                    }
                }
            }
            if (nextFuel)
            {
                AddFuel();
            }
            if (m_lives < 1)
            {
                gameState = GameState.gameOver;
            }
            if (gameState == GameState.takeOff)
            {
                foreach (Rocket rocket in m_rockets)
                {
                    if (!rocket.TakeOff())
                    {
                        m_level++;
                        gameState = GameState.nextLevel;
                    }
                }
            }
            if (gameState == GameState.nextLevel)
            {
                ResetLevel();
                gameState = GameState.gameOn;
            }

            base.Update(gameTime);
        }

        private void CheckBulletCollisions()
        {
            Bullet bulletToRemove = null;
            foreach (Enemy alien in m_enemies)
            {
                foreach (Bullet bullet in m_bullets)
                {
                    if (bullet.BulletRect.Intersects(alien.AlienRect))
                    {
                        m_explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, m_explosionTexture));
                        alien.ResetMeteor();
                        died.Play();
                        bulletToRemove = bullet;
                        m_score += 100;
                        break;
                    }
                }
            }
            if (bulletToRemove != null)
            {
                m_bullets.Remove(bulletToRemove);
            }
        }

        private void CheckEnemyCollisions()
        {
            foreach (Enemy alien in m_enemies)
            {
                if (alien.AlienRect.Intersects(m_jetman.JetmanRect) && gameState == GameState.gameOn)
                {
                    m_explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, m_explosionTexture));
                    alien.ResetMeteor();
                    died.Play();
                    m_lives--;
                    break;
                }

                foreach (Ledge ledge in m_ledge)
                {
                    if (alien.AlienRect.Intersects(ledge.LedgeRect))
                    {
                        m_explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, m_explosionTexture));
                        alien.ResetMeteor();
                        hit.Play();
                    }
                }
            }
        }

        private void CheckBonusCollisions()
        {
            foreach (Bonus bonus in m_bonus)
            {
                foreach (Ledge ledge in m_ledge)
                {
                    if (bonus.BonRect.Intersects(ledge.LedgeRect))
                    {
                        bonus.BonusLanded = true;
                    }
                }

                if (m_jetman.JetmanRect.Intersects(bonus.BonRect))
                {
                    bonus.Reset();
                    m_score += 100;
                }
            }
        }

        private void CheckRocketCollsions()
        {
            if (m_jetman.JetmanRect.Intersects(m_rockets[1].RocketRect) && !m_firstSectionLowering)
            {
                if ((int)m_jetman.JetmanPosition.X != rocketLowerPosition)
                {
                    m_rockets[1].Update((int)m_jetman.JetmanPosition.X, (int)m_jetman.JetmanPosition.Y);
                }
                else
                {
                    m_firstSectionLowering = true;
                }
            }

            if (m_jetman.JetmanRect.Intersects(m_rockets[2].RocketRect) && m_firstSectionComplete && !m_secondSectionLowering)
            {
                if ((int)m_jetman.JetmanPosition.X != rocketLowerPosition)
                {
                    m_rockets[2].Update((int)m_jetman.JetmanPosition.X, (int)m_jetman.JetmanPosition.Y);
                }
                else
                {
                    m_secondSectionLowering = true;
                }
            }
        }

        private void CheckJetManLedgeCollsions()
        {
            foreach (Ledge ledge in m_ledge)
            {
                if (m_jetman.JetmanRect.Intersects(ledge.LedgeRect))
                {
                    if (m_jetman.JetmanRect.Bottom - 3 == ledge.LedgeRect.Top ||
                        m_jetman.JetmanRect.Bottom - 2 == ledge.LedgeRect.Top ||
                        m_jetman.JetmanRect.Bottom - 1 == ledge.LedgeRect.Top)
                    {
                        y -= 1;
                        m_onGround = true;
                        if (!m_walking)
                        {
                            m_currentFrame += 1;
                            m_walking = true;
                        }
                    }
                    else
                    {
                        m_onGround = false;
                    }
                    if (m_jetman.JetmanRect.Top + 1 == ledge.LedgeRect.Bottom)
                    {
                        y += 2;
                    }
                    if (m_jetman.JetmanRect.Right - 2 == ledge.LedgeRect.Left)
                    {
                        x -= 2;
                    }
                    if (m_jetman.JetmanRect.Left + 1 == ledge.LedgeRect.Right)
                    {
                        x += 2;
                    }
                }
            }
        }

        private void CheckFuelCollisions(Fuel fuel)
        {
            if (m_jetman.JetmanRect.Intersects(fuel.FuelRect))
            {
                if ((int)m_jetman.JetmanPosition.X != fuelLowerPosition && !m_fuelLowering)
                {
                    fuel.UpdatePosition((int)m_jetman.JetmanPosition.X, (int)m_jetman.JetmanPosition.Y);
                }
                else
                {
                    m_fuelLowering = true;
                }
            }
        }

        private void LowerRocketSections()
        {
            // Lower the first rocket section into place
            if (m_firstSectionLowering && !m_firstSectionComplete)
            {
                m_firstSectionComplete = m_rockets[1].LowerSectionOne();
            }

            // Lower the second rocket section into place
            if (m_secondSectionLowering && !m_secondSectionComplete)
            {
                m_secondSectionComplete = m_rockets[2].LowerSectionTwo();
            }

        }

        private void AddFuel()
        {
            m_fuel.Clear();
            if (m_fuelLevel != 100)
            {
                m_fuel.Add(new Fuel(m_fuelTexture));
            }
            else
            {
                gameState = GameState.takeOff;
                m_score += 100;
            }
        }

        private void CheckScreenBounds()
        {
            if (y <= 50) { y = 50; }
            if (y >= 550) { y = 550; }

            if (x <= 0) { x = 0; }
            if (x >= 750) { x = 750; }
        }

        private void CheckKeyboard()
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (gameState == GameState.gameOver)
                {
                    gameState = GameState.gameStart;
                    ResetGame();
                }
            }

            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (!m_onGround)
                {
                    y += 2;
                    CheckScreenBounds();
                    m_currentFrame = 0;
                }
            }

            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                y -= 2;
                CheckScreenBounds();
                m_onGround = false;
                m_walking = false;
                m_currentFrame = 0;
            }

            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                flip = SpriteEffects.FlipHorizontally;
                x -= 2;
                m_animationTimer += m_elapsedCounter;
                CheckScreenBounds();
                if (m_onGround && m_animationTimer > 0.4)
                {
                    m_currentFrame = (m_currentFrame + 1) % 4;
                    if (m_currentFrame == 0) { m_currentFrame = 1; }
                    m_animationTimer = 0;
                }
            }

            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                flip = SpriteEffects.None;
                x += 2;
                m_animationTimer += m_elapsedCounter;
                CheckScreenBounds();
                if (m_onGround && m_animationTimer > 0.4)
                {
                    m_currentFrame = (m_currentFrame + 1) % 4;
                    if (m_currentFrame == 0) { m_currentFrame = 1; }
                    m_animationTimer = 0;
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                fire.Play();
                m_delayCounter += m_elapsedCounter;
                if (m_delayCounter > 0.4)
                {
                    if (flip == SpriteEffects.None)
                    {
                        m_bullets.Add(new Bullet(x, m_jetmanTexture.Height / 2 + y, m_bulletTexture, false));
                    }
                    else
                    {
                        m_bullets.Add(new Bullet(x, m_jetmanTexture.Height / 2 + y, m_bulletTexture, true));
                    }
                    m_delayCounter = 0;
                }
            }

            m_jetman.JetmanAnimFrame = m_currentFrame;
            m_jetman.Update(x, y, flip);
        }

        private void ResetGame()
        {
            m_score = 0;
            m_lives = 3;
            m_level = 0;
            m_fuelLevel = 0;
            m_currentFrame = 0;
            x = 150;
            y = 300;
            ResetLevel();
        }

        private void ResetLevel()
        {
            x = 150;
            y = 300;
            m_rocket1X = 422;
            m_rocket1Y = 443;
            m_rocket2X = 110;
            m_rocket2Y = 139;
            m_rocket3X = 510;
            m_rocket3Y = 75;
            m_fuelLevel = 0;
            m_fuelAdded = false;
            m_fuelLowering = false;
            m_firstSectionLowering = false;
            m_firstSectionComplete = false;
            m_secondSectionLowering = false;
            m_secondSectionComplete = false;
            rocketLocation1 = new Vector2(m_rocket1X, m_rocket1Y);
            rocketLocation2 = new Vector2(m_rocket2X, m_rocket2Y);
            rocketLocation3 = new Vector2(m_rocket3X, m_rocket3Y);
            m_rockets.Clear();
            int frame = m_level % 4;
            m_rockets.Add(new Rocket((int)rocketLocation1.X, (int)rocketLocation1.Y, m_rocketTexture, frame, 75, 61));
            m_rockets.Add(new Rocket((int)rocketLocation2.X, (int)rocketLocation2.Y, m_rocketTexture, frame + 4, 75, 61));
            m_rockets.Add(new Rocket((int)rocketLocation3.X, (int)rocketLocation3.Y, m_rocketTexture, frame + 8, 75, 61));
            foreach (Enemy alien in m_enemies)
            {
                alien.ResetMeteor();
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (gameState == GameState.gameOver)
            {
                m_spriteBatch.Begin();
                m_spriteBatch.Draw(m_menuTexture, new Vector2(0, 10), Color.White);
                m_spriteBatch.DrawString(m_font, "JetPac Remake (Monogame Update)", new Vector2(150.0f, 440.0f), Color.Yellow);
                m_spriteBatch.DrawString(m_font, "Written by Ian Wigley", new Vector2(150.0f, 460.0f), Color.Yellow);
                m_spriteBatch.DrawString(m_font, "In 2020", new Vector2(150.0f, 480.0f), Color.Yellow);
                m_spriteBatch.DrawString(m_font, "Press X to start", new Vector2(150.0f, 520.0f), Color.Yellow);
                m_spriteBatch.End();
            }
            else
            {
                m_spriteBatch.Begin();
                m_spriteBatch.DrawString(m_font, "SCORE : " + m_score.ToString("D4"), scoreLocation, Color.Yellow);
                m_spriteBatch.DrawString(m_font, "FUEL : " + m_fuelLevel + "%", scoreLocation + new Vector2(350.0f, 1.0f), Color.Yellow);
                m_spriteBatch.DrawString(m_font, "LIVES : " + m_lives, scoreLocation + new Vector2(700.0f, 1.0f), Color.Yellow);

                foreach (Star star in m_stars)
                {
                    star.Draw(m_spriteBatch);
                }

                foreach (Enemy alien in m_enemies)
                {
                    alien.Draw(m_spriteBatch);
                }

                foreach (Explosion explosion in m_explosion)
                {
                    explosion.Draw(m_spriteBatch);
                }

                foreach (Bullet bullet in m_bullets)
                {
                    bullet.Draw(m_spriteBatch);
                }

                foreach (Fuel fuel in m_fuel)
                {
                    fuel.Draw(m_spriteBatch);
                }

                foreach (Bonus bonus in m_bonus)
                {
                    bonus.Draw(m_spriteBatch);
                }

                foreach (Rocket rocket in m_rockets)
                {
                    rocket.Draw(m_spriteBatch);
                }

                foreach (Ledge ledge in m_ledge)
                {
                    ledge.Draw(m_spriteBatch);
                }

                m_jetman.Draw(m_spriteBatch);
                m_spriteBatch.End();

                foreach (Particle particle in m_particles)
                {
                    particle.Draw(m_spriteBatch);
                }
            }

            base.Draw(gameTime);
        }
    }
}
