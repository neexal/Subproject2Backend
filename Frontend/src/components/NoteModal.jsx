import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { frameworkService } from '../services/api';
import { useToast } from '../context/ToastContext';

const NoteModal = ({ show, onHide, targetId, type, userId }) => {
    const [note, setNote] = useState('');
    const [loading, setLoading] = useState(false);
    const { addToast } = useToast();

    const handleSubmit = async () => {
        if (!note.trim()) return;

        setLoading(true);
        try {
            if (type === 'movie') {
                await frameworkService.addMovieNote(userId, targetId, note);
            } else {
                await frameworkService.addPersonNote(userId, targetId, note);
            }
            addToast('Note added successfully!', 'success');
            setNote('');
            onHide();
        } catch (err) {
            addToast('Failed to add note', 'danger');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Modal show={show} onHide={onHide} centered contentClassName="glass-panel text-white">
            <Modal.Header closeButton closeVariant="white" className="border-secondary">
                <Modal.Title>Add Note</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form.Group>
                    <Form.Label className="text-secondary small">Your Note</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={3}
                        value={note}
                        onChange={(e) => setNote(e.target.value)}
                        className="bg-dark border-secondary text-white"
                        placeholder="What did you think?"
                    />
                </Form.Group>
            </Modal.Body>
            <Modal.Footer className="border-secondary">
                <Button variant="outline-light" onClick={onHide}>Cancel</Button>
                <Button variant="primary" onClick={handleSubmit} disabled={loading}>
                    {loading ? 'Saving...' : 'Save Note'}
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default NoteModal;
