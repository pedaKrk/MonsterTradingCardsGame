
namespace MonsterTradingCardsGame.Http.Interfaces
{
    public interface IHttpResponseHandler
    {
        Task SendOkAsync(object? responseBody = null);
        Task SendCreatedAsync();
        Task SendNoContentAsync();
        Task SendBadRequestAsync(object? responseBody = null);
        Task SendUnauthorizedAsync(object? responseBody = null);
        Task SendForbiddenAsync(object? responseBody = null);
        Task SendNotFoundAsync(object? responseBody = null);
        Task SendConflictAsync(object? responseBody = null);
        Task SendInternalServerErrorAsync();
    }
}
