using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DevEstate.Api.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly JwtService _jwtService;

        public UserService(UserRepository repo, JwtService jwtService)
        {
            _repo = repo;
            _jwtService = jwtService;
        }

        // -------------------- Pobranie użytkownika --------------------
        public async Task<UserDtos.Response> GetByIdAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id) ?? throw new Exception("User not found");
            return MapToResponse(user);
        }

        public async Task<List<UserDtos.Response>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(MapToResponse).ToList();
        }

        // -------------------- Tworzenie użytkownika --------------------
        public async Task<UserDtos.Response> CreateAsync(UserDtos.Create dto)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User with this email already exists");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _repo.CreateAsync(user);
            return MapToResponse(user);
        }

        // -------------------- Aktualizacja użytkownika --------------------
        public async Task UpdateAsync(string id, UserDtos.Update dto)
        {
            var user = await _repo.GetByIdAsync(id) ?? throw new Exception("User not found");

            user.Email = dto.Email ?? user.Email;
            user.FullName = dto.FullName ?? user.FullName;
            user.Role = dto.Role ?? user.Role;

            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _repo.UpdateAsync(user);
        }

        // -------------------- Usuwanie użytkownika --------------------
        public async Task DeleteAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                throw new Exception("Użytkownik nie istnieje.");

            if (user.Role == "Admin")
                throw new Exception("Nie można usunąć konta administratora.");

            await _repo.DeleteAsync(id);
        }


        // -------------------- Logowanie --------------------
        public async Task<AuthDtos.LoginResponse> LoginAsync(AuthDtos.LoginRequest dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email)
                ?? throw new Exception("Użytkownik nie istnieje.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Nieprawidłowe hasło.");

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role, user.FullName);

            return new AuthDtos.LoginResponse
            {
                Token = token,
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
                Id = user.Id
            };
        }

        // -------------------- Helper --------------------
        private static UserDtos.Response MapToResponse(User user)
        {
            return new UserDtos.Response
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }
    }
}
