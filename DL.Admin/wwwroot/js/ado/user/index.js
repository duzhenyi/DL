
var fun;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'laydate', 'api'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api,
            laydate = layui.laydate;
        laydate.render({
            elem: '#times'
            , theme: '#393D49'
            , format: 'yyyy/MM/dd'
            , range: true
        });

        table.render({
            toolbar: '#toolbar',
            elem: '#tablist',
            headers: os.getToken(),
            url: '/Cms/User/GetPages',
            page: { limits: [10, 20, 50, 100, 500, 1000, 5000, 10000], groups: 8 },
            id: 'tables',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    { field: 'LoginAccount', title: '账号', width: 120, fixed: 'left' },
                    { field: 'RelName', title: '姓名', width: 120 },
                    { field: 'Sex', title: '性别', minWidth: 200 },
                    { field: 'Status', title: '状态', width: 120 },
                    { field: 'UpLoginDate', title: '上次登录时间', width: 160, sort: true },
                    {
                        field: 'Details', title: '详情', width: 100, templet: function (res) {
                            return '<a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="look"><i class="layui-icon layui-icon-search"></i> 详情</a>';
                        }
                    }
                ]
            ]
        });

        fun = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            key: $("#key").val(),
                            time: $("#times").val()
                        }
                    });
            },
            toolSearch: function () {
                fun.reload();
            },
            toolDel: function () {
                var checkStatus = table.checkStatus('tables')
                    , data = checkStatus.data;
                if (data.length === 0) {
                    os.error("请选择要删除的项目~");
                    return;
                }
                var str = '';
                $.each(data, function (i, item) {
                    str += item.Id + ",";
                });
                layer.confirm('确定要执行批量删除吗？', function (index) {
                    layer.close(index);
                    var loadindex = layer.load(1, {
                        shade: [0.1, '#000']
                    });
                    os.ajax('Cms/User/Delete/', { parm: str }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            active.reload();
                            os.success('删除成功！');
                        } else {
                            os.error(res.msg);
                        }
                    });
                });
            },
            look: function (data) {
                layer.open({
                    title: '用户详情',
                    area: ['650px', '460px'],
                    content: '/Cms/User/Detlis?id=' + data.Id
                });
            }
        };
        table.on('toolbar(tool)', function (obj) {
            fun[obj.event] ? fun[obj.event].call(this) : '';
        });
        $('.list-search .layui-btn').on('click', function () {
            var type = $(this).data('type');
            fun[type] ? fun[type].call(this) : '';
        });
    }); 