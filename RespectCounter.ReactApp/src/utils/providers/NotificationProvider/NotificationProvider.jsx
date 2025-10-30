import React, { createContext, useContext, useState, useCallback } from "react";
import NotificationList from "./NotificationList/NotificationList"

const NotificationContext = createContext();

export const useNotification = () => useContext(NotificationContext);

export const NotificationProvider = ({ children }) => {
    const [notifications, setNotifications] = useState([]);

    const notify = useCallback(({ type = "info", message }) => {
        const id = Date.now() + Math.random();
        const createdAt = new Date().toLocaleString();
        setNotifications(prev => [...prev, { id, type, createdAt, message }]);
        
        // Auto-remove after 8s
        setTimeout(() => {
            setNotifications(prev => prev.filter(n => n.id !== id));
        }, 8000);
    }, []);

    const removeNotification = useCallback((id) => {
        setNotifications(prev => prev.filter(n => n.id !== id));
    }, []);

    return (
        <NotificationContext.Provider value={{ notify }}>
            {children}
            <NotificationList list={notifications} removeNotification={removeNotification}/>
        </NotificationContext.Provider>
    );
};