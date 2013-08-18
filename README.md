Yna Engine
==========

### Status

Yna is no more maintained, I'll work on another engine later, stay connected!

### What is Yna Engine ?

Yna is a lightweight 2D and 3D game engine using MonoGame Framework (or XNA). The goal of this project is to give the developer the ability to create games in 2D or 3D easily on multiple platforms such as Windows Phone, Windows 8, or Linux. Yna is not a complex engine compared to its competitor and suitable for all developers who want an easy way to quickly create a prototype or a game.

### Platforms

Yna Engine is currently support many platforms
* Windows XP / Vista / 7 / 8 (Desktop)
* Windows 8 & RT (Modern UI)
* Windows Phone 7 & 8
* Linux
* Mac OSX

Do you want to see it working on another platform ? Fork it and send us a pull request.

### Sample 2D

```C#
public class AnimatedSprites : YnState2D
{
	private YnEntity background;
	private YnSprite playerSprite;

	public AnimatedSprites(string name)
		: base(name)
	{
		// Background
		background = new YnEntity("Scene/GreenGround");
		Add(background);

		// Player
		playerSprite = new YnSprite("Sprites/BRivera-malesoldier");
		spriteGroup.Add(playerSprite);
	}

	public override void Initialize()
	{
		base.Initialize();
		background.SetFullscreen();
		playerSprite.Position = new Vector2(350, 350);
		playerSprite.ForceInsideScreen = true;

		// Add animations
		playerSprite.PrepareAnimation(64, 64);
		playerSprite.AddAnimation("up", 0, 8, 25, false);
		playerSprite.AddAnimation("left", 9, 17, 25, false);
		playerSprite.AddAnimation("down", 18, 26, 25, false);
		playerSprite.AddAnimation("right", 27, 35, 25, false);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		// Move the player
		if (YnG.Keys.Up)
		{
			playerSprite.Y -= 2;
			playerSprite.Play("up");
		}
		
		// Etc.
		
		// Shake the screen
		if (YnG.Keys.JustPressed(Keys.S))
			Camera.Shake(15, 2500);
	}
}
```

### Contributors
* Lead developer : Yannick Comte (@CYannick)
* Contributor : Alex Frêne (aka @Drakulo)
* Logo & graphics : Thomas Ruffier

### License
Microsoft public license. Take a look on LICENSE file for more informations