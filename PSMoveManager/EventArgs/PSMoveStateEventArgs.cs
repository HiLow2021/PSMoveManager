﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveStateEventArgs : PSMoveEventArgs
    {
        public string SerialNumber { get; }

        public bool IsConnected { get; }

        public PSMoveConnectionType ConnectionType { get; }

        public bool IsDataAvailable { get; }

        public PSMoveBatteryLevel BatteryLevel { get; }

        public float TemperatureF { get; }

        public float TemperatureC { get; }

        public PSMoveButton Buttons { get; }

        public int Trigger { get; }

        public Vector3 EulerAngles { get; }

        public Quaternion Rotation { get; }

        public PSMoveStateEventArgs(PSMoveModel model, string serial, bool isConnected, PSMoveConnectionType connectionType,
            bool isDataAvailable, PSMoveBatteryLevel battery, float temperatureF, float temperatureC,
            int buttons, byte trigger, Quaternion rotation) : base(model)
        {
            SerialNumber = serial;
            IsConnected = isConnected;
            ConnectionType = connectionType;
            IsDataAvailable = isDataAvailable;
            BatteryLevel = battery;
            TemperatureF = temperatureF;
            TemperatureC = temperatureC;
            Buttons = (PSMoveButton)buttons;
            Trigger = trigger;
            Rotation = rotation;
            EulerAngles = QuaternionHelper.ToEulerAnglesInDegrees(Rotation);
        }
    }
}
