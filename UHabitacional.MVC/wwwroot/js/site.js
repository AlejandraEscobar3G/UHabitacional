/* UHabitacional — script base */

// Confirmación genérica para botones de eliminar (data-confirm)
document.addEventListener('submit', e => {
  const form = e.target;
  const msg = form.dataset.confirm;
  if (msg && !confirm(msg)) {
    e.preventDefault();
    return false;
  }
});

// Mostrar reloj en vivo en .live-clock (si existe en la página)
function tickClock() {
  document.querySelectorAll('.live-clock').forEach(el => {
    el.textContent = new Date().toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
  });
}
setInterval(tickClock, 1000);
tickClock();
