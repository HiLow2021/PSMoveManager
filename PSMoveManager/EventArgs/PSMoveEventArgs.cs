using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveEventArgs : System.EventArgs
    {
        public PSMoveModel Model { get; }

        public PSMoveEventArgs(PSMoveModel model)
        {
            Model = model;
        }
    }
}
