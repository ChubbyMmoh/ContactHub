using ContactApi.Model.DTO;
using ContactApi.Model;

public interface IAuthRepository
{
    Task<User> Register(UserDTO userDto);
    Task<string> Login(UserDTO userDto);
    Task<bool> UserExists(string username);
}
