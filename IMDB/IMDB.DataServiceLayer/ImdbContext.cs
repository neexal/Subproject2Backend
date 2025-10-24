using Microsoft.EntityFrameworkCore;
using IMDB.DataServiceLayer.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace IMDB.DataServiceLayer;

public class ImdbContext : DbContext
{
    // Core entities
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Profession> Professions { get; set; }
    
    // Association entities
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<PersonProfession> PersonProfessions { get; set; }
    public DbSet<PersonKnownFor> PersonKnownFor { get; set; }
    
    // Credit entities
    public DbSet<CastCredit> CastCredits { get; set; }
    public DbSet<CrewCredit> CrewCredits { get; set; }
    public DbSet<CastCharacter> CastCharacters { get; set; }
    
    // Alternative title entities
    public DbSet<AlternativeTitle> AlternativeTitles { get; set; }
    public DbSet<AltTitleType> AltTitleTypes { get; set; }
    public DbSet<AltTitleAttribute> AltTitleAttributes { get; set; }
    
    // Episode and rating entities
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<ImdbRating> ImdbRatings { get; set; }
    
    // Framework entities
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<UserTitleRating> UserTitleRatings { get; set; }
    public DbSet<UserMovieBookmark> UserMovieBookmarks { get; set; }
    public DbSet<UserPersonBookmark> UserPersonBookmarks { get; set; }
    public DbSet<UserTitleNote> UserTitleNotes { get; set; }
    public DbSet<UserPersonNote> UserPersonNotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=imdb;Username=postgres;Password=admin");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // AppUser configuration
        modelBuilder.Entity<AppUser>().ToTable("appuser");
        modelBuilder.Entity<AppUser>().HasKey(u => u.UserId);
        modelBuilder.Entity<AppUser>().Property(u => u.UserId).HasColumnName("user_id");
        modelBuilder.Entity<AppUser>().Property(u => u.Email).HasColumnName("email");
        modelBuilder.Entity<AppUser>().Property(u => u.Username).HasColumnName("username");
        modelBuilder.Entity<AppUser>().Property(u => u.Password).HasColumnName("password");
        modelBuilder.Entity<AppUser>().Property(u => u.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<AppUser>().Property(u => u.LastLoginAt).HasColumnName("last_login_at");
        modelBuilder.Entity<AppUser>().Property(u => u.Status).HasColumnName("status");
        
        // Movie configuration
        modelBuilder.Entity<Movie>().ToTable("movie");
        modelBuilder.Entity<Movie>().HasKey(m => m.MovieId);
        modelBuilder.Entity<Movie>().Property(m => m.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<Movie>().Property(m => m.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<Movie>().Property(m => m.TitleType).HasColumnName("title_type");
        modelBuilder.Entity<Movie>().Property(m => m.PrimaryTitle).HasColumnName("primary_title");
        modelBuilder.Entity<Movie>().Property(m => m.OriginalTitle).HasColumnName("original_title");
        modelBuilder.Entity<Movie>().Property(m => m.IsAdult).HasColumnName("is_adult");
        modelBuilder.Entity<Movie>().Property(m => m.StartYear).HasColumnName("start_year");
        modelBuilder.Entity<Movie>().Property(m => m.EndYear).HasColumnName("end_year");
        modelBuilder.Entity<Movie>().Property(m => m.RunTimeMinutes).HasColumnName("runtime_minutes");
        modelBuilder.Entity<Movie>().Property(m => m.PlotSummary).HasColumnName("plot_summary");
        modelBuilder.Entity<Movie>().Property(m => m.PosterUrl).HasColumnName("poster_url");

        // Person configuration
        modelBuilder.Entity<Person>().ToTable("person");
        modelBuilder.Entity<Person>().HasKey(p => p.PersonId);
        modelBuilder.Entity<Person>().Property(p => p.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<Person>().Property(p => p.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<Person>().Property(p => p.PrimaryName).HasColumnName("primary_name");
        modelBuilder.Entity<Person>().Property(p => p.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(p => p.DeathYear).HasColumnName("death_year");

        // Genre configuration
        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Genre>().HasKey(g => g.GenreId);
        modelBuilder.Entity<Genre>().Property(g => g.GenreId).HasColumnName("genre_id");
        modelBuilder.Entity<Genre>().Property(g => g.GenreName).HasColumnName("genre_name");

        // Profession configuration
        modelBuilder.Entity<Profession>().ToTable("profession");
        modelBuilder.Entity<Profession>().HasKey(p => p.ProfessionId);
        modelBuilder.Entity<Profession>().Property(p => p.ProfessionId).HasColumnName("profession_id");
        modelBuilder.Entity<Profession>().Property(p => p.ProfessionName).HasColumnName("profession_name");

        // MovieGenre configuration
        modelBuilder.Entity<MovieGenre>().ToTable("moviegenre");
        modelBuilder.Entity<MovieGenre>().HasKey(mg => new { mg.MovieId, mg.GenreId });
        modelBuilder.Entity<MovieGenre>().Property(mg => mg.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<MovieGenre>().Property(mg => mg.GenreId).HasColumnName("genre_id");

        // PersonProfession configuration
        modelBuilder.Entity<PersonProfession>().ToTable("personprofession");
        modelBuilder.Entity<PersonProfession>().HasKey(pp => new { pp.PersonId, pp.ProfessionId });
        modelBuilder.Entity<PersonProfession>().Property(pp => pp.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<PersonProfession>().Property(pp => pp.ProfessionId).HasColumnName("profession_id");

        // PersonKnownFor configuration
        modelBuilder.Entity<PersonKnownFor>().ToTable("personknownfor");
        modelBuilder.Entity<PersonKnownFor>().HasKey(pkf => new { pkf.MovieId, pkf.PersonId });
        modelBuilder.Entity<PersonKnownFor>().Property(pkf => pkf.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<PersonKnownFor>().Property(pkf => pkf.PersonId).HasColumnName("person_id");

        // CastCredit configuration
        modelBuilder.Entity<CastCredit>().ToTable("castcredit");
        modelBuilder.Entity<CastCredit>().HasKey(cc => cc.CastId);
        modelBuilder.Entity<CastCredit>().Property(cc => cc.CastId).HasColumnName("cast_id");
        modelBuilder.Entity<CastCredit>().Property(cc => cc.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<CastCredit>().Property(cc => cc.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<CastCredit>().Property(cc => cc.CastOrder).HasColumnName("cast_order");

        // CastCharacter configuration
        modelBuilder.Entity<CastCharacter>().ToTable("castcharacter");
        modelBuilder.Entity<CastCharacter>().HasKey(cc => new { cc.CastId, cc.Position });
        modelBuilder.Entity<CastCharacter>().Property(cc => cc.CastId).HasColumnName("cast_id");
        modelBuilder.Entity<CastCharacter>().Property(cc => cc.CharacterName).HasColumnName("character_name");
        modelBuilder.Entity<CastCharacter>().Property(cc => cc.Position).HasColumnName("position");

        // CrewCredit configuration
        modelBuilder.Entity<CrewCredit>().ToTable("crewcredit");
        modelBuilder.Entity<CrewCredit>().HasKey(cc => cc.CrewId);
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.CrewId).HasColumnName("crew_id");
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.Department).HasColumnName("department");
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.Job).HasColumnName("job");
        modelBuilder.Entity<CrewCredit>().Property(cc => cc.CreditOrder).HasColumnName("credit_order");

        // AlternativeTitle configuration
        modelBuilder.Entity<AlternativeTitle>().ToTable("alternativetitle");
        modelBuilder.Entity<AlternativeTitle>().HasKey(at => at.AltId);
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.AltId).HasColumnName("alt_id");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.Title).HasColumnName("title");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.IsOriginalTitle).HasColumnName("is_original_title");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.RegionCode).HasColumnName("region_code");
        modelBuilder.Entity<AlternativeTitle>().Property(at => at.LanguageCode).HasColumnName("language_code");

        // AltTitleType configuration
        modelBuilder.Entity<AltTitleType>().ToTable("alttitletype");
        modelBuilder.Entity<AltTitleType>().HasKey(att => new { att.AltId, att.TypeName });
        modelBuilder.Entity<AltTitleType>().Property(att => att.AltId).HasColumnName("alt_id");
        modelBuilder.Entity<AltTitleType>().Property(att => att.TypeName).HasColumnName("type_name");

        // AltTitleAttribute configuration
        modelBuilder.Entity<AltTitleAttribute>().ToTable("alttitleattribute");
        modelBuilder.Entity<AltTitleAttribute>().HasKey(ata => new { ata.AltId, ata.AttributeName });
        modelBuilder.Entity<AltTitleAttribute>().Property(ata => ata.AltId).HasColumnName("alt_id");
        modelBuilder.Entity<AltTitleAttribute>().Property(ata => ata.AttributeName).HasColumnName("attribute_name");

        // Episode configuration
        modelBuilder.Entity<Episode>().ToTable("episode");
        modelBuilder.Entity<Episode>().HasKey(e => e.MovieId);
        modelBuilder.Entity<Episode>().Property(e => e.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<Episode>().Property(e => e.ParentSeriesId).HasColumnName("parent_series_id");
        modelBuilder.Entity<Episode>().Property(e => e.SeasonNumber).HasColumnName("season_number");
        modelBuilder.Entity<Episode>().Property(e => e.EpisodeNumber).HasColumnName("episode_number");

        // ImdbRating configuration
        modelBuilder.Entity<ImdbRating>().ToTable("imdbrating");
        modelBuilder.Entity<ImdbRating>().HasKey(ir => ir.MovieId);
        modelBuilder.Entity<ImdbRating>().Property(ir => ir.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<ImdbRating>().Property(ir => ir.Average).HasColumnName("average");
        modelBuilder.Entity<ImdbRating>().Property(ir => ir.Votes).HasColumnName("votes");

        // SearchHistory configuration
        modelBuilder.Entity<SearchHistory>().ToTable("searchhistory");
        modelBuilder.Entity<SearchHistory>().HasKey(sh => sh.SearchId);
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.SearchId).HasColumnName("search_id");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.UserId).HasColumnName("user_id");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.QueryText).HasColumnName("query_text");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.ExecutedAt).HasColumnName("executed_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.ResultsCount).HasColumnName("results_count");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.DurationMs).HasColumnName("duration_ms");

        // UserTitleRating configuration
        modelBuilder.Entity<UserTitleRating>().ToTable("usertitlerating");
        modelBuilder.Entity<UserTitleRating>().HasKey(utr => new { utr.UserId, utr.MovieId });
        modelBuilder.Entity<UserTitleRating>().Property(utr => utr.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserTitleRating>().Property(utr => utr.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<UserTitleRating>().Property(utr => utr.Rating).HasColumnName("rating");
        modelBuilder.Entity<UserTitleRating>().Property(utr => utr.RatedAt).HasColumnName("rated_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserTitleRating>().Property(utr => utr.Source).HasColumnName("source");

        // UserMovieBookmark configuration
        modelBuilder.Entity<UserMovieBookmark>().ToTable("usermoviebookmark");
        modelBuilder.Entity<UserMovieBookmark>().HasKey(umb => new { umb.UserId, umb.MovieId });
        modelBuilder.Entity<UserMovieBookmark>().Property(umb => umb.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserMovieBookmark>().Property(umb => umb.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<UserMovieBookmark>().Property(umb => umb.BookmarkedAt).HasColumnName("bookmarked_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserMovieBookmark>().Property(umb => umb.Folder).HasColumnName("folder");

        // UserPersonBookmark configuration
        modelBuilder.Entity<UserPersonBookmark>().ToTable("userpersonbookmark");
        modelBuilder.Entity<UserPersonBookmark>().HasKey(upb => new { upb.UserId, upb.PersonId });
        modelBuilder.Entity<UserPersonBookmark>().Property(upb => upb.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserPersonBookmark>().Property(upb => upb.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<UserPersonBookmark>().Property(upb => upb.BookmarkedAt).HasColumnName("bookmarked_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserPersonBookmark>().Property(upb => upb.Folder).HasColumnName("folder");

        // UserTitleNote configuration
        modelBuilder.Entity<UserTitleNote>().ToTable("usertitlenote");
        modelBuilder.Entity<UserTitleNote>().HasKey(utn => utn.NoteId);
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.NoteId).HasColumnName("note_id");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.MovieId).HasColumnName("movie_id");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.NoteBody).HasColumnName("note_body");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.UpdatedAt).HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserTitleNote>().Property(utn => utn.IsPrivate).HasColumnName("is_private");

        // UserPersonNote configuration
        modelBuilder.Entity<UserPersonNote>().ToTable("userpersonnote");
        modelBuilder.Entity<UserPersonNote>().HasKey(upn => upn.NoteId);
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.NoteId).HasColumnName("note_id");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.NoteBody).HasColumnName("note_body");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.UpdatedAt).HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserPersonNote>().Property(upn => upn.IsPrivate).HasColumnName("is_private");

        // Configure relationships
        ConfigureRelationships(modelBuilder);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // AppUser relationships
        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.SearchHistories)
            .WithOne(sh => sh.User)
            .HasForeignKey(sh => sh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.UserTitleRatings)
            .WithOne(utr => utr.User)
            .HasForeignKey(utr => utr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.UserMovieBookmarks)
            .WithOne(umb => umb.User)
            .HasForeignKey(umb => umb.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.UserPersonBookmarks)
            .WithOne(upb => upb.User)
            .HasForeignKey(upb => upb.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.UserTitleNotes)
            .WithOne(utn => utn.User)
            .HasForeignKey(utn => utn.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.UserPersonNotes)
            .WithOne(upn => upn.User)
            .HasForeignKey(upn => upn.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Movie relationships
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.MovieGenres)
            .WithOne(mg => mg.Movie)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.PersonKnownFor)
            .WithOne(pkf => pkf.Movie)
            .HasForeignKey(pkf => pkf.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.CastCredits)
            .WithOne(cc => cc.Movie)
            .HasForeignKey(cc => cc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.CrewCredits)
            .WithOne(cc => cc.Movie)
            .HasForeignKey(cc => cc.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.AlternativeTitles)
            .WithOne(at => at.Movie)
            .HasForeignKey(at => at.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Episodes)
            .WithOne(e => e.Movie)
            .HasForeignKey(e => e.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.ParentEpisodes)
            .WithOne(e => e.ParentSeries)
            .HasForeignKey(e => e.ParentSeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.ImdbRating)
            .WithOne(ir => ir.Movie)
            .HasForeignKey<ImdbRating>(ir => ir.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.UserTitleRatings)
            .WithOne(utr => utr.Movie)
            .HasForeignKey(utr => utr.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.UserMovieBookmarks)
            .WithOne(umb => umb.Movie)
            .HasForeignKey(umb => umb.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.UserTitleNotes)
            .WithOne(utn => utn.Movie)
            .HasForeignKey(utn => utn.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Person relationships
        modelBuilder.Entity<Person>()
            .HasMany(p => p.PersonProfessions)
            .WithOne(pp => pp.Person)
            .HasForeignKey(pp => pp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.PersonKnownFor)
            .WithOne(pkf => pkf.Person)
            .HasForeignKey(pkf => pkf.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.CastCredits)
            .WithOne(cc => cc.Person)
            .HasForeignKey(cc => cc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.CrewCredits)
            .WithOne(cc => cc.Person)
            .HasForeignKey(cc => cc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.UserPersonBookmarks)
            .WithOne(upb => upb.Person)
            .HasForeignKey(upb => upb.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.UserPersonNotes)
            .WithOne(upn => upn.Person)
            .HasForeignKey(upn => upn.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Genre relationships
        modelBuilder.Entity<Genre>()
            .HasMany(g => g.MovieGenres)
            .WithOne(mg => mg.Genre)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Profession relationships
        modelBuilder.Entity<Profession>()
            .HasMany(p => p.PersonProfessions)
            .WithOne(pp => pp.Profession)
            .HasForeignKey(pp => pp.ProfessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // CastCredit relationships
        modelBuilder.Entity<CastCredit>()
            .HasMany(cc => cc.CastCharacters)
            .WithOne(cc => cc.CastCredit)
            .HasForeignKey(cc => cc.CastId)
            .OnDelete(DeleteBehavior.Cascade);

        // AlternativeTitle relationships
        modelBuilder.Entity<AlternativeTitle>()
            .HasMany(at => at.AltTitleTypes)
            .WithOne(att => att.AlternativeTitle)
            .HasForeignKey(att => att.AltId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlternativeTitle>()
            .HasMany(at => at.AltTitleAttributes)
            .WithOne(ata => ata.AlternativeTitle)
            .HasForeignKey(ata => ata.AltId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    
}