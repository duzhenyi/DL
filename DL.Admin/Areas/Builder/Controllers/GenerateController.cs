using DL.Admin.Filters;
using DL.Domain.Dto.AdminDto.BuilderDto;
using DL.Domain.PublicModels;
using DL.IService.BuilderIService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using DL.Utils.Extensions;
using System.Linq;

namespace DL.Admin.Areas.Builder.Controllers
{
    [Area("Builder")]
    [Route("Builder/Generate")]
    public class GenerateController : BaseController
    {
        private readonly IGenerateService _generateService;
        public GenerateController(IGenerateService generateService)
        {
            _generateService = generateService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有表
        /// </summary>
        /// <param name="loadDbDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTable")]
        public ApiResult<List<string>> GetTable([FromBody]DbDto loadDbDto)
        {
            return _generateService.GetTable(loadDbDto);
        }

        /// <summary>
        /// 获取表所有字段
        /// </summary>
        /// <param name="loadDbDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetField")]
        public JsonResult GetField([FromBody]DbDto loadDbDto)
        {
            var res = _generateService.GetField(loadDbDto);
            return Json(new { code = 0, msg = "success", count = res.Count, data = res });
        }

        [HttpPost]
        [Route("Build")]
        public ApiResult<string> Build([FromBody]BuildDto buildDto)
        {
            if (string.IsNullOrEmpty(buildDto.modelName) || string.IsNullOrEmpty(buildDto.controllerName)
                || buildDto.fieldDtos == null)
            {
                return new ApiResult<string>() { msg = "相关参数不可为空" };
            }
            try
            {
                var rootPath = AppDomain.CurrentDomain.BaseDirectory;
                int index = rootPath?.IndexOf($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}") ?? 0;
                var mainDir = rootPath?.Substring(0, index);

                //生成实体 
                var modelTxt = GetModelTxt(buildDto);
                var modeldir = GetModelPath(mainDir, buildDto.areaName, buildDto.modelName);
                System.IO.File.WriteAllText(modeldir, modelTxt, Encoding.UTF8);

                //生成Dto
                var dtoModelTxt = GetDtoModelTxt(buildDto);
                if (!string.IsNullOrEmpty(dtoModelTxt))
                {
                    var dtoModeldir = GetDtoModelPath(mainDir, buildDto.areaName, buildDto.modelName);
                    System.IO.File.WriteAllText(dtoModeldir, dtoModelTxt, Encoding.UTF8);
                }
                //生成接口
                var iserviceTxt = GetIServiceTxt(buildDto);
                var iservicedir = GetIServicePath(mainDir, buildDto.areaName, buildDto.modelName);
                System.IO.File.WriteAllText(iservicedir, iserviceTxt, Encoding.UTF8);
                //生成实现
                var serviceTxt = GetServiceTxt(buildDto);
                var servicedir = GetServicePath(mainDir, buildDto.areaName, buildDto.modelName);
                System.IO.File.WriteAllText(servicedir, serviceTxt, Encoding.UTF8);
                //生成控制器
                var controllerTxt = GetControllerTxt(buildDto);
                var controllerdir = GetControllerPath(mainDir, buildDto.areaName, buildDto.controllerName);
                System.IO.File.WriteAllText(controllerdir, controllerTxt, Encoding.UTF8);

                //生成UI
                var uiPath = GetUIPath(mainDir, buildDto.areaName, buildDto.controllerName);
                var indexUITxt = GetIndexUITxt(buildDto);
                System.IO.File.WriteAllText(uiPath + "Index.cshtml", indexUITxt, Encoding.UTF8);

                var modifyUITxt = GetModifyUITxt(buildDto);
                System.IO.File.WriteAllText(uiPath + "Modify.cshtml", modifyUITxt, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return new ApiResult<string>() { msg = "生成失败+" + ex.Message };
            }

            return new ApiResult<string>() { msg = "生成成功" };
        }

        #region 获取文本

        /// <summary>
        /// 读文件 注意 文本文本需要右键属性换成嵌入的资源
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            //var rs = currentAssembly.GetManifestResourceNames();
            var content = string.Empty;
            var path = $"{currentAssembly.GetName().Name}.Areas.Builder.Data.{templateName}";
            using (Stream stream = currentAssembly.GetManifestResourceStream(path))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }
        #endregion

        #region 生成实体

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private string GetModelPath(string mainDir, string areaName, string modelName)
        {
            var padden = Path.DirectorySeparatorChar;
            var up = Directory.GetParent(mainDir);
            var modeldir = up.GetDirectories().Where(x => x.Name.ToLower().EndsWith(".domain")).FirstOrDefault();
            if (modeldir == null)
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}Models");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}Models{padden}{areaName}Models");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}Models");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}Models{padden}{areaName}Models");
                }

            }


            return $"{modeldir}{padden}{modelName}.cs";
        }

        /// <summary>
        /// 获取实体模板
        /// </summary> 
        /// <returns></returns>
        private string GetModelTxt(BuildDto buildDto)
        {
            var strbuilder = new StringBuilder();
            //创建列
            foreach (var item in buildDto.fieldDtos)
            {

                strbuilder.Append("\r");
                strbuilder.AppendLine("\t\t/// <summary>");
                strbuilder.AppendLine("\t\t///" + item.FieldDesc);
                strbuilder.AppendLine("\t\t/// </summary>");
                strbuilder.Append("\t\t[SugarColumn(ColumnName = \"" + item.FieldName + "\"");
                if (item.IsKey != "")
                {
                    strbuilder.Append(",IsPrimaryKey = true");
                }
                if (item.IsIdentity != "")
                {
                    strbuilder.Append(",IsIdentity = true");
                }
                if (item.IsNullable != "")
                {
                    strbuilder.Append(",IsNullable = true");
                }
                strbuilder.Append(")]");
                strbuilder.AppendLine("\r\n\t\tpublic " + item.FiledType + " " + item.FieldName.ToUpperFirst() + " { get; set; }");
            }
            var txt = ReadTemplate("Model.txt").Replace("{AreaName}", $"{buildDto.areaName.ToUpperFirst() + "Models" ?? ""}")
                                     .Replace("{TableName}", buildDto.tableName)
                                     .Replace("{ModelName}", buildDto.modelName)
                                     .Replace("{Models}", strbuilder.ToString())
                                     .Replace("{DescName}", buildDto.descName);

            return txt;
        }


        #endregion

        #region 生成控制器

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private string GetControllerPath(string mainDir, string areaName, string controllerName)
        {
            var padden = Path.DirectorySeparatorChar;
            var controllerdir = string.Empty;
            if (string.IsNullOrEmpty(areaName))
            {
                controllerdir = Directory.CreateDirectory(mainDir + $"{padden}Controllers").FullName;
            }
            else
            {
                controllerdir = Directory.CreateDirectory(mainDir + $"{padden}Areas{padden}{areaName}{padden}Controllers").FullName;
            }
            return $"{controllerdir}{padden}{controllerName}Controller.cs";
        }

        /// <summary>
        /// 获取控制器模板
        /// </summary> 
        /// <returns></returns>
        private string GetControllerTxt(BuildDto buildDto)
        {
            bool isDto = false;
            foreach (var item in buildDto.fieldDtos)
            {
                if (item.IsSearch)
                {
                    isDto = true;
                    break;
                }
            }

            var usingDto = isDto && buildDto.areaName.ToUpperFirst() != "" ? "using DL.Domain.Dto.AdminDto." + buildDto.areaName.ToUpperFirst() + "ModelsDto;" : "";

            var txt = ReadTemplate("Controller.txt").Replace("{AreaName}", $"{buildDto.areaName.ToUpperFirst() ?? ""}")
                                                    .Replace("{areaName}", $"{buildDto.areaName.ToLowerFirst() ?? ""}")
                                                    .Replace("{ModelName}", buildDto.modelName)
                                                    .Replace("{ControllerName}", $"{buildDto.controllerName.ToUpperFirst() ?? ""}")
                                                    .Replace("{UsingDto}", usingDto)
                                                    .Replace("{DescName}", buildDto.descName);
            if (isDto)
            {
                txt = txt.Replace("{PageParm}", buildDto.modelName);
            }
            txt = txt.Replace("{PageParm}", "");
            return txt;
        }

        #endregion

        #region 生成接口
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private string GetIServicePath(string mainDir, string areaName, string modelName)
        {
            var padden = Path.DirectorySeparatorChar;
            var up = Directory.GetParent(mainDir);
            var modeldir = up.GetDirectories().Where(x => x.Name.ToLower().EndsWith(".iservice")).FirstOrDefault();
            if (modeldir == null)
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}IService");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}{areaName}IService");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}IService");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}{areaName}IService");
                }

            }

            return $"{modeldir}{padden}I{modelName}Service.cs";
        }

        /// <summary>
        /// 获取接口模板
        /// </summary> 
        /// <returns></returns>
        private string GetIServiceTxt(BuildDto buildDto)
        {
            bool isDto = false;
            foreach (var item in buildDto.fieldDtos)
            {
                if (item.IsSearch)
                {
                    isDto = true;
                    break;
                }
            }

            var usingDto = isDto && buildDto.areaName.ToUpperFirst() != "" ? "using DL.Domain.Dto.AdminDto." + buildDto.areaName.ToUpperFirst() + "ModelsDto;" : "";

            var txt = ReadTemplate("IService.txt").Replace("{AreaName}", $"{buildDto.areaName.ToUpperFirst() ?? ""}")
                                                  .Replace("{ModelName}", buildDto.modelName)
                                                  .Replace("{UsingDto}", usingDto)
                                                  .Replace("{DescName}", buildDto.descName);
            if (isDto)
            {
                txt = txt.Replace("{PageParm}", buildDto.modelName);
            }
            txt = txt.Replace("{PageParm}", "");
            return txt;
        }

        #endregion

        #region 生成实现

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private string GetServicePath(string mainDir, string areaName, string modelName)
        {
            var padden = Path.DirectorySeparatorChar;
            var up = Directory.GetParent(mainDir);
            var modeldir = up.GetDirectories().Where(x => x.Name.ToLower().EndsWith(".service")).FirstOrDefault();
            if (modeldir == null)
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}Service");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}{areaName}Service");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}Service");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}{areaName}Service");
                }

            }

            return $"{modeldir}{padden}{modelName}Service.cs";
        }

        /// <summary>
        /// 获取实现模板
        /// </summary> 
        /// <returns></returns>
        private string GetServiceTxt(BuildDto buildDto)
        {
            var whereBuilder = new StringBuilder();
            var timeBuilder = new StringBuilder();
            //var selectBuilder = new StringBuilder();
            //创建列
            bool isDto = false;
            // int count = 0;
            for (int i = 0; i < buildDto.fieldDtos.Count; i++)
            {
                if (buildDto.fieldDtos[i].IsSearch)
                {
                    if (buildDto.fieldDtos[i].FiledType == "string")
                    {
                        whereBuilder.AppendLine(".WhereIF(!string.IsNullOrEmpty(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "), m => m." + buildDto.fieldDtos[i].FieldName + ".Contains(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "))");
                    }
                    else if (buildDto.fieldDtos[i].FiledType == "bool" || buildDto.fieldDtos[i].FiledType == "bool?")
                    {
                        whereBuilder.AppendLine(".WhereIF(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + ", m => m." + buildDto.fieldDtos[i].FieldName + ")");
                        whereBuilder.AppendLine(".WhereIF(!parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + ", m => !m." + buildDto.fieldDtos[i].FieldName + ")");
                    }
                    else if (buildDto.fieldDtos[i].FiledType == "DateTime" || buildDto.fieldDtos[i].FiledType == "DateTime?")
                    {
                        timeBuilder.AppendLine("string begin" + buildDto.fieldDtos[i].FieldName + "Time = string.Empty, end" + buildDto.fieldDtos[i].FieldName + "Time = string.Empty;");

                        timeBuilder.AppendLine("if (!string.IsNullOrEmpty(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "))");
                        timeBuilder.AppendLine("{");
                        timeBuilder.AppendLine("var time" + buildDto.fieldDtos[i].FieldName + "Res = UtilsHelper.SplitString(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + ", '-');");
                        timeBuilder.AppendLine("begin" + buildDto.fieldDtos[i].FieldName + "Time = time" + buildDto.fieldDtos[i].FieldName + "Res[0].Trim();");
                        timeBuilder.AppendLine("end" + buildDto.fieldDtos[i].FieldName + "Time = time" + buildDto.fieldDtos[i].FieldName + "Res[1].Trim();");
                        timeBuilder.AppendLine("}");
                        whereBuilder.AppendLine(".WhereIF(!string.IsNullOrEmpty(parm." + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "), m => m." + buildDto.fieldDtos[i].FieldName + " >= Convert.ToDateTime(begin" + buildDto.fieldDtos[i].FieldName + "Time) && m." + buildDto.fieldDtos[i].FieldName + " <= Convert.ToDateTime(end" + buildDto.fieldDtos[i].FieldName + "Time))");
                    }

                    isDto = true;
                }
                //if (buildDto.fieldDtos[i].IsShowList)
                //{
                //    if (count == 0)
                //    {
                //        selectBuilder.Append(".Select(m => new " + buildDto.modelName);
                //        selectBuilder.AppendLine("{");
                //        count++;
                //    }
                //    selectBuilder.AppendLine("                                       " + buildDto.fieldDtos[i].FieldName + "= m." + buildDto.fieldDtos[i].FieldName + ",");

                //    if (i == buildDto.fieldDtos.Count - 1)
                //    {
                //        selectBuilder.AppendLine("})");
                //    }
                //}
            }

            var usingDto = isDto && buildDto.areaName.ToUpperFirst() != "" ? "using DL.Domain.Dto.AdminDto." + buildDto.areaName.ToUpperFirst() + "ModelsDto;" : "";
            var txt = ReadTemplate("Service.txt").Replace("{AreaName}", $"{buildDto.areaName.ToUpperFirst() ?? ""}")
                                                 .Replace("{ModelName}", buildDto.modelName)
                                                 .Replace("{WhereIF}", whereBuilder.ToString())
                                                 .Replace("{TimeWhereIF}", timeBuilder.ToString())
                                                 .Replace("{UsingDto}", usingDto)
                                                 .Replace("{DescName}", buildDto.descName);
            if (isDto)
            {
                txt = txt.Replace("{PageParm}", buildDto.modelName);
            }
            txt = txt.Replace("{PageParm}", "");
            return txt;
        }

        #endregion

        #region 生成Dto

        private string GetDtoModelPath(string mainDir, string areaName, string modelName)
        {
            var padden = Path.DirectorySeparatorChar;
            var up = Directory.GetParent(mainDir);
            var modeldir = up.GetDirectories().Where(x => x.Name.ToLower().EndsWith(".domain")).FirstOrDefault();
            if (modeldir == null)
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}Dto{padden}AdminDto");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir + $"{padden}Dto{padden}AdminDto{padden}{areaName}Dto");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(areaName))
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}Dto{padden}AdminDto{padden}");
                }
                else
                {
                    modeldir = Directory.CreateDirectory(modeldir.FullName + $"{padden}Dto{padden}AdminDto{padden}{areaName}Dto");
                }

            }

            return $"{modeldir}{padden}{modelName}PageParm.cs";
        }

        /// <summary>
        /// 获取实现模板
        /// </summary> 
        /// <returns></returns>
        private string GetDtoModelTxt(BuildDto buildDto)
        {
            var strbuilder = new StringBuilder();
            var isSearch = false;
            var txt = string.Empty;
            //创建列
            foreach (var item in buildDto.fieldDtos)
            {
                if (item.IsSearch)
                {
                    strbuilder.AppendLine("\r\t\t/// <summary>");
                    strbuilder.AppendLine("\t\t///" + item.FieldDesc);
                    strbuilder.Append("\t\t/// </summary>");
                    if (item.FiledType == "DateTime" || item.FiledType == "DateTime?")
                    {
                        strbuilder.AppendLine("\r\n\t\tpublic string " + item.FieldName.ToLowerFirst() + " { get; set; }");
                    }
                    else
                    {
                        strbuilder.AppendLine("\r\t\tpublic " + item.FiledType + " " + item.FieldName.ToLowerFirst() + " { get; set; }");
                    }
                    isSearch = true;
                }
            }
            if (isSearch)
            {
                txt = ReadTemplate("DtoModel.txt").Replace("{AreaName}", $"{buildDto.areaName.ToUpperFirst() + "Models" ?? ""}")
                                   .Replace("{ModelName}", buildDto.modelName)
                                   .Replace("{Models}", strbuilder.ToString());
            }

            return txt;
        }

        #endregion

        #region 生成UI

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        private string GetUIPath(string mainDir, string areaName, string controllerName)
        {
            var padden = Path.DirectorySeparatorChar;
            var controllerdir = Directory.CreateDirectory(mainDir + $"{padden}Areas{padden}{areaName}{padden}Views{padden}{controllerName}").FullName;

            return $"{controllerdir}{padden}";
        }

        /// <summary>
        /// 获取列表UI模板
        /// </summary> 
        /// <returns></returns>
        private string GetIndexUITxt(BuildDto buildDto)
        {
            var strBuilder = new StringBuilder();
            var scriptBuilder = new StringBuilder();
            var selectBuilder = new StringBuilder();
            var layerDateBuilder = new StringBuilder();
            var btnSearchBuilder = new StringBuilder();
            var searchBuilder = new StringBuilder();
            var whereBuilder = new StringBuilder();

            for (int i = 0; i < buildDto.fieldDtos.Count; i++)
            {
                if (buildDto.fieldDtos[i].IsSearch)
                {
                    if (btnSearchBuilder.Length == 0)
                    {

                        selectBuilder.AppendLine("<div class=\"layui-collapse\">")
                                     .AppendLine("<div class=\"layui-colla-item\">")
                                     .AppendLine("<h2 class=\"layui-colla-title\">条件筛选</h2>")
                                     .AppendLine("<div class=\"layui-colla-content layui-show\">")
                                     .AppendLine("<div class=\"layui-form list-search\">");

                        btnSearchBuilder.AppendLine("<button type = \"button\" class=\"layui-btn layui-btn-sm\" data-type=\"search\"><i class=\"layui-icon layui-icon-search\"></i>搜索</button>");
                        searchBuilder.AppendLine("search: function() {");
                        searchBuilder.AppendLine("active.reload();");
                        searchBuilder.AppendLine("},");
                    }

                    selectBuilder.AppendLine("<div class=\"layui-input-inline\">")
                                 .AppendLine("<label class=\"layui-form-label\">" + buildDto.fieldDtos[i].FieldDesc + "</label>")
                                 .AppendLine("<div class=\"layui-input-inline\">");

                    if (buildDto.fieldDtos[i].FiledType == "DateTime" || buildDto.fieldDtos[i].FiledType == "DateTime?")
                    {
                        selectBuilder.AppendLine("<input class=\"layui-input\" id =\"" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Search\" autocomplete =\"off\"  >");

                        layerDateBuilder.AppendLine("laydate.render({");
                        layerDateBuilder.AppendLine("elem: '#" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "',");
                        layerDateBuilder.AppendLine(" theme: '#393D49',");
                        layerDateBuilder.AppendLine("format: 'yyyy/MM/dd',");
                        layerDateBuilder.AppendLine("range: true");
                        layerDateBuilder.AppendLine("});");
                    }
                    else if (buildDto.fieldDtos[i].FiledType == "bool" || buildDto.fieldDtos[i].FiledType == "bool?")
                    {
                        selectBuilder.AppendLine("<input type=\"checkbox\" id =\"" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Search\"  lay-text=\"展示|隐藏\" lay-skin = \"switch\" >");
                    }
                    else
                    {
                        selectBuilder.AppendLine("<input class=\"layui-input\" id =\"" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Search\" autocomplete =\"off\" placeholder =\"请输入" + buildDto.fieldDtos[i].FieldDesc + "查询\" > ");
                    }

                    selectBuilder.AppendLine("</div>")
                                 .AppendLine("</div>");

                    if (buildDto.fieldDtos[i].FiledType == "bool" || buildDto.fieldDtos[i].FiledType == "bool?")
                    {
                        whereBuilder.AppendLine(buildDto.fieldDtos[i].FieldName.ToLowerFirst() + ":$('#" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Search')[0].checked,");
                    }
                    else
                    {
                        whereBuilder.AppendLine(buildDto.fieldDtos[i].FieldName.ToLowerFirst() + ":$('#" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Search').val(),");
                    }

                }

                if (buildDto.fieldDtos[i].IsShowList)
                {
                    if (buildDto.fieldDtos[i].FiledType == "bool" || buildDto.fieldDtos[i].FiledType == "bool?")
                    {
                        strBuilder.AppendLine("{ field: '" + buildDto.fieldDtos[i].FieldName + "', title: '" + buildDto.fieldDtos[i].FieldDesc + "', align: 'center',templet: '#" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Switch' },");
                        scriptBuilder.AppendLine("<script type=\"text/html\" id='" + buildDto.fieldDtos[i].FieldName.ToLowerFirst() + "Switch'>")
                                     .AppendLine("<input type='checkbox' id='" + buildDto.fieldDtos[i].FieldName + "Switch' value=\"true\" lay-skin=\"switch\" lay-text=\"展示|隐藏\"  {{ d." + buildDto.fieldDtos[i].FieldName + "?'checked':''}} disabled=''>")
                                     .AppendLine("</script>");
                    }
                    else
                    {
                        strBuilder.AppendLine("{ field: '" + buildDto.fieldDtos[i].FieldName + "', title: '" + buildDto.fieldDtos[i].FieldDesc + "', align: 'left' },");
                    }
                }
            }

            selectBuilder.AppendLine(btnSearchBuilder.ToString())
                         .AppendLine("</div>")
                         .AppendLine("</div>")
                         .AppendLine("</div>")
                         .AppendLine("</div>");

            var txt = ReadTemplate("Index.txt").Replace("$AreaName$", $"{buildDto.areaName.ToUpperFirst() ?? ""}")
                                                             .Replace("$ControllerName$", buildDto.controllerName)
                                                             .Replace("$Select$", selectBuilder.ToString())
                                                             .Replace("$Models$", strBuilder.ToString())
                                                             .Replace("$Script$", scriptBuilder.ToString())
                                                             .Replace("$LayerDate$", layerDateBuilder.ToString())
                                                             .Replace("$Where$", whereBuilder.ToString())
                                                             .Replace("$Search$", searchBuilder.ToString())
                                                             .Replace("$DescName$", buildDto.descName);

            return txt;
        }

        /// <summary>
        /// 获取修改UI模板
        /// </summary> 
        /// <returns></returns>
        private string GetModifyUITxt(BuildDto buildDto)
        {
            var formBuilder = new StringBuilder();
            var hiddenBuilder = new StringBuilder();
            var jsBuilder = new StringBuilder();
            //创建列


            foreach (var item in buildDto.fieldDtos)
            {

                if (item.IsShowForm)
                {
                    formBuilder.AppendLine("<div class=\"layui-form-item\">");
                    formBuilder.AppendLine("<label class=\"layui-form-label\">" + item.FieldDesc + "</label>");
                    formBuilder.AppendLine("<div class=\"layui-input-block\">");

                    if (item.FiledType == "bool" || item.FiledType == "bool?")
                    {
                        formBuilder.AppendLine("<input type=\"checkbox\" name=\"" + item.FieldName + "\" lay-skin=\"switch\" lay-text = \"开启|禁用\" value = \"true\" @(Model." + item.FieldName + " ? \"checked\" : \"\") > ");
                    }
                    else if (item.FiledType == "DateTime" || item.FiledType == "DateTime?")
                    {
                        formBuilder.AppendLine("<input type=\"text\" class=\"layui-input\" lay-verify=\"date\" id=\"" + item.FieldName + "\" name=\"" + item.FieldName + "\" value =\"@Model." + item.FieldName + "\" >");

                        jsBuilder.AppendLine("laydate.render({");
                        jsBuilder.AppendLine("elem: '#" + item.FieldName + "',");
                        jsBuilder.AppendLine("theme: '#393D49'");
                        jsBuilder.AppendLine("});");
                    }
                    else
                    {
                        if (item.FiledType == "string" && item.FiledLength > 255)
                        {
                            formBuilder.AppendLine("<textarea name = \"" + item.FieldName + "\" class=\"layui-textarea\" style =\"min-height: 60px;\" placeholder =\"请输入" + item.FieldDesc + "\">@Model." + item.FieldName + "</textarea>");
                        }
                        else
                        {
                            formBuilder.AppendLine("<input type=\"text\" name=\"" + item.FieldName + "\" value =\"@Model." + item.FieldName + "\" ");
                            if (!string.IsNullOrEmpty(item.IsNullable))
                            {
                                formBuilder.Append("lay-verify =\"required\" lay-verType=\"tips\" autocomplete=\"off\" ");
                            }
                            formBuilder.Append("placeholder =\"请输入" + item.FieldDesc + "\" class=\"layui-input\">");
                        }
                    }
                    formBuilder.AppendLine("</div>");
                    formBuilder.AppendLine("</div>");
                }
                else
                {
                    hiddenBuilder.AppendLine("<input type=\"hidden\" name=\"" + item.FieldName + "\" id=\"" + item.FieldName + "\" value=\"@Model." + item.FieldName + "\" />");

                }
            }

            var txt = ReadTemplate("Modify.txt").Replace("$AreaName$", $"{buildDto.areaName.ToUpperFirst() ?? ""}")
                                                             .Replace("$ControllerName$", buildDto.controllerName)
                                                             .Replace("$Form$", formBuilder.ToString())
                                                             .Replace("$Hidden$", hiddenBuilder.ToString())
                                                             .Replace("$JavaScript$", jsBuilder.ToString())
                                                             .Replace("$ModelName$", buildDto.modelName)
                                                             .Replace("$Models$", buildDto.modelName)
                                                             .Replace("$Using$", "DL.Domain.Models." + $"{buildDto.areaName.ToUpperFirst() + "Models" ?? ""}")
                                                             .Replace("$DescName$", buildDto.descName);

            return txt;
        }
        #endregion
    }
}