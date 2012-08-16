using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Yna;

namespace Yna.Input.Kinect
{
    public class KinectController
    {
        private bool _isAvailable;

        private float maxX = 0.6f;
        private float maxY = 0.4f;

        public int HandLeftX = 0;
        public int HandLeftY = 0;
        public bool LeftClick = false;

        public int HandRightX = 0;
        public int HandRightY = 0;
        public bool RightClick = false;

        private KinectSensor kinect;

        public bool IsAvailable
        {
            get { return _isAvailable; }
            protected set { _isAvailable = value; }
        }

        public KinectController()
        {
            int nbKinect = KinectSensor.KinectSensors.Count;

            if (nbKinect > 0)
            {
                kinect = KinectSensor.KinectSensors[0];

                TransformSmoothParameters parameters = new TransformSmoothParameters
                {
                    Correction = 0.3f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f,
                    Prediction = 0.4f,
                    Smoothing = 0.7f
                };

                kinect.SkeletonStream.Enable(parameters);

                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                try
                {
                    kinect.Start();
                    if (kinect.Status == KinectStatus.Connected)
                        _isAvailable = true;
                }
                catch (System.IO.IOException ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
            else
                _isAvailable = false;
        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];

                skeletonFrame.CopySkeletonDataTo(skeletons);

                foreach (Skeleton sk in skeletons)
                {
                    if (sk.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (sk.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                            sk.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                        {
                            Joint jointRight = sk.Joints[JointType.HandRight];
                            Joint jointLeft = sk.Joints[JointType.HandLeft];

                            Joint scaledRight = jointRight.ScaleTo(YnG.DeviceWidth, YnG.DeviceHeight, maxX, maxY);
                            Joint scaledLeft = jointLeft.ScaleTo(YnG.DeviceWidth, YnG.DeviceHeight, maxX, maxY);

                            HandLeftX = (int)scaledLeft.Position.X;
                            HandLeftY = (int)scaledLeft.Position.Y;

                            HandRightX = (int)scaledRight.Position.X;
                            HandRightY = (int)scaledRight.Position.Y;

                            return;
                        }
                    }
                }
            }
        }
    }
}
