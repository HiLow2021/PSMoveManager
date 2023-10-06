using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveBatteryLevelChangedEventArgs : PSMoveEventArgs
    {
        public PSMove_Battery_Level BatteryLevel { get; }

        public PSMoveBatteryLevelChangedEventArgs(PSMoveModel model, PSMove_Battery_Level battery) : base(model)
        {
            BatteryLevel = battery;
        }
    }
}
