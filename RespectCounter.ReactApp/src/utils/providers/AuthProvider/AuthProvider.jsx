import { createContext, useContext, useState, useEffect, useCallback, useRef } from 'react';
import LoginPopup from './LoginPopup/LoginPopup';
import * as authService from '../../../services/authService';

const AuthContext = createContext();
let logoutHandler = null;

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const [showLoginPopup, setShowLoginPopup] = useState(false);
    const isCheckingAuth = useRef(false);
    
    const openLoginPopup = useCallback(() => setShowLoginPopup(true), []);
    const closeLoginPopup = useCallback(() => setShowLoginPopup(false), []);

    const handleLogin = useCallback(async (formData) => {
        await authService.login(formData);
        const user = await authService.getClaims();
        localStorage.setItem("user", JSON.stringify(user.data));
        setUser(user.data);
        setIsLoggedIn(true);
    }, []);

    const handleLogout = useCallback(async () => {
        await authService.logout();
        setIsLoggedIn(false);
        setUser(null);
        localStorage.removeItem("user");
    }, []);

    const checkAuthStatus = useCallback(async () => {
        return await authService.getClaims();
    }, []);

    useEffect(() => {
        logoutHandler = handleLogout;
    }, [handleLogout]);

    useEffect(() => {
        console.log("AuthProvider - user changed:", user);
    }, [user]);

    useEffect(() => {
        const checkAuth = async () => {
            isCheckingAuth.current = true;
            const storedUserInfo = JSON.parse(localStorage.getItem("user"));
            if (storedUserInfo) {
                console.log("AuthProvider: stored user found. Checking user's claims.")
                const userInfo = await authService.getClaims();
                console.log("userInfo:", userInfo);
                if (userInfo?.data?.isAuthenticated) {
                    setIsLoggedIn(true);
                    setUser(userInfo.data);
                    localStorage.setItem("user", JSON.stringify(userInfo.data));
                } else {
                    setIsLoggedIn(false);
                    setUser(null);
                    localStorage.removeItem("user");
                }
            }
            isCheckingAuth.current = false;
        };
        if(isCheckingAuth.current === false) {
            checkAuth();
        }
    }, []);

    return (
        <AuthContext.Provider value={{ isLoggedIn, user, login: handleLogin, logout: handleLogout, openLoginPopup, closeLoginPopup, checkIfLogged: checkAuthStatus }}>
            {children}
            {showLoginPopup && <LoginPopup/>}
        </AuthContext.Provider>
    );
};

export const callHandleLogout = () => { if (logoutHandler) logoutHandler(); };
export const useAuth = () => useContext(AuthContext);