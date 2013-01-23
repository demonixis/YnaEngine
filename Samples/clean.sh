echo "Project Cleaner"
echo "Start Cleaning..."


rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store
rm -Rf *.cachefile
rm -Rf *.userpref
rm -Rf *.v11

rm -Rf Common/bin
rm -Rf Common/obj

rm -Rf Gui/bin
rm -Rf Gui/obj

rm -Rf Models3D/bin
rm -Rf Models3D/obj

rm -Rf SamplesContent/bin
rm -Rf SamplesContent/obj

rm -Rf Terrains/bin
rm -Rf Terrains/obj

rm -Rf Sprites/bin
rm -Rf Sprites/obj

rm -Rf Storage/bin
rm -Rf Storage/obj

rm -Rf Tilemap/bin
rm -Rf Tilemap/obj

echo "Cleaning done"