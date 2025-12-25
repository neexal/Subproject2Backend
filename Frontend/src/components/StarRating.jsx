import React, { useState } from 'react';
import { FaStar, FaStarHalfAlt, FaRegStar } from 'react-icons/fa';

const StarRating = ({ rating, onRatingChange, readOnly = false }) => {
    const [hover, setHover] = useState(0);

    // Convert 0-10 rating to 0-5 stars
    const starRating = rating / 2;

    const handleMouseMove = (e, index) => {
        if (readOnly) return;

        const rect = e.currentTarget.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const width = rect.width;

        // If mouse is on the left half, it's x.5, else it's x.0
        // index is 1-based. 
        // If index is 1: left half -> 0.5, right half -> 1.0
        // If index is 2: left half -> 1.5, right half -> 2.0

        const isHalf = x < width / 2;
        const value = isHalf ? index - 0.5 : index;
        setHover(value);
    };

    const handleClick = () => {
        if (!readOnly && onRatingChange) {
            // hover is 0-5 scale. Convert back to 0-10.
            // 0.5 -> 1, 1.0 -> 2, 1.5 -> 3, etc.
            onRatingChange(hover * 2);
        }
    };

    const handleMouseLeave = () => {
        if (!readOnly) {
            setHover(0);
        }
    };

    const renderStars = () => {
        const stars = [];
        for (let i = 1; i <= 5; i++) {
            // Determine what to show based on hover state or current rating
            const effectiveRating = hover || starRating;

            let StarIcon = FaRegStar;
            if (effectiveRating >= i) {
                StarIcon = FaStar;
            } else if (effectiveRating >= i - 0.5) {
                StarIcon = FaStarHalfAlt;
            }

            if (readOnly) {
                stars.push(<StarIcon key={i} className="text-warning" />);
            } else {
                // Interactive mode
                stars.push(
                    <span
                        key={i}
                        style={{ cursor: 'pointer', display: 'inline-block' }}
                        onClick={handleClick}
                        onMouseMove={(e) => handleMouseMove(e, i)}
                        onMouseLeave={handleMouseLeave}
                    >
                        <StarIcon className="text-warning" />
                    </span>
                );
            }
        }
        return stars;
    };

    return (
        <div className="d-inline-flex align-items-center gap-1">
            {renderStars()}
            <span className="ms-2 text-muted small">({rating}/10)</span>
        </div>
    );
};

export default StarRating;
