import React from 'react';
import { Badge } from 'react-bootstrap';

/**
 * WordCloud Component
 * Displays a visual word cloud representation of words with their frequencies.
 * Words are sized and weighted based on their frequency.
 * 
 * @param {Array} words - Array of objects with {word: string, frequency: number}
 * @param {number} maxWords - Maximum number of words to display (default: 10)
 */
const WordCloud = ({ words = [], maxWords = 10 }) => {
    if (!words || words.length === 0) {
        return null;
    }

    // Take top N words
    const displayWords = words.slice(0, maxWords);
    
    // Calculate min and max frequencies for scaling
    const frequencies = displayWords.map(w => w.frequency || 0);
    const minFreq = Math.min(...frequencies);
    const maxFreq = Math.max(...frequencies);
    const range = maxFreq - minFreq || 1; // Avoid division by zero

    return (
        <div className="word-cloud d-flex flex-wrap gap-2 align-items-center justify-content-center">
            {displayWords.map((wordItem, index) => {
                const { word, frequency } = wordItem;
                // Normalize frequency to 0-1 range, then scale to font size
                const normalizedFreq = (frequency - minFreq) / range;
                const fontSize = Math.max(0.8, 0.8 + normalizedFreq * 1.2); // 0.8rem to 2rem
                const opacity = 0.7 + normalizedFreq * 0.3; // 0.7 to 1.0
                
                return (
                    <Badge
                        key={index}
                        bg="dark"
                        className="border border-secondary fw-light px-3 py-2 word-cloud-item"
                        style={{
                            fontSize: `${fontSize}rem`,
                            opacity: opacity,
                            transition: 'all 0.3s ease',
                            cursor: 'default'
                        }}
                        onMouseEnter={(e) => {
                            e.target.style.transform = 'scale(1.1)';
                            e.target.style.opacity = '1';
                        }}
                        onMouseLeave={(e) => {
                            e.target.style.transform = 'scale(1)';
                            e.target.style.opacity = opacity;
                        }}
                        title={`Frequency: ${frequency}`}
                    >
                        {word}
                    </Badge>
                );
            })}
        </div>
    );
};

export default WordCloud;

