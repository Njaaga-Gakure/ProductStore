using ProductStore.Model;

namespace ProductStore.Service.IService
{
    public interface IJWT
    {
        string CreateJWTToken(User user);
    }
}
