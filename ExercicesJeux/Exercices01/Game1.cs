﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exercices01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;
        GameObject heros;
        GameObject enemies;
        GameObject ovni;
        Texture2D fond;
  


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            this.graphics.ToggleFullScreen();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;
            this.fond = Content.Load<Texture2D>("fondPlanet.png");

            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesse = 5;
            heros.sprite = Content.Load<Texture2D>("unicornAttack.png");
            heros.position = heros.sprite.Bounds;

            enemies = new GameObject();
            enemies.estVivant = true;
            enemies.vitesse = -10;
            enemies.sprite = Content.Load<Texture2D>("douche-JhonnyBravo.png");
            enemies.position = enemies.sprite.Bounds;

            ovni = new GameObject();
            ovni.estVivant = true;
            ovni.vitesse = 15;
            ovni.sprite = Content.Load<Texture2D>("Haltère.png");
            ovni.position = ovni.sprite.Bounds;
           

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
            enemies.position.Y += enemies.vitesse;
            ovni.position.X -= ovni.vitesse;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if ((Keyboard.GetState().IsKeyDown(Keys.D)) || (Keyboard.GetState().IsKeyDown(Keys.Right)))
            {
                
                heros.position.X += heros.vitesse;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.W)) || (Keyboard.GetState().IsKeyDown(Keys.Up)))
            {
                
                heros.position.Y -= heros.vitesse;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.A)) || (Keyboard.GetState().IsKeyDown(Keys.Left)))
            {
                
                heros.position.X -=heros.vitesse;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.S)) || (Keyboard.GetState().IsKeyDown(Keys.Down)))
            {
                heros.position.Y += heros.vitesse;
            }
            // TODO: Add your update logic here
            UpdateHeros();
           UpdateEnnemies();
            UpdateOvni();
            base.Update(gameTime);
        }
        protected void UpdateHeros()
        {
            if (heros.position.X < fenetre.Left)
            {
                heros.position.X = fenetre.Left;
            }

            if (heros.position.X + heros.sprite.Width > fenetre.Right)
            {
                heros.position.X  = fenetre.Right - heros.sprite.Width;
            }

            if (heros.position.Y < fenetre.Top)
            {
                heros.position.Y = fenetre.Top;
            }
            if (heros.position.Y + heros.sprite.Height > fenetre.Bottom)
            {
                heros.position.Y = fenetre.Bottom - heros.sprite.Height;
            }
            if (heros.position.Intersects(ovni.position))
            {
                
                heros.position = heros.sprite.Bounds;
                ovni.estVivant = false;
            }
            
        }
        protected void UpdateEnnemies()
        {
            if (enemies.position.Y < fenetre.Top)
            {
                enemies.position.Y = fenetre.Top;
                enemies.vitesse = 10;
            }

            if (enemies.position.Y + enemies.sprite.Height > fenetre.Bottom)
            {
                enemies.position.Y = fenetre.Bottom - enemies.sprite.Height;
                enemies.vitesse = -10;
            }
            if (enemies.position.Intersects(heros.position))
            {
                enemies.estVivant = false;
            }
        }
        protected void UpdateOvni()
        {
            
            if (ovni.position.X < fenetre.Left)
            {
                ovni.estVivant = false;
                ovni.position.X = enemies.position.X;
                ovni.position.Y = enemies.position.Y + 15;
                spriteBatch.Begin();
                
                spriteBatch.Draw(ovni.sprite, ovni.position, Color.White);
               
                spriteBatch.End();
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            enemies.position.X = (fenetre.Right - enemies.sprite.Width);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(this.fond, GraphicsDevice.Viewport.TitleSafeArea, Color.White);
            spriteBatch.Draw(ovni.sprite, ovni.position, Color.White);
            spriteBatch.Draw(enemies.sprite, enemies.position, Color.White);
            spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

    }
}
