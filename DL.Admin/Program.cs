using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web; 

namespace DL.Admin
{

    //�û������û���ϵͳ�����ߣ��ù�����Ҫ���ϵͳ�û����á�
    //���Ź�������ϵͳ��֯��������˾�����š�С�飩�����ṹչ��֧������Ȩ�ޡ�
    //��λ��������ϵͳ�û���������ְ��
    //�˵���������ϵͳ�˵�������Ȩ�ޣ���ťȨ�ޱ�ʶ�ȡ�
    //��ɫ������ɫ�˵�Ȩ�޷��䡢���ý�ɫ�������������ݷ�ΧȨ�޻��֡�
    //�ֵ������ϵͳ�о���ʹ�õ�һЩ��Ϊ�̶������ݽ���ά����
    //����������ϵͳ��̬���ó��ò�����
    //֪ͨ���棺ϵͳ֪ͨ������Ϣ����ά����
    //������־��ϵͳ����������־��¼�Ͳ�ѯ��ϵͳ�쳣��Ϣ��־��¼�Ͳ�ѯ��
    //��¼��־��ϵͳ��¼��־��¼��ѯ������¼�쳣��
    //�����û�����ǰϵͳ�л�Ծ�û�״̬��ء�
    //��ʱ�������ߣ���ӡ��޸ġ�ɾ��)������Ȱ���ִ�н����־��
    //�������ɣ�ǰ��˴�������ɣ�java��html��xml��sql��֧��CRUD���� ��
    //ϵͳ�ӿڣ�����ҵ������Զ�������ص�api�ӿ��ĵ���
    //�����أ����ӵ�ǰϵͳCPU���ڴ桢���̡���ջ�������Ϣ��
    //���߹��������϶���Ԫ��������Ӧ��HTML���롣
    //���ӳؼ��ӣ����ӵ�ǰϵͳ���ݿ����ӳ�״̬���ɽ��з���SQL�ҳ�ϵͳ����ƿ����
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder
                      .UseContentRoot(Directory.GetCurrentDirectory())
                      .UseUrls("http://*:2020")
                      .UseStartup<Startup>();
              })
              .UseNLog()
              .Build()
              .Run(); 

        } 

    }
}
