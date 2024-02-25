
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model.sounds;

public class SoundEffects
{
    private static Song _mySound;
    private static SoundEffect _myEffect;
    
    
    public static void LoadMusic(Game game, ESoundsEffects effec)
    {
        switch (effec)
        {
            case ESoundsEffects.BackgroundSong:
                _mySound = game.Content.Load<Song>("songs/backgroundSong");
                break;
            case ESoundsEffects.BackgroundSongForMenu:
                _mySound = game.Content.Load<Song>("songs/backgroundSongForMenus");
                break;
        }
    }
    
    public static void LoadEffect(Game game, ESoundsEffects effec)
    {
        switch (effec)
        {
            case ESoundsEffects.SpaceShipDead:
                _myEffect = game.Content.Load<SoundEffect>("songs/explosion");
                break;
            case ESoundsEffects.EnemyDead:
                _myEffect = game.Content.Load<SoundEffect>("songs/invaderkilled");
                break;
            case ESoundsEffects.ShootSpaceShip:
                _myEffect = game.Content.Load<SoundEffect>("songs/shoot");
                break;
        }
    }

    public static void PlayEffects(bool loop, float volume = 1f)
    {
        MediaPlayer.Play(_mySound);
        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Volume = volume;
    }

    public static void StopMusic()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        
    }

    public static void PlaySoundEffect(Game game)
    {
        var soundEffect = _myEffect;
        soundEffect.Play();
    }
}