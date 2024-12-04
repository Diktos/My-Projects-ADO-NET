using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cafe
{
    public class BaseUserControl : UserControl
    {
        public int? Id { get; set; }
        public BaseUserControl():base() { }
        public virtual void Refresh()
        {
            
        }
    }
}
