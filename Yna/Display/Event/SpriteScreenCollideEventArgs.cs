/*
 * Crée par SharpDevelop.
 * Utilisateur: CYannick
 * Date: 02/07/2012
 * Heure: 13:14
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
using System;

namespace Yna.Display.Event
{
	public enum SpriteScreenCollide
	{
		Top = 0, Bottom, Left, Right
	}
	
	/// <summary>
	/// Description of SpriteScreenCollideEventArgs.
	/// </summary>
	public class SpriteScreenCollideEventArgs : EventArgs
	{
		private bool [] _collide;
		
		public SpriteScreenCollideEventArgs(bool [] collide)
		{
			_collide = collide;
		}
	}
}
