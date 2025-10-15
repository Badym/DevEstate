namespace DevEstate.Api.Dtos;

public static class UserDtos
{
    public class Register
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }

    public class Login
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = "Admin"; // tylko admin
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}