

//Mostrar notificación mediante el componente Toastr
function ShowNotificacion(tipo, titulo, mensaje, fadeOutTimeout, appearTimeout) {

    setTimeout(function () {
        var shortCutFunction = tipo; // success/info/warning/error
        var title = titulo || '';
        var msg = mensaje;
        var $showDuration = '3000';
        var $hideDuration = '3000';
        var $timeOut = (parseInt(fadeOutTimeout) * 1000).toString();
        var $extendedTimeOut = '1000';
        var $showEasing = 'swing';
        var $hideEasing = 'linear';
        var $showMethod = 'fadeIn';
        var $hideMethod = 'fadeOut';
        var toastIndex = 1;

        toastr.options = {
            closeButton: true,
            debug: false,
            positionClass: 'toast-top-right',
            onclick: null
        };

        if ($showDuration.length) {
            toastr.options.showDuration = $showDuration;
        }

        if ($hideDuration.length) {
            toastr.options.hideDuration = $hideDuration;
        }

        if ($timeOut.length) {
            toastr.options.timeOut = $timeOut;
        }

        if ($extendedTimeOut.length) {
            toastr.options.extendedTimeOut = $extendedTimeOut;
        }

        if ($showEasing.length) {
            toastr.options.showEasing = $showEasing;
        }

        if ($hideEasing.length) {
            toastr.options.hideEasing = $hideEasing;
        }

        if ($showMethod.length) {
            toastr.options.showMethod = $showMethod;
        }

        if ($hideMethod.length) {
            toastr.options.hideMethod = $hideMethod;
        }

        if (!msg) {
            msg = getMessage();
        }


        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;
        if ($toast.find('#okBtn').length) {
            $toast.delegate('#okBtn', 'click', function () {
                alert('you clicked me. i was toast #' + toastIndex + '. goodbye!');
                $toast.remove();
            });
        }
        if ($toast.find('#surpriseBtn').length) {
            $toast.delegate('#surpriseBtn', 'click', function () {
                alert('Surprise! you clicked me. i was toast #' + toastIndex + '. You could perform an action here.');
            });
        }
    }, parseInt(appearTimeout));
}

//Mostrar mensaje de confirmación bootbox
function ShowConfirmation(mensaje, link) {
    bootbox.confirm({
        message: mensaje,
        buttons: {
            confirm: {
                label: 'Sí',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                document.forms[0].action = link;
                document.forms[0].submit();
            }
        }
    });

}
