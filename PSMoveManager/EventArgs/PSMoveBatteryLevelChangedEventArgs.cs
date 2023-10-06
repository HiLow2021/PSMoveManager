using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveBatteryLevelChangedEventArgs : PSMoveEventArgs
    {
        public PSMoveBatteryLevel BatteryLevel { get; }

        public PSMoveBatteryLevelChangedEventArgs(PSMoveModel model, PSMoveBatteryLevel battery) : base(model)
        {
            BatteryLevel = battery;
        }
    }
}
