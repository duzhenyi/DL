
var api, $, fun, form, vm = new Vue({
    el: '#app',
    data: {
        model: {}
    },
    methods: {

    }
});

layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'form'], function () {
    var form = layui.form;
    api = layui.api;
    $ = layui.$;
    api.cloudFile();

    fun = {
        init: function () {
            api.ajax('Cms/CmsSite/GetSite', {}, function (res) {
                if (res.statusCode === 200) {
                    vm.model = res.data;
                } else {
                    api.error(res.msg);
                }
            });
        }
    }
    fun.init();

    form.on('submit(submit)', function (data) {//监听提交
        data.field.Status = data.field.Status === 'on' ? true : false;
        api.ajax("Cms/CmsSite/SaveSite", data.field, function (res) {
            if (res.statusCode == 200) {
                fun.init();
                api.success('保存成功~');
            }
            else {
                api.error(res.msg);
            }
        });
        return false;
    });
}); 