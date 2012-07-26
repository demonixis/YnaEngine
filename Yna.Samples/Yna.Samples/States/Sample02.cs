using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display;
using Yna.Input;
using Yna.State;

namespace Yna.Sample.States
{
    public class Sample02 : YnState
    {
		Sprite background;  // Background
        Sprite sonicSprite; // Sprite du joueur

        public Sample02() 
            : base(1500, 500) { }

        public override void Initialize ()
		{       
			// 1 - Background
			background = new Sprite(Vector2.Zero, "Backgrounds//sonic-background");
			Add(background);
			
			// 2 - Création d'un Sprite à la position 50, 50 en utilisant la texture soniclg4 du dossier 2d
			sonicSprite = new Sprite (new Vector2 (50, 505), "Sprites//soniclg4");
            sonicSprite.LoadContent();
			
			// Indique que le Sprite est animé, renseigne la taille d'un Sprite sur la feuille de Sprite
			sonicSprite.PrepareAnimation (50, 41);
			
			// Ajoute des animations
			// 1 - Nom de l'animation
			// 2 - Tableau d'indices ciblant les frames
			// 3 - Temps d'affichage d'une frame
			// 4 - Indique si l'animation doit etre retournée
			sonicSprite.AddAnimation ("down", new int[] { 0, 1, 2, 3 }, 150, false);
			sonicSprite.AddAnimation ("left", new int[] { 4, 5, 6, 7 }, 150, false);
			sonicSprite.AddAnimation ("right", new int[] { 8, 9, 10, 11 }, 150, false);
			sonicSprite.AddAnimation ("up", new int[] { 12, 13, 14, 15 }, 150, false);
			
			// Modifie la taille du Sprite 
			// Tout est automatiquement mis à jour
			sonicSprite.Scale = new Vector2 (1.5f, 1.5f);

			// Force le sprite à rester sur l'écran
			sonicSprite.AllowAcrossScreen = true; 
			
			// Taux d'accéleration du personnage
			sonicSprite.Acceleration = new Vector2(2.5f, 1.5f);
            sonicSprite.VelocityMax = 0.95f;
			
			// Ajoute le Sprite à la scène
			Add (sonicSprite);
		}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Les objets attachés sont mis à jour
			
			// Déplacement du Sprite
			if (YnG.Keys.Pressed(Keys.Left))
            {
                sonicSprite.Play("left");
                sonicSprite.VelocityX -= 0.3f;
            }
            else if (YnG.Keys.Pressed(Keys.Right))
            {
                sonicSprite.Play("right");
                sonicSprite.VelocityX += 0.3f;
            }

            if (YnG.Keys.Pressed(Keys.Space))
			{
				sonicSprite.Play("up");
                sonicSprite.Jump();
			}

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
        }
	}
}
