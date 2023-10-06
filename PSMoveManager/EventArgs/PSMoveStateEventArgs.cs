using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveStateEventArgs : PSMoveEventArgs
    {
        public bool IsConnected { get; }

        public PSMove_Connection_Type ConnectionType { get; }

        public bool IsDataAvailable { get; }

        public PSMove_Battery_Level BatteryLevel { get; }

        public float Temperature { get; }

        public PSMove_Button Buttons { get; }

        public int Trigger { get; }

        public Vector3 EulerAngles { get; }

        public Quaternion Rotation { get; }

        public PSMoveStateEventArgs(PSMoveModel model, bool isConnected, PSMove_Connection_Type connectionType,
            bool isDataAvailable, PSMove_Battery_Level battery, float temperature,
            int buttons, byte trigger, Quaternion rotation) : base(model)
        {
            IsConnected = isConnected;
            ConnectionType = connectionType;
            IsDataAvailable = isDataAvailable;
            BatteryLevel = battery;
            Temperature = temperature;
            Buttons = (PSMove_Button)buttons;
            Trigger = trigger;
            Rotation = rotation;
            EulerAngles = QuaternionHelper.ToEulerAnglesInDegrees(Rotation);
        }
    }
}
