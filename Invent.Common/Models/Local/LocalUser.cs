using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Models.Local
{
    public class LocalUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }


        public string Password { get; set; }
    }
}
