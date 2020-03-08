using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Possible.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
