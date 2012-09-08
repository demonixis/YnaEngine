using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Yna;

namespace Yna.Input.Kinect
{
    public class KinectSensorController
    {
        private static KinectSensorController _instance;
        private static readonly object _lock = new object();

        private bool _isAvailable;

        private float maxX = 0.4f;
        private float maxY = 0.4f;
        
        private KinectUserProfile [] _userProfiles;

        private KinectSensor _kinectSensor;

        private int _playerCount;

        #region Properties

        /// <summary>
        /// Get or Set the status of the Device
        /// </summary>
        public bool IsAvailable
        {
            get { return _isAvailable; }
            protected set { _isAvailable = value; }
        }

        /// <summary>
        /// Number of Kinect device
        /// </summary>
        private int Count 
        {
            get { return KinectSensor.KinectSensors.Count; }
        }

        /// <summary>
        /// Get the number of players
        /// </summary>
        public int PlayerCount 
        {
            get { return _playerCount; }
            protected set { _playerCount = value; }
        }

        #endregion

        private KinectSensorController()
        {
            _userProfiles = new KinectUserProfile[2];
            for (int i = 0; i < 2; i++)
                _userProfiles[i] = new KinectUserProfile();

            Initialize();
        }

        public static KinectSensorController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new KinectSensorController();
                            
                        }
                    }
                }

                return _instance;
            }
        }

        private void Initialize()
        {
            _playerCount = 0;

            for (int i = 0; i < 2; i++)
                _userProfiles[i] = new KinectUserProfile();

            if (Count > 0)
            {
                // We take the first (for now)
                _kinectSensor = KinectSensor.KinectSensors[0];

                // We adjust the defaults parameters for the sensor
                TransformSmoothParameters parameters = new TransformSmoothParameters
                {
                    Correction = 0.3f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f,
                    Prediction = 0.4f,
                    Smoothing = 0.7f
                };

                _kinectSensor.SkeletonStream.Enable(parameters);

                _kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                try
                {
                    _kinectSensor.Start();
                    _isAvailable = true;
                }
                catch (System.IO.IOException ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    _isAvailable = false;
                }
            }
            else
                _isAvailable = false;
        }

        public KinectUserProfile GetUserProfile(KinectPlayerIndex index)
        {
            return _userProfiles[(int)index];
        }

        #region Events

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                // No player
                if (skeletonFrame == null)
                    return;

                int skeletonsSize = skeletonFrame.SkeletonArrayLength;

                Skeleton[] skeletons = new Skeleton[skeletonsSize];

                skeletonFrame.CopySkeletonDataTo(skeletons);

                int currentPlayer = 0; // Current player

                for (int i = 0; i < skeletonsSize; i++)
                {
                    Skeleton sk = skeletons[i];

                    if (sk.TrackingState == SkeletonTrackingState.Tracked)
                    {

                        if (sk.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                            sk.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                        {
                            Joint jointRight = sk.Joints[JointType.HandRight];
                            Joint jointLeft = sk.Joints[JointType.HandLeft];

                            Joint scaledRight = jointRight.ScaleTo(YnG.DeviceWidth, YnG.DeviceHeight, maxX, maxY);
                            Joint scaledLeft = jointLeft.ScaleTo(YnG.DeviceWidth, YnG.DeviceHeight, maxX, maxY);

                            _userProfiles[currentPlayer].SetVector3(scaledLeft.JointType, new Vector3(
                                scaledLeft.Position.X,
                                scaledLeft.Position.Y,
                                scaledLeft.Position.Z));

                            _userProfiles[currentPlayer].SetVector3(scaledRight.JointType, new Vector3(
                               scaledRight.Position.X,
                               scaledRight.Position.Y,
                               scaledRight.Position.Z));
                        }

                        _userProfiles[currentPlayer].SetVector3(JointType.Head, new Vector3(
                            sk.Joints[JointType.Head].Position.X,
                            sk.Joints[JointType.Head].Position.Y,
                            sk.Joints[JointType.Head].Position.Z));

                        _userProfiles[currentPlayer].SetVector3(JointType.FootLeft, new Vector3(
                            sk.Joints[JointType.FootLeft].Position.X,
                            sk.Joints[JointType.FootLeft].Position.Y,
                            sk.Joints[JointType.FootLeft].Position.Z));

                        _userProfiles[currentPlayer].SetVector3(JointType.FootRight, new Vector3(
                            sk.Joints[JointType.FootRight].Position.X,
                            sk.Joints[JointType.FootRight].Position.Y,
                            sk.Joints[JointType.FootRight].Position.Z));

                        return; // TODO : We must remove that and count the number of human tracked (2 max)
                    }
                }
            }
        }

        #endregion
    }
}
