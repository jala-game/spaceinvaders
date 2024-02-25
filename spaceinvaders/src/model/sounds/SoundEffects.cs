
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model.sounds;

public class SoundEffects
{
    private Song _mySound;

    public SoundEffects(Game game,ESoundsEffects effect )
    {
        _mySound = LoadSounds(game, effect);
    }
    
    private static Song LoadSounds(Game game, ESoundsEffects effec)
    {
        Song soundEffect = null;

        switch (effec)
        {
            case ESoundsEffects.BackgroundSong:
                soundEffect = game.Content.Load<Song>("songs/backgroundSong");
                break;
            case ESoundsEffects.BackgroundSongForMenu:
                soundEffect = game.Content.Load<Song>("songs/backgroundSongForMenus");
                break;
            case ESoundsEffects.SpaceShipDead:
                soundEffect = game.Content.Load<Song>("songs/explosion");
                break;
            case ESoundsEffects.EnemyDead:
                soundEffect = game.Content.Load<Song>("songs/enemyExplosion");
                break;
            case ESoundsEffects.ShootSpaceShip:
                soundEffect = game.Content.Load<Song>("songs/shoot");
                break;
        }
        return soundEffect;
    }

    public void PlayEffects(bool loop, float volume = 1f)
    {
        MediaPlayer.Play(_mySound);
        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Volume = volume;
    }

    public void StopMusic()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        
    }
}