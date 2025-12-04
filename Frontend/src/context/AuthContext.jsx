import React, { createContext, useState, useEffect, useContext } from 'react';
import { authService } from '../services/api';
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const decoded = jwtDecode(token);
                // Check if token is expired
                if (decoded.exp * 1000 < Date.now()) {
                    logout();
                } else {
                    // Assuming the token has nameid (userId) and unique_name (username)
                    setUser({
                        id: parseInt(decoded.nameid || decoded.sub), // Adjust based on your token claims
                        username: decoded.unique_name || decoded.name,
                        email: decoded.email
                    });
                }
            } catch (error) {
                console.error("Invalid token:", error);
                logout();
            }
        }
        setLoading(false);
    }, []);

    const login = async (email, password) => {
        try {
            const response = await authService.login(email, password);
            const { token, username, userId } = response.data; // Adjust based on your AuthController response

            localStorage.setItem('token', token);
            setUser({ id: userId, username, email });
            return true;
        } catch (error) {
            console.error("Login failed:", error);
            throw error;
        }
    };

    const register = async (username, email, password) => {
        try {
            await authService.register(username, email, password);
            return true;
        } catch (error) {
            console.error("Registration failed:", error);
            throw error;
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
    };

    const value = {
        user,
        login,
        register,
        logout,
        loading
    };

    return (
        <AuthContext.Provider value={value}>
            {!loading && children}
        </AuthContext.Provider>
    );
};
