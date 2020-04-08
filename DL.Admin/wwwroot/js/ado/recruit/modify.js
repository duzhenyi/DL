var oc, api;
tinymce.init({
    selector: '#JobResponsibilities',
    height: 400,
    plugins: 'print preview code searchreplace autolink directionality visualblocks visualchars fullscreen image link media codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern help filemanager',
    toolbar: 'formatselect styleselect | bold italic forecolor backcolor | link filemanager | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat'
});
layui.config({
    base: '/js/modules/'
}).use(['layer', 'jquery', 'api', 'form', 'laydate', 'slider'], function () {
    var form = layui.form, $ = layui.jquery, laydate = layui.laydate, slider = layui.slider;
    api = layui.api;

    form.render();
    laydate.render({
        elem: '#UpdateDate'
        , theme: '#393D49'
        , type: 'datetime'
    });

    laydate.render({
        elem: '#BeginTime'
        , type: 'datetime'
    });
    laydate.render({
        elem: '#EndTime'
        , type: 'datetime'
    });

    $('.panel-addpic').css({ 'min-height': $(window).height() - 95 });
    var sliderIndex = slider.render({
        elem: '#slideSort'
        , input: true
        , change: function (value) {
            sort = value;
        }
        , theme: '#409eff'
    });

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
            $('#imgShow').attr('src', '');
        }
    };

    //监听提交
    form.on('submit(submit)', function (data) {
        $('#submit').attr('disabled', true).find('i').removeClass('layui-hide');
        data.field.Content = tinyMCE.editors[0].getContent();
        var urls = "Cms/Recruit/Add";
        if ($("#Id").val() !== '0') {
            urls = "Cms/Recruit/Update";
        }
        data.field.Audit = data.field.Audit === 'on' ? true : false;
        data.field.IsTop = data.field.IsTop === 'on' ? true : false;
        data.field.IsHot = data.field.IsHot === 'on' ? true : false;
        data.field.IsScroll = data.field.IsScroll === 'on' ? true : false;
        data.field.IsSlide = data.field.IsSlide === 'on' ? true : false;
        data.field.IsComment = data.field.IsComment === 'on' ? true : false;
        data.field.IsTimeLimit = data.field.IsTimeLimit === 'on' ? true : false;
        data.field.WorkType = data.field.WorkType === 'on' ? true : false;
        data.field.Sort = sort;

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
    form.on('switch(switchOpen)', function (data) {
        if (this.checked) {
            $('.isTime').removeClass('layui-hide');
        } else {
            $('.isTime').addClass('layui-hide');
        }
    });
});