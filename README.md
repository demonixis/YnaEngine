Yna Engine
==========

### Status



### What is Yna Engine ?

Yna is a lightweight 2D and 3D game engine using the awesome MonoGame Framework. It's designed to quickly make prototypes or small games. The engine is no more developped, please take a look at [C3DE](https://github.com/demonixis/C3DE) now.

### Platforms
* Windows (DirectX)

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
This project is licensed under the Microsoft public license
