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

echo "$MAPS_KEY"

if [ ! -n "$MAPS_KEY" ]
then
    echo "You need define the MAPS_KEY variable in App Center"
    exit
fi

    echo "Updating Maps_Key to $MAPS_KEY in APIKeys.cs"
    sed -i '' 's#MapsKey = "YOUR KEY HERE"#MapsKey = "'$MAPS_KEY'"#' ../GeohashCross/Resources/APIKeys.cs
    echo "File content:"
    cat ../GeohashCross/Resources/APIKeys.cs