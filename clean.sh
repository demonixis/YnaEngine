echo "Project Cleaner"
echo "Start Cleaning..."

rm -Rf *.suo

rm -Rf Yna/bin
rm -Rf Yna/obj

rm -Rf Yna.Samples/Yna.Samples/bin
rm -Rf Yna.Samples/Yna.Samples/obj

rm -Rf Yna.Samples/Yna.SamplesContent/bin
rm -Rf Yna.Samples/Yna.SamplesContent/obj

rm -Rf Yna.Samples3D/Yna.Samples3D/bin
rm -Rf Yna.Samples3D/Yna.Samples3D/obj

rm -Rf Yna.Samples3D/Yna.Samples3DContent/bin
rm -Rf Yna.Samples3D/Yna.Samples3DContent/obj

echo "Cleaning done"