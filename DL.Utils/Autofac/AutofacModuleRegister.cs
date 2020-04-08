using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Module = Autofac.Module;

namespace DL.Utils.Autofac
{
	public class AutofacModuleRegister : Module
    {
        public string RootPath { get; set; }
        public List<string> DllFiles { get; set; }
        public AutofacModuleRegister(string rootPath, List<string> dllFiles)
        {
            RootPath = rootPath;
            DllFiles = dllFiles;
        }
         
        protected override void Load(ContainerBuilder builder)
		{   
            foreach (var dllFile in DllFiles)
			{
				var dllFilePath = Path.Combine(RootPath, dllFile);//获取项目绝对路径
				builder.RegisterAssemblyTypes(Assembly.LoadFile(dllFilePath))//直接采用加载文件的方法
					   //.PropertiesAutowired()//开始属性注入
					   //.Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
					   .AsImplementedInterfaces()//表示注册的类型，以接口的方式注册不包括IDisposable接口
					   .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy,使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
					   .InstancePerLifetimeScope();//即为每一个依赖或调用创建一个单一的共享的实例
			}

			////拦截器
			////builder.Register(c => new AOPTest());
			////注入类
			////builder.RegisterType<UsersService>().As<UsersIService>().PropertiesAutowired().EnableInterfaceInterceptors();

			////程序集注入
			//var IRepository = Assembly.Load("DL.IRepository");
			//var Repository = Assembly.Load("DL.Repository"); 
			//Assembly.GetExecutingAssembly();
			////根据名称约定（仓储层的接口和实现均以Repository结尾），实现服务接口和服务实现的依赖
			//builder.RegisterAssemblyTypes(IRepository, Repository)
			//  .Where(t => t.Name.EndsWith("Repository"))
			//  .AsImplementedInterfaces();

		 
		}
	}
}
