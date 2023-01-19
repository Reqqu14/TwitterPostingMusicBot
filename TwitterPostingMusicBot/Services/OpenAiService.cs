using Microsoft.Extensions.Configuration;
using OpenAI;
using TwitterPostingMusicBot.Helpers;
using TwitterPostingMusicBot.Interfaces;

namespace TwitterPostingMusicBot.Services;

public class OpenAiService : IOpenAiService
{
    private readonly IConfiguration _configuration;

    public OpenAiService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetMessage(
        bool single,
        LanguageEnum language,
        string artist,
        string songTitle)
    {
        var client = new OpenAIAPI(_configuration["OpenAiToken"]);

        var prompt = string.Empty;

        if (language == LanguageEnum.Polish)
        {
            prompt = string.Format(_configuration["OpenAiTextPL"], single ? "singla" : "albumu", artist, songTitle);
        }
        else
        {
            prompt = string.Format(_configuration["OpenAiTextEN"], single ? "song" : "album", artist, songTitle);
        }

        var result =
            await client.Completions.CreateCompletionAsync(new CompletionRequest()
            {
                Prompt = prompt,
                MaxTokens = 2000
            });

        return result.ToString().Trim();
    }
}