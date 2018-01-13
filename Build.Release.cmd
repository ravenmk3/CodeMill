@echo off

set msbuild=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

set sln="CodeMill.sln"

%msbuild% %sln% /t:Clean;Build /p:Configuration=Release

@if %errorlevel% neq 0 pause