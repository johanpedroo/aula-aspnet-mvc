using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todoList.Client.Logic
{
    public class TratarException
    {
        public static string ErrorMessage(Exception ex)
        {
            var msg =
                string.IsNullOrWhiteSpace(ex.Message) ?
                ex.InnerException.Message :
                ex.Message;

            return msg;
        }
    }
}
