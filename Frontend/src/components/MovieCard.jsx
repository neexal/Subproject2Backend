import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { tmdbService } from '../services/api';

const MovieCard = ({ movie }) => {
    const [poster, setPoster] = useState(null);
    const [loading, setLoading] = useState(true);
    const noPoster = "https://placehold.co/300x450/1a1d29/ffffff?text=No+Poster";

    useEffect(() => {
        const fetchPoster = async () => {
            setLoading(true);
            
            // Priority 1: Try TMDB first (most reliable)
            if (movie.tconst) {
                try {
                    const url = await tmdbService.getMovieImage(movie.tconst);
                    if (url) {
                        setPoster(url);
                        setLoading(false);
                        return;
                    }
                } catch (e) {
                    console.warn(`[MovieCard] TMDB fetch failed for ${movie.tconst}:`, e);
                }
            }

            // Priority 2: Fallback to database URL if available
            if (movie.posterUrl && movie.posterUrl !== 'N/A' && !movie.posterUrl.includes('N/A')) {
                setPoster(movie.posterUrl);
                setLoading(false);
                return;
            }

            // Priority 3: Use placeholder
            setPoster(noPoster);
            setLoading(false);
        };

        fetchPoster();
    }, [movie.tconst, movie.posterUrl]);

    const handleImageError = (e) => {
        // If image fails to load, try TMDB one more time if we haven't already
        if (movie.tconst && poster && !poster.includes('api.themoviedb.org') && !poster.includes('placehold.co')) {
            tmdbService.getMovieImage(movie.tconst)
                .then(url => {
                    if (url) {
                        setPoster(url);
                    } else {
                        e.target.onerror = null;
                        e.target.src = noPoster;
                    }
                })
                .catch(() => {
                    e.target.onerror = null;
                    e.target.src = noPoster;
                });
        } else {
            e.target.onerror = null;
            e.target.src = noPoster;
        }
    };

    return (
        <div 
            className="glass-panel position-relative hover-scale d-flex flex-column"
            style={{ 
                width: '200px', 
                height: '300px',
                overflow: 'hidden'
            }}
        >
            {/* Image Section */}
            <div 
                className="position-relative"
                style={{ 
                    width: '100%', 
                    height: '220px',
                    backgroundColor: '#1a1d29',
                    flexShrink: 0
                }}
            >
                {loading && (
                    <div className="position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center">
                        <div className="spinner-border spinner-border-sm text-primary" role="status" style={{ width: '1.5rem', height: '1.5rem' }}>
                            <span className="visually-hidden">Loading...</span>
                        </div>
                    </div>
                )}
                <img
                    src={poster || noPoster}
                    onError={handleImageError}
                    onLoad={() => setLoading(false)}
                    alt={movie.primaryTitle}
                    className="w-100 h-100 object-fit-cover"
                    style={{ 
                        opacity: loading ? 0 : 1,
                        transition: 'opacity 0.3s ease',
                        display: 'block'
                    }}
                    loading="lazy"
                />
            </div>

            {/* Info Section at Bottom */}
            <div 
                className="p-2 d-flex flex-column justify-content-between"
                style={{ 
                    flex: 1,
                    minHeight: '80px',
                    backgroundColor: 'rgba(26, 29, 41, 0.8)'
                }}
            >
                <h6 
                    className="text-white fw-bold mb-1" 
                    title={movie.primaryTitle}
                    style={{ 
                        fontSize: '0.875rem',
                        lineHeight: '1.2',
                        display: '-webkit-box',
                        WebkitLineClamp: 2,
                        WebkitBoxOrient: 'vertical',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        minHeight: '2.4rem',
                        marginBottom: '0.5rem'
                    }}
                >
                    {movie.primaryTitle}
                </h6>
                <div className="d-flex justify-content-between align-items-center">
                    <span className="text-secondary" style={{ fontSize: '0.75rem' }}>
                        {movie.startYear || 'N/A'}
                    </span>
                    <div className="d-flex align-items-center gap-1">
                        <span className="text-warning fw-bold" style={{ fontSize: '0.875rem' }}>
                            ★ {movie.averageRating ? movie.averageRating.toFixed(1) : 'N/A'}
                        </span>
                    </div>
                </div>
            </div>
            <Link to={`/movies/${movie.movieId}`} className="stretched-link"></Link>
        </div>
    );
};

export default MovieCard;
