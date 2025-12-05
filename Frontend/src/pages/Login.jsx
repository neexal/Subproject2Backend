import React, { useState } from 'react';
import { Container, Form, Button, Card, Alert } from 'react-bootstrap';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await login(email, password);
            navigate('/');
        } catch (err) {
            setError('Failed to login. Please check your credentials.');
        }
    };

    return (

        <Container className="d-flex justify-content-center align-items-center fade-in" style={{ minHeight: '80vh' }}>
            <div className="glass-panel p-5 shadow-lg" style={{ width: '100%', maxWidth: '450px' }}>
                <h2 className="text-center mb-4 text-gradient fw-bold">Welcome Back</h2>
                {error && <Alert variant="danger" className="mb-4">{error}</Alert>}
                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-4" controlId="email">
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

                    <Form.Group className="mb-5" controlId="password">
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

                    <Button variant="primary" type="submit" className="w-100 rounded-pill fw-bold py-2 mb-4">
                        Login
                    </Button>
                </Form>
                <div className="text-center">
                    <span className="text-secondary">Don't have an account? </span>
                    <Link to="/register" className="fw-bold text-info hover-underline">Register</Link>
                </div>
            </div>
        </Container>
    );

};

export default Login;
