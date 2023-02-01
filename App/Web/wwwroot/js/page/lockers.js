~~((core, w, d, reff) => {
    'use strict';

    ~~(register => register.register(register))({
        register: register => {
            core = register;

            (a => core.utils.activator.c(a))({
                a: () => core.utils.debugPrint('INFO', 'Starting apps...'),
                b: () => core.utils.registrar(register),
                c: () => core.events.windoc.ready()
            });
        },
        ui: {
            toolbarButtons: () => {
                return {
                    btnAdd: {
                        text: 'Add New',
                        icon: 'bi-building-add',
                        attributes: {
                            title: 'Create a new customer data.'
                        },
                        event: () => core.utils.setForm({
                            id: '0',
                            cb: () => reff.dom.modal.show()
                        })
                    }
                };
            },
            grid: {
                formatter: {
                    email: (v, r) => `<a href="mailto:${v.toLowerCase()}">${v}</a>`,
                    actions: (v, r, i) => [
                        `<a href="javascript:void(0)" class="icon-row-edit btn btn-sm btn-outline-primary" data-row-id="${r.id}" title="Edit"><i class="bi bi-pencil-square"></i></a>`,
                        ` &nbsp; `,
                        `<a href="javascript:void(0)" class="icon-row-delete btn btn-sm btn-outline-danger" data-row-id="${r.id}" title="Delete"><i class="bi bi-trash"></i></a>`
                    ].join('')
                }
            },
            modal: {
                editor: () => {
                    reff.dom.modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('editorModal'));
                }
            }
        },
        events: {
            windoc: {
                ready: () => $(d).ready(() => {
                    (a => core.utils.activator.c(a))({
                        a: () => core.ui.modal.editor(),
                        b: () => core.events.form.save()
                    })
                })
            },
            grid: {
                actions: {
                    'click .icon-row-delete': (e, v, r, i) => {
                        core.utils.confirmModal({
                            title: 'Delete Confirmation',
                            message: `Are you sure want to remove locker number "${r.number}" ?`,
                            bgColor: 'danger text-white',
                            yesButton: {
                                text: 'Yes',
                                cb: () => $
                                    .ajax({
                                        method: 'DELETE',
                                        url: `${reff.curl}?id=${r.id}`
                                    })
                                    .done(() => $('#table').bootstrapTable('refresh')),
                            },
                            noButton: {
                                text: 'No',
                                cb: () => core.utils.debugPrint("INFO", `Cancel deletion process.`)
                            }
                        });
                    },
                    'click .icon-row-edit': (e, v, r, i) => core.utils.setForm({
                        id: r.id,
                        number: r.number,
                        cb: () => reff.dom.modal.show()
                    })
                }
            },
            form: {
                save: () => $('form#editorForm').submit(e => {
                    e.preventDefault();

                    let cdata = core.utils.getForm();

                    if (cdata.number === '') {
                        core.utils.showFormAlert({
                            text: 'Please fill all field which marked with asterisk ( * ).',
                            color: 'danger',
                            cb: () => $('input#txtName').focus()
                        });
                        return;
                    }

                    $
                        .ajax({
                            method: cdata.id === '0' ? 'POST' : 'PUT',
                            url: reff.curl,
                            contentType: 'application/json',
                            data: JSON.stringify(cdata)
                        })
                        .fail((a, b, c) => core.utils.showFormAlert({
                            text: a.responseText,
                            color: 'danger',
                            cb: () => $('input#txtNumber').focus()
                        }))
                        .done(r => {
                            $('#table').bootstrapTable('refresh');
                            reff.dom.modal.hide();
                        });
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
            doNothing: () => { },
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
            confirmModal: ({ title, message, yesButton, noButton, bgColor }) => {
                let noBtn = '';
                if (noButton !== undefined && noButton.hasOwnProperty('text'))
                    noBtn = `<button type="button" class="btn btn-lg btn-secondary close" data-bs-dismiss="modal"><strong>${noButton.text}</strong></button>`;

                bgColor = bgColor || 'white text-dark';


                let confirm = $(
                    [
                        `<div class="modal fade" id="confirmBox" data-bs-backdrop="static" data-bs-keyboard="true" tabindex="-1" aria-labelledby="editorModalLabel" aria-hidden="true">`,
                        `<div class="modal-dialog modal-dialog-centered" id="waitDialog"><div class="modal-content"><div class="modal-header bg-${bgColor}">`,
                        `<h5 class="modal-title" id="staticBackdropLabel"><strong>${title}</strong></h5>`,
                        `<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>`,
                        `</div><div class="modal-body">${message}</div><div class="modal-footer">${noBtn}`,
                        `<button type="button" class="btn btn-${bgColor} btn-lg confirmButton"><strong>${yesButton.text}</strong></button>`,
                        `</div></div></div></div>`
                    ].join('')
                );

                $("body").append(confirm);

                let myModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('confirmBox'));
                myModal.show();

                confirm.find("button.confirmButton").one("click", function () {
                    yesButton.cb && yesButton.cb();
                    myModal.hide();
                });

                confirm.find("button.close").one("click", function () {
                    noButton.cb && noButton.cb();
                    myModal.hide();
                })

                confirm.one('hidden.bs.modal', function () {
                    confirm.remove();
                });
            },
            setForm: ({ id, number, cb }) => {
                $('input#txtId').val(id);
                $('input#txtNumber').val(number);

                cb && cb();
            },
            getForm: () => {
                return {
                    id: $('input#txtId').val(),
                    number: $('input#txtNumber').val()
                };
            },
            showFormAlert: ({ text, color, cb }) => {
                $('div#form-alert-container')
                    .html(`<div class="alert alert-${color} form-alert" role="alert">${text}</div>`)
                    .fadeIn(750);

                setTimeout(() => {
                    $('div#form-alert-container').fadeOut(750);
                }, 5000);

                cb && cb();
            }
        }
    });
})(
    null,
    window,
    document,
    {
        debugMode: !0,
        registrarNamespace: 'lawencon',
        curl: `${window.location.origin}/api/Lockers`,
        dom: {
            modal: null
        }
    }
);