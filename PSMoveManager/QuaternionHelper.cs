using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PSMove
{
    public static class QuaternionHelper
    {
        public static Vector3 ToEulerAngles(Quaternion rotation)
        {
            var q = rotation;
            double ysqr = q.Y * q.Y;

            // pitch (x-axis rotation)
            double t0 = 2.0f * (q.W * q.X + q.Y * q.Z);
            double t1 = 1.0f - 2.0f * (q.X * q.X + ysqr);
            double pitch = Math.Atan2(t0, t1);

            // yaw (y-axis rotation)
            double t2 = 2.0f * (q.W * q.Y - q.Z * q.X);

            t2 = (t2 > 1.0f) ? 1.0f : t2;
            t2 = (t2 < -1.0f) ? -1.0f : t2;

            double yaw = Math.Asin(t2);

            // roll (z-axis rotation)
            double t3 = 2.0f * (q.W * q.Z + q.X * q.Y);
            double t4 = 1.0f - 2.0f * (ysqr + q.Z * q.Z);
            double roll = Math.Atan2(t3, t4);

            return new Vector3((float)pitch, (float)yaw, (float)roll);
        }

        public static Vector3 ToEulerAnglesInDegrees(Quaternion rotation)
        {
            var anglesInRadians = ToEulerAngles(rotation);
            var xInDegrees = RadiansToDegrees(anglesInRadians.X);
            var yInDegrees = RadiansToDegrees(anglesInRadians.Y);
            var zInDegrees = RadiansToDegrees(anglesInRadians.Z);

            return new Vector3(MapTo360(xInDegrees), MapTo360(yInDegrees), MapTo360(zInDegrees));
        }

        private static float RadiansToDegrees(float radians)
        {
            return radians * (180f / (float)Math.PI);
        }

        private static float MapTo360(float degree)
        {
            return (degree < 0) ? degree + 360 : (degree == 360) ? 0 : degree;
        }
    }
}
