import React from 'react';

import { Link } from 'react-router-dom';
import noPoster from '../assets/no-poster.png';

const MovieCard = ({ movie }) => {
    return (
        <div className="glass-panel position-relative h-100 hover-scale">
            <div className="ratio ratio-2x3">
                <img
                    src={movie.posterUrl || noPoster}
                    onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                    alt={movie.primaryTitle}
                    className="w-100 h-100 object-fit-cover"
                />
            </div>
            <div className="p-3">
                <h6 className="text-white fw-bold text-truncate mb-1" title={movie.primaryTitle}>
                    {movie.primaryTitle}
                </h6>
                <div className="d-flex justify-content-between align-items-center small">
                    <span className="text-secondary">{movie.startYear}</span>
                    <span className="text-warning fw-bold">
                        ★ {movie.averageRating ? movie.averageRating.toFixed(1) : 'N/A'}
                    </span>
                </div>
            </div>
            <Link to={`/movies/${movie.movieId}`} className="stretched-link"></Link>
        </div>
    );
};

export default MovieCard;
