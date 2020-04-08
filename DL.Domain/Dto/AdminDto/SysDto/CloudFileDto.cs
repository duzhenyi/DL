using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Domain.Dto.AdminDto.SysDto
{
    public class CloudFileDto
    {
        public int Code { get; set; } = 200;
        public string Message { get; set; }
        public string Page { get; set; } = "";
        public string Token { get; set; }
        public List<ListFileInfoDto> list { get; set; }
    }
}
