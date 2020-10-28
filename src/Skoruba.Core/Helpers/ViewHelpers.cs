namespace Skoruba.IdentityServer4.Helpers
{
    public static class ViewHelpers
    {
        public static string GetClientName(string clientId, string clientName)
        {
            return $"{clientId} ({clientName})";
        }
    }
}
