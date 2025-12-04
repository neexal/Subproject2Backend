import React from 'react';
import { Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const MovieCard = ({ movie }) => {
    return (
        <Card className="h-100 shadow-sm text-white">
            {movie.posterUrl ? (
                <Card.Img variant="top" src={movie.posterUrl} style={{ height: '350px', objectFit: 'cover' }} />
            ) : (
                <div className="d-flex align-items-center justify-content-center bg-secondary" style={{ height: '350px' }}>
                    <span>No Poster</span>
                </div>
            )}
            <Card.Body className="d-flex flex-column p-3">
                <Card.Title>{movie.primaryTitle}</Card.Title>
                <Card.Text className="mb-2">
                    <span className="badge bg-warning text-dark me-2">★ {movie.averageRating ? movie.averageRating.toFixed(1) : 'N/A'}</span>
                    <small>{movie.startYear}</small>
                </Card.Text>
                <Link to={`/movies/${movie.movieId}`} className="stretched-link"></Link>
            </Card.Body>
        </Card>
    );
};

export default MovieCard;
