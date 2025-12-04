import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Row, Col, Image, Spinner, Alert, Button, ListGroup, Badge } from 'react-bootstrap';
import { personService, frameworkService, tmdbService } from '../services/api';
import { useAuth } from '../context/AuthContext';
import noPoster from '../assets/no-poster.png';

const PersonDetail = () => {
    const { id } = useParams();
    const { user } = useAuth();
    const [person, setPerson] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [imageUrl, setImageUrl] = useState(null);
    const [isBookmarked, setIsBookmarked] = useState(false);
    const [coPlayers, setCoPlayers] = useState([]);

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
                    } catch (e) {
                        console.error("Failed to fetch co-players", e);
                    }
                }

                if (user) {
                    const bookmarks = await frameworkService.getUserPersonBookmarks(user.id);
                    const bookmarked = bookmarks.data.some(b => b.personId === parseInt(id));
                    setIsBookmarked(bookmarked);
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
        if (!user) return alert('Please login to bookmark');
        try {
            await frameworkService.togglePersonBookmark(user.id, parseInt(id));
            setIsBookmarked(!isBookmarked);
        } catch (err) {
            alert('Failed to update bookmark');
        }
    };

    if (loading) return <Container className="text-center py-5"><Spinner animation="border" /></Container>;
    if (error) return <Container className="py-5"><Alert variant="danger">{error}</Alert></Container>;
    if (!person) return null;

    return (
        <Container className="py-5">
            <Row>
                <Col md={4}>
                    <Image
                        src={imageUrl || noPoster}
                        onError={(e) => { e.target.onerror = null; e.target.src = noPoster; }}
                        fluid rounded className="mb-3"
                    />
                    {user && (
                        <div className="d-grid">
                            <Button
                                variant={isBookmarked ? "success" : "outline-primary"}
                                onClick={handleBookmark}
                            >
                                {isBookmarked ? "Bookmarked" : "Bookmark Person"}
                            </Button>
                        </div>
                    )}
                </Col>
                <Col md={8}>
                    <h1>{person.primaryName}</h1>
                    <p className="text-muted">
                        {person.birthYear ? `Born: ${person.birthYear}` : ''}
                        {person.deathYear ? ` - Died: ${person.deathYear}` : ''}
                    </p>

                    <h4 className="mt-4">Known For</h4>
                    <ListGroup variant="flush">
                        {person.knownFor && person.knownFor.map(m => (
                            <ListGroup.Item key={m.movieId} className="bg-transparent text-white">
                                <a href={`/movies/${m.movieId}`} className="text-decoration-none text-info">{m.primaryTitle}</a> ({m.startYear})
                            </ListGroup.Item>
                        ))}
                    </ListGroup>

                    {coPlayers.length > 0 && (
                        <>
                            <h4 className="mt-4">Frequent Co-Players</h4>
                            <div className="d-flex flex-wrap gap-2">
                                {coPlayers.slice(0, 10).map(cp => (
                                    <Button
                                        key={cp.nconst}
                                        variant="outline-light"
                                        size="sm"
                                        href={`/search?q=${cp.primaryName}`}
                                    >
                                        {cp.primaryName} <Badge bg="secondary">{cp.frequency}</Badge>
                                    </Button>
                                ))}
                            </div>
                        </>
                    )}
                </Col>
            </Row>
        </Container>
    );
};

export default PersonDetail;
