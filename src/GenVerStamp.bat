

@echo off
IF [%1]==[] echo Missing output file full path as parameter&&exit 2
 
SET OUT="%1"

:: use additional parameters as version identification
SET VER_STR=%2



echo Source Code: > %OUT%
IF "%VER_STR%" NEQ "" echo %VER_STR% >> %OUT%


git config --get remote.origin.url >> %OUT%
git rev-parse HEAD >> %OUT%
git log -1 --date=iso --format=%%cd >> %OUT%
echo. >> %OUT%

pushd %~dp0
SET GIT_STATUS=git status --untracked-files=no --porcelain
SET LOCALMODS=
for /F %%i in ('"%GIT_STATUS%"') do SET LOCALMODS=1
popd
if "%LOCALMODS%" EQU "" goto NO_LOCAL_MODS
echo WARNING: LOCAL MODIFICATIONS! >> %OUT%
%GIT_STATUS% >> %OUT%
:NO_LOCAL_MODS
echo. >> %OUT%

