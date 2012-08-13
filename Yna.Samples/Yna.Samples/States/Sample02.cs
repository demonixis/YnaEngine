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
    public enum MovementState
    {
        JumpingUp, JumpingDown, Walking
    }

    public class Sample02 : YnState
    {
		private YnSprite background;  // Background
        private YnSprite sonicSprite; // Sprite du joueur

        // Gestion du saut
        protected MovementState movementState;
        protected int jumpHeight;              // Hauteur du saut
        protected Vector2 initialJumpPosition; // Position initiale
        protected float jumpSpeed;             // Vitesse du saut

        public Sample02() 
            : base(1500, 500) 
        {
            // 1 - Background
            background = new YnSprite(Vector2.Zero, "Backgrounds/sonic-background");
            Add(background);

            // 2 - Création d'un Sprite à la position 50, 50 en utilisant la texture soniclg4 du dossier 2d
            sonicSprite = new YnSprite(new Vector2(50, YnG.Height - 100), "Sprites/soniclg4");
            Add(sonicSprite);

            // Saut
            movementState = MovementState.Walking;
            jumpHeight = 90;
            jumpSpeed = 6.5f;
            initialJumpPosition = Vector2.Zero;
        }

        public override void Initialize ()
		{       
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
                Jump();
			}

            // Saut
            if (movementState != MovementState.Walking)
            {
                // Le Sprite est en train de sautter
                if (movementState == MovementState.JumpingUp)
                {
                    sonicSprite.Position = new Vector2(sonicSprite.X, sonicSprite.Position.Y - jumpSpeed);

                    if (sonicSprite.Y < (initialJumpPosition.Y - jumpHeight))
                        movementState = MovementState.JumpingDown;
                }

                // Le Sprite à terminé de sauter et il redescent
                if (movementState == MovementState.JumpingDown)
                    sonicSprite.Position = new Vector2(sonicSprite.X, sonicSprite.Position.Y + jumpSpeed);

                // Le saut est terminé on replace la position Y comme au départ
                if (sonicSprite.Y >= initialJumpPosition.Y)
                {
                    //_position = new Vector2(X, _initialJumpPosition.Y);
                    movementState = MovementState.Walking;
                }
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
        }

        public void Jump()
        {
            if (movementState == MovementState.Walking)
            {
                movementState = MovementState.JumpingUp;
                initialJumpPosition = new Vector2(sonicSprite.X, sonicSprite.Y);
            }
        }
	}
}
