using Newtonsoft.Json;
using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class SysMenuRoleTreeDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 节点是否初始展开，默认 false
        /// </summary>
        public bool spread { get; set; } = true;

        /// <summary>
        /// 子集
        /// </summary>
        public List<SysMenuRoleTreeDto> children { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [JsonProperty(PropertyName = "checked")]
        public bool isChecked { get; set; } = false;


        //public int layer { get; set; }
        //public string parentID { get; set; }
        //public int sort { get; set; }
    }
}
