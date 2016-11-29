using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exercices01
{
    class GameObjectAnime
    {
        public Texture2D sprite;
        public Vector2 vitesse;
        public Vector2 direction;
        public Rectangle position;
        public Rectangle spriteAfficher; //Le rectangle affiché à l'écran
        public enum etats { attenteDroite, attenteGauche, runDroit, runGauche };
        public etats objetState;

        //Compteur qui changera le sprite affiché
        private int cpt = 0;
        //GESTION DES TABLEAUX DE SPRITES (chaque sprite est un rectangle dans le tableau)
        int runState = 0; //État de départ
        int nbEtatRun = 18; //Combien il y a de rectangles pour l’état “courrir”
        public Rectangle[] tabRunDroite =
            {
            new Rectangle(1675, 122, 180, 85),
            new Rectangle(1470, 122, 180, 85),
            new Rectangle(1270, 122, 180, 85),
            new Rectangle(1065, 122, 180, 85),
            new Rectangle(860, 122, 180, 85),
            new Rectangle(653, 122, 180, 85),
            new Rectangle(448, 122, 180, 85),
            new Rectangle(241, 122, 180, 85),
            new Rectangle(37, 9, 180, 85),
            new Rectangle(1675, 9, 180, 85),
            new Rectangle(1470, 9, 180, 85),
            new Rectangle(1270, 9, 180, 85),
            new Rectangle(1065, 9, 180, 85),
            new Rectangle(860, 9, 180, 85),
            new Rectangle(653, 9, 180, 85),
            new Rectangle(448, 9, 180, 85),
            new Rectangle(241, 9, 180, 85),
            new Rectangle(37, 9, 180, 85),

       };
        public Rectangle[] tabRunGauche =
            {
            new Rectangle(1675, 122, 150, 85),
            new Rectangle(1470, 122, 150, 85),
            new Rectangle(1270, 122, 150, 85),
            new Rectangle(1065, 122, 150, 85),
            new Rectangle(860, 122, 150, 85),
            new Rectangle(653, 122, 150, 85),
            new Rectangle(448, 122, 150, 85),
            new Rectangle(241, 122, 150, 85),
            new Rectangle(37, 9, 150, 85),
            new Rectangle(1675, 9, 150, 85),
            new Rectangle(1470, 9, 150, 85),
            new Rectangle(1270, 9, 150, 85),
            new Rectangle(1065, 9, 150, 85),
            new Rectangle(860, 9, 150, 85),
            new Rectangle(653, 9, 150, 85),
            new Rectangle(448, 9, 150, 85),
            new Rectangle(241, 9, 150, 85),
            new Rectangle(37, 9, 150, 85),

        };
        int waitState = 0;
        public Rectangle[] tabAttenteDroite =
        {
           new Rectangle(1675, 122, 180, 85),
        };


        public Rectangle[] tabAttenteGauche =
        {
              new Rectangle(1675, 122, 180, 85),
        };
        public virtual void Update(GameTime gameTime)
        {
            if (objetState == etats.attenteDroite)
            {
                spriteAfficher = tabAttenteDroite[waitState];
            }
            if (objetState == etats.attenteGauche)
            {
                spriteAfficher = tabAttenteGauche[waitState];
            }
            if (objetState == etats.runDroit)
            {
                spriteAfficher = tabRunDroite[runState];
            }
            if (objetState == etats.runGauche)
            {
                spriteAfficher = tabRunGauche[runState];
            }
            // Compteur permettant de gérer le changement d'images
            cpt++;
            if (cpt == 2) //Vitesse défilement
            {
                //Gestion de la course
                runState++;
                if (runState == nbEtatRun)
                {
                    runState = 0;
                }
                cpt = 0;
            }
        }
    }
}
