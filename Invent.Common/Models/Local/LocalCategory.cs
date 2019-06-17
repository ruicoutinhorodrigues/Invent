using SQLite;

namespace Invent.Common.Models.Local
{
    public class LocalCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
