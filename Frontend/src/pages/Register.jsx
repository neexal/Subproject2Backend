import React, { useState } from 'react';
import { Container, Form, Button, Card, Alert } from 'react-bootstrap';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Register = () => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const { register } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (password !== confirmPassword) {
            return setError('Passwords do not match');
        }

        try {
            await register(username, email, password);
            navigate('/login');
        } catch (err) {
            setError('Failed to register. Please try again.');
        }
    };

    return (

        <Container className="d-flex justify-content-center align-items-center fade-in" style={{ minHeight: '80vh' }}>
            <div className="glass-panel p-5 shadow-lg" style={{ width: '100%', maxWidth: '500px' }}>
                <h2 className="text-center mb-4 text-gradient fw-bold">Create Account</h2>
                {error && <Alert variant="danger" className="mb-4">{error}</Alert>}
                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-3" controlId="username">
                        <Form.Label className="text-secondary small text-uppercase">Username</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Enter username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="email">
                        <Form.Label className="text-secondary small text-uppercase">Email address</Form.Label>
                        <Form.Control
                            type="email"
                            placeholder="Enter email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="password">
                        <Form.Label className="text-secondary small text-uppercase">Password</Form.Label>
                        <Form.Control
                            type="password"
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Form.Group className="mb-5" controlId="confirmPassword">
                        <Form.Label className="text-secondary small text-uppercase">Confirm Password</Form.Label>
                        <Form.Control
                            type="password"
                            placeholder="Confirm Password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Button variant="primary" type="submit" className="w-100 rounded-pill fw-bold py-2 mb-4">
                        Register
                    </Button>
                </Form>
                <div className="text-center">
                    <span className="text-secondary">Already have an account? </span>
                    <Link to="/login" className="fw-bold text-info hover-underline">Login</Link>
                </div>
            </div>
        </Container>
    );

};

export default Register;
