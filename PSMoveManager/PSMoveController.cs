﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PSMove.EventArgs;

namespace PSMove
{
    public class PSMoveController
    {
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        public PSMoveModel Model { get; }

        public string SerialNumber { get; private set; }
        public int Interval { get; set; } = 10;
        public int IntervalOnDisconnected { get; set; } = 1000;
        public bool IsConnected => Model.IsConnected;
        public PSMoveConnectionType ConnectionType { get; private set; }
        public bool IsDataAvailable { get; private set; }
        public PSMoveBatteryLevel BatteryLevel { get; private set; }
        public bool IsRunning { get; private set; }

        public event EventHandler<PSMoveStateEventArgs> Elapsed;
        public event EventHandler<PSMoveConnectionChangedEventArgs> ConnectionChanged;
        public event EventHandler<PSMoveButtonEventArgs> ButtonDown;
        public event EventHandler<PSMoveButtonEventArgs> ButtonUp;
        public event EventHandler<PSMoveBatteryLevelChangedEventArgs> BatteryLevelChanged;

        public PSMoveController(PSMoveModel model)
        {
            Model = model;
            SerialNumber = model.GetSerial();
            ConnectionType = model.ConnectionType();
        }

        public void Run()
        {
            if (IsRunning)
            {
                return;
            }
            if (Model.HasOrientation() != PSMoveBool.True)
            {
                Model.EnableOrientation(PSMoveBool.True);
            }

            try
            {
                IsRunning = true;
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
                Task.Run(PollingTask).ContinueWith(x => IsRunning = false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }
            if (Model.HasOrientation() == PSMoveBool.True)
            {
                Model.EnableOrientation(PSMoveBool.False);
            }

            tokenSource?.Cancel();
        }

        public void ResetOrientation()
        {
            Model.ResetOrientation();
        }

        public void SetLeds(byte red, byte green, byte blue)
        {
            SetLedsAndVibration(red, green, blue, 0);
        }

        public void SetLeds(Color color)
        {
            SetLedsAndVibration(color.R, color.G, color.B, 0);
        }

        public void SetVibration(byte intensity)
        {
            SetLedsAndVibration(0, 0, 0, intensity);
        }

        public void SetLedsAndVibration(byte red, byte green, byte blue, byte intensity)
        {
            Model.SetLeds(red, green, blue);
            Model.SetRumble(intensity);
            Model.UpdateLeds();
        }

        public void SetLedsAndVibration(Color color, byte intensity)
        {
            SetLedsAndVibration(color.R, color.G, color.B, intensity);
        }

        public void Dispose()
        {
            Stop();
            Model.Disconnect();
            ConnectionChanged?.Invoke(this, new PSMoveConnectionChangedEventArgs(Model, false, PSMoveConnectionType.Unknown, false));
            tokenSource?.Dispose();
        }

        private async Task PollingTask()
        {
            var previousIsConnected = IsConnected;
            var previousIsAvailable = IsDataAvailable;
            var previousBatteryLevel = BatteryLevel;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    IsDataAvailable = Model.Poll() != 0;
                    BatteryLevel = Model.GetBattery();

                    if (previousIsConnected != IsConnected || previousIsAvailable != IsDataAvailable)
                    {
                        ConnectionChanged?.Invoke(this, new PSMoveConnectionChangedEventArgs(Model, IsConnected, ConnectionType, IsDataAvailable));
                    }
                    if (previousBatteryLevel != BatteryLevel)
                    {
                        BatteryLevelChanged?.Invoke(this, new PSMoveBatteryLevelChangedEventArgs(Model, BatteryLevel));
                    }

                    previousIsConnected = IsConnected;
                    previousIsAvailable = IsDataAvailable;
                    previousBatteryLevel = BatteryLevel;

                    if (IsConnected && IsDataAvailable)
                    {
                        Model.GetButtonEvents(out int pressed, out int released);

                        if (pressed > 0)
                        {
                            ButtonDown?.Invoke(this, new PSMoveButtonEventArgs(Model, pressed));
                        }
                        if (released > 0)
                        {
                            ButtonUp?.Invoke(this, new PSMoveButtonEventArgs(Model, released));
                        }

                        var battery = Model.GetBattery();
                        var temperatureF = Model.GetTemperature();
                        var temperatureC = Model.GetTemperatureInCelsius();
                        var buttons = Model.GetButtons();
                        var trigger = Model.GetTrigger();
                        Model.GetOrientation(out float w, out float x, out float y, out float z);
                        var rotation = new Quaternion(x, y, z, w);

                        Elapsed?.Invoke(this, new PSMoveStateEventArgs(Model, SerialNumber, IsConnected, ConnectionType, IsDataAvailable, battery, temperatureF, temperatureC, buttons, trigger, rotation));

                        await Task.Delay(Interval, token);
                    }
                    else
                    {
                        await Task.Delay(IntervalOnDisconnected, token);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
