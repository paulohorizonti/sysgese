
$(document).ready(function () {
   

});


function mostrarValorDoSelect() {
    // Obtém o elemento select pelo seu ID
    var selectElement = document.getElementById("idPerfil");

  
    var opcaoValor = selectElement.options[selectElement.selectedIndex].value; //esse é o id que sera mandado para controller poder filtrar

     
    $.ajax({
       
        /* url: '/Acesso/FiltrarPerfilJson?id=' + opcaoValor,*/
        url: '/Acesso/FiltrarPerfilJson',
        type: "POST",
        data: { id: opcaoValor },
        success: function (data) {

            //limpar drowpdow
            $('#idTabela').empty();
            document.getElementById('idTabela').disabled = false;
            //Preecher a segunda dropdawn
            $.each(data, function (index, item) {
                $('#idTabela').append($('<option>', {
                    value: item.Value,
                    text: item.Text

                }));
            });

        }
        
    });

}
//$(document).ready(function () {
//    // Obtém o elemento select pelo seu ID
//    var selectElement = document.getElementById("idPerfil");

//    // var opcaoTexto = selectElement.options[selectElement.selectedIndex].text;
//    var opcaoValor = selectElement.options[selectElement.selectedIndex].value; //esse é o id que sera mandado para controller poder filtrar

//    // alert(opcaoTexto); // Ferrari
//    alert(opcaoValor); // Aqui eu tenho o ID do perfil qu eeu preciso pra puxar os nomes das tabelas
//    $.ajax({
//        data: { opcaoValor: opcaoValor },
//        url: '/Acesso/FiltrarPerfilJson?id=' + opcaoValor,
//        types: "GET",
//        dataType: 'json',
//        success: function (data) {

//            var dropDown = $('#idTabela');
//            dropDown.empty();//limpa a select

//            $.each(data, function (key, item) {
//                dropDown.append($('<option></option>').val(item.Value).text(item.Text));

//            });

//        },
//        error: function (error) {
//            alert(error);
//        }

//    });

//});




