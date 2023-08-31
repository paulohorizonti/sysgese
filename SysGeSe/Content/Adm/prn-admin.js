
function chamarDetails(id) {

    var cont = document.querySelector(".prn-titulo"); //pega o titulo

    var controller = cont.innerText;

    $("#modal").load("/"+controller+"/Details/" + id, function () {
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