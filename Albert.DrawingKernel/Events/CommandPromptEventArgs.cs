using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Events
{
   public class CommandPromptEventArgs:EventArgs
    {
       public CommandPromptEventArgs(string message)
       {
           Message = message;
       }
       public string Message { get; set; }
    }
}
