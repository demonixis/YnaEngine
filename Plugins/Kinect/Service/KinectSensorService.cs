using System;
using Microsoft.Xna.Framework;
using Yna.Helpers;
using Yna.Input.Kinect;

namespace Yna.Input.Service
{
    public class KinectSensorService : IKinectSensorService
    {
        private KinectSensorController kinect;

        public KinectSensorService(Game game)
        {
            kinect = KinectSensorController.Instance;
            game.Services.AddService(typeof(IKinectSensorService), this);
        }

        bool IKinectSensorService.IsAvailable()
        {
            return kinect.IsAvailable;
        }

        Vector3 IKinectSensorService.Head(KinectPlayerIndex index)
        {
            return kinect.GetUserProfile(index).Head;
        }

        Vector3 IKinectSensorService.HandLeft(KinectPlayerIndex index)
        {
            return kinect.GetUserProfile(index).HandLeft;
        }

        Vector3 IKinectSensorService.HandRight(KinectPlayerIndex index)
        {
            return kinect.GetUserProfile(index).HandRight;
        }

        Vector3 IKinectSensorService.FootLeft(KinectPlayerIndex index)
        {
            return kinect.GetUserProfile(index).FootLeft;
        }

        Vector3 IKinectSensorService.FootRight(KinectPlayerIndex index)
        {
            return kinect.GetUserProfile(index).FootRight;
        }
    }
}
