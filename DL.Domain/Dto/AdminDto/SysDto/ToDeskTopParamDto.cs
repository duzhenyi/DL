namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class ToDeskTopParamDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 是否置顶桌面
        /// </summary>
        public bool isDeskTop { get; set; }
    }
}
