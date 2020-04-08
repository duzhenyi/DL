
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
            url: '/Cms/Article/GetPages',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    {
                        field: 'Title', title: '标题', fixed: 'left', templet: function (res) {
                            return '<a href="javascript:void(0)" lay-event="edit" class="text-color">' + res.Title + '</a>';
                        }
                    },
                    { field: 'Tag', width: 200, title: '标签' },
                    { field: 'Sort', width: 100, title: '排序', sort: true },
                    {
                        field: 'IosVersion', width: 200, title: '属性', templet: function (res) {
                            return attrHtml(res);
                        }
                    },
                    { field: 'Audit', width: 120, title: '审核状态', templet: '#switchTpl' },
                    { field: 'Hits', width: 100, title: '点击量' },
                    { field: 'UpdateDate', width: 200, title: '更新时间', sort: true }
                ]
            ],
            page: true,
            id: 'tables',
            where: {
                types: 1,
                id: '@columnId'
            }
        });

        var type = 0, isCopy = 0, selStr = '';
        active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            types: 1,
                            id: '@columnId',
                            where: $("#attr").val(),
                            key: $('#key').val(),
                            audit: type
                        },
                        done: function () {
                        }
                    });
            },
            //添加栏目
            toolAdd: function () {
                active.goModify('?column=@columnId');
            },
            goModify: function (parm = '') {
                var winH = $(window).height(), winW = $(window).width();
                layuiIndex = os.OpenRight('文章管理', "/Cms/Article/ArticleModify" + parm, winW - 220 + 'px', winH - 61 + 'px', function () {
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
                    os.ajax('Cms/Article/Delete/', { parm: str }, function (res) {
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
            toolRecycle: function () {
                var checkStatus = table.checkStatus('tables')
                    , data = checkStatus.data;
                if (data.length === 0) {
                    os.error("请选择要转移的项目~");
                    return;
                }
                var str = '';
                $.each(data, function (i, item) {
                    str += item.Id + ",";
                });
                layer.confirm('确定要执行转移到回收站吗？', function (index) {
                    layer.close(index);
                    var loadindex = layer.load(1, {
                        shade: [0.1, '#000']
                    });
                    os.ajax('Cms/Article/GoRecycle/', { parm: str, type: 0 }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            active.reload();
                            os.success('转移成功！');
                        } else {
                            os.error(res.msg);
                        }
                    });
                });
            },
            toolCopy: function () {
                isCopy = 1;
                var checkStatus = table.checkStatus('tables')
                    , data = checkStatus.data;
                if (data.length === 0) {
                    os.error("请选择要操作的项目~");
                    return;
                }
                $.each(data, function (i, item) {
                    selStr += item.Id + ",";
                });
                layer.open({
                    type: 1,
                    title: '批量复制',
                    shadeClose: true,
                    shade: 0.2,
                    area: ['650px', '500px'],
                    content: $('.copy-wall').html(),
                    zIndex: "10000",
                    success: function () {
                        form.render();
                        $('.btn-open-close').click(function () {
                            os.closeOpen();
                        });
                    },
                    end: function () {
                        selStr = '';
                    }
                });
            },
            tooltransfer: function () {
                isCopy = 2;
                var checkStatus = table.checkStatus('tables')
                    , data = checkStatus.data;
                if (data.length === 0) {
                    os.error("请选择要操作的项目~");
                    return;
                }
                $.each(data, function (i, item) {
                    selStr += item.Id + ",";
                });
                layer.open({
                    type: 1,
                    title: '批量转移',
                    shadeClose: true,
                    shade: 0.2,
                    area: ['650px', '500px'],
                    content: $('.copy-wall').html(),
                    zIndex: "10000",
                    success: function () {
                        form.render();
                        $('.btn-open-close').click(function () {
                            os.closeOpen();
                        });
                    },
                    end: function () {
                        selStr = '';
                    }
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
                active.goModify('?id=' + data.Id);
            }
        });
        form.on('submit(submit)', function (data) {
            if (selStr === '') {
                os.error('请选择要操作的项！');
                return false;
            }
            os.log(data.field);
            layer.load(1, {
                shade: [0.1, '#000']
            });
            os.ajax('Cms/Article/GoCopyOrTransfer/', { parm: selStr, type: isCopy, column: data.field.columnId }, function (res) {
                os.closeOpen();
                if (res.statusCode === 200) {
                    active.reload();
                    os.success('操作成功！');
                } else {
                    os.error(res.msg);
                }
            });
            return false;
        });
    });
function attrHtml(e) {
    var h = '';
    if (e.isTop) {
        h += '<span class="layui-badge layui-bg-cyan">推荐</span>';
    }
    if (e.isHot) {
        h += '<span class="layui-badge layui-bg-cyan">热点</span>';
    }
    if (e.isScroll) {
        h += '<span class="layui-badge layui-bg-cyan">滚动</span>';
    }
    if (e.isSlide) {
        h += '<span class="layui-badge layui-bg-cyan">幻灯</span>';
    }
    if (e.isComment) {
        h += '<span class="layui-badge layui-bg-cyan">评论</span>';
    }
    return h;
} 