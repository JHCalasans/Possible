using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Possible.Models
{
    public class ListObject : ObservableCollection<Assignment>
    {
        public ObservableCollection<Assignment> Assignments { get; set; }

        public String ItemDescription{ get; set; }

        public int ItemID { get; set; }

 
    }
}
