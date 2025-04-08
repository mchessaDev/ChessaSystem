import React from 'react';
const ConfirmDeleteModal = ({ itemName, onConfirm, onCancel, isOpen }) => {
    if (!isOpen)
        return null;
    return (React.createElement("div", { className: "fixed inset-0 bg-gray-800 bg-opacity-50 flex items-center justify-center z-50" },
        React.createElement("div", { className: "bg-white p-6 rounded-lg shadow-lg w-96" },
            React.createElement("h2", { className: "text-xl font-bold mb-4" }, "Confirma\u00E7\u00E3o de Exclus\u00E3o"),
            React.createElement("p", null,
                "Tem certeza que deseja excluir ",
                React.createElement("strong", null, itemName),
                "?"),
            React.createElement("div", { className: "flex justify-end mt-6" },
                React.createElement("button", { className: "bg-gray-300 text-gray-700 px-4 py-2 rounded mr-2", onClick: onCancel }, "Cancelar"),
                React.createElement("button", { className: "bg-red-500 text-white px-4 py-2 rounded", onClick: onConfirm }, "Excluir")))));
};
export default ConfirmDeleteModal;
