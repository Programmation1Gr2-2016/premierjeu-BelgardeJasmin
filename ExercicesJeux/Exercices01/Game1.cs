using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

using System.Collections.Generic;


namespace Exercices01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Song Tetris;
        SoundEffect hennissement;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;
        GameObject heros;
        GameObject enemies;
        GameObject[] ovni = new GameObject[10];
        Texture2D fond;
        Random alVitesse = new Random();
        GameObject menu;
        long spawnTime = 0;
        long dureeRalenti = 0;
        bool renlenti = false;
        long tempsDeChargement = 0;
        byte pourcentagePar5 = 0;
        GameObject BarreChargementRouge;
        GameObject[] TabChargementVert = new GameObject[20];
        SpriteFont Load;



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

            Load = Content.Load<SpriteFont>("Load");

            Tetris = Content.Load<Song>("tetris-gameboy-02");
            MediaPlayer.Volume = (float)(0.1);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Tetris);

            hennissement = Content.Load<SoundEffect>("sf_hennissement_01");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;
            this.fond = Content.Load<Texture2D>("ListeSprite\\fondPlanet");
            #region "Heros"
            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesseX = 10;
            heros.sprite = Content.Load<Texture2D>("ListeSprite\\unicornAttack");
            heros.position = heros.sprite.Bounds;
            #endregion


            #region "enemies"
            enemies = new GameObject();
            enemies.estVivant = true;
            enemies.vitesseX = -10;
            enemies.sprite = Content.Load<Texture2D>("ListeSprite\\douche-JhonnyBravo");
            enemies.position = enemies.sprite.Bounds;
            enemies.position.X = (fenetre.Right - enemies.sprite.Width);
            #endregion

            #region "munition"
            for (int i = 0; i < ovni.Length; i++)
            {
                ovni[i] = new GameObject();
                ovni[i].estVivant = false;

                ovni[i].vitesseX = 0;

                ovni[i].vitesseY = 0;

                ovni[i].position.X = enemies.position.X;
                ovni[i].position.Y = enemies.position.Y + 15;


                ovni[i].sprite = Content.Load<Texture2D>("ListeSprite\\Haltère");
                ovni[i].position = ovni[i].sprite.Bounds;
                ovni[i].position.X = enemies.position.X;
                ovni[i].position.Y = enemies.position.Y + 15;
            }

            #endregion
            #region "ChargementBarreRouge
            BarreChargementRouge = new GameObject();
            BarreChargementRouge.sprite = Content.Load<Texture2D>("ListeSprite\\BarreDeChargementRouge");
            BarreChargementRouge.position = BarreChargementRouge.sprite.Bounds;
            BarreChargementRouge.position.X = ((fenetre.Width / 2) - (BarreChargementRouge.sprite.Width / 2));
            BarreChargementRouge.position.Y = (fenetre.Height - BarreChargementRouge.sprite.Height);

            #endregion
            #region "chargement vert"
            for (int i = 0; i < TabChargementVert.Length; i++)
            {
                TabChargementVert[i] = new GameObject();


                TabChargementVert[i].sprite = Content.Load<Texture2D>("ListeSprite\\BarreChargementvert5%");
                TabChargementVert[i].position = TabChargementVert[i].sprite.Bounds;
                TabChargementVert[i].position.X = (BarreChargementRouge.position.X + (i * TabChargementVert[i].sprite.Width));
                TabChargementVert[i].position.Y = BarreChargementRouge.position.Y;
            }
            #endregion
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
            #region "vitesse enemie/ovni"
            if (enemies.estVivant)
            {
                enemies.position.Y += enemies.vitesseX;
            }

            if (renlenti)
            {
                for (int i = 0; i < ovni.Length; i++)
                {
                    if (ovni[i].estVivant)
                    {
                        ovni[i].position.X -= (ovni[i].vitesseX) / 2;
                        ovni[i].position.Y += (ovni[i].vitesseY) / 2;
                    }
                }
            }
            else
            {

                for (int i = 0; i < ovni.Length; i++)
                {

                    if (ovni[i].estVivant)
                    {
                        ovni[i].position.X -= (ovni[i].vitesseX) / 2;
                        ovni[i].position.Y += (ovni[i].vitesseY) / 2;
                    }
                }
            }

            #endregion

            #region "fonction touches"
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if ((Keyboard.GetState().IsKeyDown(Keys.D)) || (Keyboard.GetState().IsKeyDown(Keys.Right)))
            {

                heros.position.X += heros.vitesseX;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.W)) || (Keyboard.GetState().IsKeyDown(Keys.Up)))
            {

                heros.position.Y -= heros.vitesseX;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.A)) || (Keyboard.GetState().IsKeyDown(Keys.Left)))
            {

                heros.position.X -= heros.vitesseX;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.S)) || (Keyboard.GetState().IsKeyDown(Keys.Down)))
            {
                heros.position.Y += heros.vitesseX;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Space)) && (pourcentagePar5 >= 20))
            {

                renlenti = true;
            }

            #endregion
            // TODO: Add your update logic here
            #region "update"
            UpdateHeros();
            UpdateEnnemies();
            UpdateOvni(gameTime);


            UpdateBarreChargement(gameTime);


            base.Update(gameTime);
            #endregion
        }
        protected void UpdateHeros()
        {
            if (heros.position.X < fenetre.Left)
            {
                heros.position.X = fenetre.Left;
            }

            if (heros.position.X + heros.sprite.Width > fenetre.Right)
            {
                heros.position.X = fenetre.Right - heros.sprite.Width;
            }

            if (heros.position.Y < fenetre.Top)
            {
                heros.position.Y = fenetre.Top;
            }
            if (heros.position.Y + heros.sprite.Height > fenetre.Bottom)
            {
                heros.position.Y = fenetre.Bottom - heros.sprite.Height;
            }
            for (int i = 0; i < ovni.Length; i++)
            {
                if ((heros.position.Intersects(ovni[i].position)) && (enemies.estVivant == true))
                {
                    hennissement.Play();
                    heros.position = heros.sprite.Bounds;

                    ovni[i].position.X = enemies.position.X;
                    ovni[i].position.Y = enemies.position.Y + 15;
                    if (pourcentagePar5 < 20)
                    {
                        if (pourcentagePar5 < 2)
                        {
                            pourcentagePar5 = 0;
                        }
                        else
                        {

                            pourcentagePar5 -= 2;
                        }
                    }
                    if (renlenti)
                    {
                        renlenti = false;
                        pourcentagePar5 = 0;
                    }

                }
            }


        }

        protected void UpdateEnnemies()
        {

            if (enemies.position.Y < fenetre.Top)
            {
                enemies.position.Y = fenetre.Top;
                enemies.vitesseX = 10;
            }

            if (enemies.position.Y + enemies.sprite.Height > fenetre.Bottom)
            {
                enemies.position.Y = fenetre.Bottom - enemies.sprite.Height;
                enemies.vitesseX = -10;
            }
            if (enemies.position.Intersects(heros.position))
            {
                enemies.estVivant = false;
            }

        }

        protected void UpdateOvni(GameTime gametime)
        {
            if (gametime.TotalGameTime.Seconds > 3)
            {

                for (int i = 0; i < ovni.Length; i++)
                {
                    if (ovni[i].estVivant == false)
                    {
                        ovni[i].estVivant = true;
                        ovni[i].vitesseX = alVitesse.Next(5, 25);
                        switch (alVitesse.Next(0, 2))
                        {
                            case 1:
                                ovni[i].vitesseY = alVitesse.Next(-24, -4);
                                break;
                            case 0:
                                ovni[i].vitesseY = alVitesse.Next(5, 25);
                                break;
                        }
                    }

                }


            }
            for (int i = 0; i < ovni.Length; i++)
            {
                if ((ovni[i].position.X < fenetre.Left) || (ovni[i].position.Y < fenetre.Top) || (ovni[i].position.Y > fenetre.Bottom))
                {

                    ovni[i].vitesseX = alVitesse.Next(0, 21);
                    switch (alVitesse.Next(0, 2))
                    {
                        case 1:
                            ovni[i].vitesseY = alVitesse.Next(-24, -4);
                            break;
                        case 0:
                            ovni[i].vitesseY = alVitesse.Next(5, 25);
                            break;
                    }
                    ovni[i].position.X = enemies.position.X;
                    ovni[i].position.Y = enemies.position.Y + 15;

                }
            }


        }

        protected void UpdateBarreChargement(GameTime gametime)
        {
            tempsDeChargement += gametime.ElapsedGameTime.Milliseconds;

            if (renlenti)
            {
                if (tempsDeChargement > 1000)
                {
                    pourcentagePar5 -= 4;
                    tempsDeChargement = 0;
                }
                if (pourcentagePar5 == 0)
                {
                    renlenti = false;
                }
            }
            else
            {
                if ((tempsDeChargement > 750) && (pourcentagePar5 < 20))
                {
                    pourcentagePar5 += 1;


                    tempsDeChargement = 0;
                }
            }





        }

        protected void PouvoirRenlentie(bool renlenti)
        {
            if (renlenti)
            {
                enemies.vitesseY /= 2;
                for (int i = 0; i < ovni.Length; i++)
                {
                    ovni[i].vitesseX /= 2;
                    ovni[i].vitesseY /= 2;

                }


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
            #region "si est ralentit"
            if (renlenti)
            {
                spriteBatch.Draw(this.fond, GraphicsDevice.Viewport.TitleSafeArea, Color.Purple);
                spriteBatch.Draw(BarreChargementRouge.sprite, BarreChargementRouge.position, Color.Purple);

                for (int i = 0; i < pourcentagePar5; i++)
                {
                    spriteBatch.Draw(TabChargementVert[i].sprite, TabChargementVert[i].position, Color.Purple);
                }
                for (int i = 0; i < ovni.Length; i++)
                {
                    if (enemies.estVivant == true)
                    {
                        spriteBatch.Draw(ovni[i].sprite, ovni[i].position, Color.Blue);
                        spriteBatch.Draw(enemies.sprite, enemies.position, Color.Purple);
                    }
                }

                spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            }
            #endregion

            #region "normale"
            else
            {
                spriteBatch.Draw(this.fond, GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                spriteBatch.Draw(BarreChargementRouge.sprite, BarreChargementRouge.position, Color.White);

                for (int i = 0; i < pourcentagePar5; i++)
                {
                    spriteBatch.Draw(TabChargementVert[i].sprite, TabChargementVert[i].position, Color.White);
                }
                for (int i = 0; i < ovni.Length; i++)
                {

                    if (ovni[i].estVivant)
                    {
                        spriteBatch.Draw(ovni[i].sprite, ovni[i].position, Color.White);
                    }
                }
                if (enemies.estVivant == true)
                {

                    spriteBatch.Draw(enemies.sprite, enemies.position, Color.White);
                }
                spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            }
            #endregion


            spriteBatch.DrawString(Load, "Appuyer sur SPACEBARRE pour activer le pouvoir", new Vector2(BarreChargementRouge.position.X + 250, BarreChargementRouge.position.Y), Color.White);
            spriteBatch.End();




            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

    }
}
