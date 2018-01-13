C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe CleanUp.xml

rd /s /q Bin.Debug
rd /s /q Bin.Release
rd /s /q CodeMill.Sample\Demo\Output
rd /s /q CodeMill.Sample\Demo\DraftOutput

@if %errorlevel% neq 0 pause

del /a:H CodeMill.suo
del /a:H CodeMill.v12.suo