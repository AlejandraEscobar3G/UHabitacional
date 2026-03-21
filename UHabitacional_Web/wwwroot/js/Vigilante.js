// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $(".vigilante-create").on("click", function() {
        var id = $(this).data("id");
        $("#vigilanteCreate").load("/Vigilante/Create", function() {
            $("#vigilanteCreate").modal("show");
        });
    });

    $(".vigilante-edit").on("click", function() {
        var id = $(this).data("id");
        $("#vigilanteEdit").load("/Vigilante/Edit/" + id, function() {
            $("#vigilanteEdit").modal("show");
        });
    });

    $(".vigilante-remove").on("click", function() {
        var id = $(this).data("id");
        $("#vigilanteRemove").load("/Vigilante/Delete/" + id, function() {
            $("#vigilanteRemove").modal("show");
        });
    });
});
