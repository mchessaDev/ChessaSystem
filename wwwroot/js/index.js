import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

// Função global para o botão de exclusão
window.handleDeleteClick = (itemName, funcionarioId) => {
    console.log(`Tentando excluir o funcionário: ${itemName} - ID: ${funcionarioId}`);
    alert(`Confirmação de exclusão para ${itemName} com ID ${funcionarioId}`);
};

// Renderização do React App
const rootElement = document.getElementById('root');
if (rootElement) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(<App />); // Usando JSX
}
