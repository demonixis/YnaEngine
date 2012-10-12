using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.State
{
    /// <summary>
    /// Define the state of a state
    /// </summary>
    public enum ScreenState
    {
        Active, Hidden, TransitionOn, TransitionOff
    }

    /// <summary>
    /// Define the type of a state
    /// </summary>
    public enum ScreenType
    {
        GameState, Popup
    }

    /// <summary>
    /// Abstract Game state (also named Sreen)
    /// </summary>
    public abstract class Screen
    {
    	/// <summary>
    	/// The screen activity : inactive screens will not be updated
    	/// </summary>
    	public bool IsActive { get; set; }
    	
    	/// <summary>
    	/// The exit in progress flag
    	/// </summary>
    	public bool IsExiting { get; set; }
    	
    	/// <summary>
    	/// The screen visibility
    	/// </summary>
    	public bool IsVisible { get; set; }
    	
    	/// <summary>
    	/// The screen type (popup or gamestate)
    	/// </summary>
    	public bool IsPopup { get; set; }
    	
    	/// <summary>
        /// The current Screen state
        /// </summary>
    	private ScreenState ScreenState { get ; set;}
    	
    	/// <summary>
    	/// The current "On" transition time
    	/// </summary>
    	public float TransitionOn { get; set; }
    	
    	private float _timeTransitionOff;
    	/// <summary>
    	/// The current "Off" transition time
    	/// </summary>
    	public float TransitionOff {
    		get
    		{
    			if(_timeTransitionOff <= 0)
    			{
    				_timeTransitionOff = 1;
    			}
    			return _timeTransitionOff;
    		}
    		set{_timeTransitionOff = value;}
    	}
        
    	/// <summary>
    	/// The transition counter
    	/// </summary>
    	private float TimeTransitionCounter { get; set; }
    	
    	/// <summary>
    	/// The current transition Alpha value 
    	/// </summary>
        private float TransitionAlpha { get; set; }

        protected SpriteBatch spriteBatch;
        protected ScreenManager screenManager;
        /// <summary>
        /// Get or Set the Screen Manager
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            set
            {
                screenManager = value;
                spriteBatch = value.SpriteBatch;
            }
        }

        /// <summary>
        /// Get or Set the clear color
        /// </summary>
        public Color ClearColor
        {
            get { return screenManager.ClearColor; }
            set { screenManager.ClearColor = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The screen type</param>
        /// <param name="timeTransitionOn">Fade in time on screen switch</param>
        /// <param name="timeTransitionOff">Fade out time on screen switch</param>
        public Screen(ScreenType type, float timeTransitionOn = 1500f, float timeTransitionOff = 0f)
        {
            IsActive = true;
            IsExiting = false;
            IsVisible = true;
            IsPopup = (type == ScreenType.Popup) ? true : false;
            ScreenState = ScreenState.TransitionOn;

            TransitionOn = timeTransitionOn;
            TransitionOff = timeTransitionOff;
            TimeTransitionCounter = TransitionOn;
            TransitionAlpha = 1.0f;
        }

        public virtual void Initialize()
        {
			// Nothing here
        }

        public virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(screenManager.Game.GraphicsDevice);
        }

        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Performs the transitions
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public virtual void Update(GameTime gameTime)
        {
        	// Handle transitions
            if (ScreenState == ScreenState.TransitionOn)
            {
            	// The screen is fading in
                if (TimeTransitionCounter <= 0)
                {
                	// The fade in is done, the screen is ready, set it as active
                    ScreenState = ScreenState.Active;
                }
                else
                {
                	// The transition is in progress, step it
                    UpdateTransition(gameTime, TransitionOn, 1);
                }

            }
            else if (ScreenState == ScreenState.TransitionOff)
            {
            	// The screen is fading out
                if (TimeTransitionCounter <= 0)
                {
                	// the fade out is done, remove the screen from the manager
                    screenManager.Remove(this);
                }
                else
                {
                	// The transition is in progress, go for the next step
                    UpdateTransition(gameTime, _timeTransitionOff, 1);
                }
            }
        }

        /// <summary>
        /// Update Transition effect
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        /// <param name="timer">Fade timer</param>
        /// <param name="direction">FadeIn or FadeOut</param>
        /// <returns></returns>
        private bool UpdateTransition(GameTime gameTime, float timer, int direction)
        {
            TimeTransitionCounter -= gameTime.ElapsedGameTime.Milliseconds * direction;
            TransitionAlpha = TimeTransitionCounter / timer;
            TransitionAlpha = MathHelper.Clamp(TransitionAlpha, 0, 1);
            return TransitionAlpha >= 1 || TransitionAlpha <= 0;
        }

        /// <summary>
        /// Draw the fade effects
        /// </summary>
        /// <param name="gameTime">Time since last draw</param>
        public virtual void Draw(GameTime gameTime)
        {
        	if (ScreenState == ScreenState.TransitionOn)
                screenManager.FadeBackBufferToBlack(TransitionAlpha);
            
            else if (ScreenState == ScreenState.TransitionOff)
                screenManager.FadeBackBufferToBlack(TransitionAlpha);  
        }

        /// <summary>
        /// Quit the screen
        /// </summary>
        public void Exit()
        {
            if (TransitionOff <= 0)
            {
                UnloadContent();
                screenManager.Remove(this);
            }
            else
            {
                TimeTransitionCounter = TransitionOff;
                ScreenState = ScreenState.TransitionOff;
                IsExiting = true;
            }
        }
    }
}
