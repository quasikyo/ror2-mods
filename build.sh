#!/bin/bash

if [[ $1 != "Debug" && $1 != "Release" ]]; then
	echo "Expecting 'Debug' or 'Release' argument" >&2
	exit 1
fi

dotnet build -c $1
cp ./RumbleRain/bin/$1/netstandard2.0/RumbleRain.dll ~/AppData/Roaming/r2modmanPlus-local/RiskOfRain2/profiles/Testing/BepInEx/plugins/quasikyo-RumbleRain/RumbleRain

if [[ $1 == "Release" ]]; then
	cp ./RumbleRain/bin/Release/netstandard2.0/RumbleRain.dll ./Thunderstore/plugins/RumbleRain/
	cp ./README.md ./Thunderstore/
fi
