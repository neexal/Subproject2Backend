# Individual Reflections (Frontend Team)

## Person 1: Core Navigation & Home Experience
**Role:** Frontend Developer (UI Layout & Routing)
**Focus:** `Home.jsx`, `Navbar.jsx`, Routing

My primary contribution was architecting the initial layout and the application's landing experience. I implemented the `Home.jsx` component, designing the hero section to immediately grab the user's attention. A specific challenge was ensuring that the "Recent Searches" functionality on the home page felt dynamic. I had to read from the local storage/API to populate this list dynamically, ensuring that users could quickly jump back into their previous exploration contexts.

I also managed the global navigation state in `App.jsx` and `Navbar.jsx`. I ensured that the routing between the Home, Search, and Detail pages was seamless. I implemented active state styling for the navigation links so users always know where they are in the application hierarchy. I ensured the layout remained responsive, adjusting the navbar menu for smaller screens using CSS media queries.

## Person 2: Profile Visualization & Assets
**Role:** Frontend Developer (Visualization Specialist)
**Focus:** `PersonDetail.jsx`, `PersonCard.jsx`

I focused on how we present actor profiles and visual assets. My standout task was implementing the "Word Cloud" visualization in `PersonDetail.jsx`. This required fetching a unique dataset from our `GetPersonWords` endpoint and rendering it in a way that visually represented the actor's career themes. It was a challenge to handle the asynchronous data loading for this component without blocking the main render of the profile page.

Additionally, I tackled the "Broken Image" issue that was plaguing our cards. In `PersonCard.jsx`, I wrote logic to detect when a profile path was missing or null and swapped it with a local `no-poster.png` asset. I also worked on the `CoPlayers` section, ensuring that the list of related actors was clickable and correctly routed the user to those new profiles, effectively creating an infinite browsing loop.

## Person 3: Movie Interactions & Modals
**Role:** Frontend Developer (Interaction Design)
**Focus:** `MovieDetail.jsx`, `NoteModal.jsx`

My role centered on user interactions within the data pages, specifically for movies. I built the `NoteModal.jsx` component from scratch. The complexity here was not just the UI, but the communication with the parent component. I had to ensure that when a user saved a note, the modal validated the input before attempting to send data to the backend, and then provided immediate feedback (closing and showing a success state) upon completion.

I also integrated the star rating system in `MovieDetail.jsx`. I had to map the numerical rating from the API (0-10) to a visual representation of stars, ensuring it handled fractional ratings gracefully. I also managed the "Add to Bookmarks" state, ensuring the UI updated instantly when a user toggled the bookmark button, providing a snappy, responsive feel.

## Person 4: Search Logic & State Management
**Role:** Frontend Developer (Logic & State)
**Focus:** `Search.jsx`, `api.js`

I was responsible for the heavy lifting regarding data retrieval and search state. I built the `Search.jsx` page, which had to handle complex state changes. I implemented the logic to listen to URL query parameters, so if a user refreshed the results page, the search would persist. I also managed the loading states (spinners) to ensure the user wasn't staring at a blank screen while we fetched data from the backend.

I centralized our API calls in `api.js`. This was a crucial structural decision. Instead of having `fetch` calls scattered across every component, I created reusable functions like `searchMovies`, `getMovieDetails`, and `addNote`. This made error handling much easier; if an API endpoint changed, I only had to update it in one place. I also improved the error messages displayed to the user when a search returned zero results.
