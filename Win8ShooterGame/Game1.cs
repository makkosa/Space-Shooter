using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Shooter
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        float playerMoveSpeed;

        Texture2D mainBackground;
        Rectangle rectBackground;

        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        List<Enemy> enemies;
        Texture2D enemyTexture;
        float enemyMoveSpeed;

        Enemy shootingEnemy;
        Animation shootingEnemyAnimation;
        float shootingEnemyMoveSpeed;
        Random random;
        int enemyCount;

        Texture2D laserTexture;
        List<Laser> laserBeams;
        float laserMoveSpeed;
        TimeSpan laserSpawnTime;
        TimeSpan previousLaserSpawnTime;

        Texture2D enemyLaserTexture;
        List<Laser> enemyLaserBeams;
        float enemyLaserMoveSpeed;
        TimeSpan enemyLaserSpawnTime;
        TimeSpan prevEnemyLaserSpawnTime;

        Healer healer;
        Animation healerAnimation;

        Bomb bomb;
        Animation bombAnimation;

        Asteroids[] asteroids;
        Animation[] asteroidsAnimation;

        List<Explosion> explosions;
        Texture2D enemyExplosionTexture;
        Texture2D allyExplosionTexture;

        Texture2D brownAsteroidTexture;
        Texture2D greyAsteroidTexture;

        SpriteFont font;

        KeyboardState currentKeyboardState;
        KeyboardState previouseKeyboardState;

        GamePadState currentGamePadState;
        GamePadState previouseGamePadState;

        MouseState currentMouseState;
        MouseState previouseMouseState;

        GameState gameState;
        bool isGameEnded;

        Button playButton;
        Button exitButton;

        HealthBar healthBar;

        Sound playButtonSound;
        Sound exitButtonSound;
        Sound playerLaserSound;
        Sound enemyLaserSound;
        Sound injuredPlayerSound;
        Sound shootingEnemyExplosionSound;
        Sound asteroidExplosionSound;
        Sound bigBombSound;
        Sound playerExplosionSound;
        Sound healerCollisionSound;
        
        GameMusic gameMusic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 670;
            graphics.PreferredBackBufferWidth = 670;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Player();
            playerMoveSpeed = 8.0f;

            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            enemies = new List<Enemy>();
            shootingEnemy = new Enemy();
            enemyMoveSpeed = 3.8f;
            shootingEnemyMoveSpeed = 4.2f;
            random = new Random();

            laserBeams = new List<Laser>();
            laserMoveSpeed = 10f;
            enemyLaserBeams = new List<Laser>();
            enemyLaserMoveSpeed = 7f;

            const float SECONDS_IN_MINUTE = 60f;
            laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / 200f);
            previousLaserSpawnTime = TimeSpan.Zero;
            enemyLaserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / 100f);
            prevEnemyLaserSpawnTime = TimeSpan.Zero;

            healer = new Healer(18f, 13f);

            bomb = new Bomb(15f, 5f);

            asteroids = new Asteroids[3];
            asteroidsAnimation = new Animation[3];

            explosions = new List<Explosion>();

            TouchPanel.EnabledGestures = GestureType.FreeDrag;
            IsMouseVisible = true;
            Window.Title = "Space Shooter";

            gameState = GameState.StartMenu;
            isGameEnded = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadPlayer();

            bgLayer1.Initialize(Content, "Graphics\\bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 1);
            bgLayer2.Initialize(Content, "Graphics\\bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 2);
            mainBackground = Content.Load<Texture2D>("Graphics\\Starbasesnow");

            enemyTexture = Content.Load<Texture2D>("Graphics\\enemyAnimation");

            shootingEnemy.LoadContent("Graphics\\shootingEnemy", Content);
            shootingEnemyAnimation = new Animation();
            shootingEnemyAnimation.Initialize(shootingEnemy.Texture, Vector2.Zero, 300, 300, 1, 30, Color.White, 0.2f, true, 0, Vector2.Zero,
                                              SpriteEffects.None, 0);
            enemyCount = 0;

            laserTexture = Content.Load<Texture2D>("Graphics\\laser");
            enemyLaserTexture = Content.Load<Texture2D>("Graphics\\enemyLaser");

            healer.LoadContent("Graphics\\healer", Content);
            healerAnimation = new Animation();
            healerAnimation.Initialize(healer.Texture, Vector2.Zero, 155, 114, 3, 30, Color.White, 0.37f, true, 0, Vector2.Zero,
                                       SpriteEffects.None, 0);

            bomb.LoadContent("Graphics\\galaxy", Content);
            bombAnimation = new Animation();
            bombAnimation.Initialize(bomb.Texture, Vector2.Zero, 256, 256, 15, 30, Color.White, 0.42f, true, 0, Vector2.Zero,
                                            SpriteEffects.None, 0);

            for (int i = 0; i < asteroids.Length; i++)
            {
                asteroids[i] = new Asteroids();
                asteroidsAnimation[i] = new Animation();
            }

            asteroids[0].LoadContent("Graphics\\aestroid1", Content);
            asteroidsAnimation[0].Initialize(asteroids[0].Texture, Vector2.Zero, 447, 390, 1, 30, Color.White, 0.14f, true, 0,
                                             new Vector2(asteroids[0].Texture.Width / 2, asteroids[0].Texture.Height / 2),
                                             SpriteEffects.None, 0);

            asteroids[1].LoadContent("Graphics\\aestroid2", Content);
            asteroidsAnimation[1].Initialize(asteroids[1].Texture, Vector2.Zero, 424, 349, 1, 30, Color.White, 0.13f, true, 0, 
                                             new Vector2(asteroids[1].Texture.Width / 2, asteroids[1].Texture.Height / 2),
                                             SpriteEffects.None, 0);

            asteroids[2].LoadContent("Graphics\\aestroid3", Content);
            asteroidsAnimation[2].Initialize(asteroids[2].Texture, Vector2.Zero, 394, 323, 1, 30, Color.White, 0.14f, true, 0,
                                             new Vector2(asteroids[2].Texture.Width / 2, asteroids[2].Texture.Height / 2),
                                             SpriteEffects.None, 0);


            enemyExplosionTexture = Content.Load<Texture2D>("Graphics\\explosion");
            allyExplosionTexture = Content.Load<Texture2D>("Graphics\\blueExplosion");
            brownAsteroidTexture = Content.Load<Texture2D>("Graphics\\brownExplosion");
            greyAsteroidTexture = Content.Load<Texture2D>("Graphics\\greyExplosion");

            playButton = new Button("Graphics\\playButton", new Vector2((GraphicsDevice.Viewport.TitleSafeArea.Width / 2), 250), Content,
                                     0.9f, 1f);
            exitButton = new Button("Graphics\\exitButton", new Vector2((GraphicsDevice.Viewport.TitleSafeArea.Width / 2), 390), Content,
                                     0.9f, 1f);
            healthBar = new HealthBar("Graphics\\healthBar2", Content, 262, 41, 0.8f, new Vector2(458, 5));

            playButtonSound = new Sound(Content, "Sounds\\button", 0.3f, false);
            exitButtonSound = new Sound(Content, "Sounds\\button", 0.3f, false);
            playerLaserSound = new Sound(Content, "Sounds\\laser", 0.16f, false);
            enemyLaserSound = new Sound(Content, "Sounds\\enemyLaser", 0.17f, false);
            asteroidExplosionSound = new Sound(Content, "Sounds\\asteroidExplosion", 0.17f, false);
            shootingEnemyExplosionSound = new Sound(Content, "Sounds\\asteroidExplosion", 0.17f, false);
            bigBombSound = new Sound(Content, "Sounds\\bigBomb", 0.25f, false);
            healerCollisionSound = new Sound(Content, "Sounds\\health", 0.25f, false);
            playerExplosionSound = new Sound(Content, "Sounds\\playerExplosion", 0.3f, false);
            injuredPlayerSound = new Sound(Content, "Sounds\\injuredPlayer", 0.27f, false);

            gameMusic = new GameMusic(Content, "Sounds\\gameMusic", 0.35f, true);

            font = Content.Load<SpriteFont>("Text");
        }

        private void LoadPlayer()
        {
            Animation playerAnimation = new Animation();
            player.LoadContent("Graphics\\shipAnimation", Content);
            playerAnimation.Initialize(player.Texture, Vector2.Zero, 447, 471, 8, 30, Color.White, 0.2f, true, 0, Vector2.Zero,
                                       SpriteEffects.None, 0);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + (GraphicsDevice.Viewport.TitleSafeArea.Width / 2),
                                           GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.TitleSafeArea.Height * 0.76f));
            player.Initialize(playerAnimation, playerPosition);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            previouseKeyboardState = currentKeyboardState;
            previouseGamePadState = currentGamePadState;
            previouseMouseState = currentMouseState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();

            if ((currentGamePadState.Buttons.Back == ButtonState.Pressed && previouseGamePadState.Buttons.Back != ButtonState.Pressed)
                    || (currentKeyboardState.IsKeyDown(Keys.Escape) && !previouseKeyboardState.IsKeyDown(Keys.Escape)))
            {
                switch (gameState)
                {
                    case GameState.Playing:
                        gameState = GameState.Paused;
                        MediaPlayer.Pause();
                        break;
                    case GameState.Paused:
                        gameState = GameState.Playing;
                        MediaPlayer.Play(gameMusic.Song);
                        break;
                    default:
                        break;
                }
            }

            if (gameState != GameState.Playing)
            {
                playButton.Update(currentMouseState.X, currentMouseState.Y);
                exitButton.Update(currentMouseState.X, currentMouseState.Y);

                ButtonSound(playButton, playButtonSound);
                ButtonSound(exitButton, exitButtonSound);

                if (currentMouseState.LeftButton == ButtonState.Pressed && previouseMouseState.LeftButton != ButtonState.Pressed)
                {
                    MouseClickedOnMenu(playButton.IsMouseOver, exitButton.IsMouseOver, gameTime);
                }
            }

           if (isGameEnded) MediaPlayer.Stop();

            if (isGameEnded && enemies.Count == 0 && !shootingEnemy.Active && !bomb.Active && !healer.Active
                && laserBeams.Count == 0 && enemyLaserBeams.Count == 0 &&
                !asteroids[0].Active && !asteroids[1].Active && !asteroids[2].Active)
            {
                gameState = GameState.StartMenu;
            }

            if (gameState != GameState.Paused)
            {
                // Update the parallaxing background
                bgLayer1.Update(gameTime);
                bgLayer2.Update(gameTime);
            }

            UpdateHealer(gameTime);
            UpdateBomb(gameTime);
            UpdateAsteroids(gameTime);

            if (gameState == GameState.Playing)
            {
                UpdatePlayer(gameTime);

                UpdateEnemySpeed(gameTime);
                UpdateEnemies(gameTime);

                if (player.Active)
                    UpdateCollision();

                UpdateLaserBeams(gameTime, laserBeams);
                UpdateLaserBeams(gameTime, enemyLaserBeams);

                UpdateExplosions(gameTime);
            }

            healthBar.Update(player.Health);

            base.Update(gameTime);
        }

        private void MouseClickedOnMenu(bool playButtonMouseOver, bool exitButtonMouseOver, GameTime gameTime)
        {
            if (playButtonMouseOver)
            {
                if (gameState == GameState.StartMenu)
                {
                    healer.FirstSpawnTime += gameTime.TotalGameTime;
                    bomb.FirstSpawnTime += gameTime.TotalGameTime;
                    Asteroids.FirstSpawnTime += gameTime.TotalGameTime;

                    if (isGameEnded)
                    {
                        ReloadWorld();
                        isGameEnded = false;
                    }
                }
                
                gameState = GameState.Playing;
                MediaPlayer.Play(gameMusic.Song);
            }

            if (exitButtonMouseOver)
                Exit();
        }

        private void ReloadWorld()
        {
            LoadPlayer();
            enemyMoveSpeed = 3.8f;
        }

        private void ButtonSound(Button button, Sound sound)
        {
            if (button.IsMouseOver && !sound.Played)
            {
                sound.SoundEffectInstance.Play();
                sound.Played = true;
            }
            else if (!button.IsMouseOver) sound.Played = false;
        }

        private void UpdateCollision()
        {
            Circle playerCircle;
            Circle enemyCircle;
            Circle laserCircle;
            Circle enemyLaserCircle;
            Circle shootingEnemyCircle;
            Circle healerCircle;
            Circle bombCircle;
            Circle asteroidCircle;

            playerCircle = new Circle(player.Position, player.Width / 2);

            // Do the collision between the player and the bomb
            if (bomb.Active)
            {
                bombCircle = new Circle(bomb.Position, bomb.Width / 8);

                if (playerCircle.Intersects(bombCircle))
                {
                    KillAllEnemies();
                }
            }

            // Do the collision between the player and the shootingEnemy
            if (shootingEnemy.Active)
            {
                shootingEnemyCircle = new Circle(shootingEnemy.Position, shootingEnemy.Width / 2);

                if (playerCircle.Intersects(shootingEnemyCircle))
                {
                    AddExplosion(enemyExplosionTexture, shootingEnemy.Position, 1f);
                    player.Health -= shootingEnemy.Damage;
                    shootingEnemy.Health = 0;
                    shootingEnemy.ExplosionSound.SoundEffectInstance.Play();
                }
            }

            // Do the collision between the player and the healer
            if (healer.Active)
            {
                healerCircle = new Circle(healer.Position, healer.Height / 2);

                if (playerCircle.Intersects(healerCircle))
                {
                    AddExplosion(allyExplosionTexture, healer.Position, 1f);
                    if (player.Health < 100)
                    {
                        if (player.Health > Player.MAX_HEALTH - healer.Healing)
                            player.Health = Player.MAX_HEALTH;
                        else player.Health += healer.Healing;
                    }
                    healer.Active = false;
                    healerCollisionSound.SoundEffectInstance.Play();
                }
            }

            // Do the collision between the shootingEnemy laser and the player
            for (int u = 0; u < enemyLaserBeams.Count; u++)
            {
                enemyLaserCircle = new Circle(enemyLaserBeams[u].Position, enemyLaserBeams[u].Width / 2);

                if (playerCircle.Intersects(enemyLaserCircle))
                {
                    AddExplosion(enemyExplosionTexture, enemyLaserBeams[u].Position, 0.6f);
                    player.Health -= enemyLaserBeams[u].Damage;
                    enemyLaserBeams[u].Active = false;
                    injuredPlayerSound.SoundEffectInstance.Play();
                }
            }

            // Do the collision between the asteroids and the player
            for (int i = 0; i < asteroids.Length; i++)
            {
                if (asteroids[i].Active)
                {
                    asteroidCircle = new Circle(asteroids[i].Position, asteroids[i].Width / 2);

                    if (asteroidCircle.Intersects(playerCircle))
                    {
                        if (i == 0) AddExplosion(brownAsteroidTexture, asteroids[i].Position, 1f);
                        else AddExplosion(greyAsteroidTexture, asteroids[i].Position, 1f);

                        asteroids[i].Health = 0;
                        asteroidExplosionSound.SoundEffectInstance.Play();
                        player.Health -= asteroids[i].Damage;
                    }
                }
            }

            // Do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemyCircle = new Circle(enemies[i].Position, enemies[i].Width / 2);

                if (playerCircle.Intersects(enemyCircle))
                {
                    AddExplosion(enemyExplosionTexture, enemies[i].Position, 1f);
                    player.Health -= enemies[i].Damage;
                    enemies[i].Health = 0;
                    enemies[i].ExplosionSound.SoundEffectInstance.Play();

                    if (player.Health <= 0) player.Active = false;
                }

                // Do the collision between the player laser and the enemies
                for (int e = 0; e < laserBeams.Count; e++)
                {
                    laserCircle = new Circle(laserBeams[e].Position, laserBeams[e].Width / 2);

                    if (enemyCircle.Intersects(laserCircle) && enemies[i].Position.Y + enemies[i].Height > 70)
                    {
                        AddExplosion(enemyExplosionTexture, enemies[i].Position, 1f);
                        enemies[i].Health = 0;
                        laserBeams[e].Active = false;
                        player.Score += enemies[i].Value;
                        enemies[i].ExplosionSound.SoundEffectInstance.Play();
                    }
                }
            }


            // Do the collision between the player lasers and the entities
            for (int e = 0; e < laserBeams.Count; e++)
            {
                laserCircle = new Circle(laserBeams[e].Position, laserBeams[e].Width / 2);

                // Do the collision between the player laser and the shootingEnemy
                if (shootingEnemy.Active)
                {
                    shootingEnemyCircle = new Circle(shootingEnemy.Position, shootingEnemy.Width / 2);

                    if (shootingEnemyCircle.Intersects(laserCircle) && shootingEnemy.Position.Y + shootingEnemy.Height > 70)
                    {
                        AddExplosion(enemyExplosionTexture, shootingEnemy.Position, 1f);
                        shootingEnemy.Health = 0;
                        shootingEnemy.ExplosionSound.SoundEffectInstance.Play();
                        laserBeams[e].Active = false;
                        player.Score += shootingEnemy.Value;
                    }
                }

                // Do the collision between the player laser and the bomb
                if (bomb.Active)
                {
                    bombCircle = new Circle(bomb.Position, bomb.Width / 8);

                    if ((bombCircle.Intersects(laserCircle) && bomb.Position.Y + bomb.Height > 70) ||
                        playerCircle.Intersects(bombCircle))
                        KillAllEnemies();
                }

                // Do the collision between the player laser and the asteroid
                for (int a = 0; a < asteroids.Length; a++)
                {
                    if (asteroids[a].Active)
                    {
                        asteroidCircle = new Circle(asteroids[a].Position, asteroids[a].Width / 2);

                        if (asteroidCircle.Intersects(laserCircle) && asteroids[a].Position.Y + asteroids[a].Height > 70)
                        {
                            if (a == 0) AddExplosion(brownAsteroidTexture, asteroids[a].Position, 1f);
                            else AddExplosion(greyAsteroidTexture, asteroids[a].Position, 1f);

                            asteroids[a].Health = 0;
                            asteroidExplosionSound.SoundEffectInstance.Play();
                            laserBeams[e].Active = false;
                            player.Score += Asteroids.VALUE;
                        }
                    }
                }
            }
        }

        private void KillAllEnemies()
        {
            int killsCount = 0;

            for (int i = 0; i < enemies.Count; i++)
            {
                AddExplosion(enemyExplosionTexture, enemies[i].Position, 1f);
                enemies[i].Health = 0;
                player.Score += enemies[i].Value;
                killsCount++;
            }

            if (shootingEnemy.Active)
            {
                AddExplosion(enemyExplosionTexture, shootingEnemy.Position, 1f);
                shootingEnemy.Health = 0;
                player.Score += shootingEnemy.Value;
                killsCount++;
            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                if (asteroids[i].Active)
                {
                    if (i == 0) AddExplosion(brownAsteroidTexture, asteroids[i].Position, 1f);
                    else AddExplosion(greyAsteroidTexture, asteroids[i].Position, 1f);

                    asteroids[i].Health = 0;
                    player.Score += Asteroids.VALUE;
                    killsCount++;
                }
            }

            if (killsCount > 0)
            {
                if (bigBombSound.SoundEffectInstance.State == SoundState.Playing)
                    bigBombSound.SoundEffectInstance.Stop();
                bigBombSound.SoundEffectInstance.Play();
            }

            bomb.Active = false;
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - Enemy.PrevSpawnTime > Enemy.SpawnTime)
            {
                Enemy.PrevSpawnTime = gameTime.TotalGameTime;

                if (!isGameEnded)
                {
                    enemyCount++;

                    if (enemyCount == 7)
                    {
                        shootingEnemy.Initialize(shootingEnemyAnimation, GetRandomPosition(50, -25), 40, shootingEnemyExplosionSound);
                        enemyCount = 0;
                    }
                    else AddEnemy();
                }
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime, graphics, enemyMoveSpeed);
                if (enemies[i].Active == false) enemies.RemoveAt(i);
            }

            if (shootingEnemy.Active)
            {
                shootingEnemy.Update(gameTime, graphics, shootingEnemyMoveSpeed);
                FireEnemyLaser(gameTime);
            }
        }

        private void AddEnemy()
        {
            Animation enemyAnimation = new Animation();
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 375, 454, 8, 35, Color.White, 0.17f, true, 0, Vector2.Zero,
                                      SpriteEffects.None, 0);
            Sound enemyExplosionSound;
            enemyExplosionSound = new Sound(Content, "Sounds\\explosion", 0.17f, false);

            Enemy enemy = new Enemy();
            enemy.Initialize(enemyAnimation, GetRandomPosition(50, -enemyTexture.Height / 2), 20, enemyExplosionSound);
            enemies.Add(enemy);
        }

        private Vector2 GetRandomPosition(int indent, int positionY)
        {
            return new Vector2(random.Next(indent, GraphicsDevice.Viewport.Width - indent), positionY);
        }

        private void UpdateEnemySpeed(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - Enemy.PrevAccelerationTime > Enemy.AccelerationTime && enemyMoveSpeed < Enemy.MAX_MOVE_SPEED)
            {
                Enemy.PrevAccelerationTime = gameTime.TotalGameTime;
                enemyMoveSpeed += 0.2f;
            }
        }

        private void UpdateHealer(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - healer.PrevSpawnTime > healer.SpawnTime && gameTime.TotalGameTime > healer.FirstSpawnTime)
            {
                healer.PrevSpawnTime = gameTime.TotalGameTime;
                if (gameState == GameState.Playing && !isGameEnded)
                    healer.Initialize(healerAnimation, GetRandomPosition(50, -25));
            }

            if (healer.Active && gameState == GameState.Playing)
                healer.Update(gameTime, graphics);
        }

        private void UpdateBomb(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - bomb.PrevSpawnTime > bomb.SpawnTime && gameTime.TotalGameTime > bomb.FirstSpawnTime)
            {
                bomb.PrevSpawnTime = gameTime.TotalGameTime;
                if (gameState == GameState.Playing && !isGameEnded)
                    bomb.Initialize(bombAnimation, GetRandomPosition(50, -25));
            }

            if (bomb.Active && gameState == GameState.Playing)
                bomb.Update(gameTime, graphics);
        }

        private void UpdateAsteroids(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - Asteroids.PrevSpawnTime > Asteroids.SpawnTime)
            {
                Asteroids.PrevSpawnTime = gameTime.TotalGameTime;

                if (gameState == GameState.Playing && !isGameEnded)
                {
                    int number = random.Next(0, 3);
                    Vector2 startPosition = GetRandomPosition(-50, -25);
                    Vector2 destinationPosition = GetRandomPosition(-50, graphics.GraphicsDevice.Viewport.TitleSafeArea.Height);
                    asteroids[number].PosDelta = GetPosDelta(startPosition, destinationPosition, Asteroids.MoveSpeed);

                    asteroids[number].Initialize(asteroidsAnimation[number], startPosition);
                }
            }

            if (gameState == GameState.Playing)
            {
                for (int i = 0; i < asteroids.Length; i++)
                {
                    if (asteroids[i].Active)
                    {
                        asteroids[i].Update(gameTime, graphics);
                    }
                }
            }
        }

        private void FirePlayerLaser(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
            {
                previousLaserSpawnTime = gameTime.TotalGameTime;
                Vector2 posDelta = GetPosDelta(player.Position, new Vector2(player.Position.X, 0), laserMoveSpeed);
                AddLaser(laserTexture, player.Position, laserBeams, laserMoveSpeed, posDelta, 0, Vector2.Zero);

                playerLaserSound.SoundEffectInstance.Play();
            }
        }

        private void FireEnemyLaser(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - prevEnemyLaserSpawnTime > enemyLaserSpawnTime)
            {
                prevEnemyLaserSpawnTime = gameTime.TotalGameTime;
                Vector2 posDelta = GetPosDelta(shootingEnemy.Position, player.Position, enemyLaserMoveSpeed);
                float rotation = VectorToAngle(posDelta);
                AddLaser(enemyLaserTexture, shootingEnemy.Position, enemyLaserBeams, enemyLaserMoveSpeed, posDelta,
                         rotation, new Vector2(enemyLaserTexture.Width / 2 * 0.3f, enemyLaserTexture.Height / 2 * 0.3f));

                enemyLaserSound.SoundEffectInstance.Play();
            }
        }

        private void AddLaser(Texture2D texture, Vector2 position, List<Laser> laserList, float speed, Vector2 posDelta,
                              float rotation, Vector2 origin)
        {
            Animation laserAnimation = new Animation();
            laserAnimation.Initialize(texture, position, 34, 69, 1, 30, Color.White, 0.3f, true, rotation, origin, SpriteEffects.None, 0);
            Laser laser = new Laser();

            laser.Initialize(laserAnimation, position, speed, posDelta);
            laserList.Add(laser);
        }

        private void UpdateLaserBeams(GameTime gameTime, List<Laser> lasersList)
        {
            for (var i = 0; i < lasersList.Count; i++)
            {
                lasersList[i].Update(gameTime);

                if (!lasersList[i].Active || lasersList[i].Position.Y < -lasersList[i].Height ||
                    lasersList[i].Position.Y > graphics.GraphicsDevice.Viewport.TitleSafeArea.Height
                    || lasersList[i].Position.X < -lasersList[i].Width
                    || lasersList[i].Position.X > graphics.GraphicsDevice.Viewport.TitleSafeArea.Width)
                {
                    lasersList.Remove(lasersList[i]);
                }
            }
        }

        private float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X) - 1.5708f;
        }

        private Vector2 GetPosDelta(Vector2 startPos, Vector2 destinationPos, float moveSpeed)
        {
            Vector2 posDelta = destinationPos - startPos;
            posDelta.Normalize();
            return posDelta *= moveSpeed;
        }

        protected void AddExplosion(Texture2D texture, Vector2 position, float scale)
        {
            Animation explosionAnimation = new Animation();
            explosionAnimation.Initialize(texture, position, 256, 256, 16, 30, Color.White, scale, true, 0, new Vector2(0),
                                          SpriteEffects.None, 0);

            Explosion explosion = new Explosion();
            explosion.Initialize(explosionAnimation, position);

            explosions.Add(explosion);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (var i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);

                if (!explosions[i].Active)
                    explosions.Remove(explosions[i]);
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            if (player.Active)
            {
                // Windows 8 Touch Gestures for MonoGame
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();

                    if (gesture.GestureType == GestureType.FreeDrag)
                    {
                        player.Position += gesture.Delta;
                    }
                }

                //Get Mouse State then Capture the Button type and Respond Button Press
                Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 posDelta = mousePosition - player.Position;

                    posDelta.Normalize();
                    posDelta *= playerMoveSpeed;

                    if ((mousePosition - player.Position).Length() <= playerMoveSpeed)
                    {
                        player.Position = mousePosition;
                    }
                    else player.Position += posDelta;
                }

                // Get Thumbstick Controls
                player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
                player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

                // Use the Keyboard / Dpad
                if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) ||
                    currentGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    player.Position.X -= playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) ||
                    currentGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    player.Position.X += playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) ||
                    currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    player.Position.Y -= playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) ||
                    currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    player.Position.Y += playerMoveSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Space) || currentGamePadState.Buttons.X == ButtonState.Pressed)
                {
                    FirePlayerLaser(gameTime);
                }

                // Make sure that the player does not go out of bounds
                player.Position.X = MathHelper.Clamp(player.Position.X, player.Width / 2, GraphicsDevice.Viewport.Width - player.Width / 2);
                player.Position.Y = MathHelper.Clamp(player.Position.Y, player.Height / 2, GraphicsDevice.Viewport.Height - player.Height / 2 + 20);
            }
            else if (!isGameEnded)
            {
                AddExplosion(allyExplosionTexture, player.Position, 2.5f);
                playerExplosionSound.SoundEffectInstance.Play();
                isGameEnded = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //Draw the Main Background Texture
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);

            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            foreach (var a in asteroids)
            {
                if (a.Active)
                a.Draw(spriteBatch);
            }

            // Draw the Enemies
            foreach (var e in enemies)
            {
                e.Draw(spriteBatch);
            }

            foreach (var i in enemyLaserBeams)
            {
                i.Draw(spriteBatch);
            }

            if (shootingEnemy.Active)
                shootingEnemy.Draw(spriteBatch);

            if (healer.Active)
                healer.Draw(spriteBatch);

            if (bomb.Active)
                bomb.Draw(spriteBatch);

            foreach (var l in laserBeams)
            {
                l.Draw(spriteBatch);
            }

            if (player.Active)
                player.Draw(spriteBatch);
            
            foreach (var e in explosions)
            {
                e.Draw(spriteBatch);
            }

            healthBar.Draw(spriteBatch);

            if (gameState != GameState.Playing)
            {
                playButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "SCORE:", new Vector2(5, 8), new Color(66, 171, 223) * 0.9f);
            spriteBatch.DrawString(font, player.Score.ToString(), new Vector2(110, 8), new Color(197, 198, 200) * 0.85f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
