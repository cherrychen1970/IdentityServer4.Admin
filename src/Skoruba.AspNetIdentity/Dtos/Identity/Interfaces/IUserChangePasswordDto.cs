namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IUserChangePasswordDto : IBaseUserChangePasswordDto
    {
        string UserName { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
    }
}
