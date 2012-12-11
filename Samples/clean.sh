echo "Project Cleaner"
echo "Start Cleaning..."


rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store

rm -Rf Common/bin
rm -Rf Common/obj

rm -Rf Heightmap/bin
rm -Rf Heightmap/obj

rm -Rf Kinect/bin
rm -Rf Kinect/obj

rm -Rf Sprites/bin
rm -Rf Sprites/obj

rm -Rf Tilemap/bin
rm -Rf Tilemap/obj

rm -Rf Wiimote/bin
rm -Rf Wiimote/obj

echo "Cleaning done"