using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_Processes
{
    internal class ProcessInfo // клас для зберігання інформації про процес (модель даних для відображення у UI - формі)
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Memory { get; set; }
        public string Priority { get; set; }
    }
}
