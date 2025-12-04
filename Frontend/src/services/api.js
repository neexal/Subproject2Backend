import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const TMDB_API_KEY = import.meta.env.VITE_TMDB_API_KEY;

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Add a request interceptor to attach the token
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

export const authService = {
    login: (email, password) => api.post('/auth/login', { email, password }),
    register: (username, email, password) => api.post('/auth/register', { username, email, password }),
};

export const movieService = {
    getMovies: (page = 1, pageSize = 12) => api.get(`/movies?page=${page}&pageSize=${pageSize}`),
    getMovieById: (id) => api.get(`/movies/${id}`),
    getMovieDetails: (id) => api.get(`/movies/${id}/details`),
    searchMovies: (searchTerm, page = 1, pageSize = 12) => api.post('/movies/search', { searchTerm, page, pageSize }),

    // Phase 2 methods
    structuredSearch: (userId, title, plot, character, person) =>
        api.post('/movies/search/structured', { userId, title, plot, character, person }),
    getSimilarMovies: (movieId) => api.get(`/movies/${movieId}/similar`),
    getPopularActors: (movieId) => api.get(`/movies/${movieId}/popular-actors`),
};

export const personService = {
    getPersons: (page = 1, pageSize = 12) => api.get(`/persons?page=${page}&pageSize=${pageSize}`),
    getPersonById: (id) => api.get(`/persons/${id}`),
    getPersonDetails: (id) => api.get(`/persons/${id}/details`),
    searchPersons: (searchTerm, page = 1, pageSize = 12) => api.post('/persons/search', { searchTerm, page, pageSize }),

    // Phase 2 methods
    getCoPlayers: (actorName) => api.get(`/persons/coplayers/${actorName}`),
};

export const frameworkService = {
    // Bookmarks
    toggleMovieBookmark: (userId, movieId) => api.post('/framework/bookmark/movie', { userId, movieId }),
    togglePersonBookmark: (userId, personId) => api.post('/framework/bookmark/person', { userId, personId }),
    getUserMovieBookmarks: (userId) => api.get(`/framework/bookmarks/movie/${userId}`),
    getUserPersonBookmarks: (userId) => api.get(`/framework/bookmarks/person/${userId}`),

    // Ratings
    rateMovie: (userId, movieId, rating) => api.post('/framework/rate', { userId, movieId, rating }),
    getRatingHistory: (userId) => api.get(`/framework/ratings/${userId}`),

    // History
    getSearchHistory: (userId) => api.get(`/framework/history/${userId}`),
};

export const tmdbService = {
    getPersonImage: async (nconst) => {
        try {
            const findUrl = `https://api.themoviedb.org/3/find/${nconst}?api_key=${TMDB_API_KEY}&external_source=imdb_id`;
            const findResponse = await axios.get(findUrl);

            if (findResponse.data.person_results && findResponse.data.person_results.length > 0) {
                const person = findResponse.data.person_results[0];
                if (person.profile_path) {
                    return `https://image.tmdb.org/t/p/w200${person.profile_path}`;
                }
            }
            return null; // No image found
        } catch (error) {
            console.error("Error fetching TMDB image:", error);
            return null;
        }
    }
};

export default api;
