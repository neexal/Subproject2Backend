import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { tmdbService } from '../services/api';

const PersonCard = ({ person }) => {
    const [imageUrl, setImageUrl] = useState(null);
    const noPoster = "https://placehold.co/300x450/1a1d29/ffffff?text=No+Image";

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
        <div className="glass-panel position-relative h-100 hover-scale">
            <div className="ratio ratio-2x3">
                <img
                    src={imageUrl || noPoster}
                    onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                    alt={person.primaryName}
                    className="w-100 h-100 object-fit-cover"
                    loading="lazy"
                />
            </div>
            <div className="p-3">
                <h6 className="text-white fw-bold text-truncate mb-1" title={person.primaryName}>
                    {person.primaryName}
                </h6>
                <div className="d-flex justify-content-between align-items-center small">
                    <span className="text-secondary">
                        {person.birthYear ? `Born: ${person.birthYear}` : ''}
                    </span>
                </div>
            </div>
            <Link to={`/persons/${person.personId}`} className="stretched-link"></Link>
        </div>
    );
};

export default PersonCard;
