namespace DL.Domain.Dto.AdminDto.SysDto
{
    /// <summary>
    /// 角色授权显示权限值
    /// </summary>
    public class SysCodeDto
    {
        public string ID { get; set; }

        public string name { get; set; }

        public string codeType { get; set; }

        public bool status { get; set; } = false;
    }
}
