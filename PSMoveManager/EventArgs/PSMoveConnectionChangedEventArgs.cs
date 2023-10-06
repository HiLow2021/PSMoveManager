using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveConnectionChangedEventArgs : PSMoveEventArgs
    {
        public bool IsConnected { get; }

        public PSMoveConnectionType ConnectionType { get; }

        public bool IsDataAvailable { get; }

        public PSMoveConnectionChangedEventArgs(PSMoveModel model, bool isConnected, PSMoveConnectionType connectionType, bool isDataAvailable) : base(model)
        {
            IsConnected = isConnected;
            ConnectionType = connectionType;
            IsDataAvailable = isDataAvailable;
        }
    }
}
