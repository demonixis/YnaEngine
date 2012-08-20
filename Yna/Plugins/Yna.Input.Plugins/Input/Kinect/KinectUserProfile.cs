using System;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Yna.Input.Kinect
{
    public enum KinectPlayerIndex
    {
        One = 0, Two
    }

    public class KinectUserProfile
    {
        public int Id { get; set; }

        public bool Available { get; protected set; }

        public Vector3 HandLeft { get; protected set; }

        public Vector3 HandRight { get; protected set; }

        public KinectUserProfile()
        {
            Available = false;
            HandLeft = Vector3.Zero;
            HandRight = Vector3.Zero;
        }

        public void SetVector3(JointType jointType, Vector3 position)
        {
            if (jointType == JointType.HandLeft)
                HandLeft = position;

            else if (jointType == JointType.HandRight)
                HandRight = position;
        }

        public void SetAvailable(bool available)
        {
            Available = available;
        }
    }
}
