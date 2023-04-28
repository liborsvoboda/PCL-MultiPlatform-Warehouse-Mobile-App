using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.PclPrintService.Base
{
    public abstract class TcpSocketBase : CommonSocketBase
    {
        protected readonly int BufferSize;

        protected TcpSocketBase(int bufferSize)
        {
            BufferSize = bufferSize;
        }
    }
}
