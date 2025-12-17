import React, { useState, useEffect } from 'react';
import { Container, Form, Button, Row, Col, Spinner, Alert, ButtonGroup, ToggleButton } from 'react-bootstrap';
import { useSearchParams } from 'react-router-dom';
import { movieService, personService } from '../services/api';
import MovieCard from '../components/MovieCard';
import PersonCard from '../components/PersonCard';
import CustomPagination from '../components/Pagination';
import { useAuth } from '../context/AuthContext';

const Search = () => {
    const { user } = useAuth();
    const [searchParams, setSearchParams] = useSearchParams();
    const queryParam = searchParams.get('q');

    const [searchTerm, setSearchTerm] = useState(queryParam || '');
    const [searchType, setSearchType] = useState('movies'); // 'movies', 'persons', 'advanced'

    // Advanced Search State
    const [advTitle, setAdvTitle] = useState('');
    const [advPlot, setAdvPlot] = useState('');
    const [advCharacter, setAdvCharacter] = useState('');
    const [advPerson, setAdvPerson] = useState('');

    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const pageSize = 12;

    useEffect(() => {
        if (queryParam) {
            setSearchTerm(queryParam);
            handleSearch(null, queryParam);
        }
    }, [queryParam]);

    const handleSearch = async (e, termOverride) => {
        if (e) e.preventDefault();
        const term = termOverride !== undefined ? termOverride : searchTerm;

        setLoading(true);
        setError(null);
        try {
            const userId = user ? user.id : null;
            let response;
            if (searchType === 'movies') {
                if (!term.trim()) { setLoading(false); return; }
                response = await movieService.searchMovies(term, page, pageSize, userId);
            } else if (searchType === 'persons') {
                if (!term.trim()) { setLoading(false); return; }
                response = await personService.searchPersons(term, page, pageSize, userId);
            } else if (searchType === 'advanced') {
                const userId = user ? user.id : 0;
                response = await movieService.structuredSearch(userId, advTitle, advPlot, advCharacter, advPerson);
                
                // Transform MovieSearchResult to MovieDto format by fetching full movie data
                const transformedResults = await Promise.all(
                    response.data.map(async (result) => {
                        try {
                            // Fetch full movie data using tconst to get movieId
                            const movieResponse = await movieService.getMovieByTconst(result.tconst);
                            return movieResponse.data;
                        } catch (err) {
                            console.error(`Failed to fetch movie for tconst ${result.tconst}:`, err);
                            // Return a minimal object with tconst and title if fetch fails
                            return {
                                movieId: null,
                                tconst: result.tconst,
                                primaryTitle: result.primaryTitle,
                                startYear: null,
                                averageRating: null
                            };
                        }
                    })
                );
                
                // Filter out any results without movieId
                const validResults = transformedResults.filter(m => m.movieId !== null);
                setResults(validResults);
                setTotalPages(1);
                return;
            }

            setResults(response.data.data);
            setTotalPages(response.data.totalPages);
        } catch (err) {
            setError('Search failed. Please try again.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // Trigger search when page changes (basic search only)
    useEffect(() => {
        if (searchType !== 'advanced' && searchTerm) {
            handleSearch(null, searchTerm);
        }
    }, [page]);

    const handleTypeChange = (val) => {
        setSearchType(val);
        setPage(1);
        setResults([]);
        if (val !== 'advanced') {
            setSearchTerm('');
            setSearchParams({});
        }
    };

    return (
        <Container className="py-5 mt-5 fade-in">
            <h1 className="mb-4 text-center display-4 fw-bold">Search</h1>

            <div className="d-flex justify-content-center mb-5">
                <ButtonGroup className="glass-panel p-1">
                    <ToggleButton
                        id="radio-movies"
                        type="radio"
                        variant={searchType === 'movies' ? 'primary' : 'outline-light'}
                        name="radio"
                        value="movies"
                        checked={searchType === 'movies'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                        className="rounded-pill border-0 px-4"
                    >
                        Movies
                    </ToggleButton>
                    <ToggleButton
                        id="radio-persons"
                        type="radio"
                        variant={searchType === 'persons' ? 'primary' : 'outline-light'}
                        name="radio"
                        value="persons"
                        checked={searchType === 'persons'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                        className="rounded-pill border-0 px-4"
                    >
                        People
                    </ToggleButton>
                    <ToggleButton
                        id="radio-advanced"
                        type="radio"
                        variant={searchType === 'advanced' ? 'primary' : 'outline-light'}
                        name="radio"
                        value="advanced"
                        checked={searchType === 'advanced'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                        className="rounded-pill border-0 px-4"
                    >
                        Advanced
                    </ToggleButton>
                </ButtonGroup>
            </div>

            <Form onSubmit={(e) => { setPage(1); handleSearch(e); }} className="mb-5">
                <Row className="justify-content-center">
                    <Col md={8}>
                        {searchType !== 'advanced' ? (
                            <div className="glass-panel p-2 d-flex">
                                <Form.Control
                                    type="text"
                                    placeholder={`Search for ${searchType}...`}
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                    className="bg-transparent border-0 text-white shadow-none"
                                />
                                <Button variant="primary" type="submit" className="rounded-pill px-4">Search</Button>
                            </div>
                        ) : (
                            <div className="glass-panel p-4">
                                <Row className="g-3">
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label className="text-secondary small text-uppercase">Title</Form.Label>
                                            <Form.Control className="bg-dark border-secondary text-white" type="text" value={advTitle} onChange={e => setAdvTitle(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label className="text-secondary small text-uppercase">Plot Keyword</Form.Label>
                                            <Form.Control className="bg-dark border-secondary text-white" type="text" value={advPlot} onChange={e => setAdvPlot(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label className="text-secondary small text-uppercase">Character Name</Form.Label>
                                            <Form.Control className="bg-dark border-secondary text-white" type="text" value={advCharacter} onChange={e => setAdvCharacter(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label className="text-secondary small text-uppercase">Person Name</Form.Label>
                                            <Form.Control className="bg-dark border-secondary text-white" type="text" value={advPerson} onChange={e => setAdvPerson(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col xs={12} className="mt-4">
                                        <Button variant="primary" type="submit" className="w-100 rounded-pill">Advanced Search</Button>
                                    </Col>
                                </Row>
                            </div>
                        )}
                    </Col>
                </Row>
            </Form>

            {loading && (
                <div className="text-center py-5">
                    <Spinner animation="border" variant="primary" />
                </div>
            )}

            {error && <Alert variant="danger">{error}</Alert>}

            <div className="bento-grid">
                {results.map(item => (
                    <div key={searchType === 'persons' ? item.personId : item.movieId}>
                        {searchType === 'persons' ? (
                            <PersonCard person={item} />
                        ) : (
                            <MovieCard movie={item} />
                        )}
                    </div>
                ))}
            </div>

            {results.length > 0 && searchType !== 'advanced' && totalPages > 1 && (
                <div className="mt-5">
                    <CustomPagination page={page} totalPages={totalPages} onPageChange={setPage} />
                </div>
            )}
        </Container>
    );
};

export default Search;
