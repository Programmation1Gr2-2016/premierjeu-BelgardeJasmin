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
        GameObjectAnime heros;
        GameObject enemies;
        GameObject[] ovni = new GameObject[20];
        Texture2D fond;
        Random alVitesse = new Random();
        GameObject menu;
        long spawnTime = 0;
        KeyboardState keys = new KeyboardState();
        KeyboardState previousKeys = new KeyboardState();
        bool ralenti = false;
        long tempsDeChargement = 0;
        byte pourcentagePar5 = 0;
        byte nbennemies = 4;
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
            this.graphics.ApplyChanges();
            this.Window.Position = new Point(0, 0);


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
            heros = new GameObjectAnime();
            heros.direction = Vector2.Zero;
            heros.vitesse.X = 3;
            heros.objetState = GameObjectAnime.etats.attenteDroite;
            heros.position = new Rectangle(37, 9, 123, 85);   //Position initiale de heros
            heros.sprite = Content.Load<Texture2D>("ListeSprite\\SpriteUnicorn");
            #endregion


            #region "enemies"
            enemies = new GameObject();
            enemies.estVivant = true;
            enemies.vitesseX = 10;
            enemies.sprite = Content.Load<Texture2D>("ListeSprite\\douche-JhonnyBravo");
            enemies.position = enemies.sprite.Bounds;
            enemies.position.X = (fenetre.Right - enemies.sprite.Width);
            enemies.position.Y = 100;
            #endregion

            #region "munition"

            for (int i = 0; i < ovni.Length; i++)
            {
                ovni[i] = new GameObject();
                ovni[i].estVivant = false;

                ovni[i].vitesseX = 0;

                ovni[i].vitesseY = 0;


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

            heros.position.X += (int)(heros.vitesse.X * heros.direction.X);
            #region "vitesse enemie/ovni"


            if (ralenti)
            {
                if (enemies.estVivant)
                {
                    enemies.position.Y += enemies.vitesseY / 2;

                    enemies.position.X += enemies.vitesseX / 2;
                }
                for (int i = 0; i < ovni.Length; i++)
                {
                    if (ovni[i].estVivant)
                    {
                        ovni[i].position.X += (ovni[i].vitesseX) / 2;
                        ovni[i].position.Y += (ovni[i].vitesseY) / 2;
                    }
                }
            }
            else
            {
                if (enemies.estVivant)
                {
                    enemies.position.Y += enemies.vitesseY;

                    enemies.position.X += enemies.vitesseX;
                }
                for (int i = 0; i < ovni.Length; i++)
                {

                    if (ovni[i].estVivant)
                    {
                        ovni[i].position.X += (ovni[i].vitesseX);
                        ovni[i].position.Y += (ovni[i].vitesseY);
                    }
                }
            }

            #endregion
            keys = Keyboard.GetState();

            #region "fonction touches"
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region "Key right/D"
            if ((Keyboard.GetState().IsKeyDown(Keys.D)) || (Keyboard.GetState().IsKeyDown(Keys.Right)))
            {

                heros.direction.X = 4;
                heros.objetState = GameObjectAnime.etats.runDroit;
            }
            if (keys.IsKeyUp(Keys.Right) && previousKeys.IsKeyDown(Keys.Right) || keys.IsKeyUp(Keys.D) && previousKeys.IsKeyDown(Keys.D))
            {
                heros.direction.X = 0;
                heros.objetState = GameObjectAnime.etats.attenteDroite;
            }

            #endregion

            #region"key up/W"
            if ((Keyboard.GetState().IsKeyDown(Keys.W)) || (Keyboard.GetState().IsKeyDown(Keys.Up)))
            {

                heros.position.Y -= (int)heros.vitesse.X;
            }
            if (keys.IsKeyUp(Keys.W) && previousKeys.IsKeyDown(Keys.W) || keys.IsKeyUp(Keys.Up) && previousKeys.IsKeyDown(Keys.Up))
            {
                heros.direction.X = 0;
                heros.objetState = GameObjectAnime.etats.attenteDroite;
            }

            #endregion

            if ((Keyboard.GetState().IsKeyDown(Keys.A)) || (Keyboard.GetState().IsKeyDown(Keys.Left)))
            {

                heros.direction.X = -2;
                heros.objetState = GameObjectAnime.etats.runGauche;

            }


            if ((Keyboard.GetState().IsKeyDown(Keys.S)) || (Keyboard.GetState().IsKeyDown(Keys.Down)))
            {
                heros.position.Y += (int)heros.vitesse.X;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Space)) && (pourcentagePar5 >= 20))
            {

                ralenti = true;
            }
            else
            {
                  
            }
            #endregion
            // TODO: Add your update logic here
            #region "update"

            UpdateHeros();
            heros.Update(gameTime);
            UpdateEnnemies();
            UpdateOvni(gameTime);
            //PouvoirRenlentie(ralenti);

            UpdateBarreChargement(gameTime);
            previousKeys = keys;

            base.Update(gameTime);
            #endregion
        }
        protected void UpdateHeros()
        {
            if (heros.position.X < fenetre.Left)
            {
                heros.position.X = fenetre.Left;
            }

            if (heros.position.X + heros.spriteAfficher.Width > fenetre.Right)
            {
                heros.position.X = fenetre.Right - heros.sprite.Width;
            }

            if (heros.position.Y < fenetre.Top)
            {
                heros.position.Y = fenetre.Top;
            }
            if (heros.position.Y + heros.spriteAfficher.Height > fenetre.Bottom)
            {
                heros.position.Y = fenetre.Bottom - heros.sprite.Height;
            }
            for (int i = 0; i < ovni.Length; i++)
            {
                if ((heros.position.Intersects(ovni[i].position)) && (enemies.estVivant == true))
                {
                    hennissement.Play();
                    heros.position.X = 0;
                    heros.position.Y = 0;

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
                    if (ralenti)
                    {
                        ralenti = false;
                        pourcentagePar5 = 0;
                    }

                }
            }


        }

        protected void UpdateEnnemies()
        {

            if (enemies.position.Y < fenetre.Top)
            {


                enemies.vitesseY = alVitesse.Next(5, 11);


                switch (alVitesse.Next(0, 2))
                {
                    case 1:
                        enemies.vitesseX = alVitesse.Next(-10, 0);
                        break;
                    case 0:
                        enemies.vitesseX = alVitesse.Next(1, 11);
                        break;
                }
            }

            if (enemies.position.Y + enemies.sprite.Height > fenetre.Bottom)
            {


                enemies.vitesseY = alVitesse.Next(-10, 0);

                switch (alVitesse.Next(0, 2))
                {
                    case 1:
                        enemies.vitesseX = alVitesse.Next(1, 11);
                        break;
                    case 0:
                        enemies.vitesseX = alVitesse.Next(-10, 0);
                        break;
                }
            }
            if (enemies.position.X + enemies.sprite.Width > fenetre.Right)
            {

                enemies.vitesseX = alVitesse.Next(-10, 0);

                switch (alVitesse.Next(0, 2))
                {
                    case 1:
                        enemies.vitesseY = alVitesse.Next(1, 11);
                        break;
                    case 0:
                        enemies.vitesseY = alVitesse.Next(-10, 0);
                        break;
                }
            }
            if (enemies.position.X < fenetre.Left)
            {

                enemies.vitesseX = alVitesse.Next(1, 11);

                switch (alVitesse.Next(0, 2))
                {
                    case 1:
                        enemies.vitesseY = alVitesse.Next(1, 11);
                        break;
                    case 0:
                        enemies.vitesseY = alVitesse.Next(-10, 0);
                        break;
                }
            }
            if (enemies.position.Intersects(heros.position))
            {
                enemies.estVivant = false;
            }

        }

        protected void UpdateOvni(GameTime gametime)
        {
            if ((gametime.TotalGameTime.Seconds > 1) && (enemies.estVivant == true))
            {
                spawnTime += gametime.ElapsedGameTime.Milliseconds;
                if (spawnTime > 3000)
                {
                    if (nbennemies < ovni.Length)
                    {
                        nbennemies += 1;
                    }

                    spawnTime = 0;
                }
                for (int i = 0; i <= nbennemies - 1; i++)
                {
                    if (ovni[i].estVivant == false)
                    {
                        ovni[i].estVivant = true;
                        ovni[i].position.X = enemies.position.X;
                        ovni[i].position.Y = enemies.position.Y + 15;



                        switch (alVitesse.Next(0, 2))
                        {
                            case 1:
                                ovni[i].vitesseY = alVitesse.Next(-24, -4);
                                break;
                            case 0:
                                ovni[i].vitesseY = alVitesse.Next(5, 25);
                                break;
                        }
                        switch (alVitesse.Next(0, 2))
                        {
                            case 1:
                                ovni[i].vitesseX = alVitesse.Next(-24, -4);
                                break;
                            case 0:
                                ovni[i].vitesseX = alVitesse.Next(5, 25);
                                break;
                        }
                    }

                }


            }
            for (int i = 0; i < ovni.Length; i++)
            {
                if ((ovni[i].position.X < fenetre.Left) || (ovni[i].position.Y < fenetre.Top) || (ovni[i].position.Y > fenetre.Bottom))
                {
                    ovni[i].estVivant = false;

                }
            }


        }

        protected void UpdateBarreChargement(GameTime gametime)
        {
            tempsDeChargement += gametime.ElapsedGameTime.Milliseconds;

            if (ralenti)
            {
                if (tempsDeChargement > 500)
                {
                    pourcentagePar5 -= 4;
                    tempsDeChargement = 0;
                }
                if (pourcentagePar5 == 0)
                {
                    ralenti = false;
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

        //protected void PouvoirRenlentie(bool renlenti)
        //{
        //    if (renlenti)
        //    {
        //        enemies.vitesseY /= 2;
        //        for (int i = 0; i < ovni.Length; i++)
        //        {
        //            ovni[i].vitesseX /= 2;
        //            ovni[i].vitesseY /= 2;

        //        }


        //    }
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            #region "si est ralentit"
            if (ralenti)
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

                if ((heros.objetState == GameObjectAnime.etats.runDroit) || (heros.objetState == GameObjectAnime.etats.runDroit))
                {
                    spriteBatch.Draw(heros.sprite, heros.position, heros.spriteAfficher, Color.White);
                }
                else
                {
                    spriteBatch.Draw(heros.sprite, heros.position, heros.spriteAfficher, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }

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
                if ((heros.objetState == GameObjectAnime.etats.runDroit) || (heros.objetState == GameObjectAnime.etats.attenteDroite))
                {
                    spriteBatch.Draw(heros.sprite, heros.position, heros.spriteAfficher, Color.White);
                }
                else
                {
                    spriteBatch.Draw(heros.sprite, heros.position, heros.spriteAfficher, Color.White,0,Vector2.Zero,SpriteEffects.FlipHorizontally,0);
                }

            }
            #endregion

            if (pourcentagePar5 >= 20)
            {
                spriteBatch.DrawString(Load, "Appuyer sur SPACEBARRE pour activer le pouvoir", new Vector2(BarreChargementRouge.position.X + 250, BarreChargementRouge.position.Y), Color.White);

            }
            spriteBatch.End();




            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

    }
}
