import React from 'react';
import { Navbar, Nav, Container, Button } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const NavBar = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <Navbar expand="lg" className="fixed-top py-3">
            <Container>
                <Navbar.Brand as={Link} to="/" className="fw-bold text-uppercase tracking-wider text-gradient fs-4">
                    Movie App
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" className="border-0" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="mx-auto gap-4">
                        <Nav.Link as={Link} to="/" className="text-white opacity-75 hover-opacity-100 transition">Home</Nav.Link>
                        <Nav.Link as={Link} to="/search" className="text-white opacity-75 hover-opacity-100 transition">Search</Nav.Link>
                        {user && <Nav.Link as={Link} to="/history" className="text-white opacity-75 hover-opacity-100 transition">My History</Nav.Link>}
                    </Nav>
                    <Nav className="align-items-center gap-3">
                        {user ? (
                            <>
                                <Navbar.Text className="text-white small opacity-75">
                                    Hello, <span className="fw-bold text-white">{user.username}</span>
                                </Navbar.Text>
                                <Button as={Link} to="/change-password" variant="outline-light" size="sm" className="rounded-pill px-3">Change Password</Button>
                                <Button variant="primary" size="sm" onClick={handleLogout} className="rounded-pill px-4">Logout</Button>
                            </>
                        ) : (
                            <>
                                <Nav.Link as={Link} to="/login" className="text-white opacity-75 hover-opacity-100">Login</Nav.Link>
                                <Button as={Link} to="/register" variant="primary" size="sm" className="rounded-pill px-4">Register</Button>
                            </>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default NavBar;
