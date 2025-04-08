import React, { useState } from 'react';
import ConfirmDeleteModal from './ConfirmDeleteModal';
const App = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState(null);
    const handleDeleteClick = (itemName) => {
        setItemToDelete(itemName);
        setIsModalOpen(true);
    };
    const confirmDelete = () => {
        console.log(`✅ ${itemToDelete} foi excluído com sucesso!`);
        setIsModalOpen(false);
        setItemToDelete(null);
    };
    const cancelDelete = () => {
        setIsModalOpen(false);
        setItemToDelete(null);
    };
    // 🔥 Tornar a função disponível globalmente
    window.handleDeleteClick = handleDeleteClick;
    return (React.createElement("div", null,
        React.createElement(ConfirmDeleteModal, { itemName: itemToDelete || '', isOpen: isModalOpen, onConfirm: confirmDelete, onCancel: cancelDelete })));
};
export default App;
