using FreeSql.DataAnnotations;
using SmartCat.FreeSql;

namespace RemMai.Center.Api.Model
{
    [Table]
    public class User : IDataEntity<IMaiDbLocker>
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
