

$(document).ready(function () {

    // imprest xxxxxx
    $('#xxxxx').select2({
        theme: 'classic',
        placeholder: 'Select here.....',
        allowClear: false,
    });
    $('#xxxxx').on('select2:select', function (e) {
        __doPostBack('ddImprestCardNo', '');
    });


    // search - requisiton no
    $('#ddServiceName').select2({
        theme: 'classic',
        placeholder: 'Select here.....',
        allowClear: false,
    });



    // Reinitialize Select2 after UpdatePanel partial postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    // Reinitialize Select2 for all dropdowns
    prm.add_endRequest(function () {

        setTimeout(function () {

            // search - requisiton no
            $('#ddServiceName').select2({
                theme: 'classic',
                placeholder: 'Select here.....',
                allowClear: false,
            });

        }, 0);
    });


});