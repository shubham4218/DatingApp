using API.Entity;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CretateToken(AppUser appUser);
    }
}