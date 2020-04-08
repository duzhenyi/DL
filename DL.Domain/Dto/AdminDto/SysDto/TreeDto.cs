using Newtonsoft.Json;
using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    /// <summary>
    /// 菜单树
    /// </summary>
    public class TreeDto
    {
        /// <summary>
        /// id主键
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
        public List<TreeDto> children { get; set; }
    }
}
