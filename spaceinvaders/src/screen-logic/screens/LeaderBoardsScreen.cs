using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.model;
using spaceinvaders.model.barricades;
using spaceinvaders.utils;

namespace spaceinvaders.screen_logic.screens;

public class LeaderBoardsScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch
) : GameScreenModel
{
    private readonly ColorLeaderBoards _colors = new();
    private readonly SpriteFont _genericFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private readonly SpriteFont _title = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private EMenuOptionsLeaderBoards _chooseMenu = EMenuOptionsLeaderBoards.RightArrow;
    private int _page;
    private int _placingManage;
    private float _delayToPress = 10f;

    public override void Initialize()
    {
    }

    public override void LoadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();

        _delayToPress--;
        if (_delayToPress > 0) return;

        ModifyMenuSelection(kstate);
        SendMenuOption(kstate);
    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawHeaders();
        DrawLeaderBoards();
        DrawMenu();
        DrawMenuItemActive();
    }

    private void DrawTitle()
    {
        const string text = "HIGH SCORES";
        var textWidth = _title.MeasureString(text).X / 2;
        spriteBatch.DrawString(_title, text, new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth, 50),
            Color.Green);
    }

    private void DrawHeaders()
    {
        var headers = new List<string> { "RANK", "SCORE", "NAME" };

        float[] positionX =
        [
            graphics.PreferredBackBufferWidth / 2 - 600,
            graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString("SCORE").X / 2,
            graphics.PreferredBackBufferWidth - 300
        ];

        var controllerI = 0;

        headers.ForEach(header =>
        {
            spriteBatch.DrawString(_genericFont, header, new Vector2(positionX[controllerI], 160), Color.Yellow);
            controllerI++;
        });
    }

    private void DrawLeaderBoards()
    {
        var usersForDataBase = LocalStorage.GetUsersPaginator(10, _page);
        var positionY = 220;

        usersForDataBase.ForEach(user =>
        {
            DrawUsers(user, _colors.RandomColor(usersForDataBase.IndexOf(user)), positionY,
                usersForDataBase.IndexOf(user));
            positionY += 50;
        });
    }

    private void DrawUsers(User user, Color color, int y, int positionUser)
    {
        float[] positionX =
        [
            graphics.PreferredBackBufferWidth / 2 - 600,
            graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString("SCORE").X / 2,
            graphics.PreferredBackBufferWidth - 300
        ];

        string[] placingOptions = ["ST", "ND", "RD", "TH"];
        var placingPosition = positionUser > 3 ? placingOptions[3] : placingOptions[positionUser];

        string[] dataForUser =
            [$"{positionUser + 1 + _placingManage}{placingPosition}", $"{user.Score}", $"{user.Name}"];

        for (var i = 0; i < positionX.Length; i++)
            spriteBatch.DrawString(_genericFont, dataForUser[i], new Vector2(positionX[i], y), color);
    }

    private void DrawMenu()
    {
        string[] texts = ["  Back", "<", $"{_page + 1}", ">"];
        float[] positionsX =
        [
            100,
            graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString("<").X / 2 - 70,
            graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString($"{_page + 1}").X / 2,
            graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString(">").X / 2 + 70
        ];

        for (var i = 0; i < texts.Length; i++) DrawMenuItem(positionsX[i], texts[i], null);
    }

    private void DrawMenuItem(float x, string text, Color? color)
    {
        spriteBatch.DrawString(_genericFont, text, new Vector2(x, graphics.PreferredBackBufferHeight - 100),
            color ?? Color.White);
    }

    private void DrawMenuItemActive()
    {
        switch (_chooseMenu)
        {
            case EMenuOptionsLeaderBoards.LeaveGame:
                DrawMenuItem(100, "> Back", Color.Green);
                break;
            case EMenuOptionsLeaderBoards.RightArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString(">").X / 2 + 70, ">",
                    Color.Green);
                break;
            case EMenuOptionsLeaderBoards.LeftArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth / 2 - _genericFont.MeasureString("<").X / 2 - 70, "<",
                    Color.Green);
                break;
        }
    }

    private void SendMenuOption(KeyboardState kstate)
    {
        _delayToPress--;
        if (_delayToPress > 0) return;
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        PlaySoundEffect(ESoundsEffects.MenuEnter);
        switch (_chooseMenu)
        {
            case EMenuOptionsLeaderBoards.LeaveGame:
                LeaveTheGame();
                break;
            case EMenuOptionsLeaderBoards.LeftArrow:
                ReturnPages();
                _delayToPress = 10;
                break;
            case EMenuOptionsLeaderBoards.RightArrow:
                NextPages();
                _delayToPress = 10;
                break;
        }
    }

    private void LeaveTheGame()
    {
        var mainScreen = new MainScreen(game, graphics, contentManager, spriteBatch);
        ScreenManager.ChangeScreen(mainScreen);
    }

    private void ReturnPages()
    {
        if (_page > 0) _page--;
        if (_page == 0)
        {
            _placingManage = 0;
            return;
        }

        _placingManage -= 10;
    }

    private void NextPages()
    {
        var usersForDataBase = LocalStorage.GetUsersPaginator(10, _page + 1);
        if (usersForDataBase.Count == 0) return;
        _page++;
        _placingManage += 10;
    }

    private void ModifyMenuSelection(KeyboardState kstate)
    {
        const float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Left) && _chooseMenu > EMenuOptionsLeaderBoards.LeaveGame)
        {
            _chooseMenu--;
            _delayToPress = resetDelay;
            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }

        if (!kstate.IsKeyDown(Keys.Right) || _chooseMenu >= EMenuOptionsLeaderBoards.RightArrow) return;
        _chooseMenu++;
        _delayToPress = resetDelay;
        PlaySoundEffect(ESoundsEffects.MenuSelection);
    }

    private void PlaySoundEffect(ESoundsEffects effects)
    {
        SoundEffects.LoadEffect(game, effects);
        SoundEffects.PlaySoundEffect(0.4f);
    }
}