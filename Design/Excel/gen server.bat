set WORKSPACE=..\..

set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Tools\Luban\Luban.dll
set CONF_ROOT1=%WORKSPACE%\Design\Excel\Game
set CONF_ROOT2=%WORKSPACE%\Design\Excel\GameHot

dotnet %LUBAN_DLL% ^
    -t server ^
    -c cs-bin ^
    -d bin  ^
    --conf %CONF_ROOT1%\lubanserver.conf ^
    -x outputCodeDir=%WORKSPACE%\Server\Luban\Code ^
    -x outputDataDir=%WORKSPACE%\Server\Luban\Data ^
    -x l10n.provider=default ^
    -x l10n.textFile.path=Localization.xlsx ^
    -x l10n.textFile.keyFieldName=key

dotnet %LUBAN_DLL% ^
    -t server ^
    -c cs-bin ^
    -d bin  ^
    --conf %CONF_ROOT2%\lubanserver.conf ^
    -x outputCodeDir=%WORKSPACE%\Server\Luban\Code ^
    -x outputDataDir=%WORKSPACE%\Server\Luban\Data ^
    -x l10n.provider=default ^
    -x l10n.textFile.path=Localization.xlsx ^
    -x l10n.textFile.keyFieldName=key

pause