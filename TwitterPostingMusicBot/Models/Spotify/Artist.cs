using System;

namespace TwitterPostingMusicBot.Models.Spotify;

public class Artist
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime LastReleaseSongDate { get; set; }
}