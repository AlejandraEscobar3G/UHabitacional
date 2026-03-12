// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $(".identificacion-create").on("click", function() {
        var id = $(this).data("id");
        $("#identificacionCreate").load("/Identificacion/Create", function() {
            $("#identificacionCreate").modal("show");
        });
    });

    $(".identificacion-edit").on("click", function() {
        var id = $(this).data("id");
        $("#identificacionEdit").load("/Identificacion/Edit/" + id, function() {
            $("#identificacionEdit").modal("show");
        });
    });

    $(".identificacion-remove").on("click", function() {
        var id = $(this).data("id");
        $("#identificacionRemove").load("/Identificacion/Delete/" + id, function() {
            $("#identificacionRemove").modal("show");
        });
    });
});
