using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

    public override void LoadContent() {
        littleFont = contentManager.Load<SpriteFont>("fonts/PixeloidMono");
        bigFont = contentManager.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    }
    public override void Update(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) {
        DrawTitle();
        DrawLettersTable();
    }


    private void DrawTitle() {
        string text = "Put Your Name";
        float textWidth = bigFont.MeasureString(text).X / 2;
        Vector2 position = new(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            100);
        spriteBatch.DrawString(bigFont, text, position,
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
        return letter.GetIsActivated() ? Color.Yellow : Color.White;
    }
}