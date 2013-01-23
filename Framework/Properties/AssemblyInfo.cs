using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Les informations générales sur l’assembly sont contrôlées par 
// les attributs suivants. Modifiez la valeur de ces attributs pour modifier les informations
// associées à l’assembly.
[assembly: AssemblyTitle("Yna Framework")]
#if ANDROID 
[assembly: AssemblyProduct("Yna Framework for Android")]
#elif LINUX
[assembly: AssemblyProduct("Yna Framework for Linux")]
#elif MACOSX
[assembly: AssemblyProduct("Yna Framework for Mac OSX")]
#elif MONOGAME && WINDOWS && OPENGL
[assembly: AssemblyProduct("Yna Framework for Windows (OpenGL)")]
#elif MONOGAME && WINDOWS && DIRECTX
[assembly: AssemblyProduct("Yna Framework for Windows (DirectX)")]
#elif WINDOWS_PHONE_7
[assembly: AssemblyProduct("Yna Framework for Windows Phone 7")]
#elif WINDOWS_PHONE_8
[assembly: AssemblyProduct("Yna Framework for Windows Phone 8")]
#elif WINDOWS_STOREAPP
[assembly: AssemblyProduct("Yna Framework for Windows 8 & Windows RT")]
#elif XNA
[assembly: AssemblyProduct("Yna Framework for PC/XNA")]
#else
[assembly: AssemblyProduct("Yna Framework")]
#endif
[assembly: AssemblyDescription("Yna Game Framework")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("Copyright ©  2012-2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Si vous définissez ComVisible sur False, les types de cet assembly ne sont plus visibles 
// pour les composants COM. Si vous devez accéder à un type dans cet assembly à partir de 
// COM, définissez l’attribut ComVisible sur True pour ce type. Seuls les assemblys
// Windows prennent COM en charge.
[assembly: ComVisible(false)]

// Dans Windows, le GUID suivant est l’ID de typelib si ce 
// projet est exposé à COM. Sur les autres plateformes, il identifie de façon unique
// le conteneur de stockage lorsque l’assembly est déployé sur l’appareil.
[assembly: Guid("3fbfc831-9c98-4437-b245-afd01e77e624")]

// Les informations de version de l’assembly comprennent les valeurs suivantes :
//
//      Version majeure
//      Version mineure 
//      Numéro de version
//      Révision
//
[assembly: AssemblyVersion("1.0.0.5")]
