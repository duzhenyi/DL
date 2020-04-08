
var active, layuiIndex;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'api'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api;
        table.render({
            elem: '#tablist',
            headers: os.getToken(),
            url: '/Cms/Column/GetPages',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    { field: 'ID', width: 100, title: '编号', fixed: 'left' },
                    {
                        field: 'Title', title: '栏目名称', templet: function (res) {
                            return '<a href="javascript:void(0)" lay-event="edit" class="text-color">' + res.Title + '</a>';
                        }
                    },
                    { field: 'TempName', width: 150, title: '模板名称' },
                    {
                        field: 'Sort', width: 120, title: '排序', templet: function (res) {
                            return '<a href="javascript:void(0)" class="table-sort text-color" lay-event="sortup" title="向上"><i class="layui-icon layui-icon-return"></i></a><a href="javascript:void(0)" lay-event="sortdown" class="table-sort text-color" title="向下"><i class="layui-icon layui-icon-return"></i></a>';
                        }
                    },
                    { field: 'ClassLayer', width: 120, title: '栏目深度' },
                    { field: 'IsTopShow', width: 120, title: '是否顶部显示', templet: '#switchTpl' },
                    { width: 230, title: '操作', templet: '#tool' }
                ]
            ],
            page: true,
            id: 'tables'
        });

        active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        }
                    });
            },
            //添加栏目
            toolAdd: function () {
                active.goColumnModify();
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
                    os.ajax('Cms/Column/Delete/', { parm: str }, function (res) {
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
            goColumnModify: function (parm = '') {
                var winH = $(window).height(), winW = $(window).width();
                layuiIndex = os.OpenRight('添加栏目', "/Cms/Column/ColumnModify" + parm, winW - 220 + 'px', winH - 61 + 'px', function () {
                    if (parseInt($("#isSave").val()) === 1) {
                        $("#isSave").val('0');
                        active.reload();
                    }
                }, function () {
                    active.closeCloumnModify();
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
            }
        };

        $('.list-search .layui-btn').on('click', function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
        //监听工具条
        table.on('tool(tool)', function (obj) {
            var data = obj.data;
            if (obj.event === 'edit') {
                active.goColumnModify('?id=' + data.Id);
            }
            if (obj.event === 'AddChild') {
                active.goColumnModify('?parent=' + data.Id);
            }
            if (obj.event === 'sortdown') {
                os.ajax('Cms/Column/ColStor', { p: data.ParentId, i: data.Id, o: 1 }, function (res) {
                    if (res.statusCode === 200) {
                        active.reload();
                    }
                    else {
                        os.error(res.msg);
                    }
                });
            }
            if (obj.event === 'sortup') {
                os.log(data);
                os.ajax('Cms/Column/ColStor', { p: data.ParentId, i: data.Id, o: 0 }, function (res) {
                    if (res.statusCode === 200) {
                        active.reload();
                    }
                    else {
                        os.error(res.msg);
                    }
                });
            }
        });
    }); 