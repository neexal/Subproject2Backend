import React, { useState } from 'react';
import { Container, Form, Button, Alert } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/api';
import { useToast } from '../context/ToastContext';

const ChangePassword = () => {
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const { addToast } = useToast();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (newPassword !== confirmPassword) {
            setError("New passwords don't match");
            return;
        }

        if (newPassword.length < 6) {
            setError("Password must be at least 6 characters long");
            return;
        }

        setLoading(true);
        try {
            await authService.changePassword(currentPassword, newPassword);
            addToast('Password changed successfully', 'success');
            navigate('/history'); // Redirect to dashboard
        } catch (err) {
            setError(err.response?.data?.Message || 'Failed to change password');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container className="d-flex justify-content-center align-items-center fade-in" style={{ minHeight: '80vh' }}>
            <div className="glass-panel p-5 shadow-lg" style={{ width: '100%', maxWidth: '500px' }}>
                <h2 className="text-center mb-4 text-gradient fw-bold">Change Password</h2>
                {error && <Alert variant="danger" className="mb-4">{error}</Alert>}

                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-4">
                        <Form.Label className="text-secondary small text-uppercase">Current Password</Form.Label>
                        <Form.Control
                            type="password"
                            value={currentPassword}
                            onChange={(e) => setCurrentPassword(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Form.Group className="mb-4">
                        <Form.Label className="text-secondary small text-uppercase">New Password</Form.Label>
                        <Form.Control
                            type="password"
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Form.Group className="mb-5">
                        <Form.Label className="text-secondary small text-uppercase">Confirm New Password</Form.Label>
                        <Form.Control
                            type="password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                            className="bg-dark border-secondary text-white py-2"
                        />
                    </Form.Group>

                    <Button
                        variant="primary"
                        type="submit"
                        disabled={loading}
                        className="w-100 rounded-pill fw-bold py-2"
                    >
                        {loading ? 'Updating...' : 'Update Password'}
                    </Button>
                </Form>
            </div>
        </Container>
    );
};

export default ChangePassword;
