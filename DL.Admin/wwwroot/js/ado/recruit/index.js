var active,//用于弹出层可以调用
    layuiIndex;
layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'laydate', 'api', 'form'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api,
            form = layui.form,
            type = 0;

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
            url: '/Cms/Recruit/GetPages',
            page: true,
            id: 'tables',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    {
                        field: 'Title', title: '标题', fixed: 'left', templet: function (res) {
                            return '<a href="javascript:void(0)" lay-event="edit" class="text-color">' + res.Title + '</a>';
                        }
                    },
                    {
                        field: 'WorkType', width: 100, title: '工作类型', templet: function (res) {
                            return res.WorkType ? "全职" : "兼职";
                        }
                    },
                    { field: 'Sort', width: 100, title: '排序', sort: true },
                    {
                        field: 'IosVersion', width: 180, title: '属性', templet: function (res) {
                            return attrHtml(res);
                        }
                    },
                    { field: 'Audit', width: 100, title: '审核状态', templet: '#switchTpl' },
                    { field: 'CreateTime', width: 160, title: '创建时间', sort: true },
                    { title: '操作', minWidth: 135, templet: '#listBar', fixed: "right", align: "center" }
                ]
            ],
            where: {
                key: $('#key').val(),
                audit: type,
                time: $("#times").val()
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
                            audit: type,
                            time: $("#times").val()
                        },
                        done: function () {
                        }
                    });
            },
            //添加栏目
            toolAdd: function () {
                active.goModify();
            },
            goModify: function (parm = '') {
                var winH = $(window).height(), winW = $(window).width();
                layuiIndex = os.OpenRight('兼职管理', "/Cms/Recruit/RecruitModify" + parm, winW - 220 + 'px', winH - 61 + 'px', function () {
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
                    os.ajax('Cms/Recruit/Delete/', { parm: str }, function (res) {
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
            toolVerify: function () {
                var checkStatus = table.checkStatus('tables'), data = checkStatus.data;
                if (data.length === 0) {
                    os.error("请选择要审核的项目~");
                    return;
                }
                 
                var str = '';
                $.each(data, function (i, item) {
                    str += item.Id + ",";
                });

                layer.confirm('确定要执行批量审核吗？', {
                    btn: ['通过', '拒绝']
                }, function (index) {//通过
                    layer.close(index);
                    submitVerify(str, '');
                }, function (index) {
                    layer.close(index);
                    layer.prompt({ title: '请输入拒绝的理由', formType: 2 }, function (text, index) {
                        layer.close(index);
                        submitVerify(str, text);
                    });
                });
            },
            recruitList: function (parm) {
                var winH = $(window).height(), winW = $(window).width();
                layuiIndex = os.OpenRight('报名管理', "/Cms/Recruit/RecruitList" + parm, winW - 220 + 'px', winH - 61 + 'px', function () {
                    if (parseInt($("#isSave").val()) === 1) {
                        $("#isSave").val('0');
                        active.reload();
                    }
                }, function () {
                    active.closeCloumnModify();
                });
            }
        };
        var submitVerify = function (parm, desc) {
            var loadindex = layer.load(1, {
                shade: [0.1, '#000']
            });
            os.ajax('Cms/Recruit/Verify/', { parm: parm, desc: desc }, function (res) {
                layer.close(loadindex);
                if (res.statusCode === 200) {
                    active.reload();
                    os.success('审核成功！');
                } else {
                    os.error(res.msg);
                }
            });
        };
        table.on('toolbar(tool)', function (obj) {
            active[obj.event] ? active[obj.event].call(this) : '';
        });
        $('.list-search .layui-btn').on('click', function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
        //是否审核条件
        form.on('switch(audit)', function (data) {
            type = this.checked ? 0 : 1;
        });
        //监听工具条
        table.on('tool(tool)', function (obj) {
            var data = obj.data;
            if (obj.event === 'edit') {
                active.goModify('?id=' + data.Id);
            } else if (obj.event === 'recruitList') {
                active.recruitList('?id=' + data.Id);
            }
        });
    });


function attrHtml(e) {
    var h = '';
    if (e.IsTop) {
        h += '<span class="layui-badge layui-bg-cyan">推荐</span>';
    }
    if (e.IsHot) {
        h += '<span class="layui-badge layui-bg-cyan">热点</span>';
    }
    if (e.IsScroll) {
        h += '<span class="layui-badge layui-bg-cyan">滚动</span>';
    }
    if (e.IsSlide) {
        h += '<span class="layui-badge layui-bg-cyan">幻灯</span>';
    }
    if (e.IsComment) {
        h += '<span class="layui-badge layui-bg-cyan">评论</span>';
    }
    if (e.IsWap) {
        h += '<span class="layui-badge layui-bg-cyan">移动端</span>';
    }
    return h;
}
