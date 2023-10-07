using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveButtonEventArgs : PSMoveEventArgs
    {
        public PSMoveButton Buttons { get; }

        public PSMoveButtonEventArgs(PSMoveModel model, int buttons) : base(model)
        {
            Buttons = (PSMoveButton)buttons;
        }
    }
}
