pushd %~dp0
start /Dconfig src\Dirigent.Agent.WinForms\bin\Debug\net6.0-windows\Dirigent.Agent.exe --isMaster 1 --machineId m1  --mode trayGui
popd