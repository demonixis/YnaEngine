Yna Engine
==========

### Status

The engine is no more developped, please take a look at C3DE now.

Taje a look at the develop branch for more works.

- [C3DE](https://github.com/demonixis/C3DE)

### What is Yna Engine ?

Yna is a lightweight 2D and 3D game engine using MonoGame Framework. 
The aim of this project is to give to the developer the ability to create games in 2D or 3D easily on multiple platforms such as Windows Phone, Windows 8, or Linux. 
Yna is not a complex engine compared to others but it suitable for all developers who want an easy way to quickly create a prototype or a game.

### Platforms

Yna Engine is currently support many platforms
* Windows Desktop and Modern UI (DirectX 11 / OpenGL / SDL2)
* Windows Phone 7 & 8 (XNA)
* Linux & Mac OSX (OpenGL / SDL2)

### Sample 2D

```C#
public class AnimatedSprites : YnState2D
{
	private YnSprite background;
	private YnSprite playerSprite;

	public AnimatedSprites(string name)
		: base(name)
	{
		// Background
		background = new YnSprite("Scene/GreenGround");
		Add(background);

		// Player
		playerSprite = new YnSprite("Sprites/BRivera-malesoldier");
		spriteGroup.Add(playerSprite);
	}

	public override void Initialize()
	{
		base.Initialize();
		background.SetFullscreen();
		
		// Move the player and add a physics component
		playerSprite.Move(350, 350);
		playerSprite.AddComponent<SpritePhysics>().ForceInsideScreen = true;

		// Add animations
		var animator = playerSprite.AddComponent<SpriteAnimator>();
		animator.PrepareAnimation(64, 64);
		animator.AddAnimation("up", 0, 8, 25, false);
		animator.AddAnimation("left", 9, 17, 25, false);
		animator.AddAnimation("down", 18, 26, 25, false);
		animator.AddAnimation("right", 27, 35, 25, false);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		var animator = playerSprite.GetComponent<SpriteAnimator>();
		
		// Move the player
		if (YnG.Keys.Up)
		{
			playerSprite.Y -= 2;
			animator.Play("up");
		}
		
		// Etc.
	}
}
```

### Contributors
- Lead developer : Yannick Comte (@CYannick)
- Contributor : Alex Frêne (aka @Drakulo)
- Logo & graphics : Thomas Ruffier

### License
Microsoft public license