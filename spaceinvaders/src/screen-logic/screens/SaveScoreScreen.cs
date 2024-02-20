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

    private readonly List<List<char>> lettersPanel = [];

    public override void Initialize() {
        for (int lines = 0; lines < 4; lines++) {
            List<char> line = [];
            for (int collumns = 0; collumns < 7; collumns++) {
                int index = lines * 7 + collumns;
                line.Add(letters[index]);
                if (letters[index].Equals('Z')) break;
            }
            lettersPanel.Add(line);
        }
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
        spriteBatch.DrawString(bigFont, text, new Vector2(
            graphics.PreferredBackBufferWidth / 2 - textWidth,
            150),
            Color.White);
    }

    private void DrawLettersTable() {
        int spacement = 0;
        lettersPanel.ForEach(a => {
            for (int i = 0; i < a.Count; i++) {
                spriteBatch.DrawString(bigFont, a[i].ToString(), new Vector2(
                    120 * i,
                    200 + spacement),
                    Color.White);
            }

            spacement += 80;
        });
    }
}