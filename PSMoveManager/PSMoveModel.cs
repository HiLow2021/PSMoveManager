using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove
{
    public class PSMoveModel : IDisposable
    {
        internal IntPtr Move { get; private set; }
        public bool IsConnected => Move != IntPtr.Zero;

        public PSMoveModel()
        {
            Move = PSMoveUtility.Connect();
        }

        public PSMoveModel(int id)
        {
            Move = PSMoveUtility.ConnectById(id);
        }

        public void SetCalibrationPose(PSMoveCalibrationPoseType calibration_pose)
        {
            PSMoveAPI.psmove_set_calibration_pose(Move, calibration_pose);
        }

        public void SetCalibrationTransform(ref PSMove3AxisTransform transform)
        {
            PSMoveAPI.psmove_set_calibration_transform(Move, ref transform);
        }

        public void GetIdentityGravityCalibrationDirection(out PSMove3AxisVector out_a)
        {
            out_a = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_identity_gravity_calibration_direction(Move, ref out_a);
        }

        public void GetTransformedGravityCalibrationDirection(out PSMove3AxisVector out_a)
        {
            out_a = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_gravity_calibration_direction(Move, ref out_a);
        }

        public void GetIdentityMagnetometerCalibrationDirection(out PSMove3AxisVector out_m)
        {
            out_m = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_identity_magnetometer_calibration_direction(Move, ref out_m);
        }

        public void GetTransformedMagnetometerCalibrationDirection(out PSMove3AxisVector out_m)
        {
            out_m = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_magnetometer_calibration_direction(Move, ref out_m);
        }

        public void SetMagnetometerCalibrationDirection(ref PSMove3AxisVector m)
        {
            PSMoveAPI.psmove_set_magnetometer_calibration_direction(Move, ref m);
        }

        public void SetSensorDataBasis(PSMoveSensorDataBasisType basis_type)
        {
            PSMoveAPI.psmove_set_sensor_data_basis(Move, basis_type);
        }

        public void SetSensorDataTransform(ref PSMove3AxisTransform transform)
        {
            PSMoveAPI.psmove_set_sensor_data_transform(Move, ref transform);
        }

        public void GetTransformedMagnetometerDirection(out PSMove3AxisVector out_m)
        {
            out_m = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_magnetometer_direction(Move, ref out_m);
        }

        public void GetTransformedAccelerometerFrame3axisvector(PSMoveFrame frame, out PSMove3AxisVector out_a)
        {
            out_a = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_accelerometer_frame_3axisvector(Move, frame, ref out_a);
        }

        public void GetTransformedAccelerometerFrameDirection(PSMoveFrame frame, out PSMove3AxisVector out_a)
        {
            out_a = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_accelerometer_frame_direction(Move, frame, ref out_a);
        }

        public void GetTransformedGyroscopeFrame3axisvector(PSMoveFrame frame, out PSMove3AxisVector out_w)
        {
            out_w = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_transformed_gyroscope_frame_3axisvector(Move, frame, ref out_w);
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                PSMoveAPI.psmove_disconnect(Move);
                Move = IntPtr.Zero;
            }
        }

        public PSMoveBool IsExtConnected()
        {
            return PSMoveAPI.psmove_is_ext_connected(Move);
        }

        public PSMoveBool GetExtDeviceInfo(out PSMoveExtDeviceInfo info)
        {
            info = new PSMoveExtDeviceInfo();

            return PSMoveAPI.psmove_get_ext_device_info(Move, ref info);
        }

        public PSMoveBatteryLevel GetBattery()
        {
            return PSMoveAPI.psmove_get_battery(Move);
        }

        public int GetTemperature()
        {
            return PSMoveAPI.psmove_get_temperature(Move);
        }

        public float GetTemperatureInCelsius()
        {
            return PSMoveAPI.psmove_get_temperature_in_celsius(Move);
        }

        public byte GetTrigger()
        {
            return PSMoveAPI.psmove_get_trigger(Move);
        }

        public void GetAccelerometer(out int ax, out int ay, out int az)
        {
            ax = ay = az = 0;

            PSMoveAPI.psmove_get_accelerometer(Move, ref ax, ref ay, ref az);
        }

        public void GetGyroscope(out int gx, out int gy, out int gz)
        {
            gx = gy = gz = 0;

            PSMoveAPI.psmove_get_gyroscope(Move, ref gx, ref gy, ref gz);
        }

        public void GetAccelerometerFrame(PSMoveFrame frame, out float ax, out float ay, out float az)
        {
            ax = ay = az = 0;

            PSMoveAPI.psmove_get_accelerometer_frame(Move, frame, ref ax, ref ay, ref az);
        }

        public void GetGyroscopeFrame(PSMoveFrame frame, out float gx, out float gy, out float gz)
        {
            gx = gy = gz = 0;

            PSMoveAPI.psmove_get_gyroscope_frame(Move, frame, ref gx, ref gy, ref gz);
        }

        public void GetMagnetometer(out int mx, out int my, out int mz)
        {
            mx = my = mz = 0;

            PSMoveAPI.psmove_get_magnetometer(Move, ref mx, ref my, ref mz);
        }

        public void GetMagnetometerVector(out float mx, out float my, out float mz)
        {
            mx = my = mz = 0;

            PSMoveAPI.psmove_get_magnetometer_vector(Move, ref mx, ref my, ref mz);
        }

        public void GetMagnetometer3axisvector(out PSMove3AxisVector out_m)
        {
            out_m = new PSMove3AxisVector();

            PSMoveAPI.psmove_get_magnetometer_3axisvector(Move, ref out_m);
        }

        public void ResetMagnetometerCalibration()
        {
            PSMoveAPI.psmove_reset_magnetometer_calibration(Move);
        }

        public void SaveMagnetometerCalibration()
        {
            PSMoveAPI.psmove_save_magnetometer_calibration(Move);
        }

        public float GetMagnetometerCalibrationRange()
        {
            return PSMoveAPI.psmove_get_magnetometer_calibration_range(Move);
        }

        public PSMoveBool HasCalibration()
        {
            return PSMoveAPI.psmove_has_calibration(Move);
        }

        public void DumpCalibration()
        {
            PSMoveAPI.psmove_dump_calibration(Move);
        }

        public void EnableOrientation(PSMoveBool enabled)
        {
            PSMoveAPI.psmove_enable_orientation(Move, enabled);
        }

        public PSMoveBool HasOrientation()
        {
            return PSMoveAPI.psmove_has_orientation(Move);
        }

        public void GetOrientation(out float w, out float x, out float y, out float z)
        {
            w = x = y = z = 0;

            PSMoveAPI.psmove_get_orientation(Move, ref w, ref x, ref y, ref z);
        }

        public void ResetOrientation()
        {
            PSMoveAPI.psmove_reset_orientation(Move);
        }

        public void SetOrientationFusionType(PSMoveOrientationFusionType fusion_type)
        {
            PSMoveAPI.psmove_set_orientation_fusion_type(Move, fusion_type);
        }

        public PSMoveConnectionType ConnectionType()
        {
            return PSMoveAPI.psmove_connection_type(Move);
        }

        public PSMoveBool IsRemote()
        {
            return PSMoveAPI.psmove_is_remote(Move);
        }

        public string GetSerial()
        {
            return PSMoveAPI.psmove_get_serial(Move);
        }

        public PSMoveModelType GetModel()
        {
            return PSMoveAPI.psmove_get_model(Move);
        }

        public PSMoveBool Pair()
        {
            return PSMoveAPI.psmove_pair(Move);
        }

        public PSMoveBool PairCustom(string new_host_string)
        {
            return PSMoveAPI.psmove_pair_custom(Move, new_host_string);
        }

        public void SetRateLimiting(PSMoveBool enabled)
        {
            PSMoveAPI.psmove_set_rate_limiting(Move, enabled);
        }

        public void SetLeds(byte red, byte green, byte blue)
        {
            PSMoveAPI.psmove_set_leds(Move, red, green, blue);
        }

        public PSMoveBool SetLedPwmFrequency(long freq)
        {
            return PSMoveAPI.psmove_set_led_pwm_frequency(Move, (ulong)freq);
        }

        public void SetRumble(byte rumble)
        {
            PSMoveAPI.psmove_set_rumble(Move, rumble);
        }

        public PSMoveUpdateResult UpdateLeds()
        {
            return PSMoveAPI.psmove_update_leds(Move);
        }

        public int Poll()
        {
            return PSMoveAPI.psmove_poll(Move);
        }

        public PSMoveBool GetExtData(out byte[] data)
        {
            data = new byte[PSMoveAPI.PSMOVE_EXT_DATA_BUF_SIZE];

            return PSMoveAPI.psmove_get_ext_data(Move, ref data);
        }

        public PSMoveBool SendExtData(ref byte data, byte length)
        {
            return PSMoveAPI.psmove_send_ext_data(Move, ref data, length);
        }

        public int GetButtons()
        {
            return (int)PSMoveAPI.psmove_get_buttons(Move);
        }

        public void GetButtonEvents(out int pressed, out int released)
        {
            uint p = 0;
            uint r = 0;

            PSMoveAPI.psmove_get_button_events(Move, ref p, ref r);
            pressed = (int)p;
            released = (int)r;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
