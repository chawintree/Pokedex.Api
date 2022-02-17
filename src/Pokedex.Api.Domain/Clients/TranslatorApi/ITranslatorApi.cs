using Refit;

namespace Pokedex.Api.Domain.Clients.PokeApi
{
    public interface ITranslatorApi
    {
        [Post("/yoda/{name}")]
        Task<ApiResponse<TranslateResponse>> GetYodaTranslationAsync(string text, CancellationToken cancellation = default);

        [Post("/shakespeare/{name}")]
        Task<ApiResponse<TranslateResponse>> GetShakespeareTranslationAsync(string text, CancellationToken cancellation = default);
    }

    public record TranslateResponse(TranslateSuccess Success, TranslateContents Contents);
    public record TranslateSuccess(int Total);
    public record TranslateContents(string Translated, string Text, string Translation);
}
