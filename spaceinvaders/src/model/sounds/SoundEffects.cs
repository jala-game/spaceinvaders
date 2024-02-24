using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using spaceinvaders.model.barricades;

namespace spaceinvaders.model.sounds;

public class SoundEffects
{
    private static Song? LoadSounds(Game game, ESoundsEffects effec)
    {
        Song? soundEffect = null;

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
            case ESoundsEffects.EnemyMoving:
                soundEffect = game.Content.Load<Song>("songs/fastinvader1");
                break;
            case ESoundsEffects.EnemyDead:
                soundEffect = game.Content.Load<Song>("songs/invaderkilled");
                break;
            case ESoundsEffects.ShootSpaceShip:
                soundEffect = game.Content.Load<Song>("songs/shoot");
                break;
            case ESoundsEffects.ShootEnemy:
                soundEffect = game.Content.Load<Song>("songs/shootEnemy");
                break;
        }

        return soundEffect;
    }

    public static void PlayEffects(Game game,ESoundsEffects effect )
    {
        Song mySound = LoadSounds(game, effect);
        
        MediaPlayer.Play(mySound);

    }
}