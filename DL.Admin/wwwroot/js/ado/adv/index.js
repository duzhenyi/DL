
var active, fun, api, vm = new Vue({
    el: '#adv',
    data: {
        menu: [],
        menuActive: {}
    },
    methods: {
        classAdd: function () {
            api.Open('添加广告栏位', '/Cms/Adv/AdvClass', '750px', '550px', function () {
                fun.init();
            });
        },
        classUpdate: function (m) {
            api.Open('添加广告栏位', '/Cms/Adv/AdvClass?ID=' + m.ID, '750px', '550px', function () {
                fun.init();
            });
        },
        classDel: function (m) {
            fun.classDel(m);
        },
        goAvdList: function (m) {
            this.menuActive = m;
            active.reload();
        }
    }
});

layui.config({
    base: '/js/modules/'
}).use(['element', 'layer', 'jquery', 'api', 'table'],
    function () {
        var layer = layui.layer,
            table = layui.table,
            $ = layui.jquery,
            element = layui.element;
        api = layui.api;
        table.render({
            elem: '#tablist',
            headers: api.getToken(),
            url: '/Cms/Adv/GetAdvListPages',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    {
                        field: 'Title', title: '名称', fixed: 'left'
                    },
                    {
                        field: 'ImgUrl', width: 200, title: '广告图', templet: function (res) {
                            return fun.imgHtml(res);
                        }
                    },
                    { field: 'Status', width: 100, title: '状态', templet: '#switchTpl' },
                    { field: 'Sort', width: 80, title: '权重' },
                    { field: 'UpdateDate', width: 200, title: '更新时间' },
                    { width: 100, title: '操作', templet: '#tool' }
                ]
            ],
            page: true,
            id: 'tables',
            where: {
                key: ""
            }
        });
        fun = {
            init: function () {
                api.ajax('Cms/Adv/GetClassPages', null, function (res) {
                    if (res.statusCode === 200) {
                        vm.menu = res.data;
                        vm.$nextTick(function () {
                            element.render();
                        });
                    } else {
                        api.error(res.msg);
                    }
                });
            },
            classDel: function (m) {
                layer.confirm('确定要执行删除栏位吗？', function (index) {
                    layer.close(index);
                    var loadindex = layer.load(1, {
                        shade: [0.1, '#000']
                    });
                    api.ajax('Cms/Adv/DeleteClass/', { parm: m.ID }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            fun.init();
                        } else {
                            api.error(res.msg);
                        }
                    });
                });
            },
            imgHtml: function (m) {
                var str = '';
                if (m.AdvType === 1) {
                    str = '<a href="' + m.LinkUrl + '" target="_blank" class="text-color">';
                    if (m.LinkUrl === "null" || m.LinkUrl === null) {
                        str += "链接";
                    } else {
                        str += m.LinkUrl;
                    }
                    return str;
                }
                else if (m.AdvType === 0) {
                    str = '<a href="' + m.ImgUrl + '" target="_blank" style="display: inline-block;padding:0px 5px 0 5px">';
                    if (m.ImgUrl === null) {
                        str += '<i class="layui-icon layui-icon-picture" style="font-size:30px;"></i></a>';
                    } else {
                        str += '<img src="' + m.ImgUrl + '?imageView2/1/w/110/h/55" width="110" height="55" /></a>';
                    }
                    return str;
                }
                else {
                    str = '其它';
                }
            }
        };
        fun.init();

        active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            key: vm.menuActive.ID
                        }
                    });
            },
            toolAdd: function () {
                if (JSON.stringify(vm.menuActive) === '{}') {
                    api.error('请选择左侧栏位~');
                    return;
                } 
                api.Open('添加广告位信息', '/Cms/Adv/Advmodify?column=' + vm.menuActive.ID, '900px', '600px', function () {
                    active.reload();
                });
            },
            toolDel: function () {
                var checkStatus = table.checkStatus('tables')
                    , data = checkStatus.data;
                if (data.length === 0) {
                    api.error("请选择要删除的项目~");
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
                    api.ajax('Cms/Adv/DeleteAdvList/', { parm: str }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            active.reload();
                            api.success(res.msg);
                        } else {
                            api.error(res.msg);
                        }
                    });
                });

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
                api.Open('修改广告位信息', '/Cms/Adv/Advmodify/?column=' + data.ClassID + '&ID=' + data.ID, '900px', '600px', function () {
                    active.reload();
                });
            }
        });
    }); 