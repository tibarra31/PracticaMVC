using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.EN
{
    public class DBResponse<T>
    {
        public bool ExecutionOK;
        public string Message;
        public T Data;
        public int NumRows;
    }
}
