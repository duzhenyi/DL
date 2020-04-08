layui.config({
    base: '/js/modules/'
}).use(['table', 'layer', 'jquery', 'api', 'form'],
    function () {
        var table = layui.table,
            layer = layui.layer,
            $ = layui.jquery,
            os = layui.api,
            form = layui.form;
        form.render();
        table.render({
            elem: '#tablist',
            headers: os.getToken(),
            url: '/Cms/Article/GetPages',
            cols: [
                [
                    { type: 'checkbox', fixed: 'left' },
                    {
                        field: 'Title', title: '标题', fixed: 'left'
                    },
                    { field: 'Hits', width: 100, title: '总点击量' },
                    { field: 'MonthHits', width: 100, title: '月点击量' },
                    { field: 'WeedHits', width: 100, title: '周点击量' },
                    { field: 'WeedHits', width: 100, title: '日点击量' },
                    { field: 'DayHits', width: 100, title: '点击量' },
                    { field: 'LastHitDate', width: 200, title: '最后点击时间', sort: true }
                ]
            ],
            page: true,
            id: 'tables',
            where: {
                ID: '1',
                types: -1
            }
        });
        var type = 0, active = {
            reload: function () {
                table.reload('tables',
                    {
                        page: {
                            curr: 1
                        },
                        where: {
                            ID: '1',
                            where: $("#attr").val(),
                            key: $('#key').val(),
                            types: type
                        },
                        done: function () {
                        }
                    });
            },
            toolSearch: function () {
                active.reload();
            }
        };
        $('.list-search .layui-btn').on('click', function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
    });