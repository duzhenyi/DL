namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class SysRoleDto
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string codes { get; set; }
        public string DepartmentGroup { get; set; }
        public string ParentId { get; set; }
        public int sort { get; set; }
        public int level { get; set; }
        public bool status { get; set; }
    }
}
