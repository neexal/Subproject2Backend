# IMDB Movie Database Application
## Portfolio Project Report

**Author:** [Your Name]  
**Date:** [Current Date]  
**Organization:** Roskilde University - CIT/P Course  
**Course:** Complex IT Systems - Practice

---

# ABSTRACT

This report describes the development of a Movie Database Application that allows users to browse, search, rate, and bookmark movies and actors. The project was completed in three parts: a database system, a web service backend, and a web-based frontend application.

The database stores movie and actor information from the Internet Movie Database (IMDB). It includes features for searching movies and actors, tracking user bookmarks and ratings, and storing personal notes. The backend provides a secure web service that connects the database to the frontend. The frontend is a modern web application built with React that gives users an easy way to interact with the movie database.

The system successfully allows users to search for movies and actors, view detailed information, rate movies, save bookmarks, and keep track of their search history. All features work together to create a complete movie database experience. The project demonstrates how to build a full-stack web application using modern technologies and best practices.

Key achievements include implementing secure user authentication, creating an intuitive user interface, and successfully integrating all three layers of the application. The system is functional, user-friendly, and ready for further development.

---

# Table of Contents

1. Introduction ............................................................................................................. 8
   1.1 Introduction ................................................................................................ 8
   1.2 Problem Statement ...................................................................................... 9
   1.3 Objectives ................................................................................................... 9
   1.4 Scope and Limitation ................................................................................... 9
       1.4.1 Scope ................................................................................................ 9
       1.4.2 Limitation ......................................................................................... 9
   1.5 Development Methodology ........................................................................... 10
   1.6 Individual Effort ......................................................................................... 11

2. Background Study and Literature Review ............................................................. 12
   2.1 Background Study ...................................................................................... 12
   2.2 Literature Review ....................................................................................... 12

3. System Analysis and Design .................................................................................. 14
   3.1 System Analysis .......................................................................................... 14
       3.1.1 Requirement Identification ............................................................ 14
   3.2 System Design ............................................................................................ 26

4. Implementation ...................................................................................................... 29
   4.1 Database Implementation ............................................................................. 29
   4.2 Backend Implementation .............................................................................. 30
   4.3 Frontend Implementation .............................................................................. 32
   4.4 Tools Used .................................................................................................. 34

5. Conclusion and Future Recommendation ............................................................. 35
   5.1 Future Enhancements .................................................................................. 35
   5.2 Conclusion ................................................................................................. 35

---

# Chapter 1: Introduction

## 1.1 Introduction

This project is about building a Movie Database Application as part of the Complex IT Systems course at Roskilde University. The application helps users explore information about movies and actors from the Internet Movie Database (IMDB).

The project was divided into three main parts. First, we created a database to store movie and actor information. Second, we built a web service that connects the database to the frontend. Third, we developed a web application that users can access through their web browser.

The application allows users to search for movies and actors, view detailed information about them, rate movies, save favorites as bookmarks, and keep notes about movies and actors they are interested in. Users can also see their search history and manage their personal information.

The system uses modern web technologies to create a fast and user-friendly experience. The database uses PostgreSQL, which is a powerful database system. The backend uses ASP.NET Core, which is a framework for building web services. The frontend uses React, which is a popular library for building user interfaces.

This project demonstrates how to build a complete web application from start to finish. It shows how different parts of a system work together to create a useful application for end users.

## 1.2 Problem Statement

Modern users need easy ways to explore large amounts of information. Movie databases contain millions of movies and actors, and users need tools to find what they are looking for quickly and easily.

The main challenges we needed to solve were:

1. How to store large amounts of movie and actor data in an organized way
2. How to allow users to search through this data quickly
3. How to let users save their favorite movies and actors
4. How to enable users to rate movies and share their opinions
5. How to create a user interface that is easy to use and looks good
6. How to keep user information secure and private

We also needed to make sure that different users could have their own bookmarks, ratings, and notes without interfering with each other. The system needed to be fast, reliable, and easy to use.

## 1.3 Objectives

The main goals of this project were:

1. **Database Goals**: Create a well-organized database that can store movie and actor information efficiently. The database should support fast searching and allow users to save personal information like bookmarks and ratings.

2. **Backend Goals**: Build a web service that provides secure access to the database. The service should allow users to search, view details, rate movies, and manage their bookmarks. It should also handle user authentication to keep the system secure.

3. **Frontend Goals**: Create a user-friendly web interface that makes it easy to browse movies and actors. The interface should work well on different devices and provide a pleasant user experience.

4. **Integration Goals**: Make sure all three parts work together smoothly. The frontend should communicate with the backend, and the backend should access the database correctly.

5. **Feature Goals**: Implement advanced features like searching by multiple criteria, finding similar movies, and showing which actors work together frequently.

## 1.4 Scope and Limitation

### 1.4.1 Scope

The project includes the following features:

**Database Features:**
- Storing movie information including titles, years, genres, and ratings
- Storing actor information including names, birth years, and professions
- Supporting user accounts and authentication
- Storing user bookmarks, ratings, and notes
- Tracking user search history
- Providing database functions for searching and analysis

**Backend Features:**
- RESTful API for accessing movie and actor data
- User authentication and authorization
- Search functionality for movies and actors
- Bookmark management
- Rating system
- Note management
- Advanced search with multiple criteria
- Analysis features like finding similar movies and co-actors

**Frontend Features:**
- Home page with movie listings
- Search page for movies and actors
- Movie detail pages with full information
- Actor detail pages with career information
- User registration and login
- User dashboard showing history, bookmarks, and ratings
- Responsive design that works on mobile devices
- Integration with external image service (TMDB)

### 1.4.2 Limitation

The project has the following limitations:

**Data Limitations:**
- Uses a reduced version of the IMDB dataset, not the complete database
- Movie and actor data is read-only (users cannot add or edit)
- Limited to movies and actors, does not include TV shows in detail

**Feature Limitations:**
- No social features like sharing or commenting
- No recommendation system using machine learning
- No video streaming capabilities
- No mobile app version (web only)
- Limited to English language interface

**Technical Limitations:**
- Designed for educational purposes, not production use
- Limited user capacity testing
- Basic security measures suitable for development
- No advanced caching or performance optimization for large scale

**Integration Limitations:**
- Depends on external TMDB API for images
- Requires internet connection for full functionality
- No offline mode

## 1.5 Development Methodology

We used an iterative development approach for this project. This means we built the system in stages, testing and improving each part before moving to the next.

**Phase 1: Database Development**
We started by designing the database structure. We analyzed the provided IMDB data and created a normalized database schema. We then implemented database functions for searching and analysis. This phase took about 40 hours of work.

**Phase 2: Backend Development**
Next, we built the web service backend. We created the data access layer using Entity Framework Core, implemented RESTful API endpoints, and added authentication and security features. This phase took about 50 hours of work.

**Phase 3: Frontend Development**
Finally, we developed the user interface using React. We created all the pages and components, integrated with the backend API, and implemented user authentication. This phase took about 60 hours of work.

**Testing and Integration**
Throughout development, we tested each component as we built it. We also integrated the parts together and fixed any issues that arose. We used manual testing and some automated tests to ensure everything worked correctly.

**Tools and Technologies**
We used modern development tools including:
- PostgreSQL for the database
- ASP.NET Core for the backend
- React for the frontend
- Git for version control
- Visual Studio Code and JetBrains Rider as development environments

## 1.6 Individual Effort

This project was completed by a team of four people. Each team member contributed to different parts of the project:

**Person 1** worked on database functions, backend API integration, security features, and frontend visualization components. This person focused on connecting all the layers together.

**Person 2** worked on database design, API structure, authentication system, and frontend state management. This person focused on the overall architecture and security.

**Person 3** worked on database optimization, backend services, security middleware, and frontend components. This person focused on performance and user interface elements.

**Person 4** worked on frontend architecture, API integration, security token management, and provided input on database design. This person focused on the user experience and overall frontend structure.

All team members collaborated on testing, debugging, and documentation. The total project effort was approximately 150 hours across all team members.

---

# Chapter 2: Background Study and Literature Review

## 2.1 Background Study

Before starting this project, we studied existing movie database applications to understand what features users expect and how similar systems work.

**Internet Movie Database (IMDB)**
IMDB is the largest movie database on the internet. It contains information about millions of movies, TV shows, and actors. Users can search for movies, read reviews, and see ratings. We used a reduced version of IMDB data for our project to learn how such systems work.

**The Movie Database (TMDB)**
TMDB is another popular movie database that provides an API for accessing movie information and images. We integrated TMDB into our project to get movie posters and actor photos, since the IMDB dataset we used did not include images.

**Modern Web Applications**
We studied how modern web applications are built. Most applications today use a three-layer architecture: a database layer for storing data, a backend layer for business logic, and a frontend layer for user interface. This separation makes applications easier to develop and maintain.

**RESTful Web Services**
REST (Representational State Transfer) is a popular way to design web services. REST services use standard HTTP methods like GET and POST, and organize information as resources with unique addresses. This makes APIs easy to understand and use.

**React Framework**
React is a JavaScript library for building user interfaces. It uses a component-based approach where the interface is built from reusable pieces. React makes it easier to create interactive web applications that respond quickly to user actions.

## 2.2 Literature Review

We reviewed academic and technical resources to understand best practices for building web applications.

**Database Design Principles**
Database design books explain how to organize data efficiently using normalization. Normalization helps reduce data duplication and prevents errors. We applied these principles when designing our database schema.

**RESTful API Design**
Literature on RESTful API design explains how to create APIs that are easy to use and understand. Key principles include using clear resource names, proper HTTP methods, and consistent response formats. We followed these principles when designing our backend API.

**User Interface Design**
Usability research, particularly Nielsen's 10 Usability Heuristics, provides guidelines for creating user-friendly interfaces. These heuristics include making system status visible, preventing errors, and maintaining consistency. We applied these principles when designing our frontend.

**Security Best Practices**
Security literature explains the importance of proper authentication and authorization. We learned about JWT tokens, password hashing, and how to protect against common security threats. This knowledge helped us implement secure authentication in our system.

**Modern Web Development**
Resources on modern web development explain how to use frameworks like React and ASP.NET Core effectively. We learned about component architecture, state management, and how to structure applications for maintainability.

---

# Chapter 3: System Analysis and Design

## 3.1 System Analysis

### 3.1.1 Requirement Identification

We identified the requirements for our movie database application by analyzing what users would need and what the project specification required.

**Functional Requirements:**

1. **User Management**
   - Users must be able to create accounts
   - Users must be able to log in and log out
   - Users must be able to change their passwords
   - The system must remember which user is logged in

2. **Movie Browsing**
   - Users must be able to see a list of movies
   - Users must be able to view detailed information about each movie
   - Movie information should include title, year, genre, rating, plot, and cast
   - Movies should be displayed with posters when available

3. **Actor Browsing**
   - Users must be able to see a list of actors
   - Users must be able to view detailed information about each actor
   - Actor information should include name, birth year, known movies, and professions
   - Actors should be displayed with photos when available

4. **Search Functionality**
   - Users must be able to search for movies by title or plot
   - Users must be able to search for actors by name
   - Users must be able to perform advanced searches with multiple criteria
   - Search results should be paginated for easy browsing

5. **Bookmarking**
   - Logged-in users must be able to bookmark movies
   - Logged-in users must be able to bookmark actors
   - Users must be able to view all their bookmarks
   - Users must be able to remove bookmarks

6. **Rating System**
   - Logged-in users must be able to rate movies on a scale of 1 to 10
   - The system must calculate and display average ratings
   - Users must be able to see their own ratings
   - Users must be able to update their ratings

7. **Notes**
   - Logged-in users must be able to add notes to movies
   - Logged-in users must be able to add notes to actors
   - Users must be able to view all their notes
   - Notes should be private to each user

8. **History Tracking**
   - The system must track user search history
   - Users must be able to view their search history
   - Users must be able to view their rating history

9. **Advanced Features**
   - The system should find similar movies based on genres and years
   - The system should show which actors frequently work together
   - The system should display characteristic words for actors
   - The system should show popular actors in movies

**Non-Functional Requirements:**

1. **Performance**
   - Pages should load within a few seconds
   - Search results should appear quickly
   - The system should handle multiple users at the same time

2. **Security**
   - User passwords must be encrypted
   - User sessions must be secure
   - Users should only access their own data

3. **Usability**
   - The interface should be easy to understand
   - The interface should work on different screen sizes
   - Error messages should be clear and helpful

4. **Reliability**
   - The system should handle errors gracefully
   - Data should not be lost
   - The system should work even if some external services fail

## 3.2 System Design

**Architecture Overview**

Our system uses a three-layer architecture:

1. **Database Layer**: Stores all data including movies, actors, and user information
2. **Backend Layer**: Provides web services that access the database and handle business logic
3. **Frontend Layer**: Provides the user interface that users interact with

**Database Design**

The database is organized into several main groups of tables:

**Movie Data Tables:**
- Movie table: Stores basic movie information
- Person table: Stores actor and crew member information
- Genre table: Stores movie genres
- MovieGenre table: Links movies to genres
- CastCredit table: Links actors to movies
- CrewCredit table: Links crew members to movies

**User Framework Tables:**
- AppUser table: Stores user account information
- UserMovieBookmark table: Stores user bookmarks for movies
- UserPersonBookmark table: Stores user bookmarks for actors
- UserTitleRating table: Stores user ratings for movies
- UserTitleNote table: Stores user notes for movies
- UserPersonNote table: Stores user notes for actors
- SearchHistory table: Stores user search history

**Backend Design**

The backend is organized into controllers that handle different types of requests:

- MovieController: Handles requests related to movies
- PersonController: Handles requests related to actors
- AuthController: Handles user authentication
- FrameworkController: Handles user-specific features like bookmarks and ratings

Each controller uses a service layer to access the database. This separation makes the code easier to maintain and test.

**Frontend Design**

The frontend is organized into pages and components:

**Pages:**
- Home page: Shows popular movies
- Search page: Allows users to search
- Movie Detail page: Shows detailed movie information
- Person Detail page: Shows detailed actor information
- Login page: Allows users to log in
- Register page: Allows users to create accounts
- History page: Shows user history and bookmarks

**Components:**
- MovieCard: Displays a movie in a card format
- PersonCard: Displays an actor in a card format
- StarRating: Allows users to rate movies
- NoteModal: Allows users to add notes
- WordCloud: Displays characteristic words for actors
- Pagination: Allows users to navigate through multiple pages

**Security Design**

Security is implemented at multiple levels:

1. **Password Security**: Passwords are hashed using BCrypt before storage
2. **Authentication**: Users receive JWT tokens when they log in
3. **Authorization**: Protected features check for valid tokens
4. **Input Validation**: All user inputs are validated before processing

**Data Flow**

When a user performs an action:

1. The frontend sends a request to the backend
2. The backend validates the request and checks authentication if needed
3. The backend accesses the database to get or update data
4. The backend sends a response back to the frontend
5. The frontend updates the user interface based on the response

---

# Chapter 4: Implementation

## 4.1 Database Implementation

**Database Schema Creation**

We created the database schema using SQL scripts. The schema includes all the tables needed to store movie data and user information. We organized the tables to follow database design best practices.

**Data Migration**

We migrated data from the provided IMDB source tables into our normalized schema. This process involved writing SQL queries to move data from the original format into our new structure. We made sure all data was transferred correctly and no information was lost.

**Database Functions**

We implemented several database functions to support advanced features:

- `string_search()`: Searches movies by title or plot
- `structured_string_search()`: Advanced search with multiple criteria
- `find_coplayers()`: Finds actors who work together frequently
- `person_words()`: Generates word frequency lists for actors
- `rate()`: Handles movie rating and updates averages
- `get_similar_movies()`: Finds movies similar to a given movie
- `get_popular_actors_in_movie()`: Shows actors ordered by popularity

These functions are stored in the database and can be called from the backend. This approach keeps complex logic close to the data, which can improve performance.

**Indexing**

We created indexes on frequently searched columns to improve query performance. Indexes help the database find data faster, especially when searching through large amounts of information.

## 4.2 Backend Implementation

**Data Access Layer**

We used Entity Framework Core to access the database. Entity Framework Core is an Object-Relational Mapping (ORM) tool that allows us to work with the database using C# code instead of writing SQL queries directly.

We created a DataService class that provides methods for accessing movies, actors, and user data. This service handles all database operations and provides a clean interface for the rest of the application.

**Web Service Layer**

We implemented RESTful API endpoints using ASP.NET Core controllers. Each controller handles requests for a specific type of resource:

- Movie endpoints: Get movies, search movies, get movie details
- Person endpoints: Get actors, search actors, get actor details
- Auth endpoints: Login, register, change password
- Framework endpoints: Bookmarks, ratings, notes, history

All endpoints return data in JSON format, which is easy for the frontend to work with.

**Authentication and Security**

We implemented JWT-based authentication. When users log in, they receive a token that proves their identity. This token is included with each request to protected endpoints.

We also implemented password hashing using BCrypt. This ensures that even if someone gains access to the database, they cannot see user passwords.

**Error Handling**

We implemented consistent error handling across all endpoints. When something goes wrong, the system returns a clear error message that helps users understand what happened.

## 4.3 Frontend Implementation

**Component Development**

We built the frontend using React, which allows us to create reusable components. Each component handles a specific part of the user interface. For example, the MovieCard component displays a single movie, and we can use it multiple times to show many movies.

**State Management**

We used React's Context API to manage global state like user authentication. This allows any component to access user information without passing it through many layers.

For component-specific state, we used React's useState hook. This keeps state close to where it is used, making the code easier to understand.

**API Integration**

We created a centralized API service that handles all communication with the backend. This service uses axios to make HTTP requests. We configured interceptors to automatically add authentication tokens to requests.

**User Interface Design**

We designed the interface to be modern and easy to use. We used a glass-morphism design style with semi-transparent panels and smooth animations. The interface is responsive, meaning it works well on both desktop and mobile devices.

**Image Handling**

We integrated with the TMDB API to fetch movie posters and actor photos. When images are not available, we show placeholder images. We also implemented loading states so users know when images are being fetched.

## 4.4 Tools Used

**Database Tools:**
- PostgreSQL: Database management system
- Navicat/pgAdmin: Database administration tools
- SQL: Language for database queries and functions

**Backend Tools:**
- .NET 8.0: Development framework
- ASP.NET Core: Web framework
- Entity Framework Core: Database access framework
- Visual Studio Code / JetBrains Rider: Code editors

**Frontend Tools:**
- React 19.2: User interface library
- React Router: Navigation library
- Bootstrap 5.3: CSS framework
- Axios: HTTP client library
- Vite: Build tool and development server

**Development Tools:**
- Git: Version control
- npm: Package manager for frontend
- NuGet: Package manager for backend
- Postman: API testing tool

**External Services:**
- TMDB API: For movie and actor images

---

# Chapter 5: Conclusion and Future Recommendation

## 5.1 Future Enhancements

There are several ways this project could be improved in the future:

**Enhanced Search Features**
- Add filters for genre, year range, and rating range
- Add sorting options for search results
- Save favorite search queries

**Recommendation System**
- Implement a system that suggests movies based on user preferences
- Show movies similar to ones the user has rated highly
- Recommend actors based on movies the user likes

**Social Features**
- Allow users to share their ratings and reviews
- Enable users to follow other users
- Create discussion forums for movies

**Mobile Application**
- Develop native mobile apps for iOS and Android
- Provide push notifications for new movies
- Enable offline viewing of saved information

**Advanced Analytics**
- Show user statistics like most watched genres
- Display rating trends over time
- Provide insights into user preferences

**Performance Improvements**
- Implement caching to speed up common queries
- Add database replication for better performance
- Optimize image loading and storage

**Internationalization**
- Support multiple languages
- Localize content for different regions
- Handle different date and number formats

## 5.2 Conclusion

This project successfully demonstrates how to build a complete web application from database to user interface. We created a functional movie database system that allows users to explore movies and actors, save favorites, and rate content.

The project taught us valuable lessons about:
- Database design and normalization
- Building RESTful web services
- Creating modern user interfaces
- Implementing security features
- Integrating multiple systems together

All the main requirements from the project specification were met. The system is functional, user-friendly, and demonstrates good software engineering practices. The code is well-organized and can be extended with additional features in the future.

The project shows that with proper planning and good teamwork, it is possible to build a complex system that works well and provides value to users. The experience gained from this project will be valuable for future software development work.

---

# References

1. PostgreSQL Documentation. (2024). PostgreSQL 14 Documentation. https://www.postgresql.org/docs/14/

2. Microsoft. (2024). ASP.NET Core Documentation. https://docs.microsoft.com/en-us/aspnet/core/

3. React Team. (2024). React Documentation. https://react.dev/

4. The Movie Database (TMDB). (2024). TMDB API Documentation. https://developers.themoviedb.org/3

5. Roskilde University. (2025). CIT/P Portfolio Project Requirements. Course Material.

6. Nielsen, J. (1994). Usability Engineering. Morgan Kaufmann.

7. Fielding, R. T. (2000). Architectural Styles and the Design of Network-based Software Architectures. University of California, Irvine.

---

# Appendices

## Appendix A: Database Schema

The database schema includes tables for movies, actors, genres, user accounts, bookmarks, ratings, and notes. All tables are properly normalized and connected through foreign key relationships.

## Appendix B: API Endpoints

The backend provides RESTful API endpoints for accessing movies, actors, user authentication, and user-specific features. All endpoints return JSON data and follow consistent response formats.

## Appendix C: Component Structure

The frontend is organized into pages and reusable components. Components are structured to be maintainable and easy to understand.

## Appendix D: Individual Reflections

See reflections.md for detailed individual reflections from each team member.

---

**End of Report**

