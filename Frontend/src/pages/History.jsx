import React, { useEffect, useState } from 'react';
import { Container, Tabs, Tab, ListGroup, Spinner, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { frameworkService } from '../services/api';
import { useAuth } from '../context/AuthContext';

const History = () => {
    const { user } = useAuth();
    const [searchHistory, setSearchHistory] = useState([]);
    const [movieBookmarks, setMovieBookmarks] = useState([]);
    const [personBookmarks, setPersonBookmarks] = useState([]);
    const [ratings, setRatings] = useState([]);
    const [notes, setNotes] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!user) return;

        const fetchData = async () => {
            try {
                const [searchRes, movieRes, personRes, ratingRes, notesRes] = await Promise.all([
                    frameworkService.getSearchHistory(user.id),
                    frameworkService.getUserMovieBookmarks(user.id),
                    frameworkService.getUserPersonBookmarks(user.id),
                    frameworkService.getRatingHistory(user.id),
                    frameworkService.getUserNotes(user.id)
                ]);

                setSearchHistory(searchRes.data);
                setMovieBookmarks(movieRes.data);
                setPersonBookmarks(personRes.data);
                setRatings(ratingRes.data);
                setNotes(notesRes.data);
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
        <Container className="py-5 mt-5 fade-in">
            <h1 className="display-4 fw-bold mb-5 text-gradient">User Dashboard</h1>

            <div className="glass-panel p-4">
                <Tabs defaultActiveKey="search" className="mb-4 custom-tabs" variant="pills">
                    <Tab eventKey="search" title="Search History">
                        <div className="d-flex flex-column gap-3">
                            {searchHistory.map(item => (
                                <div key={item.searchId} className="glass-panel p-3 d-flex justify-content-between align-items-center hover-scale">
                                    <span className="text-white fs-5">"{item.queryText}"</span>
                                    <span className="text-secondary small">{new Date(item.executedAt).toLocaleString()}</span>
                                </div>
                            ))}
                            {searchHistory.length === 0 && <p className="text-muted text-center py-5">No search history found.</p>}
                        </div>
                    </Tab>

                    <Tab eventKey="bookmarks" title="Watchlist">
                        <h4 className="text-white mb-4 mt-3">Movies</h4>
                        <div className="bento-grid mb-5">
                            {movieBookmarks.map(item => (
                                <Link key={item.movieId} to={`/movies/${item.movieId}`} className="text-decoration-none">
                                    <div className="glass-panel p-4 h-100 d-flex flex-column justify-content-between hover-scale">
                                        <h5 className="text-white mb-2">{item.primaryTitle}</h5>
                                        <div className="d-flex justify-content-between align-items-end">
                                            <span className="text-secondary">{item.startYear}</span>
                                            <span className="badge bg-primary rounded-pill">Movie</span>
                                        </div>
                                    </div>
                                </Link>
                            ))}
                        </div>
                        {movieBookmarks.length === 0 && <p className="text-muted mb-5">No movie bookmarks.</p>}

                        <h4 className="text-white mb-4">People</h4>
                        <div className="bento-grid">
                            {personBookmarks.map(item => (
                                <Link key={item.personId} to={`/persons/${item.personId}`} className="text-decoration-none">
                                    <div className="glass-panel p-4 h-100 d-flex flex-column justify-content-between hover-scale">
                                        <h5 className="text-white mb-2">{item.primaryName}</h5>
                                        <div className="text-end">
                                            <span className="badge bg-info rounded-pill">Person</span>
                                        </div>
                                    </div>
                                </Link>
                            ))}
                        </div>
                        {personBookmarks.length === 0 && <p className="text-muted">No person bookmarks.</p>}
                    </Tab>

                    <Tab eventKey="ratings" title="Ratings">
                        <div className="bento-grid">
                            {ratings.map(item => (
                                <Link key={item.movieId} to={`/movies/${item.movieId}`} className="text-decoration-none">
                                    <div className="glass-panel p-4 h-100 hover-scale position-relative overflow-hidden">
                                        <div className="position-absolute top-0 end-0 p-3">
                                            <span className="display-6 fw-bold text-gold">{item.rating}</span>
                                            <span className="text-secondary fs-6">/10</span>
                                        </div>
                                        <h5 className="text-white pe-5 mb-3">{item.primaryTitle}</h5>
                                        <small className="text-secondary">Rated on {new Date(item.ratedAt).toLocaleDateString()}</small>
                                    </div>
                                </Link>
                            ))}
                        </div>
                        {ratings.length === 0 && <p className="text-muted text-center py-5">No ratings found.</p>}
                    </Tab>

                    <Tab eventKey="notes" title="My Notes">
                        <div className="d-flex flex-column gap-3">
                            {notes.map((item, idx) => (
                                <div key={idx} className="glass-panel p-4 hover-scale">
                                    <div className="d-flex justify-content-between align-items-start mb-2">
                                        <div>
                                            <span className={`badge ${(item.noteType || item.NoteType) === 'Movie' ? 'bg-primary' : 'bg-info'} me-2`}>
                                                {item.noteType || item.NoteType}
                                            </span>
                                            <h5 className="d-inline text-white">{item.titleOrName || item.TitleOrName}</h5>
                                        </div>
                                        <small className="text-secondary">{new Date(item.createdAt || item.CreatedAt).toLocaleDateString()}</small>
                                    </div>
                                    <p className="text-light opacity-75 mb-0" style={{ whiteSpace: 'pre-wrap' }}>{item.noteBody || item.NoteBody}</p>
                                </div>
                            ))}
                            {notes.length === 0 && <p className="text-muted text-center py-5">No notes found.</p>}
                        </div>
                    </Tab>
                </Tabs>
            </div>
        </Container>
    );
};

export default History;
