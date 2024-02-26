using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using spaceinvaders.enums;
using spaceinvaders.model.barricades;
using spaceinvaders.model.screens.SaveScoreScreen;
using spaceinvaders.utils;

namespace spaceinvaders.screen_logic.screens;

public class SaveScoreScreen : GameScreenModel
{
    private const int DefaultDelayToPress = 10;
    private const int LengthLimit = 8;
    private readonly ContentManager _contentManager;

    private readonly Game _game;
    private readonly GraphicsDeviceManager _graphics;

    private readonly List<string> _letters =
    [
        "A", "B", "C", "D", "E", "F", "G",
        "H", "I", "J", "K", "L", "M", "N",
        "O", "P", "Q", "R", "S", "T", "U",
        "V", "W", "X", "Y", "Z"
    ];

    private readonly List<List<IInteraction>> _lettersPanel = [];
    private readonly int _score;
    private readonly SpriteBatch _spriteBatch;

    private int _delayToPress;

    private SpriteFont _bigFont;
    private SpriteFont _doneButtonFont;
    private SpriteFont _littleFont;

    private string _userName = "";

    public SaveScoreScreen(
        Game game,
        GraphicsDeviceManager graphics,
        ContentManager contentManager,
        SpriteBatch spriteBatch,
        int score
    )
    {
        _game = game;
        _graphics = graphics;
        _contentManager = contentManager;
        _spriteBatch = spriteBatch;
        _score = score;

        CreateMatrixWithLetters();
        CreateDoneButton();
        _delayToPress = DefaultDelayToPress;
    }

    public override void Initialize()
    {
    }

    private void CreateMatrixWithLetters()
    {
        for (var lines = 0; lines < 4; lines++) CreateMatrixWithLines(lines);
    }

    private void CreateMatrixWithLines(int lines)
    {
        List<IInteraction> line = [];

        for (var columns = 0; columns < 7; columns++)
        {
            var index = lines * 7 + columns;
            line.Add(new LetterActivation(_letters[index]));

            if (_letters[index].Equals("Z")) break;
        }

        _lettersPanel.Add(line);
    }

    private void CreateDoneButton()
    {
        _lettersPanel[^1].Add(new DoneButton("DONE"));
    }

    public override void LoadContent()
    {
        _littleFont = _contentManager.Load<SpriteFont>("fonts/PixeloidMonoSaveScore");
        _bigFont = _contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        _doneButtonFont = _contentManager.Load<SpriteFont>("fonts/PixeloidMonoDoneButton");
    }

    public override void Update(GameTime gameTime)
    {
        SetActiveOptionIfNotExists();

        _delayToPress--;
        if (_delayToPress > 0) return;
        var kstate = Keyboard.GetState();
        PanelMovement(kstate);
        EnterActions(kstate);
        BackspaceAction(kstate);
    }

    private void SetActiveOptionIfNotExists()
    {
        if (_lettersPanel.Any(letterList => letterList.Any(letter => letter.GetIsActivated()))) return;

        _lettersPanel[0][0].SetActivated();
    }

    private void PanelMovement(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Down) && !kstate.IsKeyDown(Keys.Up) &&
            !kstate.IsKeyDown(Keys.Left) && !kstate.IsKeyDown(Keys.Right)) return;
        SetNewLetterActivePosition(kstate.GetPressedKeys()[0]);
        ResetDelayToPress();
    }

    private void SetNewLetterActivePosition(Keys movement)
    {
        var activePositionLine = GetLetterActivePositionLine();
        var activePositionColumn = GetLetterActivePositionColumn();

        _lettersPanel[activePositionColumn][activePositionLine].SetActivated();

        try
        {
            switch (movement)
            {
                case Keys.Down:
                    const int columnToGoToButton = 2;
                    const int lineToGoToButton = 6;

                    var lineComparison = activePositionLine == lineToGoToButton;
                    var columComparison = activePositionColumn == columnToGoToButton;
                    if (lineComparison && columComparison)
                    {
                        var buttonPosition = _lettersPanel[^1][^1];
                        buttonPosition.SetActivated();
                        return;
                    }

                    _lettersPanel[activePositionColumn + 1][activePositionLine].SetActivated();
                    break;
                case Keys.Up:
                    _lettersPanel[activePositionColumn - 1][activePositionLine].SetActivated();
                    break;
                case Keys.Left:
                    _lettersPanel[activePositionColumn][activePositionLine - 1].SetActivated();
                    break;
                case Keys.Right:
                    _lettersPanel[activePositionColumn][activePositionLine + 1].SetActivated();
                    break;
            }

            PlaySoundEffect(ESoundsEffects.MenuSelection);
        }
        catch (ArgumentOutOfRangeException)
        {
            _lettersPanel[activePositionColumn][activePositionLine].SetActivated();
        }
    }

    private int GetLetterActivePositionLine()
    {
        foreach (var letterList in _lettersPanel)
            for (var i = 0; i < letterList.Count; i++)
                if (letterList[i].GetIsActivated())
                    return i;

        return -1;
    }

    private int GetLetterActivePositionColumn()
    {
        for (var i = 0; i < _lettersPanel.Count; i++)
            if (_lettersPanel[i].Any(letter => letter.GetIsActivated()))
                return i;


        return -1;
    }

    private void EnterActions(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        PlaySoundEffect(ESoundsEffects.MenuEnter);
        ResetDelayToPress();

        var letter = _lettersPanel[GetLetterActivePositionColumn()][GetLetterActivePositionLine()];
        switch (letter.GetType())
        {
            case InteractionEnum.Text:
                if (_userName.Length > LengthLimit) return;

                _userName += letter.GetLetter();
                break;
            case InteractionEnum.Button:
                if (string.IsNullOrEmpty(_userName)) return;
                User user = new()
                {
                    Name = _userName,
                    Score = _score
                };

                LocalStorage.AddUser(user);
                SwitchToMainScreen();
                break;
        }
    }

    private void SwitchToMainScreen()
    {
        MainScreen mainScreen = new(_game, _graphics, _contentManager, _spriteBatch);
        ScreenManager.ChangeScreen(mainScreen);
    }

    private void BackspaceAction(KeyboardState kstate)
    {
        if (!kstate.IsKeyDown(Keys.Back) || string.IsNullOrEmpty(_userName)) return;
        PlaySoundEffect(ESoundsEffects.MenuEnter);
        ResetDelayToPress();
        _userName = _userName.Remove(_userName.Length - 1, 1);
    }

    private void ResetDelayToPress()
    {
        _delayToPress = DefaultDelayToPress;
    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawLettersTable();
        DrawUserName();
    }

    private void DrawTitle()
    {
        const string text = "YOUR NAME";
        var textWidth = _bigFont.MeasureString(text).X / 2;
        Vector2 position = new(
            _graphics.PreferredBackBufferWidth / 2 - textWidth,
            100);
        _spriteBatch.DrawString(_bigFont, text, position,
            Color.White);
    }

    private void DrawUserName()
    {
        var textWidth = _littleFont.MeasureString(_userName).X / 2;
        Vector2 position = new(
            _graphics.PreferredBackBufferWidth / 2 - textWidth,
            250);
        _spriteBatch.DrawString(_littleFont, _userName, position,
            Color.Red);
    }

    private void DrawLettersTable()
    {
        var gap = 0;
        _lettersPanel.ForEach(a =>
        {
            for (var i = 0; i < a.Count; i++)
            {
                var margin = 120;
                const int initialY = 400;

                if (a[i].GetType() == InteractionEnum.Button) margin += 5;

                var font = a[i].GetType() == InteractionEnum.Button ? _doneButtonFont : _bigFont;

                var initialX = Math.Abs(_graphics.PreferredBackBufferWidth / 2 -
                                         (margin + font.MeasureString(a[i].GetLetter()).X * 6));
                Vector2 position = new(initialX + margin * i, initialY + gap);
                _spriteBatch.DrawString(font, a[i].GetLetter(), position,
                    ColorIfSelected(a[i]));
            }

            gap += 80;
        });
    }

    private static Color ColorIfSelected(IInteraction letter)
    {
        return letter.GetIsActivated() ? Color.Red : Color.White;
    }

    private void PlaySoundEffect(ESoundsEffects effects)
    {
        SoundEffects.LoadEffect(_game, effects);
        SoundEffects.PlaySoundEffect(0.4f);
    }
}