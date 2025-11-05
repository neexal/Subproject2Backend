using System.ComponentModel.DataAnnotations.Schema;

namespace IMDB.DataServiceLayer.Models;

public class Person
{
    public int PersonId { get; set; }
    public string Nconst { get; set; } = string.Empty;
    public string PrimaryName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }

    // Navigation properties
    public ICollection<PersonProfession> PersonProfessions { get; set; } = new List<PersonProfession>();
    public ICollection<PersonKnownFor> PersonKnownFor { get; set; } = new List<PersonKnownFor>();
    public ICollection<CastCredit> CastCredits { get; set; } = new List<CastCredit>();
    public ICollection<CrewCredit> CrewCredits { get; set; } = new List<CrewCredit>();
    public ICollection<UserPersonBookmark> UserPersonBookmarks { get; set; } = new List<UserPersonBookmark>();
    public ICollection<UserPersonNote> UserPersonNotes { get; set; } = new List<UserPersonNote>();
}
