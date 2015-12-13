namespace NetDotNet.API.Requests
{
    public enum RequestType
    {
        GET,
        POST,
        HEAD,
        OPTIONS, // OPTIONS requests will always be handled by server, so this should never be used by dynamic pages
        TRACE // TRACE also handled by server
    }
}