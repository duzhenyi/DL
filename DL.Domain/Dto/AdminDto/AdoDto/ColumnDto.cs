using System.Collections.Generic;

namespace DL.Domain.Dto.AdminDto.AdoDto
{
    /// <summary>
    /// 栏目树结构
    /// </summary>
    public class ColumnTree
    {
        public string Id { get; set; }

        public string ColumnId { get; set; }

        public string Name { get; set; }

        public string Href { get; set; }

        public int TempId { get; set; }

        public int Sort { get; set; }

        public bool Spread { get; set; } = true;

        public bool IsAjax { get; set; } = true;

        public List<ColumnTree> children { get; set; }
    }
}
