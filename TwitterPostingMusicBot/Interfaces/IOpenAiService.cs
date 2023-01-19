using TwitterPostingMusicBot.Helpers;

namespace TwitterPostingMusicBot.Interfaces;

public interface IOpenAiService
{
    Task<string> GetMessage(bool single, LanguageEnum language, string artist, string songTitle);
}