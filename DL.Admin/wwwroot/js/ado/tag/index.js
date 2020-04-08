var active, layuiIndex;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'api', 'form'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api,
            form = layui.form;

        form.render();
        table.render({
            elem: '#tablist',
            toolbar: '#toolbar',
            headers: os.getToken(),
            url: '/Cms/Tags/GetPages',
            page: true,
            id: 'tables',
            where: {
                key: $('#key').val()
            },
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    { field: 'FirstLetter', title: '首字母', fixed: 'left' },
                    { field: 'Name', title: '名称' },
                    { field: 'Links', title: '链接地址', sort: true },
                    { field: 'Status', title: '是否启用', templet: '#switchTpl' },
                    { field: 'TagsHits', title: '点击量' },
                    { width: 100, title: '操作', templet: '#tool' }
                ]
            ]
        });

        active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            key: $('#key').val()
                        },
                        done: function () {
                        }
                    });
            },
            toolAdd: function () {
                os.Open('添加标签信息', '/Cms/Tags/TagsModify', '400px', '330px', function () {
                    active.reload();
                });
            },
            closeCloumnModify: function () {
                var $layero = $('#layui-layer' + layuiIndex);
                $layero.animate({
                    left: $layero.offset().left + $layero.width()
                }, 300, function () {
                    layer.close(layuiIndex);
                });
                return false;
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
                    str += item.ID + ",";
                });
                layer.confirm('确定要执行批量删除吗？', function (index) {
                    layer.close(index);
                    var loadindex = layer.load(1, {
                        shade: [0.1, '#000']
                    });
                    os.ajax('Cms/Tags/Delete/', { parm: str }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            active.reload();
                            os.success('删除成功！');
                        } else {
                            os.error(res.msg);
                        }
                    });
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
        //监听指定开关
        form.on('switch(switch)', function (data) {
            type = this.checked ? 0 : 1;
        });
        //监听工具条
        table.on('tool(tool)', function (obj) {
            var data = obj.data; 
            if (obj.event === 'edit') {
                os.Open('修改标签信息', '/Cms/Tags/TagsModify/?ID=' +
                    data.ID, '400px', '330px', function () {
                        active.reload();
                    });
            }
        });
    });
