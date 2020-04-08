
Win10.onReady(function () {
    Win10.setBgUrl({ //设置壁纸
        main: '/images/bg_01.jpg',
        mobile: '/images/wallpapers/mobile.jpg',
    });

    Win10.setAnimated([
        'animated flip',
        'animated bounceIn',
    ], 0.01);

    layui.config({
        base: '/js/modules/'
    }).extend({  //指定js别名 
        api: 'api',//辅助核心文件
        toastr: 'toastr',//消息提示文件
    }).use(['element', 'layer', 'jquery', 'api', 'toastr'], function () {
        var $ = layui.jquery,
            api = layui.api;
        api.ajax('Sys/Menu/AuthMenu', 'GET', {}, function (res) {
            if (res.statusCode === 200) {
                var html = '';
                var deskTopHtml = '';
                $.each(res.data, function (index, item) {
                    if (item.type === 1) {
                        if (item.url != "") {
                            html += '<div class="item" onclick="Win10.openUrl(\'' + item.url + ' \',\' ' + item.name + '\')">';

                        } else {
                            html += ' <div class="item" >';
                        }
                        if (item.icon != "") {
                            html += '<i style="color:' + item.iconColor + '" class="layui-icon ' + item.icon + ' icon fa icon "></i>';
                        }
                        html += item.name;
                        html += '</div>';
                    }
                    $.each(res.data, function (i, it) {
                        if (it.url != "" && it.parentID === item.ID) {
                            html += '<div class="sub-item" onclick="Win10.openUrl(\'' + it.url + ' \',\' ' + it.name + '\')">';
                            if (it.icon != "") {
                                html += '<i style="color:' + it.iconColor + '" class="layui-icon ' + it.icon + ' icon fa icon "></i>';
                            }
                            html += it.name + "</div>";
                        }
                    });

                    if (item.isDeskTop) {//置顶桌面
                        deskTopHtml += '<div class="shortcut" ';
                        deskTopHtml += 'onclick=\'Win10.openUrl(\"' + item.url + '\",\"<i class=\\\"layui-icon ' + item.icon + '\\\"></i>' + item.name + '\")\'>';
                        deskTopHtml += '<i style=\'color:' + item.iconColor + '\' class=\'layui-icon ' + item.icon + ' icon\'></i>';
                        deskTopHtml += '<div class="title" >' + item.name + '</div > ';
                        deskTopHtml += '</div > ';
                    }
                });
                //Win10.exit();
                html += '<div class="item loginOut" > <i class="black icon fa fa-power-off fa-fw"></i>退出登录</div>';
                console.log(html);
                console.log(deskTopHtml);
                $('#menu').html(html);
                $('#win10-shortcuts').html(deskTopHtml);
                Win10.buildList();//预处理左侧菜单 
                Win10.renderShortcuts();//渲染桌面图标


                $('.loginOut').on('click', function () {
                    layer.confirm('确定要执行退出操作吗？', function (index) {
                        layer.close(index);
                        var loadindex = layer.load(1, {
                            shade: [0.1, '#000']
                        });
                        api.ajax('Login/LogOut', 'get', {}, function (res) {
                            layer.close(loadindex);
                            if (res.statusCode === 200) {
                                window.location = res.data;
                            } else {
                                api.error(res.msg);
                            }
                        });
                    });
                });
            }
        });


    });

    setTimeout(function () {
        Win10.newMsg('技术支持', '技术支持QQ：<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=295611875&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:295611875:51" alt="点击这里给我发消息" title="点击这里给我发消息"/></a>')
    }, 2500);

    //setTimeout(function () {
    //    Win10.openUrl('<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=295611875&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:295611875:53" alt="点击这里给我发消息" title="点击这里给我发消息"/></a>', '<i class="fa fa-newspaper-o icon red"></i>在线咨询', [['300px', '380px'], 'rt'])
    //}, 2000);
});
