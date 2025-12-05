import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Row, Col, Image, Badge, Spinner, Alert, Button, Form } from 'react-bootstrap';
import { movieService, frameworkService } from '../services/api';
import StarRating from '../components/StarRating';
import NoteModal from '../components/NoteModal';
import { useAuth } from '../context/AuthContext';
import { useToast } from '../context/ToastContext';
import noPoster from '../assets/no-poster.png';

const MovieDetail = () => {
    const { id } = useParams();
    const { user } = useAuth();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [rating, setRating] = useState(0);
    const [isBookmarked, setIsBookmarked] = useState(false);
    const [showNoteModal, setShowNoteModal] = useState(false);


    const [similarMovies, setSimilarMovies] = useState([]);
    const [popularCast, setPopularCast] = useState([]);
    const [isPopularSort, setIsPopularSort] = useState(false);

    const displayedCast = isPopularSort
        ? popularCast.map(p => {
            const original = movie?.cast?.find(c => c.nconst === p.nconst);
            return {
                ...p,
                characters: original ? original.characters : [],
                personId: original ? original.personId : null,
                personName: p.primaryName // Ensure shared naming convention
            };
        })
        : (movie?.cast || []).slice(0, 10);

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const response = await movieService.getMovieDetails(id);
                setMovie(response.data);

                try {
                    const similarResponse = await movieService.getSimilarMovies(id);
                    setSimilarMovies(similarResponse.data);

                    const popularResponse = await movieService.getPopularActors(id);
                    setPopularCast(popularResponse.data);
                } catch (e) {
                    console.error("Failed to fetch advanced movie data", e);
                }

                if (user) {
                    try {
                        const bookmarks = await frameworkService.getUserMovieBookmarks(user.id);
                        const bookmarked = bookmarks.data.some(b => b.movieId === parseInt(id));
                        setIsBookmarked(bookmarked);
                    } catch (e) {
                        console.error("Failed to fetch bookmarks", e);
                    }
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
        if (!user) return addToast('Please login to rate', 'warning');
        try {
            await frameworkService.rateMovie(user.id, parseInt(id), rating);
            addToast('Rating submitted!', 'success');
        } catch (err) {
            addToast('Failed to submit rating', 'danger');
        }
    };

    const handleBookmark = async () => {
        if (!user) return addToast('Please login to bookmark', 'warning');
        try {
            await frameworkService.toggleMovieBookmark(user.id, parseInt(id));
            setIsBookmarked(!isBookmarked);
            addToast(isBookmarked ? 'Bookmark removed' : 'Movie bookmarked!', 'success');
        } catch (err) {
            addToast('Failed to update bookmark', 'danger');
        }
    };

    if (loading) return <Container className="text-center py-5"><Spinner animation="border" /></Container>;
    if (error) return <Container className="py-5"><Alert variant="danger">{error}</Alert></Container>;
    if (!movie) return null;

    return (
        <div className="fade-in">
            {/* Cinematic Background */}
            <div className="position-fixed top-0 start-0 w-100 h-100" style={{ zIndex: -1 }}>
                <div
                    style={{
                        backgroundImage: `url(${movie.posterUrl || noPoster})`,
                        backgroundSize: 'cover',
                        backgroundPosition: 'center top',
                        filter: 'blur(40px) brightness(0.2)',
                        transform: 'scale(1.1)',
                        width: '100%',
                        height: '100%'
                    }}
                />
                <div className="position-absolute top-0 start-0 w-100 h-100" style={{ background: 'linear-gradient(to bottom, rgba(10,11,20,0.4) 0%, var(--bg-primary) 100%)' }}></div>
            </div>

            <Container className="py-5 mt-4 position-relative">
                <Row className="g-5">
                    {/* Left Column: Poster & Actions */}
                    <Col lg={4} xl={3} className="d-none d-lg-block">
                        <div className="position-sticky" style={{ top: '100px' }}>
                            <div className="glass-panel p-2 mb-4 hover-scale">
                                <Image
                                    src={movie.posterUrl || noPoster}
                                    onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                                    fluid rounded
                                    className="w-100 shadow-lg"
                                    style={{ objectFit: 'cover' }}
                                />
                            </div>

                            {user && (
                                <div className="glass-panel p-4 d-grid gap-3">
                                    <Button
                                        variant={isBookmarked ? "success" : "outline-light"}
                                        onClick={handleBookmark}
                                        className="w-100 rounded-pill fw-bold"
                                    >
                                        {isBookmarked ? "✓ In Watchlist" : "+ Add to Watchlist"}
                                    </Button>

                                    <Button
                                        variant="outline-info"
                                        onClick={() => setShowNoteModal(true)}
                                        className="w-100 rounded-pill fw-bold"
                                    >
                                        + Add Note
                                    </Button>

                                    <NoteModal
                                        show={showNoteModal}
                                        onHide={() => setShowNoteModal(false)}
                                        targetId={parseInt(id)}
                                        type="movie"
                                        userId={user.id}
                                    />

                                    <div className="text-center pt-2 border-top border-secondary mt-2">
                                        <span className="text-secondary small text-uppercase tracking-wider mb-2 d-block">Your Rating</span>
                                        <div className="d-flex justify-content-center mb-2">
                                            <StarRating rating={rating} onRatingChange={setRating} />
                                        </div>
                                        <Button
                                            onClick={handleRate}
                                            disabled={rating === 0}
                                            size="sm"
                                            variant="primary"
                                            className="w-100 rounded-pill"
                                        >
                                            Submit
                                        </Button>
                                    </div>
                                </div>
                            )}
                        </div>
                    </Col>

                    {/* Mobile Poster (Visible only on small screens) */}
                    <Col xs={12} className="d-lg-none text-center mb-4">
                        <Image
                            src={movie.posterUrl || noPoster}
                            rounded
                            className="shadow-lg mb-3"
                            style={{ maxWidth: '200px' }}
                        />
                        {user && (
                            <div className="d-grid gap-2">
                                <Button
                                    variant={isBookmarked ? "success" : "outline-light"}
                                    onClick={handleBookmark}
                                    className="rounded-pill fw-bold"
                                >
                                    {isBookmarked ? "✓ Watchlist" : "+ Watchlist"}
                                </Button>
                                <Button variant="outline-info" onClick={() => setShowNoteModal(true)} size="sm" className="rounded-pill">
                                    + Add Note
                                </Button>
                            </div>
                        )}
                    </Col>

                    {/* Right Column: Details */}
                    <Col lg={8} xl={9}>
                        <h1 className="display-3 fw-bold mb-2 text-white" style={{ textShadow: '0 2px 10px rgba(0,0,0,0.5)' }}>{movie.primaryTitle}</h1>

                        <div className="d-flex align-items-center gap-3 mb-4 text-white-50 fs-5">
                            <span>{movie.startYear}</span>
                            <span>•</span>
                            <span>{movie.runTimeMinutes} min</span>
                            <span>•</span>
                            <span className="d-flex align-items-center text-gold">
                                <span className="me-1">★</span> {movie.averageRating} <span className="fs-6 ms-1 text-secondary">({movie.voteCount})</span>
                            </span>
                        </div>

                        <div className="mb-5">
                            {movie.genres && movie.genres.map(g => (
                                <Badge bg="transparent" className="border border-secondary px-3 py-2 me-2 rounded-pill fw-normal text-white hover-scale" key={g.genreId}>
                                    {g.genreName}
                                </Badge>
                            ))}
                        </div>

                        <div className="glass-panel p-5 mb-5">
                            <h4 className="text-gradient mb-4">Plot Summary</h4>
                            <p className="lead text-light opacity-75" style={{ lineHeight: '1.8' }}>{movie.plotSummary}</p>

                            <Row className="mt-5 g-4">
                                <Col md={6}>
                                    <h5 className="text-white mb-3">Details</h5>
                                    <dl className="row mb-0">
                                        <dt className="col-sm-4 text-secondary">Original Title</dt>
                                        <dd className="col-sm-8 text-white">{movie.originalTitle}</dd>
                                        <dt className="col-sm-4 text-secondary">Release Year</dt>
                                        <dd className="col-sm-8 text-white">{movie.startYear}</dd>
                                    </dl>
                                </Col>
                                <Col md={6}>
                                    <div className="d-flex justify-content-between align-items-center mb-3">
                                        <h5 className="text-white mb-0">Cast</h5>
                                        <Form.Check
                                            type="switch"
                                            id="custom-switch"
                                            label={<span className="text-white-50 small">Order by Popularity</span>}
                                            checked={isPopularSort}
                                            onChange={(e) => setIsPopularSort(e.target.checked)}
                                        />
                                    </div>
                                    <div className="d-flex flex-column gap-2" style={{ maxHeight: '400px', overflowY: 'auto' }}>
                                        {displayedCast.map(c => (
                                            <div key={c.castId || c.nconst} className="d-flex justify-content-between">
                                                <a href={`/persons/${c.personId || '#'}`} className="text-action fw-medium hover-underline text-truncate me-2">
                                                    {c.personName || c.primaryName}
                                                </a>
                                                <span className="text-secondary small text-end text-truncate w-50">
                                                    {c.characters ? `as ${c.characters.join(', ')}` : (c.weightedAverage ? `Rating: ${c.weightedAverage}` : '')}
                                                </span>
                                            </div>
                                        ))}
                                    </div>
                                </Col>
                            </Row>
                        </div>

                        {similarMovies.length > 0 && (
                            <div className="mt-5">
                                <h3 className="mb-4 border-start border-4 border-primary ps-3">You Might Also Like</h3>
                                <div className="d-flex overflow-auto gap-4 py-2 pb-4" style={{ scrollbarWidth: 'thin' }}>
                                    {similarMovies.map(m => (
                                        <div key={m.movieId} style={{ minWidth: '180px', maxWidth: '180px' }}>
                                            <a href={`/movies/${m.movieId}`} className="text-decoration-none">
                                                <div className="glass-panel h-100 hover-scale position-relative overflow-hidden">
                                                    <div className="ratio ratio-2x3">
                                                        <img
                                                            src={m.posterUrl || noPoster}
                                                            onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                                                            alt={m.primaryTitle}
                                                            className="w-100 h-100 object-fit-cover"
                                                        />
                                                    </div>
                                                    <div className="p-3">
                                                        <h6 className="text-white text-truncate mb-1">{m.primaryTitle}</h6>
                                                        <small className="text-white-50">{m.yearDiff === 0 ? 'Same Year' : `${Math.abs(m.yearDiff)} yrs apart`}</small>
                                                    </div>
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
        </div>
    );
};

export default MovieDetail;
