using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MotorBaseFisicaMG38.MyGame;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;
using MotorBaseFisicaMG38.SistemaGameObject;
using MotorBaseFisicaMG38.MyGame.Components;

namespace MotorBaseFisicaMG38
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Escena escena;

        public static Game1 INSTANCE;
        public Game1()
        {
            INSTANCE = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            CreateScene(0);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            Escena.INSTANCIA?.Update(gameTime);
            MotorFisico.Update(gameTime);
            UTGameObjectsManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Camara.ActiveCamera?.Dibujar(spriteBatch);
            foreach (Component com in Escena.INSTANCIA.components)
            {
                com.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void CreateScene(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        MenuScene menu = new MenuScene();

                        Button startButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 0),
                            Text = "Iniciar",
                        };
                        startButton.Click += StartGame_Click;

                        Button helpButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 70),
                            Text = "Instrucciones",
                        };
                        helpButton.Click += LoadInstructions_Click;

                        Button creditsButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 140),
                            Text = "Creditos",
                        };
                        creditsButton.Click += LoadCredits_Click;

                        Button exitButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 210),
                            Text = "Salir",
                        };
                        exitButton.Click += ExitGame_Click;

                        menu.components.Add(startButton);
                        menu.components.Add(helpButton);
                        menu.components.Add(creditsButton);
                        menu.components.Add(exitButton);
                    }
                    break;
                case 1:
                    {
                        MainGame game = new MainGame();
                        Button backMenu = new Button(Content.Load<Texture2D>("Button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .3f,
                            Position = new Vector2(0, 440),
                            Text = "Volver al menu",
                        };
                        backMenu.Click += LoadMainMenu;

                        Button resetButton = new Button(Content.Load<Texture2D>("Button2"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .3f,
                            Position = new Vector2(800 - Content.Load<Texture2D>("Button2").Width / 3.3f, 440),
                            Text = "Reiniciar",
                        };
                        resetButton.Click += StartGame_Click;

                        TextDisplayer text1 = new TextDisplayer(Content.Load<Texture2D>("borde3"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = 1.5f,
                            Position = new Vector2(400 - (Content.Load<Texture2D>("borde3").Width * 1.5f) / 2, 380),
                            Text = "",
                        };
                        TextDisplayer text2 = new TextDisplayer(Content.Load<Texture2D>("borde3"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = 0f,
                            Position = Vector2.Zero,
                            Text = "",
                        };
                        game.components.Add(text2);
                        game.components.Add(backMenu);
                        game.components.Add(resetButton);
                        game.components.Add(text1);
                    }
                    break;
                case 2:
                    {
                        EmptyScene instructions = new EmptyScene();
                        Button backButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 400),
                            Text = "Volver",
                        };
                        backButton.Click += LoadMainMenu;
                        instructions.components.Add(backButton);
                    }
                    break;
                case 3:
                    {
                        EmptyScene credits = new EmptyScene();
                        Button backButton = new Button(Content.Load<Texture2D>("button1"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = .5f,
                            Position = new Vector2(0, 400),
                            Text = "Volver",
                        };
                        TextDisplayer text1 = new TextDisplayer(Content.Load<Texture2D>("borde3"), Content.Load<SpriteFont>("Font"))
                        {
                            Scale = 1f,
                            Position = new Vector2(400 - Content.Load<Texture2D>("borde3").Width / 2, 400),
                            Text = "Team 15\nDiscord Salon 15",
                        };
                        backButton.Click += LoadMainMenu;
                        credits.components.Add(backButton);
                        credits.components.Add(text1);
                        credits.dibujables.Add(new Dibujable("creditos", new Vector2(400, 210), .85f));
                    }
                    break;
                default:
                    Console.WriteLine("uwu");
                    break;
            }
        }

        #region Buttons Actions
        private void StartGame_Click(object sender, System.EventArgs e)
        {
            CreateScene(1);
        }
        private void ExitGame_Click(object sender, System.EventArgs e)
        {
            Exit();
        }
        private void LoadMainMenu(object sender, System.EventArgs e)
        {
            CreateScene(0);
        }
        private void LoadCredits_Click(object sender, System.EventArgs e)
        {
            CreateScene(3);
        }
        private void LoadInstructions_Click(object sender, System.EventArgs e)
        {
            CreateScene(2);
        }
        #endregion
    }
}
