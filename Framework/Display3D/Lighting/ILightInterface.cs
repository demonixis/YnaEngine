using Microsoft.Xna.Framework;

namespace Yna.Framework.Display3D.Lighting
{
    public interface ILightDefault
    {
        Vector4 AmbientColor { get; set; }
        Vector4 DiffuseColor { get; set; }
        float AmbientIntensity { get; set; }
    }

    public interface ILightXnaDirectional
    {
        bool Enabled { get; set; }
        Vector3 DiffuseColor { get; set; }
        Vector3 SpecularColor { get; set; }
        Vector3 Direction { get; set; }
    }

    public interface ILightSpecular
    {
        Vector4 SpecularColor { get; set; }
        float SpecularIntensity { get; set; }
    }

    public interface ILightDiffuse
    {
        Vector4 DiffuseColorF { get; set; }
        Vector3 DiffuseDirectionF { get; set; }
        float DiffuseIntensity { get; set; }
    }

    public interface ILightSpotLight
    {

    }

    public interface ILightPointLight
    {

    }
}
