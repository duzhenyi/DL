layui.config({
    base: '/js/modules/'
}).extend({
    iconPicker: 'iconPicker'
}).use(['layer', 'jquery', 'api', 'form', 'ztree', 'iconPicker'], function () {
    var form = layui.form, $ = layui.jquery, os = layui.api, iconPicker = layui.iconPicker;
    var index = parent.layer.getFrameIndex(window.name);
    iconPicker.render({
        elem: '#iconPicker',
        type: 'fontClass',
        search: true,
        page: false,
        click: function (data) {
            $("#Icon").val(data.icon);
        }
    });
    //监听提交
    form.on('submit(submit)', function (data) {
        $('#submit').attr('disabled', true).find('i').removeClass('layui-hide');
        var urls = "Ado/ClassType/Add";
        if ($("#ID").val()) {
            urls = "Ado/ClassType/Update";
        }
        data.field.Status = data.field.Status === 'on' ? 1 : 0;
        data.field.IsTop = data.field.IsTop === 'on' ? true : false;
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

    $("#ParentName").click(function () {
        $(".select-tree").addClass('active');
    });
    $(".select-tree").mouseleave(function () {
        $(".select-tree").removeClass('active');
    });

    $.fn.zTree.init($("#tree"), {
        async: {
            enable: true,
            headers: os.getToken(),
            url: "/Ado/ClassType/GetTree"
        },
        callback: {
            onClick: function (event, treeId, treeNode, clickFlag) {
                $("#ParentId").val(treeNode.ID);
                $("#ParentName").val(treeNode.name);
                $(".select-tree").removeClass('active');
            }
        }
    });

}); 