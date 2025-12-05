import React, { createContext, useContext, useState, useCallback } from 'react';
import { Toast, ToastContainer } from 'react-bootstrap';

const ToastContext = createContext();

export const useToast = () => useContext(ToastContext);

export const ToastProvider = ({ children }) => {
    const [toasts, setToasts] = useState([]);

    const addToast = useCallback((message, variant = 'success') => {
        const id = Date.now();
        setToasts(prev => [...prev, { id, message, variant }]);
    }, []);

    const removeToast = useCallback((id) => {
        setToasts(prev => prev.filter(t => t.id !== id));
    }, []);

    return (
        <ToastContext.Provider value={{ addToast }}>
            {children}
            <ToastContainer position="top-end" className="p-3" style={{ zIndex: 99999, position: 'fixed', top: '20px', right: '20px' }}>
                {toasts.map(toast => (
                    <Toast
                        key={toast.id}
                        onClose={() => removeToast(toast.id)}
                        show={true}
                        delay={3000}
                        autohide
                        bg={toast.variant}
                    >
                        <Toast.Header closeButton={true}>
                            <strong className="me-auto">{toast.variant === 'danger' ? 'Error' : 'Notification'}</strong>
                        </Toast.Header>
                        <Toast.Body className={toast.variant === 'dark' ? 'text-white' : ''}>
                            {toast.message}
                        </Toast.Body>
                    </Toast>
                ))}
            </ToastContainer>
        </ToastContext.Provider>
    );
};
