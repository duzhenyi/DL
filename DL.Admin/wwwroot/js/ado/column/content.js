
Vue.component('item', {
    template: '#item-template',
    props: {
        model: Object
    },
    data: function () {
        return {
            open: true
        };
    },
    computed: {
        isFolder: function () {
            return this.model.children &&
                this.model.children.length;
        }
    },
    methods: {
        toggle: function () {
            if (this.isFolder) {
                this.open = !this.open;
            }
        },
        isModify: function (href) {
            if (href) {
                if (href.indexOf('columnmodify') !== -1) {
                    return true;
                }
            }
            return false;
        },
        goOpen: function (m) {
            fun.openModify(m);
        }
    }
});
var active, fun, layuiIndex, vm = new Vue({
    el: '#tree',
    data: {
        treeData: {}
    }
});

layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'pjax'],
    function () {
        var layer = layui.layer,
            $ = layui.jquery,
            os = layui.api;
        os.ajax('Cms/Column/GetTree', { type: 1 }, function (res) {
            if (res.statusCode === 200) {
                vm.treeData = { 'name': '栏目列表', children: res.data };
            } else {
                os.error(res.msg);
            }
        });
        fun = {
            openModify: function (m) {
                var winH = $(window).height(), winW = $(window).width();
                layuiIndex = os.OpenRight('栏目管理', m.href, winW - 220 + 'px', winH - 61 + 'px', null, function () {
                    active.closeCloumnModify();
                });
            }
        };
        active = {
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
        $.pjax({
            url: '/Ams/Article/Index?column=0',
            container: '#content-body',
            fragment: '#content-body',
            push: false
        });
        $(document).pjax('a[cur-pjax]', '#content-body',
            {
                fragment: "#content-body",
                cache: false,
                push: false,
                show: 'fade'
            }
        );
    }); 