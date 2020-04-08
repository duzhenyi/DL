using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DL.Utils.Extensions
{
    public static class EnumExtensions
    { 
        /// <summary>
        /// 获取枚举类型的Description说明
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum value)
        {
            var type = value.GetType();
            var info = type.GetField(value.ToString());
            var attrs = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length < 1)
                return string.Empty;

            return attrs[0] is DescriptionAttribute
                descriptionAttribute
                ? descriptionAttribute.Description
                : value.ToString();
        }

        public static List<OptionResultModel> ToResult(this Enum value, bool ignoreUnKnown = false)
        {
            var enumType = value.GetType();

            if (!enumType.IsEnum)
                return null;

            return Enum.GetValues(enumType).Cast<Enum>()
                .Where(m => !ignoreUnKnown || !m.ToString().Equals("UnKnown")).Select(x => new OptionResultModel
                {
                    Label = x.GetEnumText(),
                    Value = x
                }).ToList();
        }

        /// <summary>
        /// 枚举转换为返回模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreUnKnown">忽略UnKnown选项</param>
        /// <returns></returns>
        public static List<OptionResultModel> ToResult<T>(bool ignoreUnKnown = false)
        {
            var enumType = typeof(T);

            if (!enumType.IsEnum)
                return null;

            return Enum.GetValues(enumType).Cast<Enum>()
                 .Where(m => !ignoreUnKnown || !m.ToString().Equals("UnKnown")).Select(x => new OptionResultModel
                 {
                     Label = x.GetEnumText(),
                     Value = x
                 }).ToList();
        }


        /// <summary>
        /// 获取枚举显示名称
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举显示名称</returns>
        public static string GetEnumDisplayName(this Enum value)
        {
            return GetEnumDisplayName(value.GetType(), value.ToString());
        }

        /// <summary>
        /// 获取枚举显示名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">枚举值</param>
        /// <returns>枚举显示名称</returns>
        public static string GetEnumDisplayName(Type enumType, string value)
        {
            string rv = "";
            FieldInfo field = null;

            if (enumType.IsEnum())
            {
                field = enumType.GetField(value);
            }
            //如果是nullable的枚举
            if (enumType.IsGeneric(typeof(Nullable<>)) && enumType.GetGenericArguments()[0].IsEnum())
            {
                field = enumType.GenericTypeArguments[0].GetField(value);
            }

            if (field != null)
            {

                var attribs = field.GetCustomAttributes(typeof(DisplayAttribute), true).ToList();
                if (attribs.Count > 0)
                {
                    rv = ((DisplayAttribute)attribs[0]).GetName();
                }
                else
                {
                    rv = value;
                }
            }
            return rv;
        }

        public static string GetEnumDisplayName(Type enumType, int value)
        {
            string rv = "";
            FieldInfo field = null;
            string ename = "";
            if (enumType.IsEnum())
            {
                ename = enumType.GetEnumName(value);
                field = enumType.GetField(ename);
            }
            //如果是nullable的枚举
            if (enumType.IsGeneric(typeof(Nullable<>)) && enumType.GetGenericArguments()[0].IsEnum())
            {
                ename = enumType.GenericTypeArguments[0].GetEnumName(value);
                field = enumType.GenericTypeArguments[0].GetField(ename);
            }

            if (field != null)
            {

                var attribs = field.GetCustomAttributes(typeof(DisplayAttribute), true).ToList();
                if (attribs.Count > 0)
                {
                    rv = ((DisplayAttribute)attribs[0]).GetName();
                }
                else
                {
                    rv = ename;
                }
            }
            return rv;
        }

    }

}
