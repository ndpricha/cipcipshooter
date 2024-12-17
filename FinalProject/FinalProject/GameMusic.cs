using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Microsoft.VisualBasic.Devices;

namespace FinalProject
    {
      public class GameMusic
      {
          private SoundPlayer _gameMusicPlayer;
          private SoundPlayer _gameOverMusicPlayer;
          private SoundPlayer _mainFormMusicPlayer;

          public GameMusic(string gameMusicPath, string gameOverMusicPath, string mainFormMusicPath)
          {
              try
              {
                  _gameMusicPlayer = new SoundPlayer("Assets/Audio/game_music.wav");
                  _gameOverMusicPlayer = new SoundPlayer("Assets/Audio/game_over_music.wav");
                  _mainFormMusicPlayer = new SoundPlayer("Assets/Audio/main_form_music.wav");
              }
              catch (FileNotFoundException ex)
              {
                  MessageBox.Show($"Error loading sound files: {ex.Message}");
              }

          }

          public void PlayGameMusic()
          {
            StopAll();
            _gameMusicPlayer.PlayLooping(); // Memutar musik secara terus-menerus
          }

          public void PlayGameOverMusic()
          {
              StopAll();
              _gameOverMusicPlayer.PlayLooping();
          }

          public void PlayMainFormMusic()
          {
              StopAll();
              _mainFormMusicPlayer.Play();
          }

          public void StopAll()
          {
              _gameMusicPlayer.Stop();
              _gameOverMusicPlayer.Stop();
              _mainFormMusicPlayer.Stop();
          }
      }

    }

