-- GROUP: cit07, MEMBERS: Abhishek Dangol, Dipu Khatri, Nischal Ghimire, Pema Gyalbu Lama.
-- PostgreSQL Functions for IMDB Movie Application
-- This script contains all the required PostgreSQL functions for the CIT/P Portfolio Project

-- D.1 Register new user
CREATE OR REPLACE FUNCTION register_user(
  p_email TEXT,
  p_username TEXT,
  p_password TEXT
)
RETURNS INT AS $$
DECLARE
  v_user_id INT;
BEGIN
  INSERT INTO AppUser (email, username, password)
  VALUES (p_email, p_username, p_password)
  RETURNING user_id INTO v_user_id;
  
  RETURN v_user_id;
EXCEPTION
  WHEN unique_violation THEN
    RAISE EXCEPTION 'User already exists (email or username is already taken)';
END;
$$ LANGUAGE plpgsql;

-- D.1 Bookmarking Titles and Names
-- a.Movie Bookmark
CREATE OR REPLACE FUNCTION toggle_movie_bookmark(
    p_user_id INT,
    p_movie_id INT
)
RETURNS TEXT AS $$
DECLARE
    v_exists BOOLEAN;
BEGIN
    SELECT TRUE INTO v_exists
    FROM usermoviebookmark
    WHERE user_id=p_user_id AND movie_id=p_movie_id;
    
    IF v_exists THEN
        DELETE FROM usermoviebookmark
        WHERE user_id=p_user_id AND movie_id=p_movie_id;
        RETURN 'Movie bookmark removed';
    ELSE
        INSERT INTO usermoviebookmark (user_id, movie_id)
        VALUES (p_user_id, p_movie_id);
        RETURN 'Movie bookmarked';
    END IF;
END;
$$ LANGUAGE plpgsql;

-- b. Person Bookmark
CREATE OR REPLACE FUNCTION toggle_person_bookmark(
    p_user_id INT,
    p_person_id INT
)
RETURNS TEXT AS $$
DECLARE
    v_exists BOOLEAN;
BEGIN
    SELECT TRUE INTO v_exists
    FROM userpersonbookmark
    WHERE user_id = p_user_id AND person_id = p_person_id;

    IF v_exists THEN
        DELETE FROM userpersonbookmark
        WHERE user_id = p_user_id AND person_id = p_person_id;
        RETURN 'Person bookmark removed';
    ELSE
        INSERT INTO userpersonbookmark (user_id, person_id)
        VALUES (p_user_id, p_person_id);
        RETURN 'Person bookmarked';
    END IF;
END;
$$ LANGUAGE plpgsql;

-- D.1 Adding notes
-- a. Add a note to movie
CREATE OR REPLACE FUNCTION add_movie_note(
    p_user_id INT,
    p_movie_id INT,
    p_note TEXT
)
RETURNS INT AS $$
DECLARE
    v_note_id INT;
BEGIN
    INSERT INTO usertitlenote (user_id, movie_id, note_body)
    VALUES (p_user_id, p_movie_id, p_note)
    RETURNING note_id INTO v_note_id;

    RETURN v_note_id;
END;
$$ LANGUAGE plpgsql;

-- b. Add a note to a person
CREATE OR REPLACE FUNCTION add_person_note(
    p_user_id INT,
    p_person_id INT,
    p_note TEXT
)
RETURNS INT AS $$
DECLARE
    v_note_id INT;
BEGIN
    INSERT INTO userpersonnote (user_id, person_id, note_body)
    VALUES (p_user_id, p_person_id, p_note)
    RETURNING note_id INTO v_note_id;

    RETURN v_note_id;
END;
$$ LANGUAGE plpgsql;

-- D.1 Retrieving Data
-- a. Get user movie bookmark
CREATE OR REPLACE FUNCTION get_user_movie_bookmarks(p_user_id INT)
RETURNS TABLE (
    movie_id INT,
    primary_title TEXT,
    start_year INT,
    bookmarked_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT m.movie_id, m.primary_title, m.start_year, b.bookmarked_at
    FROM usermoviebookmark b
    JOIN movie m ON m.movie_id = b.movie_id
    WHERE b.user_id = p_user_id
    ORDER BY b.bookmarked_at DESC;
END;
$$ LANGUAGE plpgsql;

-- b. Get user person bookmark
CREATE OR REPLACE FUNCTION get_user_person_bookmarks(p_user_id INT)
RETURNS TABLE (
    person_id INT,
    primary_name TEXT,
    bookmarked_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT p.person_id, p.primary_name, b.bookmarked_at
    FROM userpersonbookmark b
    JOIN person p ON p.person_id = b.person_id
    WHERE b.user_id = p_user_id
    ORDER BY b.bookmarked_at DESC;
END;
$$ LANGUAGE plpgsql;

-- c. Get user notes (movies + people)
CREATE OR REPLACE FUNCTION get_user_notes(p_user_id INT)
RETURNS TABLE (
    note_type TEXT,
    title_or_name TEXT,
    note_body TEXT,
    created_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 'Movie' AS note_type, m.primary_title, n.note_body, n.created_at
    FROM usertitlenote n
    JOIN movie m ON m.movie_id = n.movie_id
    WHERE n.user_id = p_user_id
    UNION ALL
    SELECT 'Person', p.primary_name, n.note_body, n.created_at
    FROM userpersonnote n
    JOIN person p ON p.person_id = n.person_id
    WHERE n.user_id = p_user_id
    ORDER BY created_at DESC;
END;
$$ LANGUAGE plpgsql;

-- d. Get user search history
CREATE OR REPLACE FUNCTION get_search_history(p_user_id INT)
RETURNS TABLE (
    search_id INT,
    query_text TEXT,
    executed_at TIMESTAMP,
    results_count INT,
    duration_ms INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT sh.search_id, sh.query_text, sh.executed_at, sh.results_count, sh.duration_ms
    FROM searchhistory sh
    WHERE sh.user_id = p_user_id
    ORDER BY sh.executed_at DESC;
END;
$$ LANGUAGE plpgsql;

-- e. Get user rating history
CREATE OR REPLACE FUNCTION get_rating_history(p_user_id INT)
RETURNS TABLE (
    movie_id INT,
    primary_title TEXT,
    rating NUMERIC(3,1),
    rated_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT m.movie_id, m.primary_title, r.rating, r.rated_at
    FROM usertitlerating r
    JOIN movie m ON m.movie_id = r.movie_id
    WHERE r.user_id = p_user_id
    ORDER BY rated_at DESC;
END;
$$ LANGUAGE plpgsql;

-- D.2 Simple Search
CREATE OR REPLACE FUNCTION string_search(
    p_user_id INT,
    p_search_string TEXT
)
RETURNS TABLE (
    tconst TEXT,
    primary_title TEXT
) AS $$
DECLARE
    v_count INT;
BEGIN
    -- 1. Perform the search
    RETURN QUERY
    SELECT m.tconst, m.primary_title
    FROM movie m
    WHERE m.primary_title ILIKE '%' || p_search_string || '%'
       OR m.plot_summary ILIKE '%' || p_search_string || '%';

    -- 2. Count how many results were found
    SELECT COUNT(*) INTO v_count
    FROM movie m
    WHERE m.primary_title ILIKE '%' || p_search_string || '%'
       OR m.plot_summary ILIKE '%' || p_search_string || '%';

    -- 3. Log the search in SearchHistory
    INSERT INTO searchhistory (user_id, query_text, results_count, executed_at)
    VALUES (p_user_id, p_search_string, v_count, NOW());

END;
$$ LANGUAGE plpgsql;

-- D.3 Title Rating
CREATE OR REPLACE FUNCTION rate(
    p_user_id INT,
    p_movie_id INT,
    p_rating INT
)
RETURNS TEXT AS $$
DECLARE
    v_exists BOOLEAN;
    v_avg NUMERIC(3,1);
    v_votes INT;
BEGIN
    -- 1. Validate input
    IF p_rating < 1 OR p_rating > 10 THEN
        RAISE EXCEPTION 'Rating must be between 1 and 10';
    END IF;

    -- 2. Check if this user has already rated the movie
    SELECT TRUE INTO v_exists
    FROM usertitlerating
    WHERE user_id = p_user_id AND movie_id = p_movie_id;

    IF v_exists THEN
        -- Update existing rating
        UPDATE usertitlerating
        SET rating = p_rating, rated_at = NOW()
        WHERE user_id = p_user_id AND movie_id = p_movie_id;
    ELSE
        -- Insert new rating
        INSERT INTO usertitlerating (user_id, movie_id, rating, rated_at)
        VALUES (p_user_id, p_movie_id, p_rating, NOW());
    END IF;

    -- 3. Recalculate the new average rating and number of votes
    SELECT ROUND(AVG(rating)::NUMERIC,1), COUNT(*)
    INTO v_avg, v_votes
    FROM usertitlerating
    WHERE movie_id = p_movie_id;

    -- 4. Update (or insert) into ImdbRating table
    INSERT INTO imdbrating (movie_id, average, votes)
    VALUES (p_movie_id, v_avg, v_votes)
    ON CONFLICT (movie_id)
    DO UPDATE SET average = EXCLUDED.average, votes = EXCLUDED.votes;

    RETURN 'Movie ' || p_movie_id || ' rated ' || p_rating || '/10 by user ' || p_user_id ||
           '. New average: ' || to_char(v_avg, 'FM999D0') || ' (' || v_votes || ' votes)';
END;
$$ LANGUAGE plpgsql;

-- D.4 Structured string search
CREATE OR REPLACE FUNCTION structured_string_search(
    p_user_id   INT,
    p_title     TEXT,
    p_plot      TEXT, 
    p_character TEXT,
    p_person    TEXT
)
RETURNS TABLE (
    tconst TEXT,
    primary_title TEXT
) AS $$
BEGIN
    INSERT INTO searchhistory(user_id,query_text, executed_at)
    VALUES (p_user_id, CONCAT_WS(' | ', p_title, p_plot, p_character, p_person), NOW());

    RETURN QUERY
    SELECT DISTINCT m.tconst, m.primary_title
    FROM movie m
    LEFT JOIN castcredit    cc  ON cc.movie_id = m.movie_id
    LEFT JOIN castcharacter cch ON cch.cast_id  = cc.cast_id
    LEFT JOIN person        per ON per.person_id = cc.person_id
    WHERE
        (p_title     IS NULL OR p_title     = '' OR m.primary_title  ILIKE '%' || p_title     || '%')
    AND (p_plot      IS NULL OR p_plot      = '' OR m.plot_summary   ILIKE '%' || p_plot      || '%')
    AND (p_character IS NULL OR p_character = '' OR cch.character_name ILIKE '%' || p_character || '%')
    AND (p_person    IS NULL OR p_person    = '' OR per.primary_name   ILIKE '%' || p_person    || '%');
END;
$$ LANGUAGE plpgsql;

-- D.5 Finding names
CREATE OR REPLACE FUNCTION find_name(
    p_user_id INT,
    p_search_string TEXT
)
RETURNS TABLE (
    nconst TEXT,
    primary_name TEXT
) AS $$
DECLARE
    v_count INT;
BEGIN
    -- 1. Search for matching names (case-insensitive substring)
    RETURN QUERY
    SELECT p.nconst, p.primary_name
    FROM person p
    WHERE LOWER(p.primary_name) LIKE LOWER('%' || p_search_string || '%');

    -- 2. Count matches
    SELECT COUNT(*) INTO v_count
    FROM person p
    WHERE LOWER(p.primary_name) LIKE LOWER('%' || p_search_string || '%');

    -- 3. Log the search in the framework (SearchHistory)
    INSERT INTO searchhistory (user_id, query_text, results_count, executed_at)
    VALUES (p_user_id, FORMAT('name="%s"', p_search_string), v_count, NOW());
END;
$$ LANGUAGE plpgsql;

-- D.6 Finding co-players
-- Helping view
CREATE OR REPLACE VIEW vw_cast AS
SELECT 
    m.movie_id,
    m.tconst,
    m.primary_title,
    p.person_id,
    p.nconst,
    p.primary_name
FROM castcredit c
JOIN movie m ON m.movie_id = c.movie_id
JOIN person p ON p.person_id = c.person_id;

-- Main function
CREATE OR REPLACE FUNCTION find_coplayers(p_actor_name TEXT)
RETURNS TABLE (
    nconst TEXT,
    primary_name TEXT,
    frequency INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        co.nconst,
        co.primary_name,
        COUNT(DISTINCT co.movie_id)::INT AS frequency
    FROM (
        SELECT DISTINCT v1.movie_id, v1.person_id
        FROM vw_cast v1
        WHERE LOWER(v1.primary_name) LIKE LOWER('%' || p_actor_name || '%')
    ) actor_movies
    JOIN vw_cast co ON co.movie_id = actor_movies.movie_id
    WHERE co.person_id <> actor_movies.person_id
    GROUP BY co.nconst, co.primary_name
    ORDER BY frequency DESC, co.primary_name
    LIMIT 50;
END;
$$ LANGUAGE plpgsql;

-- D.7 Name rating
-- Create a new table to store the derived ratings
CREATE TABLE IF NOT EXISTS PersonRating (
    person_id INT PRIMARY KEY REFERENCES Person(person_id) ON DELETE CASCADE,
    weighted_average NUMERIC(4,2),
    total_movies INT,
    last_updated TIMESTAMP DEFAULT NOW()
);

-- update_person_ratings()
CREATE OR REPLACE FUNCTION update_person_ratings()
RETURNS VOID AS $$
BEGIN
    DELETE FROM personrating;

    INSERT INTO personrating (person_id, weighted_average, total_movies, last_updated)
    SELECT
        p.person_id,
        ROUND(SUM(ir.average * ir.votes)::NUMERIC / NULLIF(SUM(ir.votes), 0), 2) AS weighted_average,
        COUNT(DISTINCT m.movie_id) AS total_movies,
        NOW()
    FROM person p
    JOIN castcredit cc ON cc.person_id = p.person_id
    JOIN movie m ON m.movie_id = cc.movie_id
    JOIN imdbrating ir ON ir.movie_id = m.movie_id
    WHERE ir.average IS NOT NULL AND ir.votes > 0
    GROUP BY p.person_id;
END;
$$ LANGUAGE plpgsql;

-- D.8 Popular actors
CREATE OR REPLACE FUNCTION get_popular_actors_in_movie(
    p_movie_id INT
)
RETURNS TABLE (
    nconst TEXT,
    primary_name TEXT,
    weighted_average NUMERIC(3,2),
    total_movies INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.nconst,
        p.primary_name,
        pr.weighted_average,
        pr.total_movies
    FROM castcredit cc
    JOIN person p ON p.person_id = cc.person_id
    LEFT JOIN personrating pr ON pr.person_id = p.person_id
    WHERE cc.movie_id = p_movie_id
    ORDER BY pr.weighted_average DESC NULLS LAST, pr.total_movies DESC;
END;
$$ LANGUAGE plpgsql;

-- D.9. similar movies
-- similarity formula = 10 * shared_genres - ABS(year_diff)
CREATE OR REPLACE FUNCTION similar_movies_by_genre_year (p_movie_id INT)
RETURNS TABLE (
    tconst TEXT,
    primary_title TEXT,
    shared_genres INT,
    year_diff INT,
    similarity_score INT
) AS $$
BEGIN
  RETURN QUERY
  SELECT 
    m2.tconst, 
    m2.primary_title, 
    COUNT(*)::INT AS shared_genres, 
    ABS(m1.start_year - m2.start_year) AS year_diff,
    (COUNT(*) * 10 - ABS(m1.start_year - m2.start_year))::INT AS similarity_score
  FROM moviegenre mg1 
  JOIN moviegenre mg2 ON mg1.genre_id = mg2.genre_id 
  JOIN movie m1 ON m1.movie_id = mg1.movie_id
  JOIN movie m2 ON m2.movie_id = mg2.movie_id
  WHERE mg1.movie_id = p_movie_id 
    AND m2.movie_id <> p_movie_id
  GROUP BY m2.tconst, m2.primary_title, m1.start_year, m2.start_year
  ORDER BY similarity_score DESC
  LIMIT 10;
END;
$$ LANGUAGE plpgsql;

-- D.10 Frequent person words:
CREATE OR REPLACE FUNCTION person_words(person_name TEXT, top_n INT DEFAULT 20)
RETURNS TABLE (word TEXT, frequency INT) AS $$
BEGIN
  RETURN QUERY
  WITH person_movies AS (
    SELECT DISTINCT m.movie_id
    FROM castcredit cc
    JOIN movie m ON cc.movie_id = m.movie_id
    JOIN person p ON p.person_id = cc.person_id
    WHERE p.primary_name = person_name
  ), 
  raw_words AS (
    SELECT lower(wi.word) AS word
    FROM wi
    JOIN movie m ON m.tconst = wi.tconst
    JOIN person_movies pm ON pm.movie_id = m.movie_id
    WHERE wi.field IN ('p', 'c')
  )
  SELECT rw.word, COUNT(*)::INT AS frequency
  FROM raw_words rw
  WHERE rw.word NOT IN (
    'the','a','an','and','of','in','on','to','for','with','by','from',
    'is','are','be','one','film'
  )
  GROUP BY rw.word
  ORDER BY frequency DESC, rw.word ASC
  LIMIT top_n;
END;
$$ LANGUAGE plpgsql;

-- D.11 Exact Matching Query
CREATE OR REPLACE FUNCTION exact_match_titles(VARIADIC keywords TEXT[])
RETURNS TABLE (tconst TEXT, primary_title TEXT) AS $$
BEGIN
  RETURN QUERY
  WITH kws AS (
    SELECT array_agg(DISTINCT lower(btrim(k))) AS arr
    FROM unnest(keywords) AS u(k)
    WHERE k IS NOT NULL AND btrim(k) <> ''
  ),
  params AS (
    SELECT cardinality(arr) AS n FROM kws
  )
  SELECT m.tconst, m.primary_title
  FROM wi
  JOIN movie m ON m.tconst = wi.tconst
  CROSS JOIN kws
  CROSS JOIN params
  WHERE lower(wi.word) = ANY (kws.arr)
  GROUP BY m.tconst, m.primary_title, params.n
  HAVING COUNT(DISTINCT lower(wi.word)) = params.n
  ORDER BY m.primary_title;
END;
$$ LANGUAGE plpgsql;

-- D.12 Best match query
CREATE OR REPLACE FUNCTION best_match_titles(VARIADIC keywords TEXT[])
RETURNS TABLE (
  tconst TEXT,
  primary_title TEXT,
  match_count INT
) AS $$
BEGIN
  RETURN QUERY
  WITH kws AS (
    SELECT array_agg(DISTINCT lower(btrim(k))) AS arr
    FROM unnest(keywords) AS u(k)
    WHERE k IS NOT NULL AND btrim(k) <> ''
  )
  SELECT m.tconst,
         m.primary_title,
         COUNT(DISTINCT lower(wi.word))::INT AS match_count
  FROM wi
  JOIN movie m ON m.tconst = wi.tconst,
       kws
  WHERE lower(wi.word) = ANY (kws.arr)
  GROUP BY m.tconst, m.primary_title
  ORDER BY match_count DESC, m.primary_title;
END;
$$ LANGUAGE plpgsql;

-- D.13 Word to Word query
CREATE OR REPLACE FUNCTION keyword_expansion_words(VARIADIC keywords TEXT[])
RETURNS TABLE (word TEXT, freq INT) AS $$
BEGIN
  RETURN QUERY
  WITH kws AS (
    SELECT array_agg(DISTINCT lower(btrim(k))) AS arr
    FROM unnest(keywords) AS u(k)
    WHERE k IS NOT NULL AND btrim(k) <> ''
  ),
  params AS (
    SELECT cardinality(arr) AS n FROM kws
  ),
  matches AS ( 
    SELECT wi.tconst
    FROM wi
    CROSS JOIN kws
    GROUP BY wi.tconst, kws.arr
    HAVING COUNT(DISTINCT lower(wi.word)) FILTER (WHERE lower(wi.word) = ANY (kws.arr)) = (SELECT n FROM params)
  )
  SELECT lower(wi.word) AS word,
         COUNT(*)::INT  AS freq
  FROM wi
  JOIN matches USING (tconst)
  CROSS JOIN kws
  WHERE lower(wi.word) <> ALL (kws.arr)
  GROUP BY lower(wi.word)
  ORDER BY freq DESC, word
  LIMIT 100; 
END;
$$ LANGUAGE plpgsql;
