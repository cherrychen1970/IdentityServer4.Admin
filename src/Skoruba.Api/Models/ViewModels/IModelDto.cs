namespace Skoruba.Admin.ViewModels
{
    public interface IModelDto<TSubject, TBody>
    {
        TSubject Subject { get; set; }
        TBody Body { get; set; }
    }
}