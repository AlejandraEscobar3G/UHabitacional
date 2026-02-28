// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $("#EdificioId").change(function() {
        var edificioId = $(this).val();
        $.getJSON('/Inquilino/GetDepartamentosByEdificio', { edificioId: edificioId }, function(data) {
            var deptoSelect = $("#DepartamentoId");
            deptoSelect.empty();
            deptoSelect.append($('<option>', {
                value: "",
                text: "Selecciona departamento",
                disabled: true,
                selected: true
            }));
            $.each(data, function(i, item) {
                deptoSelect.append($('<option>', {
                    value: item.id,
                    text: item.numeroInt
                }));
            });
        });
    });
});
