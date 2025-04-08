import React, { useState } from 'react';
import ReactDOM from 'react-dom/client';
import ConfirmDeleteModal from './ConfirmDeleteModal';

const App: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState<string | null>(null);

    const handleDeleteClick = (itemName: string, funcionarioId: number) => {
        console.log(`Tentando excluir o funcionário: ${itemName} - ID: ${funcionarioId}`);
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

    (window as any).handleDeleteClick = handleDeleteClick; // 🔥 Expondo a função globalmente

    return (
        <div>
            <ConfirmDeleteModal
                itemName={itemToDelete || ''}
                isOpen={isModalOpen}
                onConfirm={confirmDelete}
                onCancel={cancelDelete}
            />
        </div>
    );
};

const rootElement = document.getElementById('root');
if (rootElement) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(<App />);
}
