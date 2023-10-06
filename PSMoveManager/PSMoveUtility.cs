using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove
{
    public static class PSMoveUtility
    {
        public static void Reinit()
        {
            PSMoveAPI.psmove_reinit();
        }

        public static long UtilGetTicks()
        {
            return PSMoveAPI.psmove_util_get_ticks();
        }

        public static string UtilGetDataDir()
        {
            return PSMoveAPI.psmove_util_get_data_dir();
        }

        public static string UtilGetFilePath(string filename)
        {
            return PSMoveAPI.psmove_util_get_file_path(filename);
        }

        public static string UtilGetSystemFilePath(string filename)
        {
            return PSMoveAPI.psmove_util_get_system_file_path(filename);
        }

        public static int UtilGetEnvInt(string name)
        {
            return PSMoveAPI.psmove_util_get_env_int(name);
        }

        public static string UtilGetEnvString(string name)
        {
            return PSMoveAPI.psmove_util_get_env_string(name);
        }

        public static void UtilSleepMs(int ms)
        {
            PSMoveAPI.psmove_util_sleep_ms((uint)ms);
        }

        public static void FreeMem(IntPtr buf)
        {
            PSMoveAPI.psmove_free_mem(buf);
        }

        public static PSMoveBool Init(PSMoveAPI.PSMoveVersion version)
        {
            return PSMoveAPI.psmove_init(version);
        }

        public static void SetRemoteConfig(PSMoveRemoteConfig config)
        {
            PSMoveAPI.psmove_set_remote_config(config);
        }

        public static int CountConnected()
        {
            return PSMoveAPI.psmove_count_connected();
        }

        public static IntPtr Connect()
        {
            return PSMoveAPI.psmove_connect();
        }

        public static IntPtr ConnectById(int id)
        {
            return PSMoveAPI.psmove_connect_by_id(id);
        }

        public static PSMoveBool HostPairCustom(string addr)
        {
            return PSMoveAPI.psmove_host_pair_custom(addr);
        }

        public static PSMoveBool HostPairCustomModel(string addr, PSMoveModelType model)
        {
            return PSMoveAPI.psmove_host_pair_custom_model(addr, model);
        }

        public static void TrackerSettingsSetDefault(ref PSMoveTrackerSettings settings)
        {
            PSMoveTrackerAPI.psmove_tracker_settings_set_default(ref settings);
        }

        public static IntPtr TrackerNew()
        {
            return PSMoveTrackerAPI.psmove_tracker_new();
        }

        public static IntPtr TrackerNewWithSettings(ref PSMoveTrackerSettings settings)
        {
            return PSMoveTrackerAPI.psmove_tracker_new_with_settings(ref settings);
        }

        public static IntPtr TrackerNewWithCamera(int camera)
        {
            return PSMoveTrackerAPI.psmove_tracker_new_with_camera(camera);
        }

        public static IntPtr TrackerNewWithCameraAndSettings(int camera, ref PSMoveTrackerSettings settings)
        {
            return PSMoveTrackerAPI.psmove_tracker_new_with_camera_and_settings(camera, ref settings);
        }

        public static int TrackerCountConnected()
        {
            return PSMoveTrackerAPI.psmove_tracker_count_connected();
        }

        public static IntPtr FusionNew(PSMoveTracker tracker, float z_near, float z_far)
        {
            return PSMoveTrackerAPI.psmove_fusion_new(tracker.Tracker, z_near, z_far);
        }
    }
}
