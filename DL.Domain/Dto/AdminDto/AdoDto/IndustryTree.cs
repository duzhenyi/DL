using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.AdoDto
{
    public class IndustryTree
    {
        public string title { get; set; }
        public string value { get; set; }

        public List<IndustryTree> children { get; set; }

        /// <summary>
        /// 不被序列化
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool open { get; set; } = true;
    }
}
