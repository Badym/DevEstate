namespace DevEstate.Api.Dtos;

public static class UserDtos
{
    // ------------------ Tworzenie użytkownika (CRUD, np. przez admina) ------------------
    public class Create
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!; // Admin / SuperAdmin / Moderator
        public string? Password { get; set; } // Opcjonalnie, można ustawiać przy tworzeniu
    }

    public class Update
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; } // Admin / SuperAdmin / Moderator
        public string? Password { get; set; } // jeśli zmieniamy hasło
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = "Admin"; // Admin / SuperAdmin / Moderator
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}