using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Possible.Models
{
    [Table("Item")]
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ItemID { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }

    }
}
