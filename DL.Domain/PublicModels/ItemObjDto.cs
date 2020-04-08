namespace DL.Domain.PublicModels
{
    /// <summary>
    /// 自定义排序
    /// </summary>
    public class ParmSort
    {
        public string p { get; set; }
        public string i { get; set; }
        public int o { get; set; }
    }
    public class ItemObjDto
    {
        public string title { get; set; }
        public string value { get; set; }
    }


    /// <summary>
    /// 提供字符串FromBody的解析
    /// </summary>
    public class DelParams
    {
        public string ids { get; set; }
    }
}
