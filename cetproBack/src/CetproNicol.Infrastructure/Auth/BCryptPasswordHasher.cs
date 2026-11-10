using CetproNicol.Application.Interfaces;

namespace CetproNicol.Infrastructure.Auth;

public class BCryptPasswordHasher : IPasswordHasher
{
    //hash y verify depende solo de los datos que recibe y no recuerda nada de llamadas anteriores.
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);//stateless

    public bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);//stateless
}
