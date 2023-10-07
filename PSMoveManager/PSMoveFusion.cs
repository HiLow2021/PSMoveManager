using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove
{
    public class PSMoveFusion : IDisposable
    {
        internal IntPtr Fusion { get; private set; }
        public bool IsValid => Fusion != IntPtr.Zero;

        public PSMoveFusion(PSMoveTracker tracker, float z_near, float z_far)
        {
            if (tracker.IsConnected)
            {
                Fusion = PSMoveUtility.FusionNew(tracker, z_near, z_far);
            }
        }

        public IntPtr GetProjectionMatrix()
        {
            return PSMoveTrackerAPI.psmove_fusion_get_projection_matrix(Fusion);
        }

        public IntPtr GetModelviewMatrix(PSMoveModel model)
        {
            return PSMoveTrackerAPI.psmove_fusion_get_modelview_matrix(Fusion, model.Move);
        }

        public void GetPosition(PSMoveModel model, out float x, out float y, out float z)
        {
            x = y = z = 0;

            PSMoveTrackerAPI.psmove_fusion_get_position(Fusion, model.Move, ref x, ref y, ref z);
        }

        public void Free()
        {
            if (IsValid)
            {
                PSMoveTrackerAPI.psmove_fusion_free(Fusion);
                Fusion = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Free();
        }
    }
}
