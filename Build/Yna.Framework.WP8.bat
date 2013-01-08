@echo off


setlocal
set msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set flags=/nologo /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false /p:Platform=ARM

pushd ..\Framework

call %msbuild% %flags% Yna.WindowsPhone8.csproj

popd

endlocal