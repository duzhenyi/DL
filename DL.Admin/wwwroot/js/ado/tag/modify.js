layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'form'], function () {
    var form = layui.form, $ = layui.jquery, os = layui.api;
    var index = parent.layer.getFrameIndex(window.name);
    //监听提交
    form.on('submit(submit)', function (data) {
        $('#submit').attr('disabled', true).find('i').removeClass('layui-hide');
        var urls = "Cms/Tags/Add";
        if ($("#ID").val()) {
            urls = "Cms/Tags/Update";
        }
        data.field.Status = data.field.Status === 'on' ? 1 : 0;
        os.ajax(urls, data.field, function (res) {
            $('#submit').attr('disabled', false).find('i').addClass('layui-hide');
            if (res.statusCode == 200) {
                parent.layer.close(index);
            }
            else {
                os.error(res.msg);
            }
        });
        return false;
    });
    $(".btn-open-close").on('click', function () {
        parent.layer.close(index);
    });
});
