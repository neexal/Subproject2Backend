import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Spinner, Alert } from 'react-bootstrap';
import { movieService } from '../services/api';
import MovieCard from '../components/MovieCard';
import CustomPagination from '../components/Pagination';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const pageSize = 12;

    useEffect(() => {
        const fetchMovies = async () => {
            setLoading(true);
            try {
                const response = await movieService.getMovies(page, pageSize);
                setMovies(response.data.data);
                setTotalPages(response.data.totalPages);
            } catch (err) {
                setError('Failed to fetch movies. Please try again later.');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchMovies();
    }, [page]);

    if (loading) return (
        <Container className="d-flex justify-content-center align-items-center" style={{ minHeight: '80vh' }}>
            <Spinner animation="border" role="status">
                <span className="visually-hidden">Loading...</span>
            </Spinner>
        </Container>
    );

    if (error) return (
        <Container className="mt-5">
            <Alert variant="danger">{error}</Alert>
        </Container>
    );

    return (
        <Container className="py-5">
            <h1 className="mb-4 text-center">Popular Movies</h1>
            <Row xs={1} md={2} lg={4} className="g-4">
                {movies.map(movie => (
                    <Col key={movie.movieId}>
                        <MovieCard movie={movie} />
                    </Col>
                ))}
            </Row>
            <CustomPagination page={page} totalPages={totalPages} onPageChange={setPage} />
        </Container>
    );
};

export default Home;
