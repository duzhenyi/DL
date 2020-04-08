//字体发光
const signs = document.querySelectorAll('x-sign');
const randomIn = (min, max) => (
    Math.floor(Math.random() * (max - min + 1) + min)
);
const mixupInterval = el => {
    const ms = randomIn(2000, 4000)
    el.style.setProperty('--interval', `${ms}ms`)
};

layui.config({
    base: '/js/modules/'
}).extend({  //指定js别名
    api: 'api',
    toastr: 'toastr',
}).use(['element', 'jquery', 'form', 'api'], function () {
    var form = layui.form,
        $ = layui.jquery,
        api = layui.api;

    $('#clear').on('click', function () {
        $("#loginname").val('');
        $("#password").val('');
    });
    signs.forEach(el => {
        mixupInterval(el)
        el.addEventListener('webkitAnimationIteration', () => {
            mixupInterval(el)
        })
    });

    $(".layui-btn-danger").click(function () {
        document.getElementById("forms").reset();
    });
    //清空token

    api.removeSession('DLADMIN_ACCESS_TOKEN');
    form.on('submit(loginsub)', function (data) {
        var crypt = new JSEncrypt();
        crypt.setPrivateKey(data.field.privateKey);
        var enc = crypt.encrypt(data.field.password);
        $("#password").val(enc);
        data.field.password = enc;
        var btns = $(".layui-btn-normal");
        btns.html('<i class="layui-icon layui-anim layui-anim-rotate layui-anim-loop"></i>');
        btns.attr('disabled', 'disabled');

        api.ajax('Login/Login', "POST", data.field, function (res) {
            if (res.statusCode === 200) {
                api.setSession('DLADMIN_ACCESS_TOKEN', res.data);
                setTimeout(function () {
                    var rurl = api.getUrlParam('ReturnUrl');
                    if (!rurl) {
                        window.location.href = '/Home/Index';
                    }
                    else {
                        window.location.href = rurl;
                    }
                }, 1000);
            } else {
                $(".login-tip span").html(res.msg);
                $("#password").val('');
                $(".login-tip").animate({ 'height': '30px' });
                setTimeout(function () {
                    $(".login-tip").animate({ 'height': 0 });
                    $(".login-tip span").html('');
                }, 2500);
            }
            btns.attr('disabled', false);
            setTimeout(function () {
                btns.html('登录');
            }, 1000);
        });
        return false;
    });
    $(window).resize(
        bodysize
    );
    bodysize();
    function bodysize() {
        $("body").height($(window).height());
    }
});