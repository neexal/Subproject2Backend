import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavBar from './components/NavBar';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import ChangePassword from './pages/ChangePassword';
import Search from './pages/Search';
import MovieDetail from './pages/MovieDetail';
import PersonDetail from './pages/PersonDetail';
import History from './pages/History';
import { useAuth } from './context/AuthContext';
import { ToastProvider } from './context/ToastContext';

const PrivateRoute = ({ children }) => {
  const { user, loading } = useAuth();
  if (loading) return null;
  return user ? children : <Navigate to="/login" />;
};

function App() {
  return (
    <ToastProvider>
      <Router>
        <div className="d-flex flex-column min-vh-100">
          <NavBar />
          <div className="flex-grow-1" style={{ paddingTop: 'var(--nav-height)' }}>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/search" element={<Search />} />
              <Route path="/movies/:id" element={<MovieDetail />} />
              <Route path="/persons/:id" element={<PersonDetail />} />
              <Route
                path="/history"
                element={
                  <PrivateRoute>
                    <History />
                  </PrivateRoute>
                }
              />
              <Route
                path="/change-password"
                element={
                  <PrivateRoute>
                    <ChangePassword />
                  </PrivateRoute>
                }
              />
            </Routes>
          </div>
          <footer className="bg-dark text-white text-center py-3 mt-auto">
            <small>&copy; 2025 Movie App Portfolio Project</small>
          </footer>
        </div>
      </Router>
    </ToastProvider>
  );
}

export default App;
