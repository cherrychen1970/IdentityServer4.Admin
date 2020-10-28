namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IBaseUserDto
    {
        object Id { get; }
        bool IsDefaultId();
    }
}
