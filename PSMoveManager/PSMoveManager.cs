using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSMove
{
    public class PSMoveManager : IDisposable
    {
        public IList<PSMoveModel> Models { get; private set; } = new List<PSMoveModel>();
        public bool IsOpend { get; private set; }

        public PSMoveManager() { }
        public PSMoveManager(int expectedConnectionCount)
        {
            Open(expectedConnectionCount);
        }

        public void Open(int expectedConnectionCount = 1)
        {
            if (IsOpend)
            {
                return;
            }

            var isSucceeded = PSMoveUtility.Init(PSMoveAPI.PSMove_Version.PSMOVE_CURRENT_VERSION);

            if (isSucceeded == PSMove_Bool.PSMove_True)
            {
                for (int i = 0; i < expectedConnectionCount; i++)
                {
                    Models.Add(new PSMoveModel(i));
                }

                Models = Models.Where(x => x.IsConnected).ToList();

                for (int i = 0; i < Models.Count; i++)
                {
                    Models[i].PSMoveID = i;
                }
            }

            IsOpend = isSucceeded == PSMove_Bool.PSMove_True;
        }

        public void Close()
        {
            if (!IsOpend)
            {
                return;
            }

            foreach (var item in Models)
            {
                item.Dispose();
            }

            Models.Clear();
            IsOpend = false;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
