#!/bin/bash

../Scripts/setPerformance.sh
sudo cset shield --exec  -- dotnet run --configuration Release -- --verbose -r -z
../Scripts/setPowerSave.sh
