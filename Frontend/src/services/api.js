import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const TMDB_API_KEY = import.meta.env.VITE_TMDB_API_KEY;

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});


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
    changePassword: (currentPassword, newPassword) => api.post('/auth/change-password', { currentPassword, newPassword }),
};

export const movieService = {
    getMovies: (page = 1, pageSize = 12) => api.get(`/movies?page=${page}&pageSize=${pageSize}`),
    getMovieById: (id) => api.get(`/movies/${id}`),
    getMovieByTconst: (tconst) => api.get(`/movies/tconst/${tconst}`),
    getMovieDetails: (id) => api.get(`/movies/${id}/details`),
    searchMovies: (term, page = 1, pageSize = 12, userId = null) => api.post('/movies/search', { searchTerm: term, page, pageSize, userId }),


    structuredSearch: (userId, title, plot, character, person) =>
        api.post('/framework/search/structured', { userId, title, plot, character, person }),
    getSimilarMovies: (movieId) => api.get(`/movies/${movieId}/similar`),
    getPopularActors: (movieId) => api.get(`/movies/${movieId}/popular-cast`),
};

export const personService = {
    getPersons: (page = 1, pageSize = 12) => api.get(`/persons?page=${page}&pageSize=${pageSize}`),
    getPersonById: (id) => api.get(`/persons/${id}`),
    getPersonDetails: ((id) => api.get(`/persons/${id}/details`)),
    searchPersons: (term, page = 1, pageSize = 12, userId = null) => api.post('/persons/search', { searchTerm: term, page, pageSize, userId }),
    getPersonRecentMovies: (id) => api.get(`/persons/${id}/recent-movies`),
    getCoPlayers: (name) => api.get(`/persons/name/${encodeURIComponent(name)}/coplayers`),
    getPersonWords: (name) => api.get(`/persons/name/${encodeURIComponent(name)}/words`),
};

export const frameworkService = {

    toggleMovieBookmark: (userId, movieId) => api.post('/framework/bookmarks/movies/toggle', { userId, movieId }),
    togglePersonBookmark: (userId, personId) => api.post('/framework/bookmarks/persons/toggle', { userId, personId }),
    getUserMovieBookmarks: (userId) => api.get(`/framework/bookmarks/movies/${userId}`),
    getUserPersonBookmarks: (userId) => api.get(`/framework/bookmarks/persons/${userId}`),

    addMovieNote: (userId, movieId, note) => api.post('/framework/notes/movies', { userId, movieId, note }),
    addPersonNote: (userId, personId, note) => api.post('/framework/notes/persons', { userId, personId, note }),
    getUserNotes: (userId) => api.get(`/framework/notes/${userId}`),

    rateMovie: (userId, movieId, rating) => api.post('/framework/rate', { userId, movieId, rating }),
    getUserMovieRating: (userId, movieId) => api.get(`/framework/rating/${userId}/${movieId}`),
    getRatingHistory: (userId) => api.get(`/framework/rating-history/${userId}`),


    getSearchHistory: (userId) => api.get(`/framework/search-history/${userId}`),
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
            return null;
        } catch (error) {
            console.error("Error fetching TMDB image:", error);
            return null;
        }
    },
    getMovieImage: async (tconst) => {
        try {
            const findUrl = `https://api.themoviedb.org/3/find/${tconst}?api_key=${TMDB_API_KEY}&external_source=imdb_id`;
            const findResponse = await axios.get(findUrl);

            // Check movies
            if (findResponse.data.movie_results && findResponse.data.movie_results.length > 0) {
                const movie = findResponse.data.movie_results[0];
                if (movie.poster_path) {
                    return `https://image.tmdb.org/t/p/w300${movie.poster_path}`;
                }
            }

            // Check TV shows
            if (findResponse.data.tv_results && findResponse.data.tv_results.length > 0) {
                const tv = findResponse.data.tv_results[0];
                if (tv.poster_path) {
                    return `https://image.tmdb.org/t/p/w300${tv.poster_path}`;
                }
            }

            // Check TV episodes (use still path or show poster if available)
            if (findResponse.data.tv_episode_results && findResponse.data.tv_episode_results.length > 0) {
                const ep = findResponse.data.tv_episode_results[0];
                if (ep.still_path) {
                    return `https://image.tmdb.org/t/p/w300${ep.still_path}`;
                }
            }

            return null;
        } catch (error) {
            console.error("Error fetching TMDB movie image:", error);
            return null;
        }
    },

    getMovieRating: async (tconst) => {
        try {
            const findUrl = `https://api.themoviedb.org/3/find/${tconst}?api_key=${TMDB_API_KEY}&external_source=imdb_id`;
            const findResponse = await axios.get(findUrl);

            // Check movie results for rating
            if (findResponse.data.movie_results && findResponse.data.movie_results.length > 0) {
                const movie = findResponse.data.movie_results[0];
                return movie.vote_average || null;
            }

            // Check TV results
            if (findResponse.data.tv_results && findResponse.data.tv_results.length > 0) {
                const tv = findResponse.data.tv_results[0];
                return tv.vote_average || null;
            }

            return null;
        } catch (error) {
            console.error("Error fetching TMDB movie rating:", error);
            return null;
        }
    }
};

export default api;
