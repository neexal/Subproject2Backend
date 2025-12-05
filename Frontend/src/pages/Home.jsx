import React, { useEffect, useState } from 'react';
import { Container, Spinner, Alert, Form, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { movieService } from '../services/api';
import MovieCard from '../components/MovieCard';
import CustomPagination from '../components/Pagination';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();
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



    const handleSearch = (e) => {
        e.preventDefault();
        if (searchTerm.trim()) {
            navigate(`/search?q=${encodeURIComponent(searchTerm)}`);
        }
    };

    return (
        <div className="fade-in">
            {/* Hero Section */}
            <div className="hero-section position-relative d-flex align-items-center justify-content-center text-center" style={{ minHeight: '60vh', background: 'radial-gradient(circle at center, #1f2937 0%, #0a0b14 100%)', marginTop: '-70px', paddingTop: '70px' }}>
                <div className="position-absolute w-100 h-100" style={{ background: 'url(https://image.tmdb.org/t/p/original/6KEryM6r98dLzTRm1W9kOc9h77B.jpg) center/cover no-repeat', opacity: 0.2, mixBlendMode: 'overlay' }}></div>
                <Container className="position-relative z-1">
                    <h1 className="display-3 fw-bold mb-4 text-gradient">Unlimited Movies, TV Shows, and More.</h1>
                    <p className="lead text-secondary mb-5">Discover the dark cinematic catalogue.</p>
                    <Form onSubmit={handleSearch} className="d-flex justify-content-center">
                        <div className="glass-panel p-2 d-flex w-100" style={{ maxWidth: '600px' }}>
                            <Form.Control
                                type="text"
                                placeholder="Search for a movie..."
                                className="bg-transparent border-0 text-white shadow-none"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                                style={{ fontSize: '1.1rem' }}
                            />
                            <Button variant="primary" type="submit" className="rounded-pill px-4">Search</Button>
                        </div>
                    </Form>
                </Container>
            </div>

            {/* Content Section */}
            <Container className="py-5">
                <div className="d-flex align-items-center justify-content-between mb-4">
                    <h2 className="mb-0 border-start border-4 border-danger ps-3">Popular Movies</h2>
                </div>

                <div className="bento-grid">
                    {movies.map(movie => (
                        <MovieCard key={movie.movieId} movie={movie} />
                    ))}
                </div>

                {totalPages > 1 && (
                    <div className="mt-5">
                        <CustomPagination page={page} totalPages={totalPages} onPageChange={setPage} />
                    </div>
                )}
            </Container>
        </div>
    );
};

export default Home;
