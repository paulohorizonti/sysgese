
$(document).ready(function () {
   

});


function mostrarValorDoSelect() {
    // Obtém o elemento select pelo seu ID
    var selectElement = document.getElementById("idPerfil");

   // var opcaoTexto = selectElement.options[selectElement.selectedIndex].text;
    var opcaoValor = selectElement.options[selectElement.selectedIndex].value; //esse é o id que sera mandado para controller poder filtrar

    alert(opcaoTexto); // Ferrari
    alert(opcaoValor); // ferrari


}




