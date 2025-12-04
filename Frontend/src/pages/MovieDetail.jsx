import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Row, Col, Image, Badge, Spinner, Alert, Button, Form } from 'react-bootstrap';
import { movieService, frameworkService } from '../services/api';
import { useAuth } from '../context/AuthContext';

const MovieDetail = () => {
    const { id } = useParams();
    const { user } = useAuth();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [rating, setRating] = useState(0);
    const [isBookmarked, setIsBookmarked] = useState(false);

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const response = await movieService.getMovieDetails(id);
                setMovie(response.data);

                if (user) {
                    // Check bookmark status (requires fetching all bookmarks, simplified here)
                    const bookmarks = await frameworkService.getUserMovieBookmarks(user.id);
                    const bookmarked = bookmarks.data.some(b => b.movieId === parseInt(id));
                    setIsBookmarked(bookmarked);
                }
            } catch (err) {
                setError('Failed to load movie details.');
            } finally {
                setLoading(false);
            }
        };

        fetchDetails();
    }, [id, user]);

    const handleRate = async () => {
        if (!user) return alert('Please login to rate');
        try {
            await frameworkService.rateMovie(user.id, parseInt(id), rating);
            alert('Rating submitted!');
        } catch (err) {
            alert('Failed to submit rating');
        }
    };

    const handleBookmark = async () => {
        if (!user) return alert('Please login to bookmark');
        try {
            await frameworkService.toggleMovieBookmark(user.id, parseInt(id));
            setIsBookmarked(!isBookmarked);
        } catch (err) {
            alert('Failed to update bookmark');
        }
    };

    if (loading) return <Container className="text-center py-5"><Spinner animation="border" /></Container>;
    if (error) return <Container className="py-5"><Alert variant="danger">{error}</Alert></Container>;
    if (!movie) return null;

    return (
        <Container className="py-5">
            <Row>
                <Col md={4}>
                    <Image src={movie.posterUrl || "https://via.placeholder.com/300x450?text=No+Poster"} fluid rounded className="mb-3" />
                    {user && (
                        <div className="d-grid gap-2">
                            <Button
                                variant={isBookmarked ? "success" : "outline-primary"}
                                onClick={handleBookmark}
                            >
                                {isBookmarked ? "Bookmarked" : "Bookmark Movie"}
                            </Button>
                            <div className="d-flex gap-2 align-items-center mt-3">
                                <Form.Select value={rating} onChange={(e) => setRating(parseInt(e.target.value))}>
                                    <option value="0">Rate...</option>
                                    {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(r => (
                                        <option key={r} value={r}>{r}</option>
                                    ))}
                                </Form.Select>
                                <Button onClick={handleRate} disabled={rating === 0}>Rate</Button>
                            </div>
                        </div>
                    )}
                </Col>
                <Col md={8}>
                    <h1>{movie.primaryTitle} <small className="text-muted">({movie.startYear})</small></h1>
                    <div className="mb-3">
                        {movie.genres && movie.genres.map(g => (
                            <Badge bg="secondary" className="me-1" key={g.genreId}>{g.genreName}</Badge>
                        ))}
                    </div>
                    <p className="lead">{movie.plotSummary}</p>

                    <Row className="mt-4">
                        <Col md={6}>
                            <h4>Details</h4>
                            <ul className="list-unstyled">
                                <li><strong>Original Title:</strong> {movie.originalTitle}</li>
                                <li><strong>Runtime:</strong> {movie.runTimeMinutes} mins</li>
                                <li><strong>Rating:</strong> {movie.averageRating} ({movie.voteCount} votes)</li>
                            </ul>
                        </Col>
                        <Col md={6}>
                            <h4>Cast</h4>
                            <ul className="list-unstyled">
                                {movie.cast && movie.cast.slice(0, 5).map(c => (
                                    <li key={c.castId}>
                                        <a href={`/persons/${c.personId}`} className="text-decoration-none text-info">{c.personName}</a> as {c.characters.join(', ')}
                                    </li>
                                ))}
                            </ul>
                        </Col>
                    </Row>

                    {similarMovies.length > 0 && (
                        <div className="mt-5">
                            <h4>Similar Movies</h4>
                            <div className="d-flex overflow-auto gap-3 py-2">
                                {similarMovies.map(m => (
                                    <div key={m.movieId} style={{ minWidth: '150px' }}>
                                        <a href={`/movies/${m.movieId}`} className="text-decoration-none text-white">
                                            <div className="bg-dark border border-secondary rounded p-2 h-100 text-center">
                                                <strong>{m.primaryTitle}</strong>
                                                <br />
                                                <small className="text-muted">{m.startYear}</small>
                                            </div>
                                        </a>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}
                </Col>
            </Row>
        </Container>
    );
};

export default MovieDetail;
