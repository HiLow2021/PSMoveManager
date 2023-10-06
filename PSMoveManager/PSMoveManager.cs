using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PSMove
{
    public class PSMoveManager : IDisposable
    {
        public IList<PSMoveController> Controllers { get; private set; } = new List<PSMoveController>();
        public bool IsOpend { get; private set; }

        public PSMoveManager() { }
        public PSMoveManager(int desiredConnectionCount)
        {
            Open(desiredConnectionCount);
        }

        public void Open(int desiredConnectionCount = 1)
        {
            if (IsOpend)
            {
                return;
            }

            var isSucceeded = PSMoveUtility.Init(PSMoveAPI.PSMove_Version.PSMOVE_CURRENT_VERSION);

            if (isSucceeded == PSMove_Bool.PSMove_True)
            {
                for (int i = 0; i < desiredConnectionCount; i++)
                {
                    Controllers.Add(new PSMoveController(new PSMoveModel(i)));
                }

                Controllers = Controllers.Where(x => x.IsConnected).ToList();
            }

            IsOpend = isSucceeded == PSMove_Bool.PSMove_True;
        }

        public void Close()
        {
            if (!IsOpend)
            {
                return;
            }

            foreach (var item in Controllers)
            {
                item.Dispose();
            }

            Controllers.Clear();
            IsOpend = false;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
