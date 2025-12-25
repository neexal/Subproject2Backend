import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Container, Row, Col, Image, Spinner, Alert, Button, ListGroup, Badge } from 'react-bootstrap';
import { personService, frameworkService, tmdbService } from '../services/api';
import { useAuth } from '../context/AuthContext';
import NoteModal from '../components/NoteModal';
import WordCloud from '../components/WordCloud';
import { useToast } from '../context/ToastContext';
const PersonDetail = () => {
    const { id } = useParams();

    const { user } = useAuth();
    const { addToast } = useToast();
    const [person, setPerson] = useState(null);
    const noPoster = "https://placehold.co/300x450/1a1d29/ffffff?text=No+Image";
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [imageUrl, setImageUrl] = useState(null);
    const [isBookmarked, setIsBookmarked] = useState(false);
    const [showNoteModal, setShowNoteModal] = useState(false);
    const [coPlayers, setCoPlayers] = useState([]);
    const [words, setWords] = useState([]);

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                const response = await personService.getPersonDetails(id);
                setPerson(response.data);

                if (response.data.nconst) {
                    const url = await tmdbService.getPersonImage(response.data.nconst);
                    setImageUrl(url);
                }

                if (response.data.primaryName) {
                    try {
                        const coPlayersResponse = await personService.getCoPlayers(response.data.primaryName);
                        setCoPlayers(coPlayersResponse.data);

                        const wordsResponse = await personService.getPersonWords(response.data.primaryName);
                        setWords(wordsResponse.data);
                    } catch (e) {
                        console.error("Failed to fetch advanced analysis", e);
                    }
                }

                if (user) {
                    try {
                        const bookmarks = await frameworkService.getUserPersonBookmarks(user.id);
                        const bookmarked = bookmarks.data.some(b => b.personId === parseInt(id));
                        setIsBookmarked(bookmarked);
                    } catch (e) {
                        console.error("Failed to fetch bookmarks", e);
                    }
                }
            } catch (err) {
                setError('Failed to load person details.');
            } finally {
                setLoading(false);
            }
        };

        fetchDetails();
    }, [id, user]);

    const handleBookmark = async () => {
        if (!user) return addToast('Please login to bookmark', 'warning');
        try {
            await frameworkService.togglePersonBookmark(user.id, parseInt(id));
            setIsBookmarked(!isBookmarked);
            addToast(isBookmarked ? 'Bookmark removed' : 'Person bookmarked!', 'success');
        } catch (err) {
            console.error("Bookmark error:", err);
            addToast(`Failed to update bookmark: ${err.response?.data?.Message || err.message}`, 'danger');
        }
    };

    if (loading) return <Container className="text-center py-5"><Spinner animation="border" /></Container>;
    if (error) return <Container className="py-5"><Alert variant="danger">{error}</Alert></Container>;
    if (!person) return null;

    return (
        <div className="fade-in">
            {/* Cinematic Background */}
            <div className="position-fixed top-0 start-0 w-100 h-100" style={{ zIndex: -1 }}>
                <div
                    style={{
                        backgroundImage: `url(${imageUrl || noPoster})`,
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
                    <Col lg={4} xl={3} className="d-none d-lg-block">
                        <div className="position-sticky" style={{ top: '100px' }}>
                            <div className="glass-panel p-2 mb-4 hover-scale">
                                <Image
                                    src={imageUrl || noPoster}
                                    onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                                    fluid rounded
                                    className="w-100 shadow-lg"
                                    style={{ objectFit: 'cover' }}
                                />
                            </div>
                            {user && (
                                <div className="glass-panel p-4 d-grid gap-2">
                                    <Button
                                        variant={isBookmarked ? "success" : "outline-light"}
                                        onClick={handleBookmark}
                                        className="w-100 rounded-pill fw-bold"
                                    >
                                        {isBookmarked ? "✓ Followed" : "+ Follow Person"}
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
                                        type="person"
                                        userId={user.id}
                                    />
                                </div>
                            )}
                        </div>
                    </Col>

                    {/* Mobile Person Image */}
                    <Col xs={12} className="d-lg-none text-center mb-4">
                        <Image
                            src={imageUrl || noPoster}
                            roundedCircle
                            className="shadow-lg mb-3 border border-3 border-secondary"
                            style={{ width: '150px', height: '150px', objectFit: 'cover' }}
                        />
                        {user && (
                            <div className="d-flex justify-content-center gap-2">
                                <Button
                                    variant={isBookmarked ? "success" : "outline-light"}
                                    onClick={handleBookmark}
                                    size="sm"
                                    className="rounded-pill fw-bold"
                                >
                                    {isBookmarked ? "✓ Followed" : "+ Follow"}
                                </Button>
                                <Button variant="outline-info" onClick={() => setShowNoteModal(true)} size="sm" className="rounded-pill">
                                    + Note
                                </Button>
                            </div>
                        )}
                    </Col>
                    <Col lg={8} xl={9}>
                        <h1 className="display-3 fw-bold mb-2 text-white" style={{ textShadow: '0 2px 10px rgba(0,0,0,0.5)' }}>{person.primaryName}</h1>
                        <p className="text-white-50 fs-5 mb-5">
                            {person.birthYear ? `Born: ${person.birthYear}` : ''}
                            {person.deathYear ? ` - Died: ${person.deathYear}` : ''}
                        </p>

                        <div className="glass-panel p-5 mb-5">
                            <h4 className="text-gradient mb-4">Known For</h4>
                            <div className="d-flex overflow-auto gap-4 py-2 pb-3" style={{ scrollbarWidth: 'thin' }}>
                                {person.knownFor && person.knownFor.map(m => (
                                    <div key={m.movieId} style={{ minWidth: '160px', maxWidth: '160px' }}>
                                        <Link to={`/movies/${m.movieId}`} className="text-decoration-none">
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
                                                    <small className="text-white-50">{m.startYear}</small>
                                                </div>
                                            </div>
                                        </Link>
                                    </div>
                                ))}
                            </div>

                            {coPlayers.length > 0 && (
                                <div className="mt-5">
                                    <h4 className="text-gradient mb-4">Frequent Co-Players</h4>
                                    <div className="d-flex flex-wrap gap-2">
                                        {coPlayers.slice(0, 15).map(cp => (
                                            <Button
                                                key={cp.nconst}
                                                variant="outline-light"
                                                size="sm"
                                                as="a"
                                                href={`/search?q=${encodeURIComponent(cp.primaryName)}`}
                                                className="rounded-pill px-3 d-flex align-items-center gap-2 border-secondary text-white-50"
                                                style={{ textDecoration: 'none' }}
                                            >
                                                {cp.primaryName} <Badge bg="secondary" className="text-dark bg-white">{cp.frequency}</Badge>
                                            </Button>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {words.length > 0 && (
                                <div className="mt-5">
                                    <h4 className="text-gradient mb-4">Characteristic Words</h4>
                                    <p className="text-secondary small mb-3">
                                        Words frequently associated with this person's work
                                    </p>
                                    <WordCloud words={words} maxWords={15} />
                                </div>
                            )}
                        </div>
                    </Col>
                </Row>
            </Container>
        </div>
    );
};

export default PersonDetail;
