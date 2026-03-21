// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $(".departamento-create").on("click", function() {
        var id = $(this).data("id");
        $("#departamentoCreate").load("/Departamento/Create", function() {
            $("#departamentoCreate").modal("show");
        });
    });

    $(".departamento-edit").on("click", function() {
        var id = $(this).data("id");
        $("#departamentoEdit").load("/Departamento/Edit/" + id, function() {
            $("#departamentoEdit").modal("show");
        });
    });
});
