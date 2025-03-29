document.addEventListener('DOMContentLoaded', function () {
    const campoBusca = document.getElementById('campoBusca');
    const clearIcon = document.querySelector('.clear-icon');

    if (campoBusca && clearIcon) {
        function toggleClearIcon() {
            clearIcon.style.display = campoBusca.value ? 'block' : 'none';
        }

        campoBusca.addEventListener('input', toggleClearIcon);

        clearIcon.addEventListener('click', function () {
            campoBusca.value = '';
            campoBusca.focus();
            toggleClearIcon();
        });

        toggleClearIcon();
    }
});

// 🧭 Ordenação de colunas
function sortTable(column) {
    const urlParams = new URLSearchParams(window.location.search);
    const currentSortOrder = urlParams.get("sortOrder") === "asc" ? "desc" : "asc";
    urlParams.set("sortColumn", column);
    urlParams.set("sortOrder", currentSortOrder);

    const icon = document.getElementById('sort' + column);
    if (icon) {
        icon.className = currentSortOrder === "asc" ? "fas fa-sort-up" : "fas fa-sort-down";
    }

    window.location.search = urlParams.toString();
}

// 🧾 Modal de exclusão (Bootstrap)
document.addEventListener("DOMContentLoaded", function () {
    const modalExcluir = document.getElementById('confirmarExclusaoModal');

    if (modalExcluir) {
        modalExcluir.addEventListener('show.bs.modal', function (event) {
            const botao = event.relatedTarget;
            const funcionarioId = botao.getAttribute('data-id');
            const nomeFuncionario = botao.getAttribute('data-nome');

            const form = document.getElementById("formExcluirFuncionario");
            const inputHidden = document.getElementById("funcionarioIdParaExcluir");
            const spanNome = document.getElementById('nomeFuncionario');

            if (form && inputHidden) {
                // A ação está definida corretamente aqui
                form.action = "/Funcionario/Excluir"; // SEM a variável ID na URL
                inputHidden.value = funcionarioId; // Preenche o campo hidden com o ID
            }

            if (spanNome) {
                spanNome.innerText = nomeFuncionario;
            }

            console.log("🧠 ID do funcionário definido para exclusão:", funcionarioId);
        });
    }
});



// 🌑 Dark mode
function toggleDarkMode() {
    document.body.classList.toggle("dark-mode");
}

// ⏳ Esconde alertas após 5 segundos
$(document).ready(function () {
    setTimeout(function () {
        $(".alert").fadeOut("slow");
    }, 5000);
});

// Aguarda a DOM carregar completamente
document.addEventListener("DOMContentLoaded", function () {
    const alertaSucesso = document.getElementById("alertSucesso");

    // Verifica se o alerta de sucesso está presente na página
    if (alertaSucesso) {
        // Define um tempo para ocultar o alerta
        setTimeout(function () {
            alertaSucesso.style.display = "none"; // Esconde o alerta após 5 segundos
        }, 5000); // 5000ms = 5 segundos
    }
});