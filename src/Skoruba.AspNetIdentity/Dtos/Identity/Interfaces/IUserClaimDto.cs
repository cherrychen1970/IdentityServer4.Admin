namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IUserClaimDto : IBaseUserClaimDto
    {
        string ClaimType { get; set; }
        string ClaimValue { get; set; }
    }
}
