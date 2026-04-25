@echo off

if "%~1"=="" (
    echo Usage: run file.jvs
    exit /b
)

JVS.exe "%~1" "%~n1.bat"

start "" "%~n1.bat"