using Refit;

namespace Pokedex.Api.Domain.Clients.PokeApi
{
    public interface ITranslatorApi
    {
        [Post("/yoda")]
        Task<ApiResponse<TranslateResponse>> GetYodaTranslationAsync(TranslateRequest request, CancellationToken cancellation = default);

        [Post("/shakespeare")]
        Task<ApiResponse<TranslateResponse>> GetShakespeareTranslationAsync(TranslateRequest request, CancellationToken cancellation = default);
    }

    public record TranslateRequest(string Text);
    public record TranslateResponse(TranslateSuccess Success, TranslateContents Contents);
    public record TranslateSuccess(int Total);
    public record TranslateContents(string Translated, string Text, string Translation);
}
