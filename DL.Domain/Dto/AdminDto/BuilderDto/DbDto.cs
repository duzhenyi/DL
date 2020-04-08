namespace DL.Domain.Dto.AdminDto.BuilderDto
{
    public class DbDto
    {
        public string DbConn { get; set; }
        public int DbType { get; set; }
        public string TableName { get; set; }

        public int limit { get; set; }
        public int page { get; set; }
    }
}
