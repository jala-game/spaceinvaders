using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class SaveScoreScreen : GameScreenModel {

    private Game game;
    private GraphicsDeviceManager graphics;
    private ContentManager contentManager;
    private SpriteBatch spriteBatch;
    private int score;

    private SpriteFont bigFont;
    private SpriteFont littleFont;
    private SpriteFont doneButtonFont;
    private readonly int _lengthLimit = 8;

    private readonly List<string> letters= [
        "A", "B", "C", "D", "E", "F", "G",
        "H", "I", "J", "K", "L", "M", "N",
        "O", "P", "Q", "R", "S", "T", "U",
        "V", "W", "X", "Y", "Z"
    ];

    private int _delayToPress;
    private readonly int _defaultDelayToPress = 10;

    private readonly List<List<IInteraction>> lettersPanel = [];

    public SaveScoreScreen(
        Game game,
        GraphicsDeviceManager graphics,
        ContentManager contentManager,
        SpriteBatch spriteBatch,
        int score
    ) {
        this.game = game;
        this.graphics = graphics;
        this.contentManager = contentManager;
        this.spriteBatch = spriteBatch;
        this.score = score;

        CreateMatrixWithLetters();
        CreateDoneButton();
        _delayToPress = _defaultDelayToPress;
    }

    public override void Initialize() {}

    private void CreateMatrixWithLetters() {
        for (int lines = 0; lines < 4; lines++) {
            CreateMatrixWithLines(lines);
        }
    }

    private void CreateMatrixWithLines(int lines) {
        List<IInteraction> line = [];

        for (int columns = 0; columns < 7; columns++) {
            int index = lines * 7 + columns;
            line.Add(new LetterActivation(letters[index]));

            if (letters[index].Equals("Z")) break;
        }

        lettersPanel.Add(line);
    }

    private void CreateDoneButton() {
        lettersPanel[^1].Add(new DoneButton("DONE"));
    }

    private string userName = "";

    public override void LoadContent() {
        littleFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoSaveScore");
        bigFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
        doneButtonFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoDoneButton");
    }

    public override void Update(GameTime gameTime) {
        SetActiveOptionIfNotExists();

        _delayToPress--;
        if (_delayToPress > 0) return;
        var kstate = Keyboard.GetState();
        PanelMovement(kstate);
        EnterActions(kstate);
        BackspaceAction(kstate);
    }

    private void SetActiveOptionIfNotExists() {
        foreach (List<IInteraction> letterList in lettersPanel) {
            foreach (IInteraction letter in letterList) {
                if (letter.GetIsActivated()) return;
            }
        }

        lettersPanel[0][0].SetActivated();
    }

    private void PanelMovement(KeyboardState kstate) {
        if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.Up) ||
            kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Right))
        {
            SetNewLetterActivePosition(kstate.GetPressedKeys()[0]);
            ResetDelayToPress();
        }
    }

    private void SetNewLetterActivePosition(Keys movement) {
        int activePositionLine = GetLetterActivePositionLine();
        int activePositionColumn = GetLetterActivePositionColumn();

        lettersPanel[activePositionColumn][activePositionLine].SetActivated();

        try {
            switch (movement) {
                case Keys.Down:
                    int ColumnToGoToButton = 2;
                    int LineToGoToButton = 6;

                    bool lineComparison = activePositionLine == LineToGoToButton;
                    bool columComparison = activePositionColumn ==  ColumnToGoToButton;
                    if (lineComparison && columComparison) {
                        IInteraction buttonPosition = lettersPanel[^1][^1];
                        buttonPosition.SetActivated();
                        return;
                    }

                    lettersPanel[activePositionColumn + 1][activePositionLine].SetActivated();
                    break;
                case Keys.Up:
                    lettersPanel[activePositionColumn - 1][activePositionLine].SetActivated();
                    break;
                case Keys.Left:
                    lettersPanel[activePositionColumn][activePositionLine - 1].SetActivated();
                    break;
                case Keys.Right:
                    lettersPanel[activePositionColumn][activePositionLine + 1].SetActivated();
                    break;
            }
        } catch (ArgumentOutOfRangeException) {
            lettersPanel[activePositionColumn][activePositionLine].SetActivated();
        }
    }

    private int GetLetterActivePositionLine() {
        foreach (List<IInteraction> letterList in lettersPanel ) {
            for (int i = 0; i < letterList.Count; i++) {
                if (letterList[i].GetIsActivated()) return i;
            }
        }

        return -1;
    }

    private int GetLetterActivePositionColumn() {
        for (int i = 0; i < lettersPanel.Count; i++) {
            foreach (IInteraction letter in lettersPanel[i]) {
                if (letter.GetIsActivated()) return i;
            }
        }

        return -1;
    }

    private void EnterActions(KeyboardState kstate) {
        if (!kstate.IsKeyDown(Keys.Enter)) return;

        ResetDelayToPress();

        IInteraction letter = lettersPanel[GetLetterActivePositionColumn()][GetLetterActivePositionLine()];
        switch (letter.GetType()) {
            case InteractionEnum.TEXT:
                if (userName.Length > _lengthLimit) return;

                userName += letter.GetLetter();
                break;
            case InteractionEnum.BUTTON:
                if (string.IsNullOrEmpty(userName)) return;
                User user = new()
                {
                    Name = userName,
                    Score = score
                };

                LocalStorage.AddUser(user);
                SwitchToMainScreen();
                break;
        };
    }

    private void SwitchToMainScreen() {
        MainScreen mainScreen = new(game, graphics, contentManager, spriteBatch);
        ScreenManager.ChangeScreen(mainScreen);
    }

    private void BackspaceAction(KeyboardState kstate) {
        if (!kstate.IsKeyDown(Keys.Back) || string.IsNullOrEmpty(userName)) return;

        ResetDelayToPress();
        userName = userName.Remove(userName.Length - 1, 1);;
    }

    private void ResetDelayToPress() {
        _delayToPress = _defaultDelayToPress;
    }

    public override void Draw(GameTime gameTime) {
        DrawTitle();
        DrawLettersTable();
        DrawUserName();
    }

    private void DrawTitle() {
        string text = "YOUR NAME";
        float textWidth = bigFont.MeasureString(text).X / 2;
        Vector2 position = new(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            100);
        spriteBatch.DrawString(bigFont, text, position,
            Color.White);
    }

    private void DrawUserName() {
        float textWidth = littleFont.MeasureString(userName).X / 2;
        Vector2 position = new(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            250);
        spriteBatch.DrawString(littleFont, userName, position,
            Color.Red);
    }

    private void DrawLettersTable() {
        int GAP = 0;
        lettersPanel.ForEach(a => {
            for (int i = 0; i < a.Count; i++) {
                int MARGIN = 120;
                int INITIAL_Y = 400;

                if (a[i].GetType() == InteractionEnum.BUTTON) {
                    MARGIN += 5;
                };

                SpriteFont font = a[i].GetType() == InteractionEnum.BUTTON ? doneButtonFont : bigFont;

                float INITIAL_X = Math.Abs(graphics.PreferredBackBufferWidth / 2 - (MARGIN + font.MeasureString(a[i].GetLetter()).X * 6));
                Vector2 position = new(INITIAL_X + MARGIN * i, INITIAL_Y + GAP);
                spriteBatch.DrawString(font, a[i].GetLetter(), position,
                    ColorIfSelected(a[i]));
            }

            GAP += 80;
        });
    }

    private static Color ColorIfSelected(IInteraction letter) {
        return letter.GetIsActivated() ? Color.Red : Color.White;
    }
}