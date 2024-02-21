using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class SaveScoreScreen(
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch
    ) : GameScreenModel {

    private SpriteFont bigFont;
    private SpriteFont littleFont;
    private readonly List<char> letters= [
        'A', 'B', 'C', 'D', 'E', 'F', 'G',
        'H', 'I', 'J', 'K', 'L', 'M', 'N',
        'O', 'P', 'Q', 'R', 'S', 'T', 'U',
        'V', 'W', 'X', 'Y', 'Z'
    ];

    private int _delayToPress = 10;

    private readonly List<List<LetterActivation>> lettersPanel = [];

    public override void Initialize() {
        CreateMatrixWithLetters();
    }

    private void CreateMatrixWithLetters() {
        for (int lines = 0; lines < 4; lines++) {
            CreateMatrixWithLines(lines);
        }
    }

    private void CreateMatrixWithLines(int lines) {
        List<LetterActivation> line = [];

        for (int columns = 0; columns < 7; columns++) {
            int index = lines * 7 + columns;
            line.Add(new LetterActivation(letters[index]));

            if (letters[index].Equals('Z')) break;
        }

        lettersPanel.Add(line);
    }

    private string userName = "";

    public override void LoadContent() {
        littleFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoSaveScore");
        bigFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    }
    public override void Update(GameTime gameTime) {
        SetActiveOptionIfNotExists();
        PanelMovement();
    }

    private void SetActiveOptionIfNotExists() {
        foreach (List<LetterActivation> letterList in lettersPanel) {
            foreach (LetterActivation letter in letterList) {
                if (letter.GetIsActivated()) return;
            }
        }

        lettersPanel[0][0].SetActivated();
    }

    private void PanelMovement() {
        _delayToPress--;
        if (_delayToPress > 0) return;

        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.Up) ||
            kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Right))
        {
            SetNewLetterActivePosition(kstate.GetPressedKeys()[0]);
            _delayToPress = 10;
        }
    }

    private void SetNewLetterActivePosition(Keys movement) {
        int activePositionLine = GetLetterActivePositionLine();
        int activePositionColumn = GetLetterActivePositionColumn();

        lettersPanel[activePositionColumn][activePositionLine].SetActivated();

        try {
            switch (movement) {
                case Keys.Down:
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
        foreach (List<LetterActivation> letterList in lettersPanel ) {
            for (int i = 0; i < letterList.Count; i++) {
                if (letterList[i].GetIsActivated()) return i;
            }
        }

        return -1;
    }

    private int GetLetterActivePositionColumn() {
        for (int i = 0; i < lettersPanel.Count; i++) {
            foreach (LetterActivation letter in lettersPanel[i]) {
                if (letter.GetIsActivated()) return i;
            }
        }

        return -1;
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
        string text = "WILLIAN";
        float textWidth = littleFont.MeasureString(text).X / 2;
        Vector2 position = new(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            250);
        spriteBatch.DrawString(littleFont, text, position,
            Color.White);
    }

    private void DrawLettersTable() {
        int GAP = 0;
        lettersPanel.ForEach(a => {
            for (int i = 0; i < a.Count; i++) {
                int MARGIN = 120;
                int INITIAL_Y = 400;
                float INITIAL_X = graphics.PreferredBackBufferWidth / 2 - (MARGIN + bigFont.MeasureString(a[i].GetLetter().ToString()).X * 6);
                Vector2 position = new(INITIAL_X + MARGIN * i, INITIAL_Y + GAP);
                spriteBatch.DrawString(bigFont, a[i].GetLetter().ToString(), position,
                    ColorIfSelected(a[i]));
            }

            GAP += 80;
        });
    }

    private static Color ColorIfSelected(LetterActivation letter) {
        return letter.GetIsActivated() ? Color.Red : Color.White;
    }
}