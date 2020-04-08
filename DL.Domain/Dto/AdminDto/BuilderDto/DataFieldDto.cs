namespace DL.Domain.Dto.AdminDto.BuilderDto
{
    public class DataFieldDto : DataFieldUIDto
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TbName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string TbDesc { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int FieldIndex { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FiledType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int FiledLength { get; set; }

        /// <summary>
        /// 是否标识列
        /// </summary>
        public string IsIdentity { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public string IsKey { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public string IsNullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultVal { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// 占用字节数
        /// </summary>
        public int ByteLength { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public string DecimalLength { get; set; }
    }
}
