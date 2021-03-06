﻿! function (a, b, c) {
    function k(a, b, c) {
        for (var d = [], e = 0; e < a.length; e++) {
            var f = a[e];
            if (f) {
                var h = tinycolor(f),
                    i = h.toHsl().l < .5 ? "sp-thumb-el sp-thumb-dark" : "sp-thumb-el sp-thumb-light";
                i += tinycolor.equals(b, f) ? " sp-thumb-active" : "";
                var j = g ? "background-color:" + h.toRgbString() : "filter:" + h.toFilter();
                d.push('<span title="' + h.toRgbString() + '" data-color="' + h.toRgbString() + '" class="' + i + '"><span class="sp-thumb-inner" style="' + j + ';" /></span>')
            } else {
                var k = "sp-clear-display";
                d.push('<span title="No Color Selected" data-color="" style="background-color:transparent;" class="' + k + '"></span>')
            }
        }
        return "<div class='sp-cf " + c + "'>" + d.join("") + "</div>"
    }

    function l() {
        for (var a = 0; a < e.length; a++) e[a] && e[a].hide()
    }

    function m(a, c) {
        var e = b.extend({}, d, a);
        return e.callbacks = {
            move: r(e.move, c),
            change: r(e.change, c),
            show: r(e.show, c),
            hide: r(e.hide, c),
            beforeShow: r(e.beforeShow, c)
        }, e
    }

    function n(d, n) {
        function vb() {
            p.showPaletteOnly && (p.showPalette = !0), p.palette && (M = p.palette.slice(0), N = b.isArray(M[0]) ? M : [M]), W.toggleClass("sp-flat", r), W.toggleClass("sp-input-disabled", !p.showInput), W.toggleClass("sp-alpha-enabled", p.showAlpha), W.toggleClass("sp-clear-enabled", ub), W.toggleClass("sp-buttons-disabled", !p.showButtons), W.toggleClass("sp-palette-disabled", !p.showPalette), W.toggleClass("sp-palette-only", p.showPaletteOnly), W.toggleClass("sp-initial-disabled", !p.showInitial), W.addClass(p.className), Pb()
        }

        function wb() {
            function g(a) {
                return a.data && a.data.ignore ? (Ib(b(this).data("color")), Lb()) : (Ib(b(this).data("color")), Ob(!0), Lb(), Gb()), !1
            }
            if (f && W.find("*:not(input)").attr("unselectable", "on"), vb(), kb && U.after(lb).hide(), ub || gb.hide(), r) U.after(W).hide();
            else {
                var c = "parent" === p.appendTo ? U.parent() : b(p.appendTo);
                1 !== c.length && (c = b("body")), c.append(W)
            } if (v && a.localStorage) {
                try {
                    var d = a.localStorage[v].split(",#");
                    d.length > 1 && (delete a.localStorage[v], b.each(d, function (a, b) {
                        xb(b)
                    }))
                } catch (e) { }
                try {
                    O = a.localStorage[v].split(";")
                } catch (e) { }
            }
            mb.bind("click.spectrum touchstart.spectrum", function (a) {
                V || Eb(), a.stopPropagation(), b(a.target).is("input") || a.preventDefault()
            }), (U.is(":disabled") || p.disabled === !0) && Tb(), W.click(q), cb.change(Db), cb.bind("paste", function () {
                setTimeout(Db, 1)
            }), cb.keydown(function (a) {
                13 == a.keyCode && Db()
            }), fb.text(p.cancelText), fb.bind("click.spectrum", function (a) {
                a.stopPropagation(), a.preventDefault(), Gb("cancel")
            }), gb.attr("title", p.clearText), gb.bind("click.spectrum", function (a) {
                a.stopPropagation(), a.preventDefault(), tb = !0, Lb(), r && Ob(!0)
            }), hb.text(p.chooseText), hb.bind("click.spectrum", function (a) {
                a.stopPropagation(), a.preventDefault(), Kb() && (Ob(!0), Gb())
            }), s(ab, function (a, b, c) {
                L = a / F, tb = !1, c.shiftKey && (L = Math.round(10 * L) / 10), Lb()
            }), s(Z, function (a, b) {
                I = parseFloat(b / D), tb = !1, p.showAlpha || (L = 1), Lb()
            }, Bb, Cb), s(X, function (a, b, c) {
                if (c.shiftKey) {
                    if (!R) {
                        var d = J * A,
                            e = B - K * B,
                            f = Math.abs(a - d) > Math.abs(b - e);
                        R = f ? "x" : "y"
                    }
                } else R = null;
                var g = !R || "x" === R,
                    h = !R || "y" === R;
                g && (J = parseFloat(a / A)), h && (K = parseFloat((B - b) / B)), tb = !1, p.showAlpha || (L = 1), Lb()
            }, Bb, Cb), ob ? (Ib(ob), Mb(), rb = qb || tinycolor(ob).format, xb(ob)) : Mb(), r && Fb();
            var h = f ? "mousedown.spectrum" : "click.spectrum touchstart.spectrum";
            db.delegate(".sp-thumb-el", h, g), eb.delegate(".sp-thumb-el:nth-child(1)", h, {
                ignore: !0
            }, g)
        }

        function xb(c) {
            if (u) {
                var d = tinycolor(c).toRgbString();
                if (-1 === b.inArray(d, O))
                    for (O.push(d); O.length > P;) O.shift();
                if (v && a.localStorage) try {
                    a.localStorage[v] = O.join(";")
                } catch (e) { }
            }
        }

        function yb() {
            var d, a = [],
                b = O,
                c = {};
            if (p.showPalette) {
                for (var e = 0; e < N.length; e++)
                    for (var f = 0; f < N[e].length; f++) d = tinycolor(N[e][f]).toRgbString(), c[d] = !0;
                for (e = 0; e < b.length; e++) d = tinycolor(b[e]).toRgbString(), c.hasOwnProperty(d) || (a.push(b[e]), c[d] = !0)
            }
            return a.reverse().slice(0, p.maxSelectionSize)
        }

        function zb() {
            var a = Jb(),
                c = b.map(N, function (b, c) {
                    return k(b, a, "sp-palette-row sp-palette-row-" + c)
                });
            O && c.push(k(yb(), a, "sp-palette-row sp-palette-row-selection")), db.html(c.join(""))
        }

        function Ab() {
            if (p.showInitial) {
                var a = pb,
                    b = Jb();
                eb.html(k([a, b], b, "sp-palette-row-initial"))
            }
        }

        function Bb() {
            (0 >= B || 0 >= A || 0 >= D) && Pb(), W.addClass(Q), R = null, U.trigger("dragstart.spectrum", [Jb()])
        }

        function Cb() {
            W.removeClass(Q), U.trigger("dragstop.spectrum", [Jb()])
        }

        function Db() {
            var a = cb.val();
            if (null !== a && "" !== a || !ub) {
                var b = tinycolor(a);
                b.ok ? (Ib(b), Ob(!0)) : cb.addClass("sp-validation-error")
            } else Ib(null), Ob(!0)
        }

        function Eb() {
            z ? Gb() : Fb()
        }

        function Fb() {
            var c = b.Event("beforeShow.spectrum");
            return z ? (Pb(), void 0) : (U.trigger(c, [Jb()]), x.beforeShow(Jb()) === !1 || c.isDefaultPrevented() || (l(), z = !0, b(S).bind("click.spectrum", Gb), b(a).bind("resize.spectrum", y), lb.addClass("sp-active"), W.removeClass("sp-hidden"), Pb(), Mb(), pb = Jb(), Ab(), x.show(pb), U.trigger("show.spectrum", [pb])), void 0)
        }

        function Gb(c) {
            if ((!c || "click" != c.type || 2 != c.button) && z && !r) {
                z = !1, b(S).unbind("click.spectrum", Gb), b(a).unbind("resize.spectrum", y), lb.removeClass("sp-active"), W.addClass("sp-hidden");
                var d = !tinycolor.equals(Jb(), pb);
                d && (sb && "cancel" !== c ? Ob(!0) : Hb()), x.hide(Jb()), U.trigger("hide.spectrum", [Jb()])
            }
        }

        function Hb() {
            Ib(pb, !0)
        }

        function Ib(a, b) {
            if (tinycolor.equals(a, Jb())) return Mb(), void 0;
            var c, d;
            !a && ub ? tb = !0 : (tb = !1, c = tinycolor(a), d = c.toHsv(), I = d.h % 360 / 360, J = d.s, K = d.v, L = d.a), Mb(), c && c.ok && !b && (rb = qb || c.format)
        }

        function Jb(a) {
            return a = a || {}, ub && tb ? null : tinycolor.fromRatio({
                h: I,
                s: J,
                v: K,
                a: Math.round(100 * L) / 100
            }, {
                    format: a.format || rb
                })
        }

        function Kb() {
            return !cb.hasClass("sp-validation-error")
        }

        function Lb() {
            Mb(), x.move(Jb()), U.trigger("move.spectrum", [Jb()])
        }

        function Mb() {
            cb.removeClass("sp-validation-error"), Nb();
            var a = tinycolor.fromRatio({
                h: I,
                s: 1,
                v: 1
            });
            X.css("background-color", a.toHexString());
            var b = rb;
            1 > L && (0 !== L || "name" !== b) && ("hex" === b || "hex3" === b || "hex6" === b || "name" === b) && (b = "rgb");
            var c = Jb({
                format: b
            }),
                d = "";
            if (nb.removeClass("sp-clear-display"), nb.css("background-color", "transparent"), !c && ub) nb.addClass("sp-clear-display");
            else {
                var e = c.toHexString(),
                    h = c.toRgbString();
                if (g || 1 === c.alpha ? nb.css("background-color", h) : (nb.css("background-color", "transparent"), nb.css("filter", c.toFilter())), p.showAlpha) {
                    var i = c.toRgb();
                    i.a = 0;
                    var j = tinycolor(i).toRgbString(),
                        k = "linear-gradient(left, " + j + ", " + e + ")";
                    f ? _.css("filter", tinycolor(j).toFilter({
                        gradientType: 1
                    }, e)) : (_.css("background", "-webkit-" + k), _.css("background", "-moz-" + k), _.css("background", "-ms-" + k), _.css("background", k))
                }
                d = c.toString(b)
            }
            p.showInput && cb.val(d), p.showPalette && zb(), Ab()
        }

        function Nb() {
            var a = J,
                b = K;
            if (ub && tb) bb.hide(), $.hide(), Y.hide();
            else {
                bb.show(), $.show(), Y.show();
                var c = a * A,
                    d = B - b * B;
                c = Math.max(-C, Math.min(A - C, c - C)), d = Math.max(-C, Math.min(B - C, d - C)), Y.css({
                    top: d + "px",
                    left: c + "px"
                });
                var e = L * F;
                bb.css({
                    left: e - G / 2 + "px"
                });
                var f = I * D;
                $.css({
                    top: f - H + "px"
                })
            }
        }

        function Ob(a) {
            var b = Jb(),
                c = "",
                d = !tinycolor.equals(b, pb);
            b && (c = b.toString(rb), xb(b)), ib && U.val(c), pb = b, a && d && (x.change(b), U.trigger("change", [b]))
        }

        function Pb() {
            A = X.width(), B = X.height(), C = Y.height(), E = Z.width(), D = Z.height(), H = $.height(), F = ab.width(), G = bb.width(), r || (W.css("position", "absolute"), W.offset(o(W, mb))), Nb(), p.showPalette && zb(), U.trigger("reflow.spectrum")
        }

        function Qb() {
            U.show(), mb.unbind("click.spectrum touchstart.spectrum"), W.remove(), lb.remove(), e[Ub.id] = null
        }

        function Rb(a, d) {
            return a === c ? b.extend({}, p) : d === c ? p[a] : (p[a] = d, vb(), void 0)
        }

        function Sb() {
            V = !1, U.attr("disabled", !1), mb.removeClass("sp-disabled")
        }

        function Tb() {
            Gb(), V = !0, U.attr("disabled", !0), mb.addClass("sp-disabled")
        }
        var p = m(n, d),
            r = p.flat,
            u = p.showSelectionPalette,
            v = p.localStorageKey,
            w = p.theme,
            x = p.callbacks,
            y = t(Pb, 10),
            z = !1,
            A = 0,
            B = 0,
            C = 0,
            D = 0,
            E = 0,
            F = 0,
            G = 0,
            H = 0,
            I = 0,
            J = 0,
            K = 0,
            L = 1,
            M = [],
            N = [],
            O = p.selectionPalette.slice(0),
            P = p.maxSelectionSize,
            Q = "sp-dragging",
            R = null,
            S = d.ownerDocument,
            U = (S.body, b(d)),
            V = !1,
            W = b(j, S).addClass(w),
            X = W.find(".sp-color"),
            Y = W.find(".sp-dragger"),
            Z = W.find(".sp-hue"),
            $ = W.find(".sp-slider"),
            _ = W.find(".sp-alpha-inner"),
            ab = W.find(".sp-alpha"),
            bb = W.find(".sp-alpha-handle"),
            cb = W.find(".sp-input"),
            db = W.find(".sp-palette"),
            eb = W.find(".sp-initial"),
            fb = W.find(".sp-cancel"),
            gb = W.find(".sp-clear"),
            hb = W.find(".sp-choose"),
            ib = U.is("input"),
            jb = ib && h && "color" === U.attr("type"),
            kb = ib && !r,
            lb = kb ? b(i).addClass(w).addClass(p.className) : b([]),
            mb = kb ? lb : U,
            nb = lb.find(".sp-preview-inner"),
            ob = p.color || ib && U.val(),
            pb = !1,
            qb = p.preferredFormat,
            rb = qb,
            sb = !p.showButtons || p.clickoutFiresChange,
            tb = !ob,
            ub = p.allowEmpty && !jb;
        wb();
        var Ub = {
            show: Fb,
            hide: Gb,
            toggle: Eb,
            reflow: Pb,
            option: Rb,
            enable: Sb,
            disable: Tb,
            set: function (a) {
                Ib(a), Ob()
            },
            get: Jb,
            destroy: Qb,
            container: W
        };
        return Ub.id = e.push(Ub) - 1, Ub
    }

    function o(a, c) {
        var d = 0,
            e = a.outerWidth(),
            f = a.outerHeight(),
            g = c.outerHeight(),
            h = a[0].ownerDocument,
            i = h.documentElement,
            j = i.clientWidth + b(h).scrollLeft(),
            k = i.clientHeight + b(h).scrollTop(),
            l = c.offset();
        return l.top += g, l.left -= Math.min(l.left, l.left + e > j && j > e ? Math.abs(l.left + e - j) : 0), l.top -= Math.min(l.top, l.top + f > k && k > f ? Math.abs(f + g - d) : d), l
    }

    function p() { }

    function q(a) {
        a.stopPropagation()
    }

    function r(a, b) {
        var c = Array.prototype.slice,
            d = c.call(arguments, 2);
        return function () {
            return a.apply(b, d.concat(c.call(arguments)))
        }
    }

    function s(c, d, e, g) {
        function o(a) {
            a.stopPropagation && a.stopPropagation(), a.preventDefault && a.preventDefault(), a.returnValue = !1
        }

        function p(a) {
            if (i) {
                if (f && document.documentMode < 9 && !a.button) return r();
                var b = a.originalEvent.touches,
                    e = b ? b[0].pageX : a.pageX,
                    g = b ? b[0].pageY : a.pageY,
                    h = Math.max(0, Math.min(e - j.left, l)),
                    n = Math.max(0, Math.min(g - j.top, k));
                m && o(a), d.apply(c, [h, n, a])
            }
        }

        function q(a) {
            var d = a.which ? 3 == a.which : 2 == a.button;
            a.originalEvent.touches, d || i || e.apply(c, arguments) !== !1 && (i = !0, k = b(c).height(), l = b(c).width(), j = b(c).offset(), b(h).bind(n), b(h.body).addClass("sp-dragging"), m || p(a), o(a))
        }

        function r() {
            i && (b(h).unbind(n), b(h.body).removeClass("sp-dragging"), g.apply(c, arguments)), i = !1
        }
        d = d || function () { }, e = e || function () { }, g = g || function () { };
        var h = c.ownerDocument || document,
            i = !1,
            j = {}, k = 0,
            l = 0,
            m = "ontouchstart" in a,
            n = {};
        n.selectstart = o, n.dragstart = o, n["touchmove mousemove"] = p, n["touchend mouseup"] = r, b(c).bind("touchstart mousedown", q)
    }

    function t(a, b, c) {
        var d;
        return function () {
            var e = this,
                f = arguments,
                g = function () {
                    d = null, a.apply(e, f)
                };
            c && clearTimeout(d), (c || !d) && (d = setTimeout(g, b))
        }
    }
    var d = {
        beforeShow: p,
        move: p,
        change: p,
        show: p,
        hide: p,
        color: !1,
        flat: !1,
        showInput: !1,
        allowEmpty: !1,
        showButtons: !0,
        clickoutFiresChange: !1,
        showInitial: !1,
        showPalette: !1,
        showPaletteOnly: !1,
        showSelectionPalette: !0,
        localStorageKey: !1,
        appendTo: "body",
        maxSelectionSize: 7,
        cancelText: "取消",
        chooseText: "确定",
        clearText: "重置",
        preferredFormat: !1,
        className: "",
        showAlpha: !1,
        theme: "sp-light",
        palette: ["fff", "000"],
        selectionPalette: [],
        disabled: !1
    }, e = [],
        f = !! /msie/i.exec(a.navigator.userAgent),
        g = function () {
            function a(a, b) {
                return !!~("" + a).indexOf(b)
            }
            var b = document.createElement("div"),
                c = b.style;
            return c.cssText = "background-color:rgba(0,0,0,.5)", a(c.backgroundColor, "rgba") || a(c.backgroundColor, "hsla")
        }(),
        h = function () {
            var a = b("<input type='color' value='!' />")[0];
            return "color" === a.type && "!" !== a.value
        }(),
        i = ["<div class='sp-replacer'>", "<div class='sp-preview'><div class='sp-preview-inner'></div></div>", "<div class='sp-dd'>&#9660;</div>", "</div>"].join(""),
        j = function () {
            var a = "";
            if (f)
                for (var b = 1; 6 >= b; b++) a += "<div class='sp-" + b + "'></div>";
            return ["<div class='sp-container sp-hidden'>", "<div class='sp-palette-container'>", "<div class='sp-palette sp-thumb sp-cf'></div>", "</div>", "<div class='sp-picker-container'>", "<div class='sp-top sp-cf'>", "<div class='sp-fill'></div>", "<div class='sp-top-inner'>", "<div class='sp-color'>", "<div class='sp-sat'>", "<div class='sp-val'>", "<div class='sp-dragger'></div>", "</div>", "</div>", "</div>", "<div class='sp-clear sp-clear-display'>", "</div>", "<div class='sp-hue'>", "<div class='sp-slider'></div>", a, "</div>", "</div>", "<div class='sp-alpha'><div class='sp-alpha-inner'><div class='sp-alpha-handle'></div></div></div>", "</div>", "<div class='sp-input-container sp-cf'>", "<input class='sp-input' type='text' spellcheck='false'  />", "</div>", "<div class='sp-initial sp-thumb sp-cf'></div>", "<div class='sp-button-container sp-cf'>", "<a class='sp-cancel' href='#'></a>", "<button class='sp-choose'></button>", "</div>", "</div>", "</div>"].join("")
        }(),
        v = "spectrum.id";
    b.fn.spectrum = function (a) {
        
        if ("string" == typeof a) {
            var d = this,
                f = Array.prototype.slice.call(arguments, 1);
            return this.each(function () {
                var c = e[b(this).data(v)];
                if (c) {
                    
                    var g = c[a];
                    if (!g) throw new Error("Spectrum: no such method: '" + a + "'");
                    if (a == "get") {
                        d = c.get();
                    } else if ("container" == a) {
                        d = c.container
                    } else if ("option" == a) {
                        d = c.option.apply(c, f)
                    } else if ("destroy" == a) {
                        (c.destroy(), b(this).removeData(v))
                    } else {
                        g.apply(c, f)
                    }
                    //"get" == a ? d = c.get() :
                    //    "container" == a ? d = c.container :
                    //        "option" == a ? d = c.option.apply(c, f) : "destroy" == a ? (c.destroy(), b(this).removeData(v)) : g.apply(c, f)
                }
            }), d
        }
        return this.spectrum("destroy").each(function () {
            var c = b.extend({}, a, b(this).data()),
                d = n(this, c);
            b(this).data(v, d.id)
        })
    }, b.fn.spectrum.load = !0, b.fn.spectrum.loadOpts = {}, b.fn.spectrum.draggable = s, b.fn.spectrum.defaults = d, b.spectrum = {}, b.spectrum.localization = {}, b.spectrum.palettes = {}, b.fn.spectrum.processNativeColorInputs = function () {
        h || b("input[type=color]").spectrum({
            preferredFormat: "hex6"
        })
    },
        function () {
            function j(a, b) {
                if (a = a ? a : "", b = b || {}, "object" == typeof a && a.hasOwnProperty("_tc_id")) return a;
                var c = k(a),
                    e = c.r,
                    g = c.g,
                    h = c.b,
                    i = c.a,
                    l = f(100 * i) / 100,
                    n = b.format || c.format;
                return 1 > e && (e = f(e)), 1 > g && (g = f(g)), 1 > h && (h = f(h)), {
                    ok: c.ok,
                    format: n,
                    _tc_id: d++,
                    alpha: i,
                    getAlpha: function () {
                        return i
                    },
                    setAlpha: function (a) {
                        i = u(a), l = f(100 * i) / 100
                    },
                    toHsv: function () {
                        var a = o(e, g, h);
                        return {
                            h: 360 * a.h,
                            s: a.s,
                            v: a.v,
                            a: i
                        }
                    },
                    toHsvString: function () {
                        var a = o(e, g, h),
                            b = f(360 * a.h),
                            c = f(100 * a.s),
                            d = f(100 * a.v);
                        return 1 == i ? "hsv(" + b + ", " + c + "%, " + d + "%)" : "hsva(" + b + ", " + c + "%, " + d + "%, " + l + ")"
                    },
                    toHsl: function () {
                        var a = m(e, g, h);
                        return {
                            h: 360 * a.h,
                            s: a.s,
                            l: a.l,
                            a: i
                        }
                    },
                    toHslString: function () {
                        var a = m(e, g, h),
                            b = f(360 * a.h),
                            c = f(100 * a.s),
                            d = f(100 * a.l);
                        return 1 == i ? "hsl(" + b + ", " + c + "%, " + d + "%)" : "hsla(" + b + ", " + c + "%, " + d + "%, " + l + ")"
                    },
                    toHex: function (a) {
                        return q(e, g, h, a)
                    },
                    toHexString: function (a) {
                        return "#" + q(e, g, h, a)
                    },
                    toRgb: function () {
                        return {
                            r: f(e),
                            g: f(g),
                            b: f(h),
                            a: i
                        }
                    },
                    toRgbString: function () {
                        return 1 == i ? "rgb(" + f(e) + ", " + f(g) + ", " + f(h) + ")" : "rgba(" + f(e) + ", " + f(g) + ", " + f(h) + ", " + l + ")"
                    },
                    toPercentageRgb: function () {
                        return {
                            r: f(100 * v(e, 255)) + "%",
                            g: f(100 * v(g, 255)) + "%",
                            b: f(100 * v(h, 255)) + "%",
                            a: i
                        }
                    },
                    toPercentageRgbString: function () {
                        return 1 == i ? "rgb(" + f(100 * v(e, 255)) + "%, " + f(100 * v(g, 255)) + "%, " + f(100 * v(h, 255)) + "%)" : "rgba(" + f(100 * v(e, 255)) + "%, " + f(100 * v(g, 255)) + "%, " + f(100 * v(h, 255)) + "%, " + l + ")"
                    },
                    toName: function () {
                        return 0 === i ? "transparent" : s[q(e, g, h, !0)] || !1
                    },
                    toFilter: function (a) {
                        var c = q(e, g, h),
                            d = c,
                            f = Math.round(255 * parseFloat(i)).toString(16),
                            k = f,
                            l = b && b.gradientType ? "GradientType = 1, " : "";
                        if (a) {
                            var m = j(a);
                            d = m.toHex(), k = Math.round(255 * parseFloat(m.alpha)).toString(16)
                        }
                        return "progid:DXImageTransform.Microsoft.gradient(" + l + "startColorstr=#" + A(f) + c + ",endColorstr=#" + A(k) + d + ")"
                    },
                    toString: function (a) {
                        var b = !!a;
                        a = a || this.format;
                        var c = !1,
                            d = !b && 1 > i && i > 0,
                            e = d && ("hex" === a || "hex6" === a || "hex3" === a || "name" === a);
                        return "rgb" === a && (c = this.toRgbString()), "prgb" === a && (c = this.toPercentageRgbString()), ("hex" === a || "hex6" === a) && (c = this.toHexString()), "hex3" === a && (c = this.toHexString(!0)), "name" === a && (c = this.toName()), "hsl" === a && (c = this.toHslString()), "hsv" === a && (c = this.toHsvString()), e ? this.toRgbString() : c || this.toHexString()
                    }
                }
            }

            function k(a) {
                var b = {
                    r: 0,
                    g: 0,
                    b: 0
                }, c = 1,
                    d = !1,
                    e = !1;
                return "string" == typeof a && (a = D(a)), "object" == typeof a && (a.hasOwnProperty("r") && a.hasOwnProperty("g") && a.hasOwnProperty("b") ? (b = l(a.r, a.g, a.b), d = !0, e = "%" === String(a.r).substr(-1) ? "prgb" : "rgb") : a.hasOwnProperty("h") && a.hasOwnProperty("s") && a.hasOwnProperty("v") ? (a.s = B(a.s), a.v = B(a.v), b = p(a.h, a.s, a.v), d = !0, e = "hsv") : a.hasOwnProperty("h") && a.hasOwnProperty("s") && a.hasOwnProperty("l") && (a.s = B(a.s), a.l = B(a.l), b = n(a.h, a.s, a.l), d = !0, e = "hsl"), a.hasOwnProperty("a") && (c = a.a)), c = u(c), {
                    ok: d,
                    format: a.format || e,
                    r: g(255, h(b.r, 0)),
                    g: g(255, h(b.g, 0)),
                    b: g(255, h(b.b, 0)),
                    a: c
                }
            }

            function l(a, b, c) {
                return {
                    r: 255 * v(a, 255),
                    g: 255 * v(b, 255),
                    b: 255 * v(c, 255)
                }
            }

            function m(a, b, c) {
                a = v(a, 255), b = v(b, 255), c = v(c, 255);
                var f, i, d = h(a, b, c),
                    e = g(a, b, c),
                    j = (d + e) / 2;
                if (d == e) f = i = 0;
                else {
                    var k = d - e;
                    switch (i = j > .5 ? k / (2 - d - e) : k / (d + e), d) {
                        case a:
                            f = (b - c) / k + (c > b ? 6 : 0);
                            break;
                        case b:
                            f = (c - a) / k + 2;
                            break;
                        case c:
                            f = (a - b) / k + 4
                    }
                    f /= 6
                }
                return {
                    h: f,
                    s: i,
                    l: j
                }
            }

            function n(a, b, c) {
                function g(a, b, c) {
                    return 0 > c && (c += 1), c > 1 && (c -= 1), 1 / 6 > c ? a + 6 * (b - a) * c : .5 > c ? b : 2 / 3 > c ? a + 6 * (b - a) * (2 / 3 - c) : a
                }
                var d, e, f;
                if (a = v(a, 360), b = v(b, 100), c = v(c, 100), 0 === b) d = e = f = c;
                else {
                    var h = .5 > c ? c * (1 + b) : c + b - c * b,
                        i = 2 * c - h;
                    d = g(i, h, a + 1 / 3), e = g(i, h, a), f = g(i, h, a - 1 / 3)
                }
                return {
                    r: 255 * d,
                    g: 255 * e,
                    b: 255 * f
                }
            }

            function o(a, b, c) {
                a = v(a, 255), b = v(b, 255), c = v(c, 255);
                var f, i, d = h(a, b, c),
                    e = g(a, b, c),
                    j = d,
                    k = d - e;
                if (i = 0 === d ? 0 : k / d, d == e) f = 0;
                else {
                    switch (d) {
                        case a:
                            f = (b - c) / k + (c > b ? 6 : 0);
                            break;
                        case b:
                            f = (c - a) / k + 2;
                            break;
                        case c:
                            f = (a - b) / k + 4
                    }
                    f /= 6
                }
                return {
                    h: f,
                    s: i,
                    v: j
                }
            }

            function p(a, b, c) {
                a = 6 * v(a, 360), b = v(b, 100), c = v(c, 100);
                var d = e.floor(a),
                    f = a - d,
                    g = c * (1 - b),
                    h = c * (1 - f * b),
                    i = c * (1 - (1 - f) * b),
                    j = d % 6,
                    k = [c, h, g, g, i, c][j],
                    l = [i, c, c, h, g, g][j],
                    m = [g, g, i, c, c, h][j];
                return {
                    r: 255 * k,
                    g: 255 * l,
                    b: 255 * m
                }
            }

            function q(a, b, c, d) {
                var e = [A(f(a).toString(16)), A(f(b).toString(16)), A(f(c).toString(16))];
                return d && e[0].charAt(0) == e[0].charAt(1) && e[1].charAt(0) == e[1].charAt(1) && e[2].charAt(0) == e[2].charAt(1) ? e[0].charAt(0) + e[1].charAt(0) + e[2].charAt(0) : e.join("")
            }

            function t(a) {
                var b = {};
                for (var c in a) a.hasOwnProperty(c) && (b[a[c]] = c);
                return b
            }

            function u(a) {
                return a = parseFloat(a), (isNaN(a) || 0 > a || a > 1) && (a = 1), a
            }

            function v(a, b) {
                y(a) && (a = "100%");
                var c = z(a);
                return a = g(b, h(0, parseFloat(a))), c && (a = parseInt(a * b, 10) / 100), e.abs(a - b) < 1e-6 ? 1 : a % b / parseFloat(b)
            }

            function w(a) {
                return g(1, h(0, a))
            }

            function x(a) {
                return parseInt(a, 16)
            }

            function y(a) {
                return "string" == typeof a && -1 != a.indexOf(".") && 1 === parseFloat(a)
            }

            function z(a) {
                return "string" == typeof a && -1 != a.indexOf("%")
            }

            function A(a) {
                return 1 == a.length ? "0" + a : "" + a
            }

            function B(a) {
                return 1 >= a && (a = 100 * a + "%"), a
            }

            function D(a) {
                a = a.replace(b, "").replace(c, "").toLowerCase();
                var d = !1;
                if (r[a]) a = r[a], d = !0;
                else if ("transparent" == a) return {
                    r: 0,
                    g: 0,
                    b: 0,
                    a: 0,
                    format: "name"
                };
                var e; 
                return (e = C.rgb.exec(a)) ? {
                    r: e[1],
                    g: e[2],
                    b: e[3]
                } : (e = C.rgba.exec(a)) ? {
                    r: e[1],
                    g: e[2],
                    b: e[3],
                    a: e[4]
                } : (e = C.hsl.exec(a)) ? {
                    h: e[1],
                    s: e[2],
                    l: e[3]
                } : (e = C.hsla.exec(a)) ? {
                    h: e[1],
                    s: e[2],
                    l: e[3],
                    a: e[4]
                } : (e = C.hsv.exec(a)) ? {
                    h: e[1],
                    s: e[2],
                    v: e[3]
                } : (e = C.hex6.exec(a)) ? {
                    r: x(e[1]),
                    g: x(e[2]),
                    b: x(e[3]),
                    format: d ? "name" : "hex"
                } : (e = C.hex3.exec(a)) ? {
                    r: x(e[1] + "" + e[1]),
                    g: x(e[2] + "" + e[2]),
                    b: x(e[3] + "" + e[3]),
                    format: d ? "name" : "hex"
                } : !1
            }
            var b = /^[\s,#]+/,
                c = /\s+$/,
                d = 0,
                e = Math,
                f = e.round,
                g = e.min,
                h = e.max,
                i = e.random;
            j.fromRatio = function (a, b) {
                if ("object" == typeof a) {
                    var c = {};
                    for (var d in a) a.hasOwnProperty(d) && (c[d] = "a" === d ? a[d] : B(a[d]));
                    a = c
                }
                return j(a, b)
            }, j.equals = function (a, b) {
                return a && b ? j(a).toRgbString() == j(b).toRgbString() : !1
            }, j.random = function () {
                return j.fromRatio({
                    r: i(),
                    g: i(),
                    b: i()
                })
            }, j.desaturate = function (a, b) {
                b = 0 === b ? 0 : b || 10;
                var c = j(a).toHsl();
                return c.s -= b / 100, c.s = w(c.s), j(c)
            }, j.saturate = function (a, b) {
                b = 0 === b ? 0 : b || 10;
                var c = j(a).toHsl();
                return c.s += b / 100, c.s = w(c.s), j(c)
            }, j.greyscale = function (a) {
                return j.desaturate(a, 100)
            }, j.lighten = function (a, b) {
                b = 0 === b ? 0 : b || 10;
                var c = j(a).toHsl();
                return c.l += b / 100, c.l = w(c.l), j(c)
            }, j.darken = function (a, b) {
                b = 0 === b ? 0 : b || 10;
                var c = j(a).toHsl();
                return c.l -= b / 100, c.l = w(c.l), j(c)
            }, j.complement = function (a) {
                var b = j(a).toHsl();
                return b.h = (b.h + 180) % 360, j(b)
            }, j.triad = function (a) {
                var b = j(a).toHsl(),
                    c = b.h;
                return [j(a), j({
                    h: (c + 120) % 360,
                    s: b.s,
                    l: b.l
                }), j({
                    h: (c + 240) % 360,
                    s: b.s,
                    l: b.l
                })]
            }, j.tetrad = function (a) {
                var b = j(a).toHsl(),
                    c = b.h;
                return [j(a), j({
                    h: (c + 90) % 360,
                    s: b.s,
                    l: b.l
                }), j({
                    h: (c + 180) % 360,
                    s: b.s,
                    l: b.l
                }), j({
                    h: (c + 270) % 360,
                    s: b.s,
                    l: b.l
                })]
            }, j.splitcomplement = function (a) {
                var b = j(a).toHsl(),
                    c = b.h;
                return [j(a), j({
                    h: (c + 72) % 360,
                    s: b.s,
                    l: b.l
                }), j({
                    h: (c + 216) % 360,
                    s: b.s,
                    l: b.l
                })]
            }, j.analogous = function (a, b, c) {
                b = b || 6, c = c || 30;
                var d = j(a).toHsl(),
                    e = 360 / c,
                    f = [j(a)];
                for (d.h = (d.h - (e * b >> 1) + 720) % 360; --b;) d.h = (d.h + e) % 360, f.push(j(d));
                return f
            }, j.monochromatic = function (a, b) {
                b = b || 6;
                for (var c = j(a).toHsv(), d = c.h, e = c.s, f = c.v, g = [], h = 1 / b; b--;) g.push(j({
                    h: d,
                    s: e,
                    v: f
                })), f = (f + h) % 1;
                return g
            }, j.readability = function (a, b) {
                var c = j(a).toRgb(),
                    d = j(b).toRgb(),
                    e = (299 * c.r + 587 * c.g + 114 * c.b) / 1e3,
                    f = (299 * d.r + 587 * d.g + 114 * d.b) / 1e3,
                    g = Math.max(c.r, d.r) - Math.min(c.r, d.r) + Math.max(c.g, d.g) - Math.min(c.g, d.g) + Math.max(c.b, d.b) - Math.min(c.b, d.b);
                return {
                    brightness: Math.abs(e - f),
                    color: g
                }
            }, j.readable = function (a, b) {
                var c = j.readability(a, b);
                return c.brightness > 125 && c.color > 500
            }, j.mostReadable = function (a, b) {
                for (var c = null, d = 0, e = !1, f = 0; f < b.length; f++) {
                    var g = j.readability(a, b[f]),
                        h = g.brightness > 125 && g.color > 500,
                        i = 3 * (g.brightness / 125) + g.color / 500;
                    (h && !e || h && e && i > d || !h && !e && i > d) && (e = h, d = i, c = j(b[f]))
                }
                return c
            };
            var r = j.names = {
                aliceblue: "f0f8ff",
                antiquewhite: "faebd7",
                aqua: "0ff",
                aquamarine: "7fffd4",
                azure: "f0ffff",
                beige: "f5f5dc",
                bisque: "ffe4c4",
                black: "000",
                blanchedalmond: "ffebcd",
                blue: "00f",
                blueviolet: "8a2be2",
                brown: "a52a2a",
                burlywood: "deb887",
                burntsienna: "ea7e5d",
                cadetblue: "5f9ea0",
                chartreuse: "7fff00",
                chocolate: "d2691e",
                coral: "ff7f50",
                cornflowerblue: "6495ed",
                cornsilk: "fff8dc",
                crimson: "dc143c",
                cyan: "0ff",
                darkblue: "00008b",
                darkcyan: "008b8b",
                darkgoldenrod: "b8860b",
                darkgray: "a9a9a9",
                darkgreen: "006400",
                darkgrey: "a9a9a9",
                darkkhaki: "bdb76b",
                darkmagenta: "8b008b",
                darkolivegreen: "556b2f",
                darkorange: "ff8c00",
                darkorchid: "9932cc",
                darkred: "8b0000",
                darksalmon: "e9967a",
                darkseagreen: "8fbc8f",
                darkslateblue: "483d8b",
                darkslategray: "2f4f4f",
                darkslategrey: "2f4f4f",
                darkturquoise: "00ced1",
                darkviolet: "9400d3",
                deeppink: "ff1493",
                deepskyblue: "00bfff",
                dimgray: "696969",
                dimgrey: "696969",
                dodgerblue: "1e90ff",
                firebrick: "b22222",
                floralwhite: "fffaf0",
                forestgreen: "228b22",
                fuchsia: "f0f",
                gainsboro: "dcdcdc",
                ghostwhite: "f8f8ff",
                gold: "ffd700",
                goldenrod: "daa520",
                gray: "808080",
                green: "008000",
                greenyellow: "adff2f",
                grey: "808080",
                honeydew: "f0fff0",
                hotpink: "ff69b4",
                indianred: "cd5c5c",
                indigo: "4b0082",
                ivory: "fffff0",
                khaki: "f0e68c",
                lavender: "e6e6fa",
                lavenderblush: "fff0f5",
                lawngreen: "7cfc00",
                lemonchiffon: "fffacd",
                lightblue: "add8e6",
                lightcoral: "f08080",
                lightcyan: "e0ffff",
                lightgoldenrodyellow: "fafad2",
                lightgray: "d3d3d3",
                lightgreen: "90ee90",
                lightgrey: "d3d3d3",
                lightpink: "ffb6c1",
                lightsalmon: "ffa07a",
                lightseagreen: "20b2aa",
                lightskyblue: "87cefa",
                lightslategray: "789",
                lightslategrey: "789",
                lightsteelblue: "b0c4de",
                lightyellow: "ffffe0",
                lime: "0f0",
                limegreen: "32cd32",
                linen: "faf0e6",
                magenta: "f0f",
                maroon: "800000",
                mediumaquamarine: "66cdaa",
                mediumblue: "0000cd",
                mediumorchid: "ba55d3",
                mediumpurple: "9370db",
                mediumseagreen: "3cb371",
                mediumslateblue: "7b68ee",
                mediumspringgreen: "00fa9a",
                mediumturquoise: "48d1cc",
                mediumvioletred: "c71585",
                midnightblue: "191970",
                mintcream: "f5fffa",
                mistyrose: "ffe4e1",
                moccasin: "ffe4b5",
                navajowhite: "ffdead",
                navy: "000080",
                oldlace: "fdf5e6",
                olive: "808000",
                olivedrab: "6b8e23",
                orange: "ffa500",
                orangered: "ff4500",
                orchid: "da70d6",
                palegoldenrod: "eee8aa",
                palegreen: "98fb98",
                paleturquoise: "afeeee",
                palevioletred: "db7093",
                papayawhip: "ffefd5",
                peachpuff: "ffdab9",
                peru: "cd853f",
                pink: "ffc0cb",
                plum: "dda0dd",
                powderblue: "b0e0e6",
                purple: "800080",
                red: "f00",
                rosybrown: "bc8f8f",
                royalblue: "4169e1",
                saddlebrown: "8b4513",
                salmon: "fa8072",
                sandybrown: "f4a460",
                seagreen: "2e8b57",
                seashell: "fff5ee",
                sienna: "a0522d",
                silver: "c0c0c0",
                skyblue: "87ceeb",
                slateblue: "6a5acd",
                slategray: "708090",
                slategrey: "708090",
                snow: "fffafa",
                springgreen: "00ff7f",
                steelblue: "4682b4",
                tan: "d2b48c",
                teal: "008080",
                thistle: "d8bfd8",
                tomato: "ff6347",
                turquoise: "40e0d0",
                violet: "ee82ee",
                wheat: "f5deb3",
                white: "fff",
                whitesmoke: "f5f5f5",
                yellow: "ff0",
                yellowgreen: "9acd32"
            }, s = j.hexNames = t(r),
                C = function () {
                    var a = "[-\\+]?\\d+%?",
                        b = "[-\\+]?\\d*\\.\\d+%?",
                        c = "(?:" + b + ")|(?:" + a + ")",
                        d = "[\\s|\\(]+(" + c + ")[,|\\s]+(" + c + ")[,|\\s]+(" + c + ")\\s*\\)?",
                        e = "[\\s|\\(]+(" + c + ")[,|\\s]+(" + c + ")[,|\\s]+(" + c + ")[,|\\s]+(" + c + ")\\s*\\)?";
                    return {
                        rgb: new RegExp("rgb" + d),
                        rgba: new RegExp("rgba" + e),
                        hsl: new RegExp("hsl" + d),
                        hsla: new RegExp("hsla" + e),
                        hsv: new RegExp("hsv" + d),
                        hex3: /^([0-9a-fA-F]{1})([0-9a-fA-F]{1})([0-9a-fA-F]{1})$/,
                        hex6: /^([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})$/
                    }
                }();
            a.tinycolor = j
        }(), b(function () {
            b.fn.spectrum.load && b.fn.spectrum.processNativeColorInputs()
        })
}(window, jQuery);