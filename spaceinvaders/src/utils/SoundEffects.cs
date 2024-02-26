using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using spaceinvaders.model.barricades;

namespace spaceinvaders.utils;

public abstract class SoundEffects
{
    private static Song _mySound;
    private static SoundEffect _myEffect;

    public static void LoadMusic(Game game, ESoundsEffects effect)
    {
        switch (effect)
        {
            case ESoundsEffects.BackgroundSong:
                _mySound = game.Content.Load<Song>("songs/backgroundSong");
                break;
            case ESoundsEffects.BackgroundSongForMenu:
                _mySound = game.Content.Load<Song>("songs/backgroundSongForMenus");
                break;
        }
    }

    public static void LoadEffect(Game game, ESoundsEffects effect)
    {
        _myEffect = effect switch
        {
            ESoundsEffects.SpaceShipDead => game.Content.Load<SoundEffect>("songs/explosion"),
            ESoundsEffects.EnemyDead => game.Content.Load<SoundEffect>("songs/invaderkilled"),
            ESoundsEffects.ShootSpaceShip => game.Content.Load<SoundEffect>("songs/shoot"),
            ESoundsEffects.MenuEnter => game.Content.Load<SoundEffect>("songs/menuenter"),
            ESoundsEffects.MenuSelection => game.Content.Load<SoundEffect>("songs/menuselection"),
            ESoundsEffects.RedShip => game.Content.Load<SoundEffect>("songs/redship-song"),
            _ => _myEffect
        };
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

    public static void PlaySoundEffect(float volume = 0.1f)
    {
        var soundEffect = _myEffect;
        var soundEffectInstance = soundEffect.CreateInstance();
        soundEffectInstance.Volume = volume;
        soundEffectInstance.Play();
    }
}