var active;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'laydate', 'api', 'form'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api,
            form = layui.form;

        laydate = layui.laydate;
        laydate.render({
            elem: '#times',
            theme: '#393D49',
            format: 'yyyy/MM/dd',
            range: true
        });

        form.render();
        table.render({
            elem: '#tablist',
            toolbar: '#toolbar',
            headers: os.getToken(),
            url: '/Cms/Recruit/GetRecruitList',
            page: true,
            id: 'tables',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    {
                        field: 'Name', title: '姓名', fixed: 'left', templet: function (res) {
                            return '<a href="javascript:void(0)" lay-event="edit" class="text-color">' + res.Name + '</a>';
                        }
                    },
                    { field: 'Tel', width: 100, title: '联系电话' },
                    { field: 'UserId', width: 100, title: '用户信息' },
                    { field: 'CreateTime', width: 160, title: '创建时间', sort: true }
                ]
            ],
            where: {
                key: $('#key').val(),
                time: $("#times").val(),
                id: '@id'
            }
        });


        active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            key: $('#key').val(),
                            time: $("#times").val(),
                            id: '@id'
                        },
                        done: function () {
                        }
                    });
            },
            toolSearch: function () {
                active.reload();
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
                    os.ajax('Cms/Recruit/DeleteRecruitList/', { parm: str }, function (res) {
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
            edit: function (data) {
                layer.open({
                    title: '用户详情',
                    area: ['650px', '460px'],
                    content: '/Cms/User/Detlis?id=' + data.Id
                });
            }
        };
        table.on('toolbar(tool)', function (obj) {
            active[obj.event] ? active[obj.event].call(this) : '';
        });
        $('.list-search .layui-btn').on('click', function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
    });
