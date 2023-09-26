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
                "showDuration": "4000",
                "hideDuration": "4000",
                "timeOut": "4000",
                "extendedTimeOut": "4000",
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

    $("#modal").load("/" + controller + "/Details/" + id, function () {
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


function chamarEdit(id) {
    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/" + controller + "/Edit/" + id, function () {
        $('#modal').modal("show");

    });
}

function maiuscula(z) {
    v = z.value.toUpperCase();
    z.value = v;
}

//mascaras
$(document).ready(function () {
    $('.date').mask('99/99/9999');
    $('.time').mask('00:00:00');
    $('#cep').mask('99.999-999');

    $('.phone').mask('9999-9999');
    $('#cnpj').mask('99.999.999/9999-99');
    $('#telefone').mask('(99)99999-9999');
    $(".senha").mask("xxxxxxxxx");
    $("#cest").mask("99.999.99");
    $("#ProcuraNCM").mask("9999.99.99");
    $("#ncm").mask("9999.99.99");
    $(".decimal").mask("9999,999");
    $(".pr-aliq").mask("9999.999");
});

jQuery(function ($) {
    $("#campoData").mask("99/99/9999");
    $("#campoTelefone").mask("(999) 999-9999");

});