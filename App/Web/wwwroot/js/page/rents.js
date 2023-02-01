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
                        text: 'New Rent Transactions',
                        icon: 'bi-box-fill',
                        attributes: {
                            title: 'Create a new rent transaction.'
                        },
                        event: () => {
                            $
                                .ajax({
                                    method: 'GET',
                                    url: `${reff.curl}/availlockerstorent`,
                                    contentType: 'application/json',
                                })
                                .fail((a, b, c) => {
                                    core.utils.confirmModal({
                                        bgColor: 'danger white-text',
                                        title: 'Loading Lockers Failure',
                                        message: a.responseText,
                                        yesButton: { text: 'OK' }
                                    });
                                })
                                .done(r => {
                                    $('div#form-alert-container').hide(0);
                                    $('#txtPassword').val('');
                                    
                                    let tgt = $('#selectLocker');

                                    let iHtml = '';
                                    (r !== undefined && r.length > 0) && r.forEach((v, i) => { iHtml += `<option value="${v.id}">${v.number}</option>`; });
                                    tgt.html(iHtml);

                                    $
                                        .ajax({
                                            method: 'GET',
                                            url: `${window.location.origin}/api/customers`,
                                            contentType: 'application/json',
                                            data: JSON.stringify({ limit: 1000000 })
                                        })
                                        .fail((a, b, c) => {
                                            core.utils.confirmModal({
                                                bgColor: 'danger white-text',
                                                title: 'Loading Customers Failure',
                                                message: a.responseText,
                                                yesButton: { text: 'OK' }
                                            });
                                        })
                                        .done(r => {
                                            let tgt = $('#selectCustomer');

                                            let iHtml = '';
                                            (r !== undefined && r.total > 0) && r.rows.forEach((v, i) => { iHtml += `<option value="${v.id}">${v.name}</option>`; });
                                            tgt.html(iHtml);

                                            reff.dom.newRentModal.show();
                                        });
                                });
                        }
                    }
                };
            },
            grid: {
                formatter: {
                    email: (v, r) => `<a href="mailto:${v.toLowerCase()}">${v}</a>`,
                    date: (v, r, i) => new Intl.DateTimeFormat('id-ID', { dateStyle: 'long' }).format(core.utils.toDate(r.rentDate)),
                    money: (v, r) => core.utils.IDRFormatter(v),
                    totalDays: (v, r, i) => {
                        let diff = (core.utils.diffDays(new Date(), core.utils.toDate(r.rentDate))) - 1;
                        return diff >= 0 ? `${diff} Days` : '';
                    },
                    actions: (v, r, i) => `<a href="javascript:void(0)" class="icon-row-edit btn btn-sm btn-outline-primary" data-row-id="${r.id}" title="Edit"><i class="bi bi-dropbox"></i></a>`,
                    footer: {
                        label: () => 'Total',
                        rented: data => data.reduce((n, { totalActiveRent }) => n + totalActiveRent, 0),
                        returned: data => data.reduce((n, { totalReturned }) => n + totalReturned, 0),
                        trans: data => data.reduce((n, { totalRentTrans }) => n + totalRentTrans, 0),
                        fine: data => new Intl.NumberFormat('id-ID', {
                            style: 'currency',
                            currency: 'IDR'
                        }).format(data.reduce((n, { totalPaidFine }) => n + totalPaidFine, 0))
                    },
                    details: (i, r, e) => {
                        let tblId = `tbl_for_row_${r.customerId}`;

                        $(e).html($('#detailTable').clone(true).attr('id', tblId).show());
                        $(`#${tblId}`).bootstrapTable({
                            classes: 'table table-bordered table-hover table-striped table-sm',
                            url: `${reff.curl}/${r.customerId}`,
                            columns: [
                                {
                                    field: 'id',
                                    title: 'ID',
                                    visible: false,
                                },
                                {
                                    field: 'lockerId',
                                    title: 'Locker ID',
                                    visible: false,
                                },
                                {
                                    field: 'number',
                                    title: 'Locker Number',
                                },
                                {
                                    field: 'rentDate',
                                    title: 'Rent Date',
                                    formatter: lawencon.ui.grid.formatter.date
                                },
                                {
                                    title: 'Total Days',
                                    width: 200,
                                    formatter: lawencon.ui.grid.formatter.totalDays
                                },
                                {
                                    title: 'Actions',
                                    width: 200,
                                    align: 'center',
                                    formatter: lawencon.ui.grid.formatter.actions,
                                    events: lawencon.events.grid.actions
                                }
                            ]
                        });
                    }
                },
                style: {
                    rented: (v, r, i) => {
                        if (r.totalActiveRent >= 3) return { css: { "background-color": '#f8d7da' } };
                        if (r.totalActiveRent >= 2) return { css: { "background-color": '#ffe69c' } };

                        return { css: { "background-color": '#a6e9d5' } };
                    }
                }
            },
            modal: {
                editor: () => {
                    reff.dom.modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('editorModal'));
                    reff.dom.newRentModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('rentNewModal'));
                }
            }
        },
        events: {
            windoc: {
                ready: () => $(d).ready(() => {
                    (a => core.utils.activator.c(a))({
                        a: () => core.ui.modal.editor(),
                        b: () => core.events.form.submitUnlock(),
                        c: () => core.events.form.submitNew()
                    })
                })
            },
            grid: {
                actions: {
                    'click .icon-row-edit': (e, v, r, i) => {
                        let days = core.ui.grid.formatter.totalDays(null, r, null);
                        let iDay = parseInt(days);

                        let calcFine = ({ ttlDay, isForgotPassword }) => (ttlDay < 2 ? 0 : (ttlDay - 1) * 5000) + (!isForgotPassword ? 0 : 25000);
                        let alertMsg = fine => 10000 >= fine
                            ? (
                                0 == 10000 - fine
                                    ? `The deposit money is <strong>non-refundable</strong> because it has been used entirely for payment of fines.`
                                    : `Return the deposit of <strong>${core.utils.IDRFormatter(10000 - fine)}</strong> to the customer.`
                            )
                            : `Charge an additional fee of <strong>${core.utils.IDRFormatter(fine - 10000)}</strong> as a fine.`;

                        core.utils.setFormOpenLocker({
                            id: r.id,
                            number: r.number,
                            days: days,
                            fine: core.utils.IDRFormatter(calcFine({ ttlDay: iDay, isForgotPassword: false })),
                            cb: () => {
                                reff.dom.modal.show();
                                core.utils.showFormAlert({ text: alertMsg(calcFine({ ttlDay: iDay, isForgotPassword: false })), color: 'success' });

                                $('input#chkForgotPassword')
                                    .off('change')
                                    .on('change', function () {
                                        let txtPwd = $('input#txtPassword');
                                        txtPwd.val('');
                                        this.checked ? txtPwd.attr('readonly', 'readonly') : txtPwd.removeAttr('readonly');

                                        let calculatedFine = calcFine({ ttlDay: iDay, isForgotPassword: this.checked });
                                        $('input#txtTotalFine').val(core.utils.IDRFormatter(calculatedFine));
                                        core.utils.showFormAlert({ text: alertMsg(calculatedFine), color: 'success' });
                                    });
                            }
                        });
                    }
                }
            },
            form: {
                submitUnlock: () => $('form#editorForm').submit(e => {
                    e.preventDefault();

                    let cData = {
                        id: $('#txtId').val(),
                        password: $('#txtPassword').val(),
                        totalFine: parseInt($('#txtTotalFine').val().replace(/[^0-9]+/g, "")),
                        isForgotPassword: $('#chkForgotPassword').prop('checked')
                    };

                    if (!cData.isForgotPassword && cData.password === '') {
                        core.utils.showFormAlert({ text: 'Please fill password field, or tick on Forgot password field.', color: 'danger', autoHide: true });
                        return;
                    }

                    $
                        .ajax({
                            method: 'PUT',
                            url: reff.curl,
                            contentType: 'application/json',
                            data: JSON.stringify(cData)
                        })
                        .fail((a, b, c) => core.utils.showFormAlert({ text: a.responseText, color: 'danger', autoHide: true }))
                        .done(r => {
                            $('#table').bootstrapTable('refresh');
                            reff.dom.modal.hide();
                        });
                }),
                submitNew: () => $('form#RentNewForm').submit(e => {
                    e.preventDefault();

                    let cData = {
                        customerId: $('#selectCustomer option:selected').val(),
                        lockerId: $('#selectLocker option:selected').val()
                    };

                    $
                        .ajax({
                            method: 'POST',
                            url: reff.curl,
                            contentType: 'application/json',
                            data: JSON.stringify(cData)
                        })
                        .fail((a, b, c) => core.utils.showFormAlert({ text: a.responseText, color: 'danger', autoHide: true }))
                        .done(r => {
                            $('#table').bootstrapTable('refresh');
                            reff.dom.newRentModal.hide();
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
            isValidData: data => Array.isArray(data) ?
                data.length > 0 : data && typeof data === 'object' ?
                    !(Object.keys(data).length === 0 && data.constructor === Object) : data !== undefined && data !== null && data !== '',
            reEvent: ({ target, event, func }) => {
                const ivd = core.utils.isValidData;

                ivd(target) && ivd(event) && ivd(func) && typeof func === 'function' && (() => {
                    target = $(target);
                    target.off(event).on(event, func);
                })();
            },
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
            setFormOpenLocker: ({ id, number, days, fine, cb }) => {
                $('input#txtId').val(id);
                $('input#txtLockerNumber').val(number);
                $('input#txtTotalDays').val(days);
                $('input#txtTotalFine').val(fine);

                $('input#txtPassword').removeAttr('readonly');
                $('input#chkForgotPassword').prop('checked', false);

                cb && cb();
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
            showFormAlert: ({ text, color = 'danger', autoHide = false, cb }) => {
                $('div#form-alert-container')
                    .html(`<div class="alert alert-${color} form-alert" role="alert">${text}</div>`)
                    .fadeOut(50)
                    .fadeIn(750);

                autoHide && setTimeout(() => { $('div#form-alert-container').fadeOut(750); }, 5000);
                cb && cb();
            },
            toDate: dateStr => {
                const [year, month, day] = dateStr.split("-")
                return new Date(year, month - 1, day)
            },
            diffDays: (date_1, date_2) => Math.ceil((date_1.getTime() - date_2.getTime()) / (1000 * 3600 * 24)),
            IDRFormatter: v => new Intl.NumberFormat('id-ID', { style: 'currency', currency: 'IDR', maximumFractionDigits: 0, minimumFractionDigits: 0 }).format(v)
        }
    });
})(
    null,
    window,
    document,
    {
        debugMode: !0,
        registrarNamespace: 'lawencon',
        curl: `${window.location.origin}/api/rentals`,
        dom: {
            modal: null,
            newRentModal: null
        }
    }
);