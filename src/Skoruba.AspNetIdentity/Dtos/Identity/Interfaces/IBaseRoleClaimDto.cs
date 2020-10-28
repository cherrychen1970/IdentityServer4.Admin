namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IBaseRoleClaimDto
    {
        int ClaimId { get; set; }
        object RoleId { get; }
    }
}
