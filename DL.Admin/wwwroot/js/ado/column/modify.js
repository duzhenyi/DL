
var oc, api;
tinymce.init({
    selector: '#Content',
    height: 400,
    plugins: 'print preview code searchreplace autolink directionality visualblocks visualchars fullscreen image link media codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern help filemanager',
    toolbar: 'formatselect styleselect | bold italic forecolor backcolor | link filemanager | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat'
});
layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'form'], function () {
    var form = layui.form, $ = layui.jquery;
    api = layui.api;
    form.render();
    $('.panel-addpic').css({ 'min-height': $(window).height() - 95 });

    api.cloudFile();
    oc = {
        setContent(v) {
            var imgIndex = v.lastIndexOf('/');
            var str = v.substring(imgIndex + 1, v.length);
            if (api.isExtImage(v)) {
                tinyMCE.editors[0].execCommand('mceInsertContent', false, '<img src="' + v + '" alt="' + str + '"/>')
            } else {
                tinyMCE.editors[0].execCommand('mceInsertContent', false, '<p style="padding:12px 20px;background-color: #edf3f5;"><a href="' + v + '" target="_blank" title="' + str + '">' + str + '</a></p>')
            }
        },
        fileSave(v) {
            $(".select-newimg").addClass('layui-hide');
            $(".add-photo-wall").removeClass('layui-hide');
            $('#ImgUrl').val(v);
            $('#imgShow').attr('src', v);
        },
        deleteFile() {
            $(".select-newimg").removeClass('layui-hide');
            $(".add-photo-wall").addClass('layui-hide');
            $('#ImgUrl').val('');
            $('#imgShow').attr('src', v);
        }
    };

    //监听提交
    form.on('submit(submit)', function (data) {
        $('#submit').attr('disabled', true).find('i').removeClass('layui-hide');
        data.field.Content = tinyMCE.editors[0].getContent();
        var urls = "Cms/Column/Add";
        if ($("#Id").val() !== '0') {
            urls = "Cms/Column/Update";
        }
        data.field.IsTopShow = data.field.IsTopShow === 'on' ? true : false;
        data.field.IsWapShow = data.field.IsWapShow === 'on' ? true : false;
        api.ajax(urls, data.field, function (res) {
            $('#submit').attr('disabled', false).find('i').addClass('layui-hide');
            if (res.statusCode === 200) {
                var $$ = parent.layui.jquery;
                $$("#isSave").val('1');
                api.success('保存成功~');
                setTimeout(function () {
                    parent.active.closeCloumnModify();
                }, 500);
            }
            else {
                api.error(res.msg);
            }
        });
        return false;
    });
    $(".btn-open-close").on('click', function () {
        parent.active.closeCloumnModify();
    });
}); 