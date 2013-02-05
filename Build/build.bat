@echo off


setlocal
set msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set flags=/nologo /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false
set flagsARM=/nologo /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false /p:Platform=ARM

pushd ..\Engine

call %msbuild% %flags% Yna.Linux.csproj
call %msbuild% %flags% Yna.Windows8.csproj
call %msbuild% %flags% Yna.WindowsGL.csproj
call %msbuild% %flags% Yna.WindowsDX.csproj
call %msbuild% %flags% Yna.WindowsPhone7.csproj
call %msbuild% %flagsARM% Yna.WindowsPhone8.csproj

popd

endlocal