#!/bin/bash

if [[ $2 != "Debug" && $2 != "Release" ]]; then
	echo "Expecting 'Debug' or 'Release' argument" >&2
	exit 1
fi

dotnet build ./$1/$1.csproj -c $2
cp ./$1/bin/$2/netstandard2.1/$1.dll ~/AppData/Roaming/r2modmanPlus-local/RiskOfRain2/profiles/Testing/BepInEx/plugins/quasikyo-$1/$1

if [[ $2 == "Release" ]]; then
	cp ./$1/bin/Release/netstandard2.1/$1.dll ./$1/Thunderstore/plugins/$1/
	cp ./$1/README.md ./$1/Thunderstore/
fi
