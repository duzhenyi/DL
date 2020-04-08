layui.define(['layer', 'toastr'], function (exports) {
    "use strict";
    var $ = layui.jquery,
        layer = layui.layer,
        toastr = layui.toastr;
    var key = "DLADMIN_ACCESS_TOKEN";
    var api = {
        cloudFile: function () {
            $(".dl-cloud").click(function () {
                var input_text = $(this).data("text");
                var showImg = $(this).data('img');
                var type = $(this).data('type'); //edit=编辑器  sign=默认表单  iframe=弹出层  form=带图片显示
                var frameId = window.frameElement && window.frameElement.id || '', frameUrl = '';
                if (frameId) {
                    frameUrl = '&frameid=' + frameId;
                }
                api.Open('图片资源库', '/Sys/Img/Index/?type=' + type + '&img=' + showImg + '&control=' + input_text + frameUrl, '950px', '600px');
            });
        },
        error: function (msg) {
            layer.msg(msg, { icon: 5 });
        },
        warning: function (msg) {
            layer.msg(msg, { icon: 7 });
        },
        success: function (msg) {
            layer.msg(msg, { icon: 6 });
        },
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        setSession: function (key, options) {
            localStorage.setItem(key, JSON.stringify(options));
        },
        getSession: function (key) {
            var obj = localStorage.getItem(key);
            if (obj != null) {
                return JSON.parse(obj);
            }
            return null;
        },
        removeSession: function (key) {
            localStorage.removeItem(key);
        },
        getToken: function () {
            var token = api.getSession(key);
            return { 'Authorization': 'Bearer ' + token };
        },
        Open: function (title, url, width, height, fun, full = false,maxmin=true) {
            var index = top.layer.open({
                type: 2,
                title: title,
                shadeClose: false,
                shade: 0.8,
                //move:false,
                skin: 'layer-cur-open',
                maxmin: maxmin, //开启最大化最小化按钮
                area: [width, height],
                content: url,
                zIndex: "10000",
                end: function () {
                    if (fun) fun();
                }
            });
            if (full) {
                top.layer.full(index);
            }
        },
        ajax: function (url, method, data, callFun) {
            var httpUrl = "/", token = api.getSession(key);
            var _headers = {};
            if (token !== null) {
                _headers = {
                    'Authorization': 'Bearer ' + token
                };
            }
            method = method || 'GET';
            data = method === 'GET' ? data : JSON.stringify(data);
            console.log(data);
            $.ajax(httpUrl + url, {
                data: data,
                contentType: 'application/json',
                dataType: 'json', //服务器返回json格式数据
                type: method, //HTTP请求类型
                timeout: 10 * 1000, //超时时间设置为50秒；
                headers: _headers,
                success: function (data) {
                    callFun(data);
                },
                error: function (xhr, type, errorThrown) {
                    if (type === 'timeout') {
                        api.error('连接超时，请稍后重试！');
                    } else if (type === 'error') {
                        api.error('连接异常，请稍后重试！');
                    }
                }
            });

        }
    };

  

    exports('api', api);
});