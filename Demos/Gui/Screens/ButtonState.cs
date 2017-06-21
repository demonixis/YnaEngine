using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine.Graphics.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Yna.Samples.Screens
{
    /// <summary>
    /// Sample state for buttons.
    /// </summary>
    public class ButtonState : YnState2D
    {
        public ButtonState(string name)
            : base(name, false, true)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // The GUI set up must be done in the initialize method
            BuildGUI();
        }

        private void BuildGUI()
        {
            /*
            // Here is the simpliest creation of a text button. This button will be placed
            // at [0,0] as no position is set. The default button size is also used.
            YnTextButton basicButton = new YnTextButton("Simple button");
            Gui.Add(basicButton);

            // You can provide additionnal informations to place it on screen and give
            // it custom size. Note that creating a big button does not impact the font
            // used to render the inner text.
            YnTextButton sizedButton = new YnTextButton("This is a BIG sized button");
            sizedButton.X = 10;
            sizedButton.Y = 50;
            sizedButton.Width = 450;
            sizedButton.Height = 100;
            Gui.Add(sizedButton);

            // All widgets can have borders. These borders are fully customizable, from
            // plain colors to textures. The default skin does not have borders but you
            // can create yours. To use custom borders, you will first have to create a 
            // new skin. The simple way to do it is to generate one. This will generate
            // a skin you will be able to work with. This one is the default skin
            YnSkin customSkin = YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/DefaultFont");

            // We'll modify this skin to add the wanted borders. Here we go! The YnBorder
            // is just an generic aggregation of properties. It contains 8 textures, one
            // for each corner, and one for each border. You can fill it as you want.
            // For this example, we will create plain color borders. This is built in
            // and can be used like this :
            YnBorder customBorders = new YnBorder(Color.Firebrick, 3);

            // Skins have 3 states : default, hovered and clicked. These 3 states can be
            // customized freely. Note that the states hovered and clicked will be used
            // only if the widget has the flag Animated set to true. If a widget is animated
            // but it's skin does not provide a border definition for hover or clicked state,
            // the default border will be used. No need to define useless things.
            customSkin.BorderDefault = customBorders;

            // Now that our skin is built, we have to register it into the GUI with a name key :
            YnGui.RegisterSkin("simpleBorderSkin", customSkin);
            // By doing this, you make this skin available for all widgets. All you have to do now
            // is to change the SkinName property of a widget to make it use this skin instead
            // of the default one.

            // The skin is ready to use, we can now create our custom borered button!
            YnTextButton borderedButton = new YnTextButton("Bordered Button");
            borderedButton.X = 10;
            borderedButton.Y = 160;
            borderedButton.SkinName = "simpleBorderSkin"; // Change the skin used
            // Buttons does'nt have borders by default so we have to force it to true in order
            // to render the borders we've created.
            borderedButton.HasBorders = true;
            Gui.Add(borderedButton);

            // Note that borders are rendered outside of the widget's bounds. Keep this in mind when
            // managing your widgets position. This choice make possible to simply handle borders
            // with different sizes. Something like this :
            YnSkin armageddonSkin = YnSkinGenerator.Generate(Color.Red, "Fonts/DefaultFont");

            YnBorder armageddonBorder = new YnBorder();
            armageddonBorder.TopLeft = YnGraphics.CreateTexture(Color.RoyalBlue, 20, 16);
            armageddonBorder.Top = YnGraphics.CreateTexture(Color.Salmon, 1, 32);
            armageddonBorder.TopRight = YnGraphics.CreateTexture(Color.Turquoise, 20, 16);
            armageddonBorder.Right = YnGraphics.CreateTexture(Color.YellowGreen, 80, 1);
            armageddonBorder.BottomRight = YnGraphics.CreateTexture(Color.PaleTurquoise, 5, 5);
            armageddonBorder.Bottom = YnGraphics.CreateTexture(Color.MintCream, 1, 16);
            armageddonBorder.BottomLeft = YnGraphics.CreateTexture(Color.LightSalmon, 18, 40);
            armageddonBorder.Left = YnGraphics.CreateTexture(Color.HotPink, 12, 1);
            
            armageddonSkin.BorderDefault = armageddonBorder;
            YnGui.RegisterSkin("armageddon", armageddonSkin);

            YnTextButton armageddonButton = new YnTextButton("THIS IS MADNESS!");
            armageddonButton.X = 50;
            armageddonButton.Y = 250;
            armageddonButton.SkinName = "armageddon";
            armageddonButton.HasBorders = true;
            Gui.Add(armageddonButton);
            */
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Gamepad.Back(PlayerIndex.One) || YnG.Keys.JustPressed(Keys.Escape))
                YnG.SetStateActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
