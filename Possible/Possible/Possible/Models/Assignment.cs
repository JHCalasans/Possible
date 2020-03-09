using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Possible.Models
{
    [Table("Assignment")]
    public class Assignment
    {
        [PrimaryKey, AutoIncrement]
        public int AssignmentID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public String Color { get; set; }
        public int ItemID { get; set; }

        public String DateString 
        { 
            get { return Date.ToString("MM/dd/yyyy"); }
        }


    }
}
