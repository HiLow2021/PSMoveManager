using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove
{
    public class PSMoveTracker : IDisposable
    {
        internal IntPtr Tracker { get; private set; }
        public bool IsConnected => Tracker != IntPtr.Zero;

        public PSMoveTracker()
        {
            Tracker = PSMoveUtility.TrackerNew();
        }

        public int Update(PSMoveModel model)
        {
            return PSMoveTrackerAPI.psmove_tracker_update(Tracker, model?.Move ?? IntPtr.Zero);
        }

        public void Annotate()
        {
            PSMoveTrackerAPI.psmove_tracker_annotate(Tracker);
        }

        public IntPtr GetFrame()
        {
            return PSMoveTrackerAPI.psmove_tracker_get_frame(Tracker);
        }

        public PSMoveTrackerRGBImage GetImage()
        {
            return PSMoveTrackerAPI.psmove_tracker_get_image(Tracker);
        }

        public int GetPosition(PSMoveModel model, out float x, out float y, out float radius)
        {
            x = y = radius = 0;

            return PSMoveTrackerAPI.psmove_tracker_get_position(Tracker, model.Move, ref x, ref y, ref radius);
        }

        public void GetSize(out int width, out int height)
        {
            width = height = 0;

            PSMoveTrackerAPI.psmove_tracker_get_size(Tracker, ref width, ref height);
        }

        public float DistanceFromRadius(float radius)
        {
            return PSMoveTrackerAPI.psmove_tracker_distance_from_radius(Tracker, radius);
        }

        public void SetDistanceParameters(float height, float center, float hwhm, float shape)
        {
            PSMoveTrackerAPI.psmove_tracker_set_distance_parameters(Tracker, height, center, hwhm, shape);
        }

        public void Free()
        {
            if (IsConnected)
            {
                PSMoveTrackerAPI.psmove_tracker_free(Tracker);
                Tracker = IntPtr.Zero;
            }
        }

        public void SetAutoUpdateLeds(PSMoveModel model, PSMoveBool auto_update_leds)
        {
            PSMoveTrackerAPI.psmove_tracker_set_auto_update_leds(Tracker, model.Move, auto_update_leds);
        }

        public PSMoveBool GetAutoUpdateLeds(PSMoveModel model)
        {
            return PSMoveTrackerAPI.psmove_tracker_get_auto_update_leds(Tracker, model.Move);
        }

        public void SetDimming(float dimming)
        {
            PSMoveTrackerAPI.psmove_tracker_set_dimming(Tracker, dimming);
        }

        public float GetDimming()
        {
            return PSMoveTrackerAPI.psmove_tracker_get_dimming(Tracker);
        }

        public void SetExposure(PSMoveTrackerExposure exposure)
        {
            PSMoveTrackerAPI.psmove_tracker_set_exposure(Tracker, exposure);
        }

        public PSMoveTrackerExposure GetExposure()
        {
            return PSMoveTrackerAPI.psmove_tracker_get_exposure(Tracker);
        }

        public void EnableDeinterlace(PSMoveBool enabled)
        {
            PSMoveTrackerAPI.psmove_tracker_enable_deinterlace(Tracker, enabled);
        }

        public void SetMirror(PSMoveBool enabled)
        {
            PSMoveTrackerAPI.psmove_tracker_set_mirror(Tracker, enabled);
        }

        public PSMoveBool GetMirror()
        {
            return PSMoveTrackerAPI.psmove_tracker_get_mirror(Tracker);
        }

        public PSMoveTrackerStatus Enable(PSMoveModel model)
        {
            return PSMoveTrackerAPI.psmove_tracker_enable(Tracker, model.Move);
        }

        public PSMoveTrackerStatus EnableWithColor(PSMoveModel model, byte red, byte green, byte blue)
        {
            return PSMoveTrackerAPI.psmove_tracker_enable_with_color(Tracker, model.Move, red, green, blue);
        }

        public void Disable(PSMoveModel model)
        {
            PSMoveTrackerAPI.psmove_tracker_disable(Tracker, model.Move);
        }

        public int GetColor(PSMoveModel model, out byte red, out byte green, out byte blue)
        {
            red = green = blue = 0;

            return PSMoveTrackerAPI.psmove_tracker_get_color(Tracker, model.Move, ref red, ref green, ref blue);
        }

        public int GetCameraColor(PSMoveModel model, out byte red, out byte green, out byte blue)
        {
            red = green = blue = 0;

            return PSMoveTrackerAPI.psmove_tracker_get_camera_color(Tracker, model.Move, ref red, ref green, ref blue);
        }

        public int SetCameraColor(PSMoveModel model, byte red, byte green, byte blue)
        {
            return PSMoveTrackerAPI.psmove_tracker_set_camera_color(Tracker, model.Move, red, green, blue);
        }

        public PSMoveTrackerStatus GetStatus(PSMoveModel model)
        {
            return PSMoveTrackerAPI.psmove_tracker_get_status(Tracker, model.Move);
        }

        public void UpdateImage()
        {
            PSMoveTrackerAPI.psmove_tracker_update_image(Tracker);
        }

        public void Dispose()
        {
            Free();
        }
    }
}
