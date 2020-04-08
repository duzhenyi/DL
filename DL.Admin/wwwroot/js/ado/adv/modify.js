
layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'form', 'slider', 'laydate'], function () {
    var form = layui.form, $ = layui.jquery, os = layui.api, slider = layui.slider, laydate = layui.laydate, sort =@Model.Sort;
    os.cloudFile();
    var index = parent.layer.getFrameIndex(window.name);
    var sliderIndex = slider.render({
        elem: '#slideSort'
        , input: true
        , change: function (value) {
            sort = value;
        }
        , theme: '#409eff'
    });
    //赋值
    sliderIndex.setValue('@Model.Sort');
form.val('adv', {
    "AdvType": "@Model.AdvType"
    , "Target": "@Model.Target"
})
laydate.render({
    elem: '#BeginTime'
    , type: 'datetime'
});
laydate.render({
    elem: '#EndTime'
    , type: 'datetime'
});
//监听提交
form.on('submit(submit)', function (data) {
    $('#submit').attr('disabled', true).find('i').removeClass('layui-hide');
    var urls = "Cms/Adv/AddAdvList";
    if ($("#ID").val()) {
        urls = "Cms/Adv/UpdateAdvList";
    }
    data.field.Status = data.field.Status === 'on' ? true : false;
    data.field.IsTimeLimit = data.field.IsTimeLimit === 'on' ? true : false;
    data.field.Sort = sort;
    os.ajax(urls, data.field, function (res) {
        $('#submit').attr('disabled', false).find('i').addClass('layui-hide');
        if (res.statusCode === 200) {
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
form.on('switch(switchOpen)', function (data) {
    if (this.checked) {
        $('.isTime').removeClass('layui-hide');
    } else {
        $('.isTime').addClass('layui-hide');
    }
});
}); 