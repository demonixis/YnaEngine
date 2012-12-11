echo "Project Cleaner"
echo "Start Cleaning..."

cd ../

rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store

rm -Rf Framework/bin
rm -Rf Framework/obj

rm -Rf Plugins/Wiimote/bin
rm -Rf Plugins/Wiimote/debug

rm -Rf Plugins/Kinect/bin
rm -Rf Plugins/Kinect/debug

echo "Cleaning done"