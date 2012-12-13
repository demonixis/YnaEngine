echo "Project Cleaner"
echo "Start Cleaning..."


rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store

rm -Rf Common/bin
rm -Rf Common/obj

rm -Rf Terrains/bin
rm -Rf Terrains/obj

rm -Rf Sprites/bin
rm -Rf Sprites/obj

rm -Rf Tilemap/bin
rm -Rf Tilemap/obj

echo "Cleaning done"