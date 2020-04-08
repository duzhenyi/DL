
layui.define(['laypage', 'form'], function (exports) {
    "use strict";

    var IconPicker = function () {
        this.v = '0.1.beta';
    }, _MOD = 'iconPicker',
        _this = this,
        $ = layui.jquery,
        laypage = layui.laypage,
        form = layui.form,
        BODY = 'body',
        TIPS = '请选择图标';

    /**
     * 渲染组件
     */
    IconPicker.prototype.render = function (options) {
        var opts = options,
            // DOM选择器
            elem = opts.elem,
            // 数据类型：fontClass/unicode
            type = opts.type == null ? 'fontClass' : opts.type,
            // 是否分页：true/false
            page = opts.page,
            // 每页显示数量
            limit = limit == null ? 12 : opts.limit,
            // 是否开启搜索：true/false
            search = opts.search == null ? true : opts.search,
            // 点击回调
            click = opts.click,
            // 渲染成功后的回调
            success = opts.success,
            // json数据
            data = {},
            // 唯一标识
            tmp = new Date().getTime(),
            // 是否使用的class数据
            isFontClass = opts.type === 'fontClass',
            // 初始化时input的值
            ORIGINAL_ELEM_VALUE = $(elem).val(),
            TITLE = 'layui-select-title',
            TITLE_ID = 'layui-select-title-' + tmp,
            ICON_BODY = 'layui-iconpicker-' + tmp,
            PICKER_BODY = 'layui-iconpicker-body-' + tmp,
            PAGE_ID = 'layui-iconpicker-page-' + tmp,
            LIST_BOX = 'layui-iconpicker-list-box',
            selected = 'layui-form-selected',
            unselect = 'layui-unselect';

        var a = {
            init: function () {
                data = common.getData[type]();

                a.hideElem().createSelect().createBody().toggleSelect();
                a.preventEvent();
                common.loadCss();

                if (success) {
                    success(this.successHandle());
                }

                return a;
            },
            successHandle: function () {
                var d = {
                    options: opts,
                    data: data,
                    id: tmp,
                    elem: $('#' + ICON_BODY)
                };
                return d;
            },
            /**
             * 隐藏elem
             */
            hideElem: function () {
                $(elem).hide();
                return a;
            },
            /**
             * 绘制select下拉选择框
             */
            createSelect: function () {
                var oriIcon = '<i class="layui-icon">';

                // 默认图标
                if (ORIGINAL_ELEM_VALUE === '') {
                    if (isFontClass) {
                        ORIGINAL_ELEM_VALUE = 'layui-icon-circle-dot';
                    } else {
                        ORIGINAL_ELEM_VALUE = '&#xe617;';
                    }
                }

                if (isFontClass) {
                    oriIcon = '<i class="layui-icon ' + ORIGINAL_ELEM_VALUE + '">';
                } else {
                    oriIcon += ORIGINAL_ELEM_VALUE;
                }
                oriIcon += '</i>';

                var selectHtml = '<div class="layui-iconpicker layui-unselect layui-form-select" id="' + ICON_BODY + '">' +
                    '<div class="' + TITLE + '" id="' + TITLE_ID + '">' +
                    '<div class="layui-iconpicker-item">' +
                    '<span class="layui-iconpicker-icon layui-unselect">' +
                    oriIcon +
                    '</span>' +
                    '<i class="layui-edge"></i>' +
                    '</div>' +
                    '</div>' +
                    '<div class="layui-anim layui-anim-upbit" style="">' +
                    '123' +
                    '</div>';
                console.log(selectHtml)
                $(elem).after(selectHtml);
                return a;
            },
            /**
             * 展开/折叠下拉框
             */
            toggleSelect: function () {
                var item = '#' + TITLE_ID + ' .layui-iconpicker-item,#' + TITLE_ID + ' .layui-iconpicker-item .layui-edge';
                a.event('click', item, function (e) {
                    console.log('xxxx');
                    var $icon = $('#' + ICON_BODY);
                    if ($icon.hasClass(selected)) {
                        $icon.removeClass(selected).addClass(unselect);
                    } else {
                        $icon.addClass(selected).removeClass(unselect);
                    }
                    e.stopPropagation();
                });
                return a;
            },
            /**
             * 绘制主体部分
             */
            createBody: function () {
                // 获取数据
                var searchHtml = '';

                if (search) {
                    searchHtml = '<div class="layui-iconpicker-search">' +
                        '<input class="layui-input">' +
                        '<i class="layui-icon">&#xe615;</i>' +
                        '</div>';
                }

                // 组合dom
                var bodyHtml = '<div class="layui-iconpicker-body" id="' + PICKER_BODY + '">' +
                    searchHtml +
                    '<div class="' + LIST_BOX + '"></div> ' +
                    '</div>';
                $('#' + ICON_BODY).find('.layui-anim').eq(0).html(bodyHtml);
                a.search().createList().check().page();

                return a;
            },
            /**
             * 绘制图标列表
             * @param text 模糊查询关键字
             * @returns {string}
             */
            createList: function (text) {
                var d = data,
                    l = d.length,
                    pageHtml = '',
                    listHtml = $('<div class="layui-iconpicker-list">')//'<div class="layui-iconpicker-list">';

                // 计算分页数据
                var _limit = limit, // 每页显示数量
                    _pages = l % _limit === 0 ? l / _limit : parseInt(l / _limit + 1), // 总计多少页
                    _id = PAGE_ID;

                // 图标列表
                var icons = [];

                for (var i = 0; i < l; i++) {
                    var obj = d[i];

                    // 判断是否模糊查询
                    if (text && obj.indexOf(text) === -1) {
                        continue;
                    }

                    // 每个图标dom
                    var icon = '<div class="layui-iconpicker-icon-item" title="' + obj + '">';
                    if (isFontClass) {
                        icon += '<i class="layui-icon ' + obj + '"></i>';
                    } else {
                        icon += '<i class="layui-icon">' + obj.replace('amp;', '') + '</i>';
                    }
                    icon += '</div>';

                    icons.push(icon);
                }

                // 查询出图标后再分页
                l = icons.length;
                _pages = l % _limit === 0 ? l / _limit : parseInt(l / _limit + 1);
                for (var i = 0; i < _pages; i++) {
                    // 按limit分块
                    var lm = $('<div class="layui-iconpicker-icon-limit" id="layui-iconpicker-icon-limit-' + (i + 1) + '">');

                    for (var j = i * _limit; j < (i + 1) * _limit && j < l; j++) {
                        lm.append(icons[j]);
                    }

                    listHtml.append(lm);
                }

                // 无数据
                if (l === 0) {
                    listHtml.append('<p class="layui-iconpicker-tips">无数据</p>');
                }

                // 判断是否分页
                if (page) {
                    $('#' + PICKER_BODY).addClass('layui-iconpicker-body-page');
                    pageHtml = '<div class="layui-iconpicker-page" id="' + PAGE_ID + '">' +
                        '<div class="layui-iconpicker-page-count">' +
                        '<span id="' + PAGE_ID + '-current">1</span>/' +
                        '<span id="' + PAGE_ID + '-pages">' + _pages + '</span>' +
                        ' (<span id="' + PAGE_ID + '-length">' + l + '</span>)' +
                        '</div>' +
                        '<div class="layui-iconpicker-page-operate">' +
                        '<i class="layui-icon" id="' + PAGE_ID + '-prev" data-index="0" prev>&#xe603;</i> ' +
                        '<i class="layui-icon" id="' + PAGE_ID + '-next" data-index="2" next>&#xe602;</i> ' +
                        '</div>' +
                        '</div>';
                }


                $('#' + ICON_BODY).find('.layui-anim').find('.' + LIST_BOX).html('').append(listHtml).append(pageHtml);
                return a;
            },
            // 阻止Layui的一些默认事件
            preventEvent: function () {
                var item = '#' + ICON_BODY + ' .layui-anim';
                a.event('click', item, function (e) {
                    e.stopPropagation();
                });
                return a;
            },
            // 分页
            page: function () {
                var icon = '#' + PAGE_ID + ' .layui-iconpicker-page-operate .layui-icon';

                $(icon).unbind('click');
                a.event('click', icon, function (e) {
                    var elem = e.currentTarget,
                        total = parseInt($('#' + PAGE_ID + '-pages').html()),
                        isPrev = $(elem).attr('prev') !== undefined,
                        // 按钮上标的页码
                        index = parseInt($(elem).attr('data-index')),
                        $cur = $('#' + PAGE_ID + '-current'),
                        // 点击时正在显示的页码
                        current = parseInt($cur.html());

                    // 分页数据
                    if (isPrev && current > 1) {
                        current = current - 1;
                        $(icon + '[prev]').attr('data-index', current);
                    } else if (!isPrev && current < total) {
                        current = current + 1;
                        $(icon + '[next]').attr('data-index', current);
                    }
                    $cur.html(current);

                    // 图标数据
                    $('.layui-iconpicker-icon-limit').hide();
                    $('#layui-iconpicker-icon-limit-' + current).show();
                    e.stopPropagation();
                });
                return a;
            },
            /**
             * 搜索
             */
            search: function () {
                var item = '#' + PICKER_BODY + ' .layui-iconpicker-search .layui-input';
                a.event('input propertychange', item, function (e) {
                    var elem = e.target,
                        t = $(elem).val();
                    a.createList(t);
                });
                return a;
            },
            /**
             * 点击选中图标
             */
            check: function () {
                var item = '#' + PICKER_BODY + ' .layui-iconpicker-icon-item';
                a.event('click', item, function (e) {
                    var el = $(e.currentTarget).find('.layui-icon'),
                        icon = '';
                    if (isFontClass) {
                        var clsArr = el.attr('class').split(/[\s\n]/),
                            cls = clsArr[1],
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .layui-icon').html('').attr('class', clsArr.join(' '));
                    } else {
                        var cls = el.html(),
                            icon = cls;
                        $('#' + TITLE_ID).find('.layui-iconpicker-item .layui-icon').html(icon);
                    }

                    $('#' + ICON_BODY).removeClass(selected).addClass(unselect);
                    $(elem).attr('value', icon);
                    // 回调
                    if (click) {
                        click({
                            icon: icon
                        });
                    }

                });
                return a;
            },
            // 监听原始input数值改变
            inputListen: function () {
                var el = $(elem);
                // TODO
            },
            event: function (evt, el, fn) {
                $(BODY).on(evt, el, fn);
            }
        };

        var common = {
            /**
             * 加载样式表
             */
            loadCss: function () {
                var css = '.layui-iconpicker {max-width: 280px;}.layui-iconpicker .layui-anim{display:none;position:absolute;left:0;top:42px;padding:5px 0;z-index:899;min-width:100%;border:1px solid #d2d2d2;max-height:300px;overflow-y:auto;background-color:#fff;border-radius:2px;box-shadow:0 2px 4px rgba(0,0,0,.12);box-sizing:border-box;}.layui-iconpicker-item{border:1px solid #e6e6e6;width:90px;height:38px;border-radius:4px;cursor:pointer;position:relative;}.layui-iconpicker-icon{border-right:1px solid #e6e6e6;-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;display:block;width:60px;height:100%;float:left;text-align:center;background:#fff;transition:all .3s;}.layui-iconpicker-icon i{line-height:38px;font-size:18px;}.layui-iconpicker-item > .layui-edge{left:70px;}.layui-iconpicker-item:hover{border-color:#D2D2D2!important;}.layui-iconpicker-item:hover .layui-iconpicker-icon{border-color:#D2D2D2!important;}.layui-iconpicker.layui-form-selected .layui-anim{display:block;}.layui-iconpicker-body{padding:6px;}.layui-iconpicker .layui-iconpicker-list{background-color:#fff;border:1px solid #ccc;border-radius:4px;}.layui-iconpicker .layui-iconpicker-icon-item{display:inline-block;width:21.1%;line-height:36px;text-align:center;cursor:pointer;vertical-align:top;height:36px;margin:4px;border:1px solid #ddd;border-radius:2px;transition:300ms;}.layui-iconpicker .layui-iconpicker-icon-item i.layui-icon{font-size:17px;}.layui-iconpicker .layui-iconpicker-icon-item:hover{background-color:#eee;border-color:#ccc;-webkit-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;-moz-box-shadow:0 0 2px #aaa,0 0 2px #fff inset;box-shadow:0 0 2px #aaa,0 0 2px #fff inset;text-shadow:0 0 1px #fff;}.layui-iconpicker-search{position:relative;margin:0 0 6px 0;border:1px solid #e6e6e6;border-radius:2px;transition:300ms;}.layui-iconpicker-search:hover{border-color:#D2D2D2!important;}.layui-iconpicker-search .layui-input{cursor:text;display:inline-block;width:86%;border:none;padding-right:0;margin-top:1px;}.layui-iconpicker-search .layui-icon{position:absolute;top:11px;right:4%;}.layui-iconpicker-tips{text-align:center;padding:8px 0;cursor:not-allowed;}.layui-iconpicker-page{margin-top:6px;margin-bottom:-6px;font-size:12px;padding:0 2px;}.layui-iconpicker-page-count{display:inline-block;}.layui-iconpicker-page-operate{display:inline-block;float:right;cursor:default;}.layui-iconpicker-page-operate .layui-icon{font-size:12px;cursor:pointer;}.layui-iconpicker-body-page .layui-iconpicker-icon-limit{display:none;}.layui-iconpicker-body-page .layui-iconpicker-icon-limit:first-child{display:block;}';
                $('head').append('<style rel="stylesheet">' + css + '</style>');
            },
            /**
             * 获取数据
             */
            getData: {
                fontClass: function () {
                    var arr = ["","layui-icon-zadd", "layui-icon-zairplane", "layui-icon-zairplane-l", "layui-icon-zalipay", "layui-icon-zanalysis", "layui-icon-zandroid", "layui-icon-zangle-down-l", "layui-icon-zangle-left-l", "layui-icon-zangle-right-l", "layui-icon-zangle-up-l", "layui-icon-zapple", "layui-icon-zarrow-down-l", "layui-icon-zarrow-up-l", "layui-icon-zaso-l", "layui-icon-zassociation-l", "layui-icon-zbaiduwangpan", "layui-icon-zbar-chart", "layui-icon-zbar-chart-l", "layui-icon-zbattery", "layui-icon-zbattery-l", "layui-icon-zbell", "layui-icon-zbell-l", "layui-icon-zbilibili", "layui-icon-zbitcoin", "layui-icon-zblackboard-l", "layui-icon-zbluetooth", "layui-icon-zbluetooth-l", "layui-icon-zboard", "layui-icon-zboard-l", "layui-icon-zbook", "layui-icon-zbook-l", "layui-icon-zbookmark", "layui-icon-zbookmark-l", "layui-icon-zbriefcase", "layui-icon-zbriefcase-l", "layui-icon-zbrush", "layui-icon-zbrush-l", "layui-icon-zbug", "layui-icon-zbug-l", "layui-icon-zbuilding", "layui-icon-zbuilding-l", "layui-icon-zbuy", "layui-icon-zbuy-l", "layui-icon-zcalculator", "layui-icon-zcalculator-l", "layui-icon-zcalendar", "layui-icon-zcalendar-l", "layui-icon-zcamber", "layui-icon-zcamber-l", "layui-icon-zcamber-o", "layui-icon-zcamera", "layui-icon-zcamera-l", "layui-icon-zcaomei", "layui-icon-zcategory-l", "layui-icon-zcertificate", "layui-icon-zchemistry", "layui-icon-zchemistry-l", "layui-icon-zchoose-list-l", "layui-icon-zchrome", "layui-icon-zchuangzaoshi", "layui-icon-zcircle", "layui-icon-zcircle-l", "layui-icon-zcircle-o", "layui-icon-zclip-l", "layui-icon-zclock", "layui-icon-zclock-l", "layui-icon-zclose-l", "layui-icon-zclothes", "layui-icon-zclothes-l", "layui-icon-zcloud", "layui-icon-zcloud-l", "layui-icon-zcode-branch", "layui-icon-zcode-edit-l", "layui-icon-zcode-file", "layui-icon-zcode-file-l", "layui-icon-zcode-fork", "layui-icon-zcode-l", "layui-icon-zcode-plugin-l", "layui-icon-zcoin", "layui-icon-zcoin-l", "layui-icon-zcollection", "layui-icon-zcome-l", "layui-icon-zcommand", "layui-icon-zcommand-2", "layui-icon-zcommand-l", "layui-icon-zcomment", "layui-icon-zcomment-l", "layui-icon-zcomputer", "layui-icon-zcomputer-l", "layui-icon-zconfiguration", "layui-icon-zconnection", "layui-icon-zconstruct-l", "layui-icon-zcontainer-l", "layui-icon-zcontrol", "layui-icon-zcontrol-rank", "layui-icon-zcrown", "layui-icon-zcrown-l", "layui-icon-zcss3", "layui-icon-zcup", "layui-icon-zcup-l", "layui-icon-zdata-test", "layui-icon-zdesign-edit-l", "layui-icon-zdesign-shape-l", "layui-icon-zdetect-l", "layui-icon-zd-glasses", "layui-icon-zdiamond-l", "layui-icon-zdoc-edit", "layui-icon-zdoc-file", "layui-icon-zdoc-file-l", "layui-icon-zdownload-l", "layui-icon-zdribbble", "layui-icon-zdropbox", "layui-icon-zd-space", "layui-icon-zeye", "layui-icon-zeye-l", "layui-icon-zfacebook", "layui-icon-zfile", "layui-icon-zfile-l", "layui-icon-zfilm", "layui-icon-zfilm-l", "layui-icon-zfire-l", "layui-icon-zfirewall", "layui-icon-zfirewall-l", "layui-icon-zfolder-l", "layui-icon-zfont", "layui-icon-zforum", "layui-icon-zgame", "layui-icon-zgame-l", "layui-icon-zgeometry-shape-l", "layui-icon-zgift", "layui-icon-zgift-l", "layui-icon-zgithub", "layui-icon-zgithub-logo", "layui-icon-zgit-l", "layui-icon-zgreatwall", "layui-icon-zhacker", "layui-icon-zhand-bevel", "layui-icon-zhand-button", "layui-icon-zhande-vertical", "layui-icon-zhand-gather", "layui-icon-zhand-grasp", "layui-icon-zhand-horizontal", "layui-icon-zhand-pointer", "layui-icon-zhand-slide", "layui-icon-zhand-stop", "layui-icon-zhand-touch", "layui-icon-zheadset", "layui-icon-zheadset-l", "layui-icon-zheart", "layui-icon-zheart-l", "layui-icon-zhome", "layui-icon-zhome-l", "layui-icon-zhtml5", "layui-icon-zimage", "layui-icon-zimage-l", "layui-icon-zinbox", "layui-icon-zinbox-l", "layui-icon-zinfo-l", "layui-icon-zInstagram", "layui-icon-zkeyboard", "layui-icon-zkeyboard-l", "layui-icon-zlabel-info-l", "layui-icon-zlaptop", "layui-icon-zlaptop-l", "layui-icon-zlayers", "layui-icon-zlayout-grid", "layui-icon-zlayout-grids", "layui-icon-zlayout-list", "layui-icon-zlight", "layui-icon-zlight-flash-l", "layui-icon-zlight-l", "layui-icon-zlightning", "layui-icon-zlightning-l", "layui-icon-zlink-l", "layui-icon-zlinux", "layui-icon-zlist-clipboard", "layui-icon-zlist-clipboard-l", "layui-icon-zlocation", "layui-icon-zlocation-l", "layui-icon-zlock", "layui-icon-zlock-l", "layui-icon-zmap", "layui-icon-zmap-l", "layui-icon-zmedal", "layui-icon-zmedal-l", "layui-icon-zmenu-l", "layui-icon-zmessage", "layui-icon-zmessage-l", "layui-icon-zmicrochip", "layui-icon-zmicrochip-l", "layui-icon-zmicrophone", "layui-icon-zmicrophone-l", "layui-icon-zmicrosoft", "layui-icon-zmobile", "layui-icon-zmobile-l", "layui-icon-zmoments", "layui-icon-zmoney", "layui-icon-zmouse", "layui-icon-zmouse-l", "layui-icon-zmusic", "layui-icon-zmusic-file", "layui-icon-zmusic-file-l", "layui-icon-zmusic-l", "layui-icon-zmusic-note", "layui-icon-zmusic-note-l", "layui-icon-znetwork-l", "layui-icon-znew-l", "layui-icon-znewspaper-l", "layui-icon-zoperation", "layui-icon-zout-l", "layui-icon-zoverlapping", "layui-icon-zpad", "layui-icon-zpaper", "layui-icon-zpaper-plane", "layui-icon-zpause-l", "layui-icon-zpaypal", "layui-icon-zpen", "layui-icon-zpen-write", "layui-icon-zpeople", "layui-icon-zpinterest", "layui-icon-zpixels", "layui-icon-zplatform", "layui-icon-zplay-l", "layui-icon-zpokemon-ball", "layui-icon-zprinter", "layui-icon-zprinter-l", "layui-icon-zproduct-l", "layui-icon-zprogram", "layui-icon-zprogram-framework-l", "layui-icon-zprototype", "layui-icon-zprototype-select-l", "layui-icon-zqq", "layui-icon-zqrcode-l", "layui-icon-zquote-left", "layui-icon-zquote-right", "layui-icon-zqzone", "layui-icon-zraspberry", "layui-icon-zread", "layui-icon-zread-l", "layui-icon-zred-envelope", "layui-icon-zright-clipboard", "layui-icon-zright-clipboard-l", "layui-icon-zrocket", "layui-icon-zrocket-l", "layui-icon-zrollerbrush", "layui-icon-zrollerbrush-l", "layui-icon-zrss", "layui-icon-zruler", "layui-icon-zruler-l", "layui-icon-zsave", "layui-icon-zsave-l", "layui-icon-zscan-l", "layui-icon-zscissors", "layui-icon-zsearch-l", "layui-icon-zserver", "layui-icon-zserver-l", "layui-icon-zservers", "layui-icon-zsetting", "layui-icon-zsetting-l", "layui-icon-zshare", "layui-icon-zshield-l", "layui-icon-zshopping-cart", "layui-icon-zshopping-cart-l", "layui-icon-zsite-folder-l", "layui-icon-zslider-l", "layui-icon-zsquare", "layui-icon-zsquare-l", "layui-icon-zsquare-o", "layui-icon-zstar", "layui-icon-zstar-l", "layui-icon-zsteam", "layui-icon-zstorage-l", "layui-icon-zsword-l", "layui-icon-ztab", "layui-icon-ztab-select", "layui-icon-ztag", "layui-icon-ztag-l", "layui-icon-ztaiji", "layui-icon-ztalk", "layui-icon-ztalk-l", "layui-icon-ztaobao", "layui-icon-ztelegram", "layui-icon-zthumbs-up", "layui-icon-zthumbs-up-l", "layui-icon-ztime", "layui-icon-ztime-l", "layui-icon-ztime-plugin-l", "layui-icon-ztmall", "layui-icon-ztransmission-l", "layui-icon-ztrash", "layui-icon-ztrash-l", "layui-icon-ztriangle", "layui-icon-ztriangle-l", "layui-icon-ztriangle-o", "layui-icon-ztruck", "layui-icon-ztruck-l", "layui-icon-ztwitter", "layui-icon-zupload-l", "layui-icon-zusb", "layui-icon-zusb-l", "layui-icon-zuser", "layui-icon-zuser-framework", "layui-icon-zuser-l", "layui-icon-zv2ex", "layui-icon-zvector-design", "layui-icon-zvideo-camera-l", "layui-icon-zvideo-file", "layui-icon-zvideo-file-l", "layui-icon-zvimeo", "layui-icon-zvolume", "layui-icon-zvolume-l", "layui-icon-zvolume-x-l", "layui-icon-zwatch", "layui-icon-zwatch-l", "layui-icon-zwebcam", "layui-icon-zwebcam-l", "layui-icon-zweb-edit", "layui-icon-zweibo", "layui-icon-zweixin", "layui-icon-zweixinzhifu", "layui-icon-zwifi", "layui-icon-zwordpress", "layui-icon-zworld-l", "layui-icon-zwrench-l", "layui-icon-zwrite-l", "layui-icon-zx-buy-l", "layui-icon-zyoutube", "layui-icon-zzhihu", "@font-face {", "src: url('/font/fontawesome-webfonteot?v=470');", "font-weight: normal;", "layui-icon-zzglass", "layui-icon-zzmusic", "layui-icon-zzsearch", "layui-icon-zzenvelope-o", "layui-icon-zzheart", "layui-icon-zzstar", "layui-icon-zzstar-o", "layui-icon-zzuser", "layui-icon-zzfilm", "layui-icon-zzth-large", "layui-icon-zzth", "layui-icon-zzth-list", "layui-icon-zzcheck", "layui-icon-zzremove:before, layui-icon-zzclose:before, layui-icon-zztimes", "layui-icon-zzsearch-plus", "layui-icon-zzsearch-minus", "layui-icon-zzpower-off", "layui-icon-zzsignal", "layui-icon-zzgear:before, layui-icon-zzcog", "layui-icon-zztrash-o", "layui-icon-zzhome", "layui-icon-zzfile-o", "layui-icon-zzclock-o", "layui-icon-zzroad", "layui-icon-zzdownload", "layui-icon-zzarrow-circle-o-down", "layui-icon-zzarrow-circle-o-up", "layui-icon-zzinbox", "layui-icon-zzplay-circle-o", "layui-icon-zzrotate-right:before, layui-icon-zzrepeat", "layui-icon-zzrefresh", "layui-icon-zzlist-alt", "layui-icon-zzlock", "layui-icon-zzflag", "layui-icon-zzheadphones", "layui-icon-zzvolume-off", "layui-icon-zzvolume-down", "layui-icon-zzvolume-up", "layui-icon-zzqrcode", "layui-icon-zzbarcode", "layui-icon-zztag", "layui-icon-zztags", "layui-icon-zzbook", "layui-icon-zzbookmark", "layui-icon-zzprint", "layui-icon-zzcamera", "layui-icon-zzfont", "layui-icon-zzbold", "layui-icon-zzitalic", "layui-icon-zztext-height", "layui-icon-zztext-width", "layui-icon-zzalign-left", "layui-icon-zzalign-center", "layui-icon-zzalign-right", "layui-icon-zzalign-justify", "layui-icon-zzlist", "layui-icon-zzdedent:before, layui-icon-zzoutdent", "layui-icon-zzindent", "layui-icon-zzvideo-camera", "layui-icon-zzphoto:before, layui-icon-zzimage:before, layui-icon-zzpicture-o", "layui-icon-zzpencil", "layui-icon-zzmap-marker", "layui-icon-zzadjust", "layui-icon-zztint", "layui-icon-zzedit:before, layui-icon-zzpencil-square-o", "layui-icon-zzshare-square-o", "layui-icon-zzcheck-square-o", "layui-icon-zzarrows", "layui-icon-zzstep-backward", "layui-icon-zzfast-backward", "layui-icon-zzbackward", "layui-icon-zzplay", "layui-icon-zzpause", "layui-icon-zzstop", "layui-icon-zzforward", "layui-icon-zzfast-forward", "layui-icon-zzstep-forward", "layui-icon-zzeject", "layui-icon-zzchevron-left", "layui-icon-zzchevron-right", "layui-icon-zzplus-circle", "layui-icon-zzminus-circle", "layui-icon-zztimes-circle", "layui-icon-zzcheck-circle", "layui-icon-zzquestion-circle", "layui-icon-zzinfo-circle", "layui-icon-zzcrosshairs", "layui-icon-zztimes-circle-o", "layui-icon-zzcheck-circle-o", "layui-icon-zzban", "layui-icon-zzarrow-left", "layui-icon-zzarrow-right", "layui-icon-zzarrow-up", "layui-icon-zzarrow-down", "layui-icon-zzmail-forward:before, layui-icon-zzshare", "layui-icon-zzexpand", "layui-icon-zzcompress", "layui-icon-zzplus", "layui-icon-zzminus", "layui-icon-zzasterisk", "layui-icon-zzexclamation-circle", "layui-icon-zzgift", "layui-icon-zzleaf", "layui-icon-zzfire", "layui-icon-zzeye", "layui-icon-zzeye-slash", "layui-icon-zzwarning:before, layui-icon-zzexclamation-triangle", "layui-icon-zzplane", "layui-icon-zzcalendar", "layui-icon-zzrandom", "layui-icon-zzcomment", "layui-icon-zzmagnet", "layui-icon-zzchevron-up", "layui-icon-zzchevron-down", "layui-icon-zzretweet", "layui-icon-zzshopping-cart", "layui-icon-zzfolder", "layui-icon-zzfolder-open", "layui-icon-zzarrows-v", "layui-icon-zzarrows-h", "layui-icon-zzbar-chart-o:before, layui-icon-zzbar-chart", "layui-icon-zztwitter-square", "layui-icon-zzfacebook-square", "layui-icon-zzcamera-retro", "layui-icon-zzkey", "layui-icon-zzgears:before, layui-icon-zzcogs", "layui-icon-zzcomments", "layui-icon-zzthumbs-o-up", "layui-icon-zzthumbs-o-down", "layui-icon-zzstar-half", "layui-icon-zzheart-o", "layui-icon-zzsign-out", "layui-icon-zzlinkedin-square", "layui-icon-zzthumb-tack", "layui-icon-zzexternal-link", "layui-icon-zzsign-in", "layui-icon-zztrophy", "layui-icon-zzgithub-square", "layui-icon-zzupload", "layui-icon-zzlemon-o", "layui-icon-zzphone", "layui-icon-zzsquare-o", "layui-icon-zzbookmark-o", "layui-icon-zzphone-square", "layui-icon-zztwitter", "layui-icon-zzfacebook-f:before, layui-icon-zzfacebook", "layui-icon-zzgithub", "layui-icon-zzunlock", "layui-icon-zzcredit-card", "layui-icon-zzfeed:before, layui-icon-zzrss", "layui-icon-zzhdd-o", "layui-icon-zzbullhorn", "layui-icon-zzbell", "layui-icon-zzcertificate", "layui-icon-zzhand-o-right", "layui-icon-zzhand-o-left", "layui-icon-zzhand-o-up", "layui-icon-zzhand-o-down", "layui-icon-zzarrow-circle-left", "layui-icon-zzarrow-circle-right", "layui-icon-zzarrow-circle-up", "layui-icon-zzarrow-circle-down", "layui-icon-zzglobe", "layui-icon-zzwrench", "layui-icon-zztasks", "layui-icon-zzfilter", "layui-icon-zzbriefcase", "layui-icon-zzarrows-alt", "layui-icon-zzgroup:before, layui-icon-zzusers", "layui-icon-zzchain:before, layui-icon-zzlink", "layui-icon-zzcloud", "layui-icon-zzflask", "layui-icon-zzcut:before, layui-icon-zzscissors", "layui-icon-zzcopy:before, layui-icon-zzfiles-o", "layui-icon-zzpaperclip", "layui-icon-zzsave:before, layui-icon-zzfloppy-o", "layui-icon-zzsquare", "layui-icon-zznavicon:before, layui-icon-zzreorder:before, layui-icon-zzbars", "layui-icon-zzlist-ul", "layui-icon-zzlist-ol", "layui-icon-zzstrikethrough", "layui-icon-zzunderline", "layui-icon-zztable", "layui-icon-zzmagic", "layui-icon-zztruck", "layui-icon-zzpinterest", "layui-icon-zzpinterest-square", "layui-icon-zzgoogle-plus-square", "layui-icon-zzgoogle-plus", "layui-icon-zzmoney", "layui-icon-zzcaret-down", "layui-icon-zzcaret-up", "layui-icon-zzcaret-left", "layui-icon-zzcaret-right", "layui-icon-zzcolumns", "layui-icon-zzunsorted:before, layui-icon-zzsort", "layui-icon-zzsort-down:before, layui-icon-zzsort-desc", "layui-icon-zzsort-up:before, layui-icon-zzsort-asc", "layui-icon-zzenvelope", "layui-icon-zzlinkedin", "layui-icon-zzrotate-left:before, layui-icon-zzundo", "layui-icon-zzlegal:before, layui-icon-zzgavel", "layui-icon-zzdashboard:before, layui-icon-zztachometer", "layui-icon-zzcomment-o", "layui-icon-zzcomments-o", "layui-icon-zzflash:before, layui-icon-zzbolt", "layui-icon-zzsitemap", "layui-icon-zzumbrella", "layui-icon-zzpaste:before, layui-icon-zzclipboard", "layui-icon-zzlightbulb-o", "layui-icon-zzexchange", "layui-icon-zzcloud-download", "layui-icon-zzcloud-upload", "layui-icon-zzuser-md", "layui-icon-zzstethoscope", "layui-icon-zzsuitcase", "layui-icon-zzbell-o", "layui-icon-zzcoffee", "layui-icon-zzcutlery", "layui-icon-zzfile-text-o", "layui-icon-zzbuilding-o", "layui-icon-zzhospital-o", "layui-icon-zzambulance", "layui-icon-zzmedkit", "layui-icon-zzfighter-jet", "layui-icon-zzbeer", "layui-icon-zzh-square", "layui-icon-zzplus-square", "layui-icon-zzangle-double-left", "layui-icon-zzangle-double-right", "layui-icon-zzangle-double-up", "layui-icon-zzangle-double-down", "layui-icon-zzangle-left", "layui-icon-zzangle-right", "layui-icon-zzangle-up", "layui-icon-zzangle-down", "layui-icon-zzdesktop", "layui-icon-zzlaptop", "layui-icon-zztablet", "layui-icon-zzmobile-phone:before, layui-icon-zzmobile", "layui-icon-zzcircle-o", "layui-icon-zzquote-left", "layui-icon-zzquote-right", "layui-icon-zzspinner", "layui-icon-zzcircle", "layui-icon-zzmail-reply:before, layui-icon-zzreply", "layui-icon-zzgithub-alt", "layui-icon-zzfolder-o", "layui-icon-zzfolder-open-o", "layui-icon-zzsmile-o", "layui-icon-zzfrown-o", "layui-icon-zzmeh-o", "layui-icon-zzgamepad", "layui-icon-zzkeyboard-o", "layui-icon-zzflag-o", "layui-icon-zzflag-checkered", "layui-icon-zzterminal", "layui-icon-zzcode", "layui-icon-zzmail-reply-all:before, layui-icon-zzreply-all", "layui-icon-zzstar-half-empty:before, layui-icon-zzstar-half-full:before, layui-icon-zzstar-half-o", "layui-icon-zzlocation-arrow", "layui-icon-zzcrop", "layui-icon-zzcode-fork", "layui-icon-zzunlink:before, layui-icon-zzchain-broken", "layui-icon-zzquestion", "layui-icon-zzinfo", "layui-icon-zzexclamation", "layui-icon-zzsuperscript", "layui-icon-zzsubscript", "layui-icon-zzeraser", "layui-icon-zzpuzzle-piece", "layui-icon-zzmicrophone", "layui-icon-zzmicrophone-slash", "layui-icon-zzshield", "layui-icon-zzcalendar-o", "layui-icon-zzfire-extinguisher", "layui-icon-zzrocket", "layui-icon-zzmaxcdn", "layui-icon-zzchevron-circle-left", "layui-icon-zzchevron-circle-right", "layui-icon-zzchevron-circle-up", "layui-icon-zzchevron-circle-down", "layui-icon-zzhtml5", "layui-icon-zzcss3", "layui-icon-zzanchor", "layui-icon-zzunlock-alt", "layui-icon-zzbullseye", "layui-icon-zzellipsis-h", "layui-icon-zzellipsis-v", "layui-icon-zzrss-square", "layui-icon-zzplay-circle", "layui-icon-zzticket", "layui-icon-zzminus-square", "layui-icon-zzminus-square-o", "layui-icon-zzlevel-up", "layui-icon-zzlevel-down", "layui-icon-zzcheck-square", "layui-icon-zzpencil-square", "layui-icon-zzexternal-link-square", "layui-icon-zzshare-square", "layui-icon-zzcompass", "layui-icon-zztoggle-down:before, layui-icon-zzcaret-square-o-down", "layui-icon-zztoggle-up:before, layui-icon-zzcaret-square-o-up", "layui-icon-zztoggle-right:before, layui-icon-zzcaret-square-o-right", "layui-icon-zzeuro:before, layui-icon-zzeur", "layui-icon-zzgbp", "layui-icon-zzdollar:before, layui-icon-zzusd", "layui-icon-zzrupee:before, layui-icon-zzinr", "layui-icon-zzcny:before, layui-icon-zzrmb:before, layui-icon-zzyen:before, layui-icon-zzjpy", "layui-icon-zzruble:before, layui-icon-zzrouble:before, layui-icon-zzrub", "layui-icon-zzwon:before, layui-icon-zzkrw", "layui-icon-zzbitcoin:before, layui-icon-zzbtc", "layui-icon-zzfile", "layui-icon-zzfile-text", "layui-icon-zzsort-alpha-asc", "layui-icon-zzsort-alpha-desc", "layui-icon-zzsort-amount-asc", "layui-icon-zzsort-amount-desc", "layui-icon-zzsort-numeric-asc", "layui-icon-zzsort-numeric-desc", "layui-icon-zzthumbs-up", "layui-icon-zzthumbs-down", "layui-icon-zzyoutube-square", "layui-icon-zzyoutube", "layui-icon-zzxing", "layui-icon-zzxing-square", "layui-icon-zzyoutube-play", "layui-icon-zzdropbox", "layui-icon-zzstack-overflow", "layui-icon-zzinstagram", "layui-icon-zzflickr", "layui-icon-zzadn", "layui-icon-zzbitbucket", "layui-icon-zzbitbucket-square", "layui-icon-zztumblr", "layui-icon-zztumblr-square", "layui-icon-zzlong-arrow-down", "layui-icon-zzlong-arrow-up", "layui-icon-zzlong-arrow-left", "layui-icon-zzlong-arrow-right", "layui-icon-zzapple", "layui-icon-zzwindows", "layui-icon-zzandroid", "layui-icon-zzlinux", "layui-icon-zzdribbble", "layui-icon-zzskype", "layui-icon-zzfoursquare", "layui-icon-zztrello", "layui-icon-zzfemale", "layui-icon-zzmale", "layui-icon-zzgittip:before, layui-icon-zzgratipay", "layui-icon-zzsun-o", "layui-icon-zzmoon-o", "layui-icon-zzarchive", "layui-icon-zzbug", "layui-icon-zzvk", "layui-icon-zzweibo", "layui-icon-zzrenren", "layui-icon-zzpagelines", "layui-icon-zzstack-exchange", "layui-icon-zzarrow-circle-o-right", "layui-icon-zzarrow-circle-o-left", "layui-icon-zztoggle-left:before, layui-icon-zzcaret-square-o-left", "layui-icon-zzdot-circle-o", "layui-icon-zzwheelchair", "layui-icon-zzvimeo-square", "layui-icon-zzturkish-lira:before, layui-icon-zztry", "layui-icon-zzplus-square-o", "layui-icon-zzspace-shuttle", "layui-icon-zzslack", "layui-icon-zzenvelope-square", "layui-icon-zzwordpress", "layui-icon-zzopenid", "layui-icon-zzinstitution:before, layui-icon-zzbank:before, layui-icon-zzuniversity", "layui-icon-zzmortar-board:before, layui-icon-zzgraduation-cap", "layui-icon-zzyahoo", "layui-icon-zzgoogle", "layui-icon-zzreddit", "layui-icon-zzreddit-square", "layui-icon-zzstumbleupon-circle", "layui-icon-zzstumbleupon", "layui-icon-zzdelicious", "layui-icon-zzdigg", "layui-icon-zzpied-piper-pp", "layui-icon-zzpied-piper-alt", "layui-icon-zzdrupal", "layui-icon-zzjoomla", "layui-icon-zzlanguage", "layui-icon-zzfax", "layui-icon-zzbuilding", "layui-icon-zzchild", "layui-icon-zzpaw", "layui-icon-zzspoon", "layui-icon-zzcube", "layui-icon-zzcubes", "layui-icon-zzbehance", "layui-icon-zzbehance-square", "layui-icon-zzsteam", "layui-icon-zzsteam-square", "layui-icon-zzrecycle", "layui-icon-zzautomobile:before, layui-icon-zzcar", "layui-icon-zzcab:before, layui-icon-zztaxi", "layui-icon-zztree", "layui-icon-zzspotify", "layui-icon-zzdeviantart", "layui-icon-zzsoundcloud", "layui-icon-zzdatabase", "layui-icon-zzfile-pdf-o", "layui-icon-zzfile-word-o", "layui-icon-zzfile-excel-o", "layui-icon-zzfile-powerpoint-o", "layui-icon-zzfile-photo-o:before, layui-icon-zzfile-picture-o:before, layui-icon-zzfile-image-o", "layui-icon-zzfile-zip-o:before, layui-icon-zzfile-archive-o", "layui-icon-zzfile-sound-o:before, layui-icon-zzfile-audio-o", "layui-icon-zzfile-movie-o:before, layui-icon-zzfile-video-o", "layui-icon-zzfile-code-o", "layui-icon-zzvine", "layui-icon-zzcodepen", "layui-icon-zzjsfiddle", "layui-icon-zzlife-bouy:before, layui-icon-zzlife-buoy:before, layui-icon-zzlife-saver:before, layui-icon-zzsupport:before, layui-icon-zzlife-ring", "layui-icon-zzcircle-o-notch", "layui-icon-zzra:before, layui-icon-zzresistance:before, layui-icon-zzrebel", "layui-icon-zzge:before, layui-icon-zzempire", "layui-icon-zzgit-square", "layui-icon-zzgit", "layui-icon-zzy-combinator-square:before, layui-icon-zzyc-square:before, layui-icon-zzhacker-news", "layui-icon-zztencent-weibo", "layui-icon-zzqq", "layui-icon-zzwechat:before, layui-icon-zzweixin", "layui-icon-zzsend:before, layui-icon-zzpaper-plane", "layui-icon-zzsend-o:before, layui-icon-zzpaper-plane-o", "layui-icon-zzhistory", "layui-icon-zzcircle-thin", "layui-icon-zzheader", "layui-icon-zzparagraph", "layui-icon-zzsliders", "layui-icon-zzshare-alt", "layui-icon-zzshare-alt-square", "layui-icon-zzbomb", "layui-icon-zzsoccer-ball-o:before, layui-icon-zzfutbol-o", "layui-icon-zztty", "layui-icon-zzbinoculars", "layui-icon-zzplug", "layui-icon-zzslideshare", "layui-icon-zztwitch", "layui-icon-zzyelp", "layui-icon-zznewspaper-o", "layui-icon-zzwifi", "layui-icon-zzcalculator", "layui-icon-zzpaypal", "layui-icon-zzgoogle-wallet", "layui-icon-zzcc-visa", "layui-icon-zzcc-mastercard", "layui-icon-zzcc-discover", "layui-icon-zzcc-amex", "layui-icon-zzcc-paypal", "layui-icon-zzcc-stripe", "layui-icon-zzbell-slash", "layui-icon-zzbell-slash-o", "layui-icon-zztrash", "layui-icon-zzcopyright", "layui-icon-zzat", "layui-icon-zzeyedropper", "layui-icon-zzpaint-brush", "layui-icon-zzbirthday-cake", "layui-icon-zzarea-chart", "layui-icon-zzpie-chart", "layui-icon-zzline-chart", "layui-icon-zzlastfm", "layui-icon-zzlastfm-square", "layui-icon-zztoggle-off", "layui-icon-zztoggle-on", "layui-icon-zzbicycle", "layui-icon-zzbus", "layui-icon-zzioxhost", "layui-icon-zzangellist", "layui-icon-zzcc", "layui-icon-zzshekel:before, layui-icon-zzsheqel:before, layui-icon-zzils", "layui-icon-zzmeanpath", "layui-icon-zzbuysellads", "layui-icon-zzconnectdevelop", "layui-icon-zzdashcube", "layui-icon-zzforumbee", "layui-icon-zzleanpub", "layui-icon-zzsellsy", "layui-icon-zzshirtsinbulk", "layui-icon-zzsimplybuilt", "layui-icon-zzskyatlas", "layui-icon-zzcart-plus", "layui-icon-zzcart-arrow-down", "layui-icon-zzdiamond", "layui-icon-zzship", "layui-icon-zzuser-secret", "layui-icon-zzmotorcycle", "layui-icon-zzstreet-view", "layui-icon-zzheartbeat", "layui-icon-zzvenus", "layui-icon-zzmars", "layui-icon-zzmercury", "layui-icon-zzintersex:before, layui-icon-zztransgender", "layui-icon-zztransgender-alt", "layui-icon-zzvenus-double", "layui-icon-zzmars-double", "layui-icon-zzvenus-mars", "layui-icon-zzmars-stroke", "layui-icon-zzmars-stroke-v", "layui-icon-zzmars-stroke-h", "layui-icon-zzneuter", "layui-icon-zzgenderless", "layui-icon-zzfacebook-official", "layui-icon-zzpinterest-p", "layui-icon-zzwhatsapp", "layui-icon-zzserver", "layui-icon-zzuser-plus", "layui-icon-zzuser-times", "layui-icon-zzhotel:before, layui-icon-zzbed", "layui-icon-zzviacoin", "layui-icon-zztrain", "layui-icon-zzsubway", "layui-icon-zzmedium", "layui-icon-zzyc:before, layui-icon-zzy-combinator", "layui-icon-zzoptin-monster", "layui-icon-zzopencart", "layui-icon-zzexpeditedssl", "layui-icon-zzbattery-4:before, layui-icon-zzbattery:before, layui-icon-zzbattery-full", "layui-icon-zzbattery-3:before, layui-icon-zzbattery-three-quarters", "layui-icon-zzbattery-2:before, layui-icon-zzbattery-half", "layui-icon-zzbattery-1:before, layui-icon-zzbattery-quarter", "layui-icon-zzbattery-0:before, layui-icon-zzbattery-empty", "layui-icon-zzmouse-pointer", "layui-icon-zzi-cursor", "layui-icon-zzobject-group", "layui-icon-zzobject-ungroup", "layui-icon-zzsticky-note", "layui-icon-zzsticky-note-o", "layui-icon-zzcc-jcb", "layui-icon-zzcc-diners-club", "layui-icon-zzclone", "layui-icon-zzbalance-scale", "layui-icon-zzhourglass-o", "layui-icon-zzhourglass-1:before, layui-icon-zzhourglass-start", "layui-icon-zzhourglass-2:before, layui-icon-zzhourglass-half", "layui-icon-zzhourglass-3:before, layui-icon-zzhourglass-end", "layui-icon-zzhourglass", "layui-icon-zzhand-grab-o:before, layui-icon-zzhand-rock-o", "layui-icon-zzhand-stop-o:before, layui-icon-zzhand-paper-o", "layui-icon-zzhand-scissors-o", "layui-icon-zzhand-lizard-o", "layui-icon-zzhand-spock-o", "layui-icon-zzhand-pointer-o", "layui-icon-zzhand-peace-o", "layui-icon-zztrademark", "layui-icon-zzregistered", "layui-icon-zzcreative-commons", "layui-icon-zzgg", "layui-icon-zzgg-circle", "layui-icon-zztripadvisor", "layui-icon-zzodnoklassniki", "layui-icon-zzodnoklassniki-square", "layui-icon-zzget-pocket", "layui-icon-zzwikipedia-w", "layui-icon-zzsafari", "layui-icon-zzchrome", "layui-icon-zzfirefox", "layui-icon-zzopera", "layui-icon-zzinternet-explorer", "layui-icon-zztv:before, layui-icon-zztelevision", "layui-icon-zzcontao", "layui-icon-zz500px", "layui-icon-zzamazon", "layui-icon-zzcalendar-plus-o", "layui-icon-zzcalendar-minus-o", "layui-icon-zzcalendar-times-o", "layui-icon-zzcalendar-check-o", "layui-icon-zzindustry", "layui-icon-zzmap-pin", "layui-icon-zzmap-signs", "layui-icon-zzmap-o", "layui-icon-zzmap", "layui-icon-zzcommenting", "layui-icon-zzcommenting-o", "layui-icon-zzhouzz", "layui-icon-zzvimeo", "layui-icon-zzblack-tie", "layui-icon-zzfonticons", "layui-icon-zzreddit-alien", "layui-icon-zzedge", "layui-icon-zzcredit-card-alt", "layui-icon-zzcodiepie", "layui-icon-zzmodx", "layui-icon-zzfort-awesome", "layui-icon-zzusb", "layui-icon-zzproduct-hunt", "layui-icon-zzmixcloud", "layui-icon-zzscribd", "layui-icon-zzpause-circle", "layui-icon-zzpause-circle-o", "layui-icon-zzstop-circle", "layui-icon-zzstop-circle-o", "layui-icon-zzshopping-bag", "layui-icon-zzshopping-basket", "layui-icon-zzhashtag", "layui-icon-zzbluetooth", "layui-icon-zzbluetooth-b", "layui-icon-zzpercent", "layui-icon-zzgitlab", "layui-icon-zzwpbeginner", "layui-icon-zzwpforms", "layui-icon-zzenvira", "layui-icon-zzuniversal-access", "layui-icon-zzwheelchair-alt", "layui-icon-zzquestion-circle-o", "layui-icon-zzblind", "layui-icon-zzaudio-description", "layui-icon-zzvolume-control-phone", "layui-icon-zzbraille", "layui-icon-zzassistive-listening-systems", "layui-icon-zzasl-interpreting:before, layui-icon-zzamerican-sign-language-interpreting", "layui-icon-zzdeafness:before, layui-icon-zzhard-of-hearing:before, layui-icon-zzdeaf", "layui-icon-zzglide", "layui-icon-zzglide-g", "layui-icon-zzsigning:before, layui-icon-zzsign-language", "layui-icon-zzlow-vision", "layui-icon-zzviadeo", "layui-icon-zzviadeo-square", "layui-icon-zzsnapchat", "layui-icon-zzsnapchat-ghost", "layui-icon-zzsnapchat-square", "layui-icon-zzpied-piper", "layui-icon-zzfirst-order", "layui-icon-zzyoast", "layui-icon-zzthemeisle", "layui-icon-zzgoogle-plus-circle:before, layui-icon-zzgoogle-plus-official", "layui-icon-zzfa:before, layui-icon-zzfont-awesome", "layui-icon-zzhandshake-o", "layui-icon-zzenvelope-open", "layui-icon-zzenvelope-open-o", "layui-icon-zzlinode", "layui-icon-zzaddress-book", "layui-icon-zzaddress-book-o", "layui-icon-zzvcard:before, layui-icon-zzaddress-card", "layui-icon-zzvcard-o:before, layui-icon-zzaddress-card-o", "layui-icon-zzuser-circle", "layui-icon-zzuser-circle-o", "layui-icon-zzuser-o", "layui-icon-zzid-badge", "layui-icon-zzdrivers-license:before, layui-icon-zzid-card", "layui-icon-zzdrivers-license-o:before, layui-icon-zzid-card-o", "layui-icon-zzquora", "layui-icon-zzfree-code-camp", "layui-icon-zztelegram", "layui-icon-zzthermometer-4:before, layui-icon-zzthermometer:before, layui-icon-zzthermometer-full", "layui-icon-zzthermometer-3:before, layui-icon-zzthermometer-three-quarters", "layui-icon-zzthermometer-2:before, layui-icon-zzthermometer-half", "layui-icon-zzthermometer-1:before, layui-icon-zzthermometer-quarter", "layui-icon-zzthermometer-0:before, layui-icon-zzthermometer-empty", "layui-icon-zzshower", "layui-icon-zzbathtub:before, layui-icon-zzs15:before, layui-icon-zzbath", "layui-icon-zzpodcast", "layui-icon-zzwindow-maximize", "layui-icon-zzwindow-minimize", "layui-icon-zzwindow-restore", "layui-icon-zztimes-rectangle:before, layui-icon-zzwindow-close", "layui-icon-zztimes-rectangle-o:before, layui-icon-zzwindow-close-o", "layui-icon-zzbandcamp", "layui-icon-zzgrav", "layui-icon-zzetsy", "layui-icon-zzimdb", "layui-icon-zzravelry", "layui-icon-zzeercast", "layui-icon-zzmicrochip", "layui-icon-zzsnowflake-o", "layui-icon-zzsuperpowers", "layui-icon-zzwpexplorer", "layui-icon-zzmeetup", "@font-face {", "src: url(/font/iconfonteot?v=226_rc2);", "layui-icon {", "font-size: 16px;", "-webkit-font-smoothing: antialiased;", "layui-icon-duihua", "layui-icon-shezhi", "layui-icon-yinshenim", "layui-icon-search", "layui-icon-fenxiang1", "layui-icon-shezhi1", "layui-icon-yinqing", "layui-icon-close", "layui-icon-close-fill", "layui-icon-baobiao", "layui-icon-star", "layui-icon-yuandian", "layui-icon-chat", "layui-icon-logo", "layui-icon-list", "layui-icon-tubiao", "layui-icon-ok-circle", "layui-icon-huanfu2", "layui-icon-On-line", "layui-icon-biaoge", "layui-icon-right", "layui-icon-left", "layui-icon-cart-simple", "layui-icon-cry", "layui-icon-smile", "layui-icon-survey", "layui-icon-tree", "layui-icon-iconfont17", "layui-icon-tianjia", "layui-icon-download-circle", "layui-icon-xuanzemoban48", "layui-icon-gongju", "layui-icon-face-surprised", "layui-icon-bianji", "layui-icon-speaker", "layui-icon-down", "layui-icon-wenjian", "layui-icon-layouts", "layui-icon-duigou", "layui-icon-tianjia1", "layui-icon-yaoyaozhibofanye", "layui-icon-read", "layui-icon-404", "layui-icon-lunbozutu", "layui-icon-help", "layui-icon-daima1", "layui-icon-jinshui", "layui-icon-username", "layui-icon-find-fill", "layui-icon-about", "layui-icon-location", "layui-icon-up", "layui-icon-pause", "layui-icon-riqi", "layui-icon-uploadfile", "layui-icon-delete", "layui-icon-play", "layui-icon-top", "layui-icon-haoyouqingqiu", "layui-icon-refresh-3", "layui-icon-weibiaoti1", "layui-icon-chuangkou", "layui-icon-comiisbiaoqing", "layui-icon-zhengque", "layui-icon-dollar", "layui-icon-iconfontwodehaoyou", "layui-icon-wenjianxiazai", "layui-icon-tupian", "layui-icon-lianjie", "layui-icon-diamond", "layui-icon-jilu", "layui-icon-liucheng", "layui-icon-fontstrikethrough", "layui-icon-unlink", "layui-icon-bianjiwenzi", "layui-icon-sanjiao", "layui-icon-danxuankuanghouxuan", "layui-icon-danxuankuangxuanzhong", "layui-icon-juzhongduiqi", "layui-icon-youduiqi", "layui-icon-zuoduiqi", "layui-icon-gongsisvgtubiaozongji22", "layui-icon-gongsisvgtubiaozongji23", "layui-icon-refresh-2", "layui-icon-loading-1", "layui-icon-return", "layui-icon-jiacu", "layui-icon-uploading", "layui-icon-liaotianduihuaimgoutong", "layui-icon-video", "layui-icon-headset", "layui-icon-wenjianjiafan", "layui-icon-shouji", "layui-icon-tianjia2", "layui-icon-wenjianjia", "layui-icon-biaoqing", "layui-icon-html", "layui-icon-biaodan", "layui-icon-cart", "layui-icon-camera-fill", "layui-icon-25", "layui-icon-emwdaima", "layui-icon-fire", "layui-icon-set", "layui-icon-zitixiahuaxian", "layui-icon-sanjiao1", "layui-icon-tips", "layui-icon-tupian-copy-copy", "layui-icon-more-vertical", "layui-icon-zhuti2", "layui-icon-loading", "layui-icon-xieti", "layui-icon-refresh-1", "layui-icon-rmb", "layui-icon-home", "layui-icon-user", "layui-icon-notice", "layui-icon-login-weibo", "layui-icon-voice", "layui-icon-download", "layui-icon-login-qq", "layui-icon-snowflake", "layui-icon-yemian1", "layui-icon-template", "layui-icon-auz", "layui-icon-console", "layui-icon-app", "layui-icon-prev", "layui-icon-website", "layui-icon-next", "layui-icon-component", "layui-icon-more", "layui-icon-login-wechat", "layui-icon-shrink-right", "layui-icon-spread-left", "layui-icon-camera", "layui-icon-note", "layui-icon-refresh", "layui-icon-nv", "layui-icon-nan", "layui-icon-password", "layui-icon-senior", "layui-icon-theme", "layui-icon-tread", "layui-icon-praise", "layui-icon-star-fill", "layui-icon-template-1", "layui-icon-loading-2"];
                    return arr;
                }
                //,
                //unicode: function () {
                //    return ["&amp;#xe6c9;", "&amp;#xe67b;", "&amp;#xe67a;", "&amp;#xe678;", "&amp;#xe679;", "&amp;#xe677;", "&amp;#xe676;", "&amp;#xe675;", "&amp;#xe673;", "&amp;#xe66f;", "&amp;#xe9aa;", "&amp;#xe672;", "&amp;#xe66b;", "&amp;#xe668;", "&amp;#xe6b1;", "&amp;#xe702;", "&amp;#xe66e;", "&amp;#xe68e;", "&amp;#xe674;", "&amp;#xe669;", "&amp;#xe666;", "&amp;#xe66c;", "&amp;#xe66a;", "&amp;#xe667;", "&amp;#xe7ae;", "&amp;#xe665;", "&amp;#xe664;", "&amp;#xe716;", "&amp;#xe656;", "&amp;#xe653;", "&amp;#xe663;", "&amp;#xe6c6;", "&amp;#xe6c5;", "&amp;#xe662;", "&amp;#xe661;", "&amp;#xe660;", "&amp;#xe65d;", "&amp;#xe65f;", "&amp;#xe671;", "&amp;#xe65e;", "&amp;#xe659;", "&amp;#xe735;", "&amp;#xe756;", "&amp;#xe65c;", "&amp;#xe715;", "&amp;#xe705;", "&amp;#xe6b2;", "&amp;#xe6af;", "&amp;#xe69c;", "&amp;#xe698;", "&amp;#xe657;", "&amp;#xe65b;", "&amp;#xe65a;", "&amp;#xe681;", "&amp;#xe67c;", "&amp;#xe601;", "&amp;#xe857;", "&amp;#xe655;", "&amp;#xe770;", "&amp;#xe670;", "&amp;#xe63d;", "&amp;#xe63e;", "&amp;#xe654;", "&amp;#xe652;", "&amp;#xe651;", "&amp;#xe6fc;", "&amp;#xe6ed;", "&amp;#xe688;", "&amp;#xe645;", "&amp;#xe64f;", "&amp;#xe64e;", "&amp;#xe64b;", "&amp;#xe62b;", "&amp;#xe64d;", "&amp;#xe64a;", "&amp;#xe64c;", "&amp;#xe650;", "&amp;#xe649;", "&amp;#xe648;", "&amp;#xe647;", "&amp;#xe646;", "&amp;#xe644;", "&amp;#xe62a;", "&amp;#xe643;", "&amp;#xe63f;", "&amp;#xe642;", "&amp;#xe641;", "&amp;#xe640;", "&amp;#xe63c;", "&amp;#xe63b;", "&amp;#xe63a;", "&amp;#xe639;", "&amp;#xe638;", "&amp;#xe637;", "&amp;#xe636;", "&amp;#xe635;", "&amp;#xe634;", "&amp;#xe633;", "&amp;#xe632;", "&amp;#xe631;", "&amp;#xe630;", "&amp;#xe62f;", "&amp;#xe62e;", "&amp;#xe62d;", "&amp;#xe62c;", "&amp;#xe629;", "&amp;#xe628;", "&amp;#xe625;", "&amp;#xe623;", "&amp;#xe621;", "&amp;#xe620;", "&amp;#xe61f;", "&amp;#xe61c;", "&amp;#xe60b;", "&amp;#xe619;", "&amp;#xe61a;", "&amp;#xe603;", "&amp;#xe602;", "&amp;#xe617;", "&amp;#xe615;", "&amp;#xe614;", "&amp;#xe613;", "&amp;#xe612;", "&amp;#xe611;", "&amp;#xe60f;", "&amp;#xe60e;", "&amp;#xe60d;", "&amp;#xe60c;", "&amp;#xe60a;", "&amp;#xe609;", "&amp;#xe605;", "&amp;#xe607;", "&amp;#xe606;", "&amp;#xe604;", "&amp;#xe600;", "&amp;#xe658;", "&amp;#x1007;", "&amp;#x1006;", "&amp;#x1005;", "&amp;#xe608;"];
                //}
            }
        };

        a.init();
        return new IconPicker();
    };

    /**
     * 选中图标
     * @param filter lay-filter
     * @param iconName 图标名称，自动识别fontClass/unicode
     */
    IconPicker.prototype.checkIcon = function (filter, iconName) {
        var p = $('*[lay-filter=' + filter + ']').next().find('.layui-iconpicker-item .layui-icon'),
            c = iconName;

        if (c.indexOf('#xe') > 0) {
            p.html(c);
        } else {
            p.html('').attr('class', 'layui-icon ' + c);
        }
    };

    var iconPicker = new IconPicker();
    exports(_MOD, iconPicker);
});