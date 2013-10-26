@echo off

setlocal
set msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set flags=/nologo /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false

pushd .

call %msbuild% %flags% Engine.SDL2.sln
call %msbuild% %flags% Engine.Windows8.sln
call %msbuild% %flags% Engine.Windows.sln
call %msbuild% %flags% Engine.WindowsPhone.sln

popd

endlocal