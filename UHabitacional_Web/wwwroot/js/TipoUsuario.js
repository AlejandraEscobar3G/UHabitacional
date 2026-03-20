// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $(".tipousuario-create").on("click", function() {
        var id = $(this).data("id");
        $("#tipoUsuarioCreate").load("/TipoUsuario/Create", function() {
            $("#tipoUsuarioCreate").modal("show");
        });
    });

    $(".tipousuario-edit").on("click", function() {
        var id = $(this).data("id");
        $("#tipoUsuarioEdit").load("/TipoUsuario/Edit/" + id, function() {
            $("#tipoUsuarioEdit").modal("show");
        });
    });

    $(".tipousuario-remove").on("click", function() {
        var id = $(this).data("id");
        $("#tipoUsuarioRemove").load("/TipoUsuario/Delete/" + id, function() {
            $("#tipoUsuarioRemove").modal("show");
        });
    });
});
