Param( $buildconf="Release" )

& "$PSScriptRoot\build-win.ps1" $buildconf

dotnet publish src\Dirigent.CLI\Dirigent.CLI.csproj -f net5.0-windows -r win-x64 --self-contained false -o src\Dirigent.CLI\publish\win-x64\$buildconf
dotnet publish src\Dirigent.Agent.WinForms\Dirigent.Agent.WinForms.csproj -f net5.0-windows -r win-x64 --self-contained false -o src\Dirigent.Agent.WinForms\publish\win-x64\$buildconf
dotnet publish src\Dirigent.Gui.ImGui\Dirigent.Gui.ImGui.csproj -f net5.0-windows -r win-x64 --self-contained false -o src\Dirigent.Gui.ImGui\publish\win-x64\$buildconf

New-Item -ItemType Directory -Force -Path release\win-x64\$buildconf

#robocopy src\Dirigent.Agent\publish\win-x64\$buildconf\ release\win-x64\$buildconf\ /MIR
robocopy src\Dirigent.Gui.ImGui\publish\win-x64\$buildconf release\win-x64\$buildconf\ /E
robocopy src\Dirigent.CLI\publish\win-x64\$buildconf release\win-x64\$buildconf\ /E
robocopy src\Dirigent.Agent.WinForms\publish\win-x64\$buildconf release\win-x64\$buildconf\ /E

Copy-Item "$PSScriptRoot\VersionStamp.txt" -Destination release\win-x64\$buildconf -Force

