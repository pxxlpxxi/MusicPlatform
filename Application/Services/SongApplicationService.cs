using MusicPlatform.Application.Models;
using MusicPlatform.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlatform.Application.Services
{
    public class SongApplicationService
    {
        private readonly SongService _songService;

        public SongApplicationService(SongService songService)
        {
            _songService = songService;
        }

        public List<SongInfo> GetSongs()
        {
            return _songService
                .GetSongs()
                .Select(song => _songService.GetSongInfoBySongId(song.Id))
                .ToList();
        }

        public SongInfo GetSong(int songId)
        {
            return _songService.GetSongInfoBySongId(songId);
        }

        public List<SongInfo> SearchSongs(string searchTerm)
        {
            return _songService
                .SearchSongs(searchTerm)
                .Select(song => _songService.GetSongInfoBySongId(song.Id))
                .ToList();
        }

        public void DeleteSong(int songId)
        {
            _songService.DeleteSong(songId);
        }
        public void UpdateSongTitle(int songId, string newTitle)
        {
            _songService.UpdateSongTitle(songId, newTitle);
        }

    }

}
