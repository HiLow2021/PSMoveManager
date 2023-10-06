using System;
using System.Collections.Generic;
using System.Text;

namespace PSMove.EventArgs
{
    public class PSMoveButtonEventArgs : PSMoveEventArgs
    {
        public PSMove_Button Buttons { get; }

        public PSMoveButtonEventArgs(PSMoveModel model, int buttons) : base(model)
        {
            Buttons = (PSMove_Button)buttons;
        }
    }
}
