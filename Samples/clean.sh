echo "Project Cleaner"
echo "Start Cleaning..."


rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store
rm -Rf *.cachefile
rm -Rf *.userpref
rm -Rf *.v11

rm -Rf */bin
rm -Rf */obj

echo "Cleaning done"