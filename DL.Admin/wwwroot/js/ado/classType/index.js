
var active, layuiIndex;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'ztree', 'api', 'form'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.common,
            form = layui.form;
        var ID = '', typeName = '';

        $.fn.zTree.init($("#tree"), {
            async: {
                enable: true,
                headers: os.getToken(),
                url: "/Ado/ClassType/GetTree"
            },
            callback: {
                onClick: function (event, treeId, treeNode, clickFlag) {
                    ID = treeNode.ID;
                    active.reload();
                }
            }
        });

        form.render();
        table.render({
            elem: '#tablist',
            toolbar: '#toolbar',
            headers: os.getToken(),
            url: '/Ado/ClassType/GetPages',
            page: true,
            id: 'tables',
            where: {
                key: $('#key').val()
            },
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    { field: 'Title', title: '名称', fixed: 'left' },
                    { field: 'Val', title: '参数值', fixed: 'Center' },
                    { field: 'Layer', title: '深度', fixed: 'Center' },
                    { field: 'IndustryIcon', title: '图标', templet: '#toolIcon' },
                    { field: 'IndustryColor', title: '图标颜色', templet: '#toolColor' },
                    { field: 'IsTop', title: '是否置顶', templet: '#IsTop' },
                    { field: 'CreateTime', title: '创建时间', fixed: 'left' },
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
                os.Open('添加分级信息', '/Cms/Industry/Modify', '400px', '480px', function () {
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
                    os.ajax('Cms/Industry/Delete/', { parm: str }, function (res) {
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
                os.Open('修改分级信息', '/Ado/ClassType/Modify/?ID=' +
                    data.ID, '400px', '480px', function () {
                        active.reload();
                    });
            }
        });
    }); 