using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class SysCodeTypeDto
    {
        public string id { get; set; }
        public string title { get; set; }
        public string parent { get; set; }
        public int sort { get; set; }
    }

    public class SysCodeTypeTree
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<SysCodeTypeTree> children { get; set; }
        public bool spread { get; set; } = true;
    }
}
