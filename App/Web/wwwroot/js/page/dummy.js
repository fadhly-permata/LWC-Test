~~((core, w, $, reff) => {
    'use strict';

    ~~(register => register.register(register))({
        register: register => {
            core = register;
            core.options = reff.options;

            (a => core.utils.activator.c(a))({
                a: () => core.utils.debugPrint('EasyGo JavaScript Helper'),
                b: () => core.utils.registrar(register),
                c: () => core.utils.activator.c(core.events.windoc)
            })
        },
        ui: {
            toastr: options => (self => self.register(self))({
                register: self => self.act(self),
                opt: core.utils.json.merge(core.options.toastr, options),
                act: (self) => {
                    if (self.opt.debugMode)
                        core.utils.debugPrint('Toastr', self.opt);

                    w.toastr.options = self.opt;
                    w.toastr[self.opt.bsColor](self.opt.message, self.opt.title);
                }
            }),
            toastrError: options => (self => self.register(self))({
                register: self => self.act(self),
                opt: core.utils.json.merge(core.options.toastrError, options),
                act: self => {
                    self.opt.message = core.utils.templateTranslator(self.opt.message, self.opt.templateTranslatorData);

                    if (self.opt.debugMode)
                        core.utils.debugPrint('Toastr Error', self.opt);

                    core.ui.toastr({
                        debugMode: !!0,
                        bsColor: 'error',
                        title: self.opt.title,
                        message: self.opt.message
                    });
                }
            }),

            templateManager: {
                TopMenu: {
                    destroyAll: () => (self => self.register(self))({
                        register: self => self.act(self),
                        dom: $('li', reff.doms.topBar.menu()),
                        act: self => self.dom.remove()
                    }),
                    rootMenu: {
                        destroy: index => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $(`li.kt-menu__item.kt-menu__item--submenu.kt-menu__item--rel:eq(${index})`, reff.doms.topBar.menu()),
                            act: self => self.dom.remove()
                        }),
                        addWithoutChild: options => (self => {
                            self.register(self);
                        })({
                            register: self => self.act(self),
                            dom: reff.doms.topBar.menu(),
                            opt: core.utils.json.merge(core.options.topBarRootMenu_AddWhithoutChild, options),
                            cnt: self => $('li.kt-menu__item.kt-menu__item--submenu.kt-menu__item--rel', self.dom).length,
                            act: self => {
                                const opt = self.opt;
                                if (opt.index < 0) opt.index = 0;

                                const tpl = `
                                    <li class="kt-menu__item kt-menu__item--submenu kt-menu__item--rel" data-ktmenu-submenu-toggle="{ac.ApplicationState.TopMenuExpandOn}" aria-haspopup="true">
                                        <a href="${opt.url}" class="kt-menu__link kt-menu__toggle">
                                            <span class="kt-menu__link-text">${opt.title}</span>
                                            <i class="kt-menu__ver-arrow la la-angle-right"></i>
                                        </a>
                                    </li>
                                `;

                                !!1 === (0 === opt.index || 0 === self.cnt(self)) ?
                                    self.dom.prepend(tpl) :
                                    $(`li.kt-menu__item.kt-menu__item--submenu.kt-menu__item--rel:eq(${opt.index})`, reff.doms.topBar.menu()).after(tpl);
                            }
                        }),
                        // TODO: SELESAIKAN INI
                        addWithChilds: options => (self => self.register(self))({
                            register: self => {}
                        }),
                        edit: (oldOption, newOption) => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $(`li.kt-menu__item.kt-menu__item--submenu.kt-menu__item--rel > a[href="${oldOption.url}"]:contains('${oldOption.title}')`, reff.doms.topBar.menu()),
                            paramChecker: () => core.utils.isValidData(oldOption) || core.utils.isValidData(newOption),
                            act: self => {
                                if (!self.paramChecker()) {
                                    core.ui.toastrError({
                                        debugMode: !!0,
                                        title: 'Arguments Null Exception',
                                        message: 'Invalid supplied param for {{method}}',
                                        templateTranslatorData: {
                                            method: `<code>${reff.registrarNamespace}.ui.templateManager.topMenu.rootMenu.edit()</code>`
                                        }
                                    });
                                    return;
                                }

                                const tgt = self.dom;
                                const newUrl = oldOption.url === 'javascript:;' && tgt.siblings('div.kt-menu__submenu').length > 0 ?
                                    'javascript:;' :
                                    newOption.url;

                                if (tgt.length === 0) {
                                    core.ui.toastrError({
                                        debugMode: !!0,
                                        title: 'Not Found',
                                        message: 'Top menu yang Anda maksud tidak dapat ditemukan.'
                                    });
                                    return;
                                }

                                tgt
                                    .attr('href', newUrl)
                                    .find('span.kt-menu__link-text')
                                    .text(newOption.title);
                            }
                        }),
                        visibility: (index, visible = true) => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $(`li.kt-menu__item.kt-menu__item--submenu.kt-menu__item--rel:eq(${index})`, reff.doms.topBar.menu()),
                            act: self => self.dom[`fade${visible ? 'In' : 'Out'}`](750)
                        })
                    },
                    // TODO: SELESAIKAN INI
                    childMenu: {
                        getMenu: (parent, child) => $(`a[href="${parent.url}"]:contains('${parent.title}')`, reff.doms.topBar.menu())
                            .siblings('div.kt-menu__submenu')
                            .find(`a[href="${child.url}"]:contains('${child.title}')`)
                            .closest('li'),
                        destroy: (parent, child) => core.ui.templateManager.topBar.childMenu.getMenu(parent, child).remove()
                    }
                },
                quickPanel: {
                    tabs: {
                        getTabByIndex: index => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $('.kt-quick-panel__nav > ul > li.nav-item', reff.doms.quickPanel()),
                            idx: self => !index || isNaN(index) || index < 0 || index >= self.dom.length ? 0 : index,
                            act: self => $($(self.dom[self.idx(self)])[0])
                        }),
                        getTabContentByIndex: index => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $('.kt-quick-panel__content > .tab-content > .tab-pane', reff.doms.quickPanel()),
                            idx: self => !index || isNaN(index) || index < 0 || index >= self.dom.length ? 0 : index,
                            act: self => $($(self.dom[self.idx(self)])[0])
                        }),
                        add: options => (self => self.register(self))({
                            register: self => self.act(self),
                            tab: $('.kt-quick-panel__nav > .nav-tabs', reff.doms.quickPanel()),
                            ctn: $('.kt-quick-panel__content > .tab-content', reff.doms.quickPanel()),
                            opt: core.utils.json.merge(reff.options.quickPanel_addTab, options),
                            builder: {
                                tab: self => {
                                    const cid = core.utils.slugify({
                                        text: self.opt.title,
                                        prefix: 'kt_quick_panel_tab',
                                        separator: '_'
                                    });
                                    return `
                                        <li class="nav-item">
                                            <a class="nav-link" data-toggle="tab" href="#${cid}" role="tab">${self.opt.title}</a>
                                        </li>
                                    `;
                                },
                                ctn: self => {
                                    const cid = core.utils.slugify({
                                        text: self.opt.title,
                                        prefix: 'kt_quick_panel_tab',
                                        separator: '_'
                                    });
                                    return `
                                        <div class="tab-pane fade show kt-scroll" id="${cid}" role="tabpanel">
                                            <div class="kt-notification">
                                            </div>
                                        </div>
                                    `;
                                }
                            },
                            act: self => {
                                const cnt = self.tab.find('li').length;
                                if (cnt === 0) self.opt.index = 0;

                                const sbt = self.builder.tab(self);
                                const sbc = self.builder.ctn(self);

                                if (self.opt.index === 0) {
                                    self.tab.prepend(sbt);
                                    self.ctn.prepend(sbc);

                                } else {
                                    self.tab.find(`li.nav-item:nth-child(${self.opt.index})`).after(sbt);
                                    self.ctn.find(`div.tab-pane:nth-child(${self.opt.index})`).after(sbc);
                                }
                            }
                        }),
                        remove: index => (self => self.register(self))({
                            register: self => self.act(self),
                            act: () => {
                                const qpt = core.ui.templateManager.quickPanel.tabs;
                                qpt.getTabByIndex(index).remove();
                                qpt.getTabContentByIndex(index).remove();
                            }
                        }),
                        hide: (index, cb) => $(core.ui.templateManager.quickPanel.tabs.getTabByIndex(index))[core.options.common.removeAnimation](
                            core.options.common.removeDuration,
                            () => cb && cb()
                        ),
                        show: (index, cb) => $(core.ui.templateManager.quickPanel.tabs.getTabByIndex(index))[core.options.common.showAnimation](
                            core.options.common.showDuration,
                            () => cb && cb()
                        ),
                        changeActive: index => (self => self.register(self))({
                            register: self => self.act(self),
                            dom: $('.kt-quick-panel__nav > ul > li.nav-item > a.nav-link', reff.doms.quickPanel()),
                            idx: () => !index || isNaN(index) || index < 0 || index > self.dom.length ? 0 : index,
                            act: self => {
                                self.dom.removeClass('active');
                                $(self.dom[self.idx]).trigger('click');
                            }
                        })
                    },
                    content: {
                        addRow: options => (self => self.register(self))({
                            register: self => self.act(self),
                            opt: core.utils.json.merge(reff.options.quickPanel_AddRow, options),
                            act: self => {
                                const so = self.opt;

                                so.onClick = core.utils.defaultValue(so.onClick, '#');
                                const nhref = typeof so.onClick === 'function' ? 'javascript:;' : '#';
                                const qpa = `${core.utils.slugify({text: so.title, prefix: 'qpa'})}-${Math.round(Math.random() * 999999)}`;

                                const html = so.styleVersion === 2 ?
                                    `
                                        <a href="${nhref}" class="kt-notification-v2__item ${qpa}">
                                            <div class="kt-notification-v2__item-icon">
                                                <i class="${so.icon} kt-font-${so.iconColor}"></i>
                                            </div>
                                            <div class="kt-notification-v2__itek-wrapper">
                                                <div class="kt-notification-v2__item-title">
                                                    ${so.title}
                                                </div>
                                                <div class="kt-notification-v2__item-desc">
                                                    ${so.desc}
                                                </div>
                                            </div>
                                        </a>
                                    ` :
                                    `
                                        <a href="${nhref}" class="kt-notification__item ${qpa}">
                                            <div class="kt-notification__item-icon">
                                                <i class="${so.icon} kt-font-${so.iconColor}"></i>
                                            </div>
                                            <div class="kt-notification__item-details">
                                                <div class="kt-notification__item-title">
                                                    ${so.title}
                                                </div>
                                                <div class="kt-notification__item-time">
                                                    ${so.desc}
                                                </div>
                                            </div>
                                        </a>
                                    `;

                                const tgt = $(core.ui.templateManager.quickPanel.tabs.getTabContentByIndex(self.opt.tabIndex))
                                    .find(`div.kt-notification${so.styleVersion === 2 ? '-v2' : ''}`);

                                tgt[so.insertMode](html);

                                if (nhref === 'javascript:;')
                                    core.utils.revent(tgt.find(`a.${qpa}`), 'click', e => so.onClick(e))
                            }
                        }),
                        removeRow: (tabIndex, rowIndex) => core.ui.templateManager.quickPanel.tabs.getTabContentByIndex(tabIndex)
                            .find(`div[class^="kt-notification"] > a[class^="kt-notification"]`)
                            .eq(rowIndex)
                            .remove()
                    }
                },
                loadingScreen: {
                    hide: cb => (self => self.register(self))({
                        register: self => self.act(self),
                        dom: reff.doms.loadingScreen(),
                        act: self => core.utils.dom.hide(self.dom, cb)
                    }),
                    show: cb => (self => self.register(self))({
                        register: self => self.act(self),
                        dom: reff.doms.loadingScreen(),
                        act: self => core.utils.dom.show(self.dom, cb)
                    }),
                    redirect: url => (self => self.register(self))({
                        register: self => self.act(self),
                        dom: reff.doms.loadingScreen(),
                        act: self => core.utils.dom.show(self.dom, () => {
                            setTimeout(() => {
                                window.location = url
                            }, core.options.common.redirectAnimationDuration);
                        })
                    })
                }
            }
        },
        events: {
            windoc: {
                ready: () => $((a => core.utils.activator.c(a))({
                    a: () => setTimeout(() => core.ui.templateManager.loadingScreen.hide(), core.options.common.readyAnimationDuration),
                    b: () => core.utils.activator.c(core.events.anchors)
                }))
            },
            anchors: {
                // redirect: () => core.utils.revent($('body a[href]'), 'click', e => console.log(e))
                redirect: () => (self => self.register(self))({
                    register: self => core.utils.revent(self.dom, self.eve, e => self.act(e)),
                    dom: $('body a[href]'),
                    eve: 'click',
                    act: e => {
                        const ne = $(e.currentTarget);
                        const href = ne.attr('href');

                        if (href && href.split('')[0] !== '#' && href !== 'javascript:;') {
                            e.preventDefault();
                            core.ui.templateManager.loadingScreen.redirect(href)
                            return;
                        }

                        core.utils.doNothing();
                    }
                })
            }
        },
        utils: {
            registrar: () => {
                w[reff.registrarNamespace] = core;

                delete w[reff.registrarNamespace].register;
                delete w[reff.registrarNamespace].utils.registrar;
            },
            activator: {
                a: (events, root) => events.split('|').forEach(a => root[a]()),
                b: (events, root) => events.split('|').forEach(a => root[a]['register']()),
                c: root => {
                    for (let item in root) root.hasOwnProperty(item) && 'register' !== item && 'function' === typeof root[item] && root[item]();
                },
                d: function (events, root) {
                    '0|1'.split('|').forEach(x => delete arguments[x]);

                    const argues = [];
                    for (let i in arguments) arguments.hasOwnProperty(i) && argues.push(arguments[i]);
                    events.split('|').forEach(x => root[x].apply(root, argues));
                },
                e: function (root) {
                    delete arguments['0'];
                    const argues = [];
                    for (let i in arguments) arguments.hasOwnProperty(i) && argues.push(arguments[i]);
                    for (let i in root) root.hasOwnProperty(i) && 'register' !== i && 'function' === typeof root[i] && root[i].apply(root, argues);
                }
            },

            doNothing: () => {},

            // DEBUGGER
            debugPrint: function (session) {
                if (reff.debugMode === false || arguments.length < 1) return;
                delete arguments['0'];

                const sty = 'color: black; font-style: italic; font-weight: bold; font-size: 120%; background-color: #a2caee; padding: 0 120px 0 120px;';

                const argues = [];
                for (let i in arguments)
                    if (arguments.hasOwnProperty(i)) argues.push(arguments[i]);
                if (argues.length === 0) argues.push('PASS');

                console.info(`%c[BOF:: ${session}]`, sty);

                console.group('[DATA]');
                argues.forEach(x => console.log(x));
                console.groupEnd();

                console.info(`%c[EOF:: ${session}]`, sty);
            },

            // TRANSLATOR (STILL DUMMY)
            templateTranslator: (text, data) => {
                $.each(data, (k, v) => {
                    text = text.replace(RegExp(`{{${k}}}`, 'g'), v);
                });
                return text;
            },

            // DOM MANIPULATION
            dom: {
                hide: (dom, cb) => (self => self.register(self))({
                    register: self => self.act(self),
                    opt: core.options.common,
                    act: self => {
                        setTimeout(() => cb && cb(), self.opt.hideDuration);
                        $(dom)[self.opt.removeAnimation](self.opt.removeDuration)
                    }
                }),
                show: (dom, cb) => (self => self.register(self))({
                    register: self => self.act(self),
                    opt: core.options.common,
                    act: self => {
                        setTimeout(() => cb && cb(), self.opt.showDuration);
                        $(dom)[self.opt.showAnimation](self.opt.showDuration)
                    }
                })
            },

            // JSON MANIPULATION
            json: {
                copy: json => $.extend(true, {}, json),
                merge: (json, njson) => $.extend(true, core.utils.json.copy(json), njson)
            },

            // COMMONS HELPER
            once: options => (self => self.register(self))({
                register: self => self.act(self),
                opt: core.utils.json.merge(core.options.once, options),
                cto: opt => {
                    clearTimeout(w[opt.name]);
                    delete w[opt.name];
                },
                act: self => {
                    const opt = self.opt;
                    if (w[opt.name]) self.cto();

                    w[opt.name] = setTimeout(() => {
                        opt.cb && opt.cb();
                        self.cto(opt);
                    }, opt.interval);
                }
            }),
            isValidData: data => Array.isArray(data) ?
                data.length > 0 : data && typeof data === 'object' ?
                !(Object.keys(data).length === 0 && data.constructor === Object) : data !== undefined && data !== null && data !== '',
            defaultValue: (value, onNullValue) => core.utils.isValidData(value) ? value : onNullValue,

            // EVENTS
            revent: (target, event, func) => {
                const ivd = core.utils.isValidData;

                ivd(target) && ivd(event) && ivd(func) && typeof func === 'function' && (() => {
                    target = $(target);
                    target.off(event).on(event, func);
                })();
            },

            // NUMBER HELPER
            tryParseInt: (value, def = 0) => isNaN(parseInt(value)) || Infinity === value ? def : parseInt(value),
            tryParseFloat: (value, def = 0.0) => isNaN(parseFloat(value)) || Infinity === value ? def : parseFloat(value),
            numberFormat: (num, fractionDigits = 2) => num.toFixed(fractionDigits).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'),

            // string helper
            slugify: options => (self => self.register(self))({
                register: self => self.act(self),
                opt: core.utils.json.merge(core.options.slugify, options),
                act: self => {
                    const opt = self.opt;
                    if (!core.utils.isValidData(opt.text)) return '';

                    opt.suffix = !core.utils.isValidData(opt.suffix) ? '' : `${opt.separator}${opt.suffix}`;
                    opt.prefix = !core.utils.isValidData(opt.prefix) ? '' : `${opt.prefix}${opt.separator}`;

                    return `${opt.prefix}${(opt.text.toLowerCase().replace(RegExp(' ', 'g'), opt.separator))}${opt.suffix}`;
                }
            }),

            // ENCODER
            encryptor: source => w.btoa(source),
            decryptor: source => w.atob(source)
        }
    });
})(
    null,
    window,
    window.jQuery, {
        debugMode: !0,
        debugPrintConfigs: {},
        registrarNamespace: 'easyGo',
        doms: {
            topBar: {
                menu: () => $('div#kt_header_menu > ul.kt-menu__nav'),
                toolbar: () => $('#kt-header__topbar')
            },
            quickPanel: () => $('div#kt_quick_panel'),
            loadingScreen: () => $('div#fp-loading-screen')
        },
        options: {
            common: {
                removeAnimation: 'fadeOut',
                showAnimation: 'fadeIn',
                removeDuration: 750,
                showDuration: 750,
                readyAnimationDuration: 5000,
                redirectAnimationDuration: 1500
            },
            once: {
                name: `tmr_${Math.round(Math.random() * 999999)}`,
                interval: 100,
                cb: null
            },
            slugify: {
                text: '',
                prefix: '',
                suffix: '',
                separator: '-'
            },
            toastr: {
                closeButton: !!1,
                debug: !!0,
                debugMode: !!1,
                newestOnTop: !!1,
                progressBar: !!1,
                positionClass: "toast-bottom-right",
                preventDuplicates: !!1,
                showDuration: "300",
                hideDuration: "1000",
                timeOut: "5000",
                extendedTimeOut: "1000",
                showEasing: "swing",
                hideEasing: "linear",
                showMethod: "fadeIn",
                hideMethod: "fadeOut",
                bsColor: "success",
                title: "enter the title",
                message: "enter your message"
            },
            toastrError: {
                debugMode: !!1,
                title: 'Error Title',
                message: 'Error {{data}}',
                templateTranslatorData: {
                    data: '123456'
                }
            },
            topBarRootMenu_AddWhithoutChild: {
                index: 0,
                url: '#',
                title: 'Sample Menu'
            },
            topBarRootMenu_AddWithChilds: {
                index: 0,
                title: 'Sample Menu',
                childs: [{
                        url: '#',
                        title: 'Child 1'
                    },
                    {
                        url: '#',
                        title: 'Child 2'
                    },
                    {
                        url: '#',
                        title: 'Child 3',
                        childs: [{
                                url: '#',
                                title: 'Child 1'
                            },
                            {
                                url: '#',
                                title: 'Child 2'
                            }
                        ]
                    }
                ]
            },
            quickPanel_addTab: {
                index: 0,
                title: 'Tab Title'
            },
            quickPanel_AddRow: {
                tabIndex: 0,
                icon: 'flaticon-bell',
                iconColor: 'brand',
                title: 'Notification Title',
                desc: 'Notification description',
                styleVersion: 1,
                onClick: null,
                insertMode: 'append'
            }
        }
    }
);