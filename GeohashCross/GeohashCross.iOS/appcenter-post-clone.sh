#!/usr/bin/env bash
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"
echo ">>>>>>>>>>>>>>>>>>>>>>>>>> POST CLONE SCRIPT"

echo "This maps key not in quotes"
echo $MAPS_KEY
echo "This maps key in quotes"
echo "$MAPS_KEY"
echo "This maps key no dollar no quote"
echo MAPS_KEY
echo "This maps key with dollar in quotes no underscore"
echo "$MAPSKEY"

echo "About to check if there is a maps key"
if [ ! -n $MAPS_KEY ]
then
    echo "You need define the MAPS_KEY variable in App Center"
    exit 1
fi

    echo "Updating Maps_Key to $MAPS_KEY in APIKeys.cs"
    sed -i '' 's#MapsKey = "YOUR KEY HERE"#MapsKey = "'$MAPS_KEY'"#' ../GeohashCross/Resources/APIKeys.cs
    echo "File content:"
    cat ../GeohashCross/Resources/APIKeys.cs

echo "$AnalyticsIOSKey"
echo "$AnalyticsAndroidKey"

if [ -n "$AnalyticsIOSKey" ]
then
    echo "Updating AnalyticsIOSKey to $AnalyticsIOSKey in APIKeys.cs"
    sed -i '' 's#MapsKey = "YOUR KEY HERE"#MapsKey = "'$MAPS_KEY'"#' ../GeohashCross/Resources/APIKeys.cs


    exit
fi