2MGFX

What is it ?
------------

It's a tool created by MonoGame team who allow you to convert your XNA shader (.fx) in MonoGame compatible format (.fxg)

How use it ?
------------

Open a command line and go in the 2MGFX directory. Place your fx file in the same folder and type :

# OpenGL compatibility
2MGFX yourShader.fx yourShader.fxg 

# DirectX 11 compatibility
2MGFX yourShader.fx yourShader.mgfxo \DX11

Place the file generated in your content directory and load it with the content manager. Don't worry because it's not an xna.

More informations ?
-------------------

https://github.com/mono/MonoGame/wiki/MonoGame-Content-Processing