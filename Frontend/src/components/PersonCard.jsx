import React, { useState, useEffect } from 'react';
import { Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { tmdbService } from '../services/api';

const PersonCard = ({ person }) => {
    const [imageUrl, setImageUrl] = useState(null);

    useEffect(() => {
        const fetchImage = async () => {
            if (person.nconst) {
                const url = await tmdbService.getPersonImage(person.nconst);
                setImageUrl(url);
            }
        };
        fetchImage();
    }, [person.nconst]);

    return (
        <Card className="h-100 shadow-sm text-white">
            <Card.Img
                variant="top"
                src={imageUrl || "https://via.placeholder.com/200x300?text=No+Image"}
                style={{ height: '350px', objectFit: 'cover' }}
            />
            <Card.Body className="d-flex flex-column p-3">
                <Card.Title>{person.primaryName}</Card.Title>
                <Card.Text className="text-muted small">
                    {person.birthYear ? `Born: ${person.birthYear}` : ''}
                </Card.Text>
                <Link to={`/persons/${person.personId}`} className="stretched-link"></Link>
            </Card.Body>
        </Card>
    );
};

export default PersonCard;
