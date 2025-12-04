import React, { useEffect, useState } from 'react';
import { Container, Tabs, Tab, ListGroup, Spinner, Alert } from 'react-bootstrap';
import { frameworkService } from '../services/api';
import { useAuth } from '../context/AuthContext';

const History = () => {
    const { user } = useAuth();
    const [searchHistory, setSearchHistory] = useState([]);
    const [movieBookmarks, setMovieBookmarks] = useState([]);
    const [personBookmarks, setPersonBookmarks] = useState([]);
    const [ratings, setRatings] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!user) return;

        const fetchData = async () => {
            try {
                const [searchRes, movieRes, personRes, ratingRes] = await Promise.all([
                    frameworkService.getSearchHistory(user.id),
                    frameworkService.getUserMovieBookmarks(user.id),
                    frameworkService.getUserPersonBookmarks(user.id),
                    frameworkService.getRatingHistory(user.id)
                ]);

                setSearchHistory(searchRes.data);
                setMovieBookmarks(movieRes.data);
                setPersonBookmarks(personRes.data);
                setRatings(ratingRes.data);
            } catch (err) {
                console.error("Failed to fetch history data", err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [user]);

    if (!user) return <Container className="py-5"><Alert variant="warning">Please login to view history.</Alert></Container>;
    if (loading) return <Container className="text-center py-5"><Spinner animation="border" /></Container>;

    return (
        <Container className="py-5">
            <h1 className="mb-4">My History</h1>
            <Tabs defaultActiveKey="search" className="mb-3">
                <Tab eventKey="search" title="Search History">
                    <ListGroup>
                        {searchHistory.map(item => (
                            <ListGroup.Item key={item.searchId} className="d-flex justify-content-between align-items-center">
                                <span>Searched for: <strong>{item.queryText}</strong></span>
                                <small className="text-muted">{new Date(item.executedAt).toLocaleString()}</small>
                            </ListGroup.Item>
                        ))}
                        {searchHistory.length === 0 && <p className="text-muted mt-3">No search history found.</p>}
                    </ListGroup>
                </Tab>
                <Tab eventKey="bookmarks" title="Bookmarks">
                    <h5 className="mt-3">Movies</h5>
                    <ListGroup className="mb-4">
                        {movieBookmarks.map(item => (
                            <ListGroup.Item key={item.movieId}>
                                <a href={`/movies/${item.movieId}`}>{item.movieTitle}</a>
                            </ListGroup.Item>
                        ))}
                        {movieBookmarks.length === 0 && <p className="text-muted">No movie bookmarks.</p>}
                    </ListGroup>

                    <h5>People</h5>
                    <ListGroup>
                        {personBookmarks.map(item => (
                            <ListGroup.Item key={item.personId}>
                                <a href={`/persons/${item.personId}`}>{item.personName}</a>
                            </ListGroup.Item>
                        ))}
                        {personBookmarks.length === 0 && <p className="text-muted">No person bookmarks.</p>}
                    </ListGroup>
                </Tab>
                <Tab eventKey="ratings" title="Ratings">
                    <ListGroup>
                        {ratings.map(item => (
                            <ListGroup.Item key={item.movieId} className="d-flex justify-content-between align-items-center">
                                <span>
                                    Rated <a href={`/movies/${item.movieId}`}>{item.movieTitle}</a>: <strong>{item.rating}/10</strong>
                                </span>
                                <small className="text-muted">{new Date(item.ratedAt).toLocaleDateString()}</small>
                            </ListGroup.Item>
                        ))}
                        {ratings.length === 0 && <p className="text-muted mt-3">No ratings found.</p>}
                    </ListGroup>
                </Tab>
            </Tabs>
        </Container>
    );
};

export default History;
