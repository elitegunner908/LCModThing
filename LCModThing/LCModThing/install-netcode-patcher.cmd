cd ../
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli --version 3.*
cd LCModThing
start /b del "install-netcode-patcher.cmd"
