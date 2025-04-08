import React, { FC } from 'react';

interface ConfirmDeleteModalProps {
    itemName: string;
    onConfirm: () => void;
    onCancel: () => void;
    isOpen: boolean;
}

const ConfirmDeleteModal: FC<ConfirmDeleteModalProps> = ({ itemName, onConfirm, onCancel, isOpen }) => {
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 bg-gray-800 bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <h2 className="text-xl font-bold mb-4">Confirmação de Exclusão</h2>
                <p>Tem certeza que deseja excluir <strong>{itemName}</strong>?</p>
                <div className="flex justify-end mt-6">
                    <button
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded mr-2"
                        onClick={onCancel}
                    >
                        Cancelar
                    </button>
                    <button
                        className="bg-red-500 text-white px-4 py-2 rounded"
                        onClick={onConfirm}
                    >
                        Excluir
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmDeleteModal;
