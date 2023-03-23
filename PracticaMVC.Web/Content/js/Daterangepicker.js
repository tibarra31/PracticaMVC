//Asigna el control daterangepicker a todos los controles que tengan en su id "txtFecha"
$(function () {
    $('input[id*="TxtFechas"]').daterangepicker({
        "locale": {
            "format": "DD/MMMM/YYYY",
            "separator": " - ",
            "applyLabel": "APLICAR",
            "cancelLabel": "CANCELAR",
            "fromLabel": "De",
            "toLabel": "A",
            "customRangeLabel": "Personalizar...",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        },
        opens: 'center',
        autoUpdateInput: false,
        showDropdowns: true
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('DD-MMMM-YYYY') + ' to ' + end.format('DD-MMMM-YYYY'));
    });

    $('input[id*="TxtFechas"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MMMM/YYYY') + ' - ' + picker.endDate.format('DD/MMMM/YYYY'));
    });

    $('input[id*="TxtFechas"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });

});