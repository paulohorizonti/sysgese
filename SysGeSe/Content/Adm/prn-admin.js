$(document).ready(function () {
    $(function () {
        var displayMessage = function (message, msgType) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "10000",
                "hideDuration": "10000",
                "timeOut": "10000",
                "extendedTimeOut": "10000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr[msgType](message);
        };

        if ($('#success').val()) {
            displayMessage($('#success').val(), 'success');
        }
        if ($('#info').val()) {
            displayMessage($('#info').val(), 'info');
        }
        if ($('#warning').val()) {
            displayMessage($('#warning').val(), 'warning');
        }
        if ($('#error').val()) {
            displayMessage($('#error').val(), 'error');
        }
    });

});



//$(function () {
//    if ('@ViewBag.HasErrors') {
//        $('#modal').modal("show");
//    }

//});

function chamarDetails(id) {

    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/"+controller+"/Details/" + id, function () {
        $('#modal').modal("show");
    });
}



function chamarDelete(id) {

    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/" + controller + "/Delete/" + id, function () {
        $('#modal').modal("show");
    });
}


function chamarIncluir() {

    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/" + controller + "/Incluir/", function () {
        $('#modal').modal("show");
    });
}



function fecharModal() {
    $('#modal').modal("hide");
}


function chamarEdit(id, condicao) {
    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/" +controller+"/Edit/" + id, function () {
        $('#modal').modal("show");

        if (condicao == false)
            $('#detailsBack').hide();
        if (condicao == true)
            $('#indexBack').hide();
    });
}

function maiuscula(z) {
    v = z.value.toUpperCase();
    z.value = v;
}