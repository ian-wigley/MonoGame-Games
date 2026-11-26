//////////////////////////////////////////////////////////
//                                                      //
//         JetPacReloaded - Written by Ian Wigley       //
//                                                      //
//////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JetPacReloaded;

internal enum GameState
{
    GameStart,
    GameOn,
    GameOver,
    TakeOff,
    NextLevel
}

public class Game1 : Game
{
    private readonly List<Bonus> _bonus = [];
    private readonly List<Enemy> _enemies = [];
    private readonly List<Fuel> _fuel = [];
    private readonly GraphicsDeviceManager _graphics;
    private readonly List<Ledge> _ledge = [];
    private readonly List<Particle> _particles = [];
    private readonly List<Rocket> _rockets = [];
    private readonly List<Star> _stars = [];
    private List<Bullet> _bullets = [];
    private List<Explosion> _explosion = [];
    
    private SoundEffect _died;
    private SoundEffect _fire;
    private SoundEffect _hit;

    private SpriteBatch _spriteBatch;
    private SpriteEffects _flip = SpriteEffects.None;
    private SpriteFont _font;
    private GameState _gameState = GameState.GameOn;
    private GraphicsManager _graphicsManager;
    private RenderTarget2D _mainTarget;
    
    private Camera _camera;
    private JetMan _jetMan;

    private Effect _lightingEffect;
    private RenderTarget2D _lightsTarget;
   
    private Texture2D _lightMask;
    private Texture2D _jetManTexture;
    private Texture2D _jetManMask;
    private Texture2D _bonusTexture;
    private Texture2D _laserTexture;
    private Texture2D _mEnemyTexture;
    private Texture2D _mExplosionTexture;
    private Texture2D _mFuelTexture;
    private Texture2D _mParticleTexture;
    private Texture2D _mRocketTexture;
    private Texture2D _mRustyTexture;
    private Texture2D _mStarTexture;
    private Texture2D _texture;
    private Texture2D _undergroundTiles;
    private Texture2D _menuTexture;
    
    private bool _drawnWithLighting;
    private bool _mFirstSectionComplete;
    private bool _mFirstSectionLowering;
    private bool _mFuelAdded;
    private bool _mFuelLowering;
    private bool _mOnGround;
    private bool _mSecondSectionComplete;
    private bool _mSecondSectionLowering;
    private bool _mWalking;
    private bool _wallCollision;

    private double _mAnimationTimer;
    private double _mDelayCounter;
    private double _mGameStartingDelay;    
    private const double ElapsedCounter = 0.1;
    
    private const int ScrollX = 0;
    private const int ScrollY = 0;
    private const int RocketLowerPosition = 422;
    private const int FuelLowerPosition = 430;

    private int _level;
    private int _lives = 3000;
    private int _rocket1X = 2000; //422;
    private int _rocket1Y = 850; //443;
    private int _rocket2X = 110;
    private int _rocket2Y = 139;
    private int _rocket3X = 510;
    private int _rocket3Y = 75;
    private int _score;
    private int _x = 400;
    private int _y = 300;
    private int _currentFrame;
    private int _fuelLevel;
    
    private Vector2 _rocketLocation1;
    private Vector2 _rocketLocation2;
    private Vector2 _rocketLocation3;
    private Vector2 _jetManPosition;
    private readonly Vector2 _scoreLocation = new(10, 10);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024; // 800;
        _graphics.PreferredBackBufferHeight = 768; // 600;
        _graphics.ApplyChanges();

        _rocketLocation1 = new Vector2(_rocket1X, _rocket1Y);
        _rocketLocation2 = new Vector2(_rocket2X, _rocket2Y);
        _rocketLocation3 = new Vector2(_rocket3X, _rocket3Y);
        _jetManPosition = new Vector2(_x, _y);

        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData([Color.DarkSlateGray]);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("font");
        _bonusTexture = Content.Load<Texture2D>("bonus");
        _laserTexture = Content.Load<Texture2D>("bullet");
        _mEnemyTexture = Content.Load<Texture2D>("enemies");
        _mExplosionTexture = Content.Load<Texture2D>("explosion");
        _mFuelTexture = Content.Load<Texture2D>("fuel");


        // _jetManTexture = Content.Load<Texture2D>("man");
        _jetManTexture = Content.Load<Texture2D>("jetman");

        // _menuTexture = Content.Load<Texture2D>("loading");
        _mParticleTexture = Content.Load<Texture2D>("particle");
        _mRocketTexture = Content.Load<Texture2D>("rocket");
        _mStarTexture = Content.Load<Texture2D>("star");

        _undergroundTiles = Content.Load<Texture2D>("undergroundTiles");
        _mRustyTexture = Content.Load<Texture2D>("rusty");
        
        _jetManMask = Content.Load<Texture2D>("playermask");
        _lightMask = Content.Load<Texture2D>("lightmask");
        _lightingEffect = Content.Load<Effect>("Effect1");
        
        var pp = GraphicsDevice.PresentationParameters;
        _lightsTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
        _mainTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

        // died = Content.Load<SoundEffect>("died");
        // fire = Content.Load<SoundEffect>("fire");
        // hit = Content.Load<SoundEffect>("hit");

        _jetMan = new JetMan(400, 300, _jetManTexture, _texture);

        _rockets.Add(new Rocket((int)_rocketLocation1.X, (int)_rocketLocation1.Y, _mRocketTexture, 0, 75, 61));
        _rockets.Add(new Rocket((int)_rocketLocation2.X, (int)_rocketLocation2.Y, _mRocketTexture, 4, 75, 61));
        _rockets.Add(new Rocket((int)_rocketLocation3.X, (int)_rocketLocation3.Y, _mRocketTexture, 8, 75, 61));

        _bonus.Add(new Bonus(_bonusTexture));

        for (var i = 0; i < 10; i++) _enemies.Add(new Enemy(_mEnemyTexture));

        for (var i = 0; i < 40; i++)
        {
            _stars.Add(new Star(_mStarTexture));
            _particles.Add(new Particle(_mParticleTexture));
        }

        _graphicsManager = new GraphicsManager();
        _graphicsManager.LoadLevels();

        _camera = new Camera();
        _camera.Follow(_jetMan);
    }

    protected override void Update(GameTime gameTime)
    {
        //CheckKeyboard();

        if (_gameState == GameState.GameStart)
        {
            _mGameStartingDelay += ElapsedCounter;
            if (_mGameStartingDelay > 10) _gameState = GameState.GameOn;
        }

        CheckScreenBounds();
        if (!_mWalking)
        {
            //y += 1;
        }
        //VerticalScrollDown();

        foreach (var star in _stars) star.Update();

        foreach (var alien in _enemies) alien.Update(_level);

        var tempBullet = new List<Bullet>();
        foreach (var bullet in _bullets.Where(bullet => !bullet.Offscreen))
        {
            bullet.Update();
            tempBullet.Add(bullet);
        }

        _bullets = tempBullet;

        var tempExplosion = new List<Explosion>();
        foreach (var explosion in _explosion)
            if (!explosion.AnimationComplete)
            {
                explosion.Update();
                tempExplosion.Add(explosion);
            }

        _explosion = tempExplosion;

        foreach (var bonus in _bonus) bonus.Update();

        CheckRocketCollsions();
        LowerRocketSections();
        CheckBulletCollisions();
        CheckEnemyCollisions();
        CheckBonusCollisions();
        ////            CheckJetManLedgeCollsions();
        //m_graphicsManager.TileList.ForEach(tile =>
        //{
        //    //    if (tile.Drawable(x, y)) 
        //    { CheckJetManLedgeCollsions(tile); /* i.TileRect);*/ }
        //});

        CheckKeyboard();

        foreach (var particle in _particles)
            if (_fuelLevel < 100)
                particle.Update(_x, _y, _flip != SpriteEffects.None, !_mOnGround);
            else
                particle.Update(RocketLowerPosition + 36, _rockets[0].RocketRect.Y + 36, false, true);

        if (_mSecondSectionComplete && !_mFuelAdded)
        {
            AddFuel();
            _mFuelAdded = true;
        }

        var nextFuel = false;
        foreach (var fuel in _fuel)
        {
            fuel.Update(_ledge);
            CheckFuelCollisions(fuel);
            if (_mFuelLowering && fuel.LowerFuel() && _fuelLevel < 100)
            {
                //if (fuel.LowerFuel() && m_fuelLevel < 100)
                //{
                //if (m_fuelLevel < 100)
                //{
                _fuelLevel += 25;
                _mFuelLowering = false;
                nextFuel = true;
                //}
                //}
            }
        }


        //if (m_scrolling)
        //{
        //    foreach (Ledge ledge in m_ledge)
        //    {

        //        ledge.Update(m_srollX, m_srollY);

        //    }
        //    m_scrolling = false;
        //}

        if (nextFuel) AddFuel();
        if (_lives < 1) _gameState = GameState.GameOver;
        if (_gameState == GameState.TakeOff)
            foreach (var rocket in _rockets)
                if (!rocket.TakeOff())
                {
                    _level++;
                    _gameState = GameState.NextLevel;
                }

        if (_gameState == GameState.NextLevel)
        {
            ResetLevel();
            _gameState = GameState.GameOn;
        }

        //if (m_jetman.JetmanPosition.X > 400 || m_jetman.JetmanPosition.Y < 630) //   m_camera.CameraPos.X < 0)
        //{
        _camera.Follow(_jetMan);
        //}

        base.Update(gameTime);
    }

    private void CheckBulletCollisions()
    {
        Bullet bulletToRemove = null;
        foreach (var alien in _enemies)
        foreach (var bullet in _bullets)
            if (bullet.BulletRect.Intersects(alien.AlienRect))
            {
                _explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, _mExplosionTexture));
                alien.ResetMeteor();
                // died.Play();
                bulletToRemove = bullet;
                _score += 100;
                break;
            }

        if (bulletToRemove != null) _bullets.Remove(bulletToRemove);
    }

    private void CheckEnemyCollisions()
    {
        foreach (var alien in _enemies)
        {
            if (alien.AlienRect.Intersects(_jetMan.JetManRect) && _gameState == GameState.GameOn)
            {
                _explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, _mExplosionTexture));
                alien.ResetMeteor();
                // died.Play();
                _lives--;
                break;
            }

            foreach (var ledge in _ledge)
                if (alien.AlienRect.Intersects(ledge.LedgeRect))
                {
                    _explosion.Add(new Explosion(alien.AlienRect.X, alien.AlienRect.Y, _mExplosionTexture));
                    alien.ResetMeteor();
                    // hit.Play();
                }
        }
    }

    private void CheckBonusCollisions()
    {
        foreach (var bonus in _bonus)
        {
            foreach (var ledge in _ledge)
                if (bonus.BonRect.Intersects(ledge.LedgeRect))
                    bonus.BonusLanded = true;

            if (_jetMan.JetManRect.Intersects(bonus.BonRect))
            {
                bonus.Reset();
                _score += 100;
            }
        }
    }

    private void CheckRocketCollsions()
    {
        if (_jetMan.JetManRect.Intersects(_rockets[1].RocketRect) && !_mFirstSectionLowering)
        {
            if ((int)_jetMan.JetManPosition.X != RocketLowerPosition)
                _rockets[1].Update((int)_jetMan.JetManPosition.X, (int)_jetMan.JetManPosition.Y);
            else
                _mFirstSectionLowering = true;
        }

        if (_jetMan.JetManRect.Intersects(_rockets[2].RocketRect) && _mFirstSectionComplete && !_mSecondSectionLowering)
        {
            if ((int)_jetMan.JetManPosition.X != RocketLowerPosition)
                _rockets[2].Update((int)_jetMan.JetManPosition.X, (int)_jetMan.JetManPosition.Y);
            else
                _mSecondSectionLowering = true;
        }
    }

    //private void CheckJetManLedgeCollsions()
    //{
    //    foreach (Ledge ledge in m_ledge)
    //    {
    //        if (m_jetman.JetmanRect.Intersects(ledge.LedgeRect))
    //        {
    //            if (m_jetman.JetmanRect.Bottom - 3 == ledge.LedgeRect.Top ||
    //                m_jetman.JetmanRect.Bottom - 2 == ledge.LedgeRect.Top ||
    //                m_jetman.JetmanRect.Bottom - 1 == ledge.LedgeRect.Top)
    //            {
    //                y -= 1;
    //                m_onGround = true;
    //                if (!m_walking)
    //                {
    //                    m_currentFrame += 1;
    //                    m_walking = true;
    //                }
    //            }
    //            else
    //            {
    //                m_onGround = false;
    //            }
    //            if (m_jetman.JetmanRect.Top + 1 == ledge.LedgeRect.Bottom)
    //            {
    //                y += 2;
    //            }
    //            if (m_jetman.JetmanRect.Right - 2 == ledge.LedgeRect.Left)
    //            {
    //                x -= 2;
    //            }
    //            if (m_jetman.JetmanRect.Left + 1 == ledge.LedgeRect.Right)
    //            {
    //                x += 2;
    //            }
    //        }
    //    }
    //}

    private void CheckJetManLedgeCollisions(Tile tile) //Rectangle ledge)
    {
        _wallCollision = false;
        var ledge = tile.TileRect;
        if (!_jetMan.JetManRect.Intersects(tile.TileRect)) return;
        switch (tile.GetTileCollisionType)
        {
            //if (m_jetman.JetmanRect.Intersects(tile.TileRect))
            case TileCollisionType.Wall:
            {
                _wallCollision = true;

                // Calculate half sizes.
                var halfWidthA = ledge.Width / 2.0f;
                float halfHeightA = ledge.Height; // / 2.0f;
                var halfWidthB = _jetMan.JetManRect.Width / 2.0f;
                float halfHeightB = _jetMan.JetManRect.Height; // / 2.0f;

                // Calculate centers.
                var centerA = new Vector2(ledge.Left + halfWidthA, ledge.Top + halfHeightA);
                var centerB = new Vector2(_jetMan.JetManRect.Left + halfWidthB, _jetMan.JetManRect.Top + halfHeightB);

                // Calculate current and minimum-non-intersecting distances between centers.
                var distanceX = centerA.X - centerB.X;
                var distanceY = centerA.Y - centerB.Y;
                var minDistanceX = halfWidthA + halfWidthB;
                var minDistanceY = halfHeightA + halfHeightB;

                //// Calculate and return intersection depths.
                var depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
                var depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

                if (depthX is >= -2 and <= 2)
                {
                    _wallCollision = true;
                    _x -= (int)Math.Ceiling(depthX) - 6;
                    _x = 64;

                    if (centerA.X < centerB.X) _x = ledge.Right + 2;
                    if (centerA.X > centerB.X) _x = _jetMan.JetManRect.Left - 4;
                }

                if (depthY < -50) _y = _jetMan.JetManRect.Top + 4;

                break;
            }
            case TileCollisionType.Platform:
                _y = _jetMan.JetManRect.Top - 4;
                break;
        }


        _jetManPosition = new Vector2(_x, _y);
        //            else {
        //               if (m_jetman.JetmanRect.Intersects(ledge))
        //               {
        //                   // Calculate the distance from the Man to the Ledge

        //                   // Calculate half sizes.
        //                   float halfWidthA = ledge.Width / 2.0f;
        //                   float halfHeightA = ledge.Height / 2.0f;
        //                   float halfWidthB = m_jetman.JetmanRect.Width / 2.0f;
        //                   float halfHeightB = m_jetman.JetmanRect.Height / 2.0f;

        //                   // Calculate centers.
        //                   Vector2 centerA = new Vector2(ledge.Left + halfWidthA, ledge.Top + halfHeightA);
        //                   Vector2 centerB = new Vector2(m_jetman.JetmanRect.Left + halfWidthB, m_jetman.JetmanRect.Top + halfHeightB);

        //                   // Calculate current and minimum-non-intersecting distances between centers.
        //                   float distanceX = centerA.X - centerB.X;
        //                   float distanceY = centerA.Y - centerB.Y;
        //                   float minDistanceX = halfWidthA + halfWidthB;
        //                   float minDistanceY = halfHeightA + halfHeightB;

        //                   // Calculate and return intersection depths.
        //                   float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        //                   float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;


        //                   if (depthX != 0)
        //                   {
        //                       m_wallCollsion = true;
        ////                       x -= (int)Math.Ceiling(depthX);
        //                   }

        //                   //if (depthY != 0)
        //                   //{
        //                   //    y -= (int)Math.Ceiling(depthY) - 2;
        //                   //    m_walking = true;
        //                   //}

        //                   //if (m_jetman.JetmanRect.Bottom - 3 == ledge.Top ||
        //                   //    m_jetman.JetmanRect.Bottom - 2 == ledge.Top ||
        //                   //    m_jetman.JetmanRect.Bottom - 1 == ledge.Top)
        //                   //{
        //                   //    y -= 1;
        //                   //    m_onGround = true;
        //                   //    if (!m_walking)
        //                   //    {
        //                   //        m_currentFrame += 1;
        //                   //        m_walking = true;
        //                   //    }
        //                   //}
        //                   //else
        //                   //{
        //                   //    m_onGround = false;
        //                   //}

        //                   //if (m_jetman.JetmanRect.Top + 4 == ledge.Bottom ||
        //                   //    m_jetman.JetmanRect.Top + 3 == ledge.Bottom ||
        //                   //    m_jetman.JetmanRect.Top + 2 == ledge.Bottom ||
        //                   //    m_jetman.JetmanRect.Top + 1 == ledge.Bottom)
        //                   //{
        //                   //    y += 2;
        //                   //}

        //                   //if (m_jetman.JetmanRect.Right - 4 == ledge.Left ||
        //                   //    m_jetman.JetmanRect.Right - 3 == ledge.Left ||
        //                   //    m_jetman.JetmanRect.Right - 2 == ledge.Left ||
        //                   //    m_jetman.JetmanRect.Right - 1 == ledge.Left)
        //                   //{
        //                   //    x -= 2;
        //                   //}

        //                   //if (m_jetman.JetmanRect.Left + 4 == ledge.Right ||
        //                   //    m_jetman.JetmanRect.Left + 3 == ledge.Right ||
        //                   //    m_jetman.JetmanRect.Left + 2 == ledge.Right ||
        //                   //    m_jetman.JetmanRect.Left + 1 == ledge.Right)
        //                   //{
        //                   //    x += 2;
        //                   //}
        //               }
        //           }
    }

    private void CheckFuelCollisions(Fuel fuel)
    {
        if (_jetMan.JetManRect.Intersects(fuel.FuelRect))
        {
            if ((int)_jetMan.JetManPosition.X != FuelLowerPosition && !_mFuelLowering)
                fuel.UpdatePosition((int)_jetMan.JetManPosition.X, (int)_jetMan.JetManPosition.Y);
            else
                _mFuelLowering = true;
        }
    }

    private void LowerRocketSections()
    {
        // Lower the first rocket section into place 
        if (_mFirstSectionLowering && !_mFirstSectionComplete) _mFirstSectionComplete = _rockets[1].LowerSectionOne();

        // Lower the first rocket section into place 
        if (_mSecondSectionLowering && !_mSecondSectionComplete)
            _mSecondSectionComplete = _rockets[2].LowerSectionTwo();
    }

    private void AddFuel()
    {
        _fuel.Clear();
        if (_fuelLevel != 100)
        {
            _fuel.Add(new Fuel(_mFuelTexture));
        }
        else
        {
            _gameState = GameState.TakeOff;
            _score += 100;
        }
    }

    private void CheckScreenBounds()
    {
        //if (y <= 50) { y = 50; }
        //if (y >= 861) { y = 861; }
        //if (x <= 0) { x = 0; }
        //if (x >= 750) { x = 750; }
    }

    //private void HorizontalScrollRight()
    //{
    //    if (x > 400 && x < 800 && m_srollX > -1000)
    //    {
    //        m_srollX -= 1;
    //        foreach (Ledge ledge in m_ledge)
    //        {
    //            ledge.Update(-2, 0);
    //        }
    //    }
    //}

    //private void HorizontalScrollLeft()
    //{
    //    if (x < 400 && x > -10 && m_srollX < 0)
    //    {
    //        m_srollX += 1;
    //        foreach (Ledge ledge in m_ledge)
    //        {
    //            ledge.Update(2, 0);
    //        }
    //    }
    //}

    //private void VerticalScrollDown()
    //{
    //    if (y > 300 && y < 600 && m_srollY > -1000)
    //    {
    //        m_srollY -= 1;
    //        foreach (Ledge ledge in m_ledge)
    //        {
    //            ledge.Update(0, -2);
    //        }
    //    }
    //}

    //private void VerticalScrollUp()
    //{
    //    if (y < 300 && y > 0 && m_srollY < 0)
    //    {
    //        m_srollY += 1;
    //        foreach (Ledge ledge in m_ledge)
    //        {
    //            ledge.Update(0, 2);
    //        }
    //    }
    //}

    private void CheckKeyboard()
    {
        var testX = 0;
        var testY = 0;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.X))
            if (_gameState == GameState.GameOver)
            {
                _gameState = GameState.GameStart;
                ResetGame();
            }

        if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Down))
            if (!_mOnGround)
            {
                //y += 2;
                testY = 2;
                CheckScreenBounds();
                _currentFrame = 0;
            }

        if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            //y -= 2;
            testY = -2;
            CheckScreenBounds();
            _mOnGround = false;
            _mWalking = false;
            _currentFrame = 0;
        }

        if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _flip = SpriteEffects.FlipHorizontally;
            if (!_wallCollision)
            {
//                    x -= 4;
            }

            testX = -4;
            _mAnimationTimer += ElapsedCounter;
            CheckScreenBounds();
            if (_mOnGround && _mAnimationTimer > 0.4)
            {
                _currentFrame = (_currentFrame + 1) % 4;
                if (_currentFrame == 0) _currentFrame = 1;
                _mAnimationTimer = 0;
            }
        }

        if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _flip = SpriteEffects.None;
            if (!_wallCollision)
            {
//                    x += 4;
            }

            testX = 4;
            _mAnimationTimer += ElapsedCounter;
            CheckScreenBounds();
            if (_mOnGround && _mAnimationTimer > 0.4)
            {
                _currentFrame = (_currentFrame + 1) % 4;
                if (_currentFrame == 0) _currentFrame = 1;
                _mAnimationTimer = 0;
            }
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.LeftControl))
        {
            // fire.Play();
            _mDelayCounter += ElapsedCounter;
            if (_mDelayCounter > 0.4)
            {
                _bullets.Add(_flip == SpriteEffects.None
                    ? new Bullet(_x, _jetMan.JetManRect.Center.Y /*   m_jetmanTexture.Height / 2 + y*/, _laserTexture,
                        false)
                    : new Bullet(_x, _jetMan.JetManRect.Center.Y /*m_jetmanTexture.Height / 2 + y*/, _laserTexture,
                        true));
                _mDelayCounter = 0;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A)) _drawnWithLighting = true;
        if (Keyboard.GetState().IsKeyDown(Keys.S)) _drawnWithLighting = false;

        _jetMan.JetManAnimFrame = _currentFrame;
        //m_jetManPosition = new Vector2(x, y);
        float floaterX = _x += (int)GetVelocityX(testX);
        float floaterY = _y += (int)GetVelocityY(testY);
        _jetManPosition = new Vector2(floaterX, floaterY);
        //m_jetman.Update(x, y, flip);
        _jetMan.Update(_jetManPosition, _flip);
    }

    private float CheckForHorizontalIntersections(float increment)
    {
        // Create a new Rectangle based on the players current position for testing Rectangle overlaps
        var testRect = new Rectangle((int)(_jetMan.JetManPosition.X + increment), (int)_jetMan.JetManPosition.Y,
            _jetMan.JetManRect.Width, _jetMan.JetManRect.Height - 5);
        if (_graphicsManager.TileList.Any(tile => testRect.Intersects(tile.TileRect))) return 0;
        return increment;
    }


    /**
     * Method to check 1 pixel by pixel incremental vertical collision detection
     * @param increment  Incremental per pixel value
     * @param playerRectangle  Players Rectangle
     * @param tileList    List of tiles used to check against
     * @return
     */
    private float CheckForVerticalIntersections(float increment) //, Rectangle playerRectangle, List<Tile> tileList)
    {
        // Create a new Rectangle based on the players current position for testing
        var testRect = new Rectangle((int)_jetMan.JetManPosition.X, (int)(_jetMan.JetManPosition.Y + increment),
            _jetMan.JetManRect.Width, _jetMan.JetManRect.Height - 5);
        if (_graphicsManager.TileList.Any(tile => testRect.Intersects(tile.TileRect))) return 0;
        return increment;
    }


    /**
     * This method executes a loop determined by the speed variable:
     * +ve or -ve to check if a rectangle intersection would occur.
     * This method returns the number of pixels that the player can move based on the speed and if a collision occurs,
     * i.e. If the speed is (-/+ve) 2 & no collision occurs the returned value would
     *
     * @param speed
     * @param floorsActive
     * @return
     */
    private float GetVelocityX(float speed) //, List<Tile> floorsActive)
    {
        float pixelMovement = 0;
        var pixels = 0;
        switch (speed)
        {
            case 0:
                return pixelMovement;
            case > 0:
            {
                for (float increment = 0; increment < speed; increment++)
                    //                       pixelMovement = checkForHorizontalIntersections(++pixels, playerRectangle, floorsActive);
                    pixelMovement = CheckForHorizontalIntersections(++pixels);

                break;
            }
            case < 0:
            {
                pixels = 0;
                for (float increment = 0; increment > speed; increment--)
                    //                       pixelMovement = checkForHorizontalIntersections(--pixels, playerRectangle, floorsActive);
                    pixelMovement = CheckForHorizontalIntersections(--pixels);

                break;
            }
        }

        return pixelMovement;
    }


    /**
     * This method executes a loop determined by the speed variable:
     * +ve or -ve to check if a rectangle intersection would occur.
     * This method returns the number of pixels that the player can move based on the speed and if a collision occurs,
     * i.e. If the speed is (-/+ve) 2 & no collision occurs the returned value would
     *
     * @param speed
     * @param floorsActive
     * @return
     */
    private float GetVelocityY(float speed)
    {
        float pixelMovement = 0;
        var pixels = 0;
        switch (speed)
        {
            case 0:
                return pixelMovement;
            case > 0:
            {
                for (float increment = 0; increment < speed; increment++)
                    pixelMovement = CheckForVerticalIntersections(++pixels); //, playerRectangle, floorsActive);

                break;
            }
            case < 0:
            {
                pixels = 0;
                for (float increment = 0; increment > speed; increment--)
                    pixelMovement = CheckForVerticalIntersections(--pixels); //, playerRectangle, floorsActive);

                break;
            }
        }

        return pixelMovement;
    }


    private void ResetGame()
    {
        _score = 0;
        _lives = 3;
        _level = 0;
        _fuelLevel = 0;
        _currentFrame = 0;
        _x = 150;
        _y = 300;
        ResetLevel();
    }

    private void ResetLevel()
    {
        _x = 150;
        _y = 300;
        _rocket1X = 422;
        _rocket1Y = 443;
        _rocket2X = 110;
        _rocket2Y = 139;
        _rocket3X = 510;
        _rocket3Y = 75;
        _fuelLevel = 0;
        _mFuelAdded = false;
        _mFuelLowering = false;
        _mFirstSectionLowering = false;
        _mFirstSectionComplete = false;
        _mSecondSectionLowering = false;
        _mSecondSectionComplete = false;
        _rocketLocation1 = new Vector2(_rocket1X, _rocket1Y);
        _rocketLocation2 = new Vector2(_rocket2X, _rocket2Y);
        _rocketLocation3 = new Vector2(_rocket3X, _rocket3Y);
        _rockets.Clear();
        var frame = _level % 4;
        _rockets.Add(new Rocket((int)_rocketLocation1.X, (int)_rocketLocation1.Y, _mRocketTexture, frame, 75, 61));
        _rockets.Add(new Rocket((int)_rocketLocation2.X, (int)_rocketLocation2.Y, _mRocketTexture, frame + 4, 75, 61));
        _rockets.Add(new Rocket((int)_rocketLocation3.X, (int)_rocketLocation3.Y, _mRocketTexture, frame + 8, 75, 61));
        foreach (var alien in _enemies) alien.ResetMeteor();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        if (_gameState == GameState.GameOver)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_menuTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_font, "JetPac the Remake (Monogame Update)", new Vector2(150.0f, 440.0f),
                Color.Yellow);
            _spriteBatch.DrawString(_font, "Written by Ian Wigley", new Vector2(150.0f, 460.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "In 2020", new Vector2(150.0f, 480.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "Press X to start", new Vector2(150.0f, 520.0f), Color.Yellow);
        }
        else
        {
            if (_drawnWithLighting)
            {
                // Create a Light Mask to pass to the pixel shader
                GraphicsDevice.SetRenderTarget(_lightsTarget);
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_jetManMask, new Vector2(_x - 280, _y - 280), Color.White);
                //m_spriteBatch.Draw(lightMask, new Vector2(100, 0), Color.White);
                _spriteBatch.Draw(_lightMask, new Vector2(160, 100), Color.White);
                _spriteBatch.Draw(_lightMask, new Vector2(590, 100), Color.White);
                //m_spriteBatch.Draw(lightMask, new Vector2(500, 200), Color.White);
                _spriteBatch.End();

                // Draw the main scene to the Render Target
                GraphicsDevice.SetRenderTarget(_mainTarget);
            }

            //https://www.youtube.com/watch?v=ceBCDKU_mNw
            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            _spriteBatch.Draw(_mRustyTexture, new Vector2(0, 0), Color.White);

            _graphicsManager.TileList.ForEach(i =>
            {
                if (!i.Drawable(_x, _y)) return;
                _spriteBatch.Draw(_texture, i.TileRect, i.TileTexture, Color.White);
                _spriteBatch.Draw(_undergroundTiles, i.TileRect, i.TileTexture, Color.White);
            });

//            foreach (var star in _stars) star.Draw(_spriteBatch);
            foreach (var alien in _enemies) alien.Draw(_spriteBatch);
            foreach (var explosion in _explosion) explosion.Draw(_spriteBatch);
            foreach (var bullet in _bullets) bullet.Draw(_spriteBatch);
            foreach (var fuel in _fuel) fuel.Draw(_spriteBatch);
            foreach (var bonus in _bonus) bonus.Draw(_spriteBatch);
            foreach (var rocket in _rockets) rocket.Draw(_spriteBatch);
            foreach (var ledge in _ledge) ledge.Draw(_spriteBatch);

            _jetMan.Draw(_spriteBatch);

            _spriteBatch.End();

//            foreach (var particle in _particles) particle.Draw(_spriteBatch, _camera);

            if (_drawnWithLighting)
            {
                // Draw the main scene with a pixel
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                
                _lightingEffect.Parameters["lightMask"].SetValue(_lightsTarget);
                _lightingEffect.CurrentTechnique.Passes[0].Apply();
                
                _spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.White);
                _spriteBatch.End();
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "SCORE : " + _score.ToString("D4"), 
                _scoreLocation + new Vector2(1.0f, 1.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "FUEL : " + _fuelLevel + "%", 
                _scoreLocation + new Vector2(350.0f, 1.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "LIVES : " + _lives, 
                _scoreLocation + new Vector2(700.0f, 1.0f), Color.Yellow);

            _spriteBatch.DrawString(_font, "x : " + _x + " y : " + _y, _scoreLocation + new Vector2(1.0f, 15.0f),
                Color.Yellow);
            _spriteBatch.DrawString(_font, "x scroll : " + ScrollX + " y scroll : " + ScrollY,
                _scoreLocation + new Vector2(1.0f, 30.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "x camera : " + _camera.CameraPos.X + " y camera : " + _camera.CameraPos.Y,
                _scoreLocation + new Vector2(1.0f, 45.0f), Color.Yellow);
            _spriteBatch.DrawString(_font, "x : " + _jetMan.JetManRect.X + " y : " + _jetMan.JetManRect.Y,
                _scoreLocation + new Vector2(1.0f, 60.0f), Color.Yellow);
            //m_spriteBatch.DrawString(m_font, "m_onGround: " + m_onGround, scoreLocation + new Vector2(1.0f, 25.0f), Color.Yellow);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}