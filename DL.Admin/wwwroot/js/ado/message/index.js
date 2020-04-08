
var app = new Vue({
    el: '#app',
    data: {
        model: {}
    }
});
var active, fun, api, vm = new Vue({
    el: '#adv',
    data: {
        list: [],
        active: {},
        allModel: { id: 0 },
        moreTip: '点击加载更多',
        parm: {
            page: 1,
            limit: 40
        }
    },
    methods: {
        read: function (m, type) {
            if (m.status) {
                api.success('已经读过了！');
            } else {
                fun.read(m, type);
            }
        },
        del: function (m, type) {
            fun.del(m, type);
        },
        goModel: function (m) {
            this.active = m;
            app.model = m;
        },
        loadMore: function () {
            this.parm.page += 1;
            fun.init();
        }
    }
});
layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api'],
    function () {
        var layer = layui.layer,
            $ = layui.jquery,
            os = layui.api;
        $('.layui-colla-content').css({ 'height': $(window).height() - 100 });
        fun = {
            init: function () {
                os.ajax('Cms/Message/GetPages', vm.parm, function (res) {
                    if (res.data.totalPages === 1 || vm.parm.page === res.data.totalPages) {
                        vm.moreTip = '';
                    }
                    if (res.data.totalPages === 0) {
                        vm.moreTip = '没有留言信息';
                    }
                    if (res.statusCode === 200) {
                        if (vm.parm.page === 1) {
                            vm.list = res.data.items;
                        } else {
                            $.each(res.data.items, function (i, m) {
                                vm.list.push(m);
                            });
                        }
                    } else {
                        os.error(res.msg);
                    }
                });
            },
            read: function (m, type) {
                os.ajax('Cms/Message/Read', { parm: m.Id, type: type }, function (res) {
                    if (res.statusCode === 200) {
                        os.success('标记已读成功~');
                        fun.init();
                    } else {
                        os.error(res.msg);
                    }
                });
            },
            del: function (m, type) {
                layer.confirm('确定要执行删除该留言吗？', function (index) {
                    layer.close(index);
                    var loadindex = layer.load(1, {
                        shade: [0.1, '#000']
                    });
                    os.ajax('Cms/Message/Delete/', { parm: m.Id, type: type }, function (res) {
                        layer.close(loadindex);
                        if (res.statusCode === 200) {
                            fun.init();
                        } else {
                            os.error(res.msg);
                        }
                    });
                });
            }
        };
        fun.init();
    }); 