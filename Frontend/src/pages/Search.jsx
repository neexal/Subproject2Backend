import React, { useState } from 'react';
import { Container, Form, Button, Row, Col, Spinner, Alert, ButtonGroup, ToggleButton } from 'react-bootstrap';
import { movieService, personService } from '../services/api';
import MovieCard from '../components/MovieCard';
import PersonCard from '../components/PersonCard';
import CustomPagination from '../components/Pagination';
import { useAuth } from '../context/AuthContext';

const Search = () => {
    const { user } = useAuth();
    const [searchTerm, setSearchTerm] = useState('');
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

    const handleSearch = async (e) => {
        if (e) e.preventDefault();

        setLoading(true);
        setError(null);
        try {
            let response;
            if (searchType === 'movies') {
                if (!searchTerm.trim()) return;
                response = await movieService.searchMovies(searchTerm, page, pageSize);
            } else if (searchType === 'persons') {
                if (!searchTerm.trim()) return;
                response = await personService.searchPersons(searchTerm, page, pageSize);
            } else if (searchType === 'advanced') {
                // Advanced search requires login for history tracking in backend usually, 
                // but we'll pass 0 if not logged in or handle as needed.
                // The backend signature is structuredSearch(userId, ...).
                const userId = user ? user.id : 0;
                response = await movieService.structuredSearch(userId, advTitle, advPlot, advCharacter, advPerson);
                // Structured search might not return paged response in the same format, 
                // assuming it returns a list directly based on backend analysis.
                // If backend returns List<MovieSearchResult>, we wrap it.
                setResults(response.data);
                setTotalPages(1); // No pagination for advanced search in this iteration
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

    // Trigger search when page changes (only for basic search)
    React.useEffect(() => {
        if (searchType !== 'advanced' && searchTerm) {
            handleSearch();
        }
    }, [page]);

    // Reset page when search term or type changes
    const handleTypeChange = (val) => {
        setSearchType(val);
        setPage(1);
        setResults([]);
        setSearchTerm('');
    };

    return (
        <Container className="py-5">
            <h1 className="mb-4 text-center">Search</h1>

            <div className="d-flex justify-content-center mb-4">
                <ButtonGroup>
                    <ToggleButton
                        id="radio-movies"
                        type="radio"
                        variant="outline-primary"
                        name="radio"
                        value="movies"
                        checked={searchType === 'movies'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                    >
                        Movies
                    </ToggleButton>
                    <ToggleButton
                        id="radio-persons"
                        type="radio"
                        variant="outline-primary"
                        name="radio"
                        value="persons"
                        checked={searchType === 'persons'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                    >
                        People
                    </ToggleButton>
                    <ToggleButton
                        id="radio-advanced"
                        type="radio"
                        variant="outline-primary"
                        name="radio"
                        value="advanced"
                        checked={searchType === 'advanced'}
                        onChange={(e) => handleTypeChange(e.currentTarget.value)}
                    >
                        Advanced
                    </ToggleButton>
                </ButtonGroup>
            </div>

            <Form onSubmit={(e) => { setPage(1); handleSearch(e); }} className="mb-5">
                <Row className="justify-content-center">
                    <Col md={8}>
                        {searchType !== 'advanced' ? (
                            <div className="d-flex gap-2">
                                <Form.Control
                                    type="text"
                                    placeholder={`Search for ${searchType}...`}
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                                <Button variant="primary" type="submit">Search</Button>
                            </div>
                        ) : (
                            <div className="p-4 border rounded bg-dark text-white">
                                <Row className="g-3">
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label>Title</Form.Label>
                                            <Form.Control type="text" value={advTitle} onChange={e => setAdvTitle(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label>Plot Keyword</Form.Label>
                                            <Form.Control type="text" value={advPlot} onChange={e => setAdvPlot(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label>Character Name</Form.Label>
                                            <Form.Control type="text" value={advCharacter} onChange={e => setAdvCharacter(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col md={6}>
                                        <Form.Group>
                                            <Form.Label>Person Name</Form.Label>
                                            <Form.Control type="text" value={advPerson} onChange={e => setAdvPerson(e.target.value)} />
                                        </Form.Group>
                                    </Col>
                                    <Col xs={12}>
                                        <Button variant="primary" type="submit" className="w-100">Advanced Search</Button>
                                    </Col>
                                </Row>
                            </div>
                        )}
                    </Col>
                </Row>
            </Form>

            {loading && (
                <div className="text-center">
                    <Spinner animation="border" />
                </div>
            )}

            {error && <Alert variant="danger">{error}</Alert>}

            <Row xs={1} md={2} lg={4} className="g-4">
                {results.map(item => (
                    <Col key={searchType === 'persons' ? item.personId : item.movieId}>
                        {searchType === 'persons' ? (
                            <PersonCard person={item} />
                        ) : (
                            <MovieCard movie={item} />
                        )}
                    </Col>
                ))}
            </Row>

            {results.length > 0 && searchType !== 'advanced' && (
                <CustomPagination page={page} totalPages={totalPages} onPageChange={setPage} />
            )}
        </Container>
    );
};

export default Search;
