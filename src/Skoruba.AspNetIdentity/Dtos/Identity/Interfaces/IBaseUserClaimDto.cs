namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IBaseUserClaimDto
    {
        int ClaimId { get; set; }
        object UserId { get; }
    }
}
