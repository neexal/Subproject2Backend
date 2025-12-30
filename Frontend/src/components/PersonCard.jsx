import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { tmdbService } from '../services/api';

const PersonCard = ({ person }) => {
    const [imageUrl, setImageUrl] = useState(null);
    const [loading, setLoading] = useState(true);
    const noPoster = "https://placehold.co/300x450/1a1d29/ffffff?text=No+Image";

    useEffect(() => {
        const fetchImage = async () => {
            setLoading(true);
            
            // Try to fetch from TMDB using nconst
            if (person.nconst) {
                try {
                    const url = await tmdbService.getPersonImage(person.nconst);
                    if (url) {
                        setImageUrl(url);
                        setLoading(false);
                        return;
                    }
                } catch (e) {
                    console.warn(`[PersonCard] TMDB fetch failed for ${person.nconst}:`, e);
                }
            }

            // Fallback to placeholder
            setImageUrl(noPoster);
            setLoading(false);
        };

        fetchImage();
    }, [person.nconst]);

    const handleImageError = (e) => {
        // If image fails to load, try TMDB one more time if we haven't already
        if (person.nconst && imageUrl && !imageUrl.includes('api.themoviedb.org') && !imageUrl.includes('placehold.co')) {
            tmdbService.getPersonImage(person.nconst)
                .then(url => {
                    if (url) {
                        setImageUrl(url);
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
                    src={imageUrl || noPoster}
                    onError={handleImageError}
                    onLoad={() => setLoading(false)}
                    alt={person.primaryName}
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
                    title={person.primaryName}
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
                    {person.primaryName}
                </h6>
                <div className="d-flex justify-content-between align-items-center">
                    <span className="text-secondary" style={{ fontSize: '0.75rem' }}>
                        {person.birthYear ? `Born: ${person.birthYear}` : ''}
                    </span>
                </div>
            </div>
            <Link to={`/persons/${person.personId}`} className="stretched-link"></Link>
        </div>
    );
};

export default PersonCard;
