using SQLite;

namespace Invent.Common.Models.Local
{
    public class LocalColor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
