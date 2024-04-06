

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
    $('#ddScRequisitionNo').select2({
        theme: 'classic',
        placeholder: 'Select here.....',
        allowClear: false,
    });




    // service name
    $('#ddServiceName').select2({
        theme: 'classic',
        placeholder: 'Select here.....',
        allowClear: false,
    });

    // uom
    $('#ddUOM').select2({
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
            $('#ddScRequisitionNo').select2({
                theme: 'classic',
                placeholder: 'Select here.....',
                allowClear: false,
            });



            // service name
            $('#ddServiceName').select2({
                theme: 'classic',
                placeholder: 'Select here.....',
                allowClear: false,
            });

            // uom
            $('#ddUOM').select2({
                theme: 'classic',
                placeholder: 'Select here.....',
                allowClear: false,
            });




        }, 0);
    });






    // datatables
    $("#gridSearchxx").prepend(

        $("<thead></thead>").append(
            $("#gridSearchxx").find("tr:first")
        )

    ).DataTable({

        scrollX: false,
        sScrollXInner: "100%",
        bFilter: true,
        bSort: true,
        bPaginate: true,

        scrollCollapse: false,
        paging: true,
        searching: true,
        ordering: true,
        info: true,
        lengthChange: true,
        responsive: true,

        pagingType: 'full_numbers',

        lengthMenu: [
            [5, 10, 20, 25, 50, -1],
            [5, 10, 20, 25, 50, "All"]
        ],

        search: {
            return: false
        },
        language: {
            search: "Search: ",
            decimal: ',',
            thousands: '.'
        },
        initComplete: function () {
            $('.dataTables_filter input').attr('placeholder', 'Search here......')
        },

    });

});