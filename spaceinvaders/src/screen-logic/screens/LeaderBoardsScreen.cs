using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using spaceinvaders.model;

namespace spaceinvaders.screen_logic.screens;

public class LeaderBoardsScreen(
    Game game,
    GraphicsDeviceManager graphics,
    ContentManager contentManager,
    SpriteBatch spriteBatch
) : GameScreenModel
{
    private SpriteFont _title = game.Content.Load<SpriteFont>("fonts/PixeloidMonoGameOver");
    private SpriteFont _genericFont = game.Content.Load<SpriteFont>("fonts/PixeloidMonoMenu");
    private int _page = 0;
    private ColorLeaderBoards _colors = new();

    public override void Initialize() { }

    public override void LoadContent() { }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        DrawTitle();
        DrawHeaders();
        DrawLeaderBoards();
    }

    private void DrawTitle()
    {
        string text = "HIGH SCORES";
        float textWidth = _title.MeasureString(text).X / 2;
        spriteBatch.DrawString(_title,text,new Vector2(graphics.PreferredBackBufferWidth / 2 - textWidth,50),Color.Green);
    }

    private void DrawHeaders()
    {
        List<string> headers = new List<string>() { "RANK", "SCORE" , "NAME"};
        
        float[] positionX = new[] { graphics.PreferredBackBufferWidth / 2 - 600, 
            graphics.PreferredBackBufferWidth / 2 - (_genericFont.MeasureString("SCORE").X / 2),
            graphics.PreferredBackBufferWidth - 300
        };
        
        int controllerI = 0;
        
        headers.ForEach(header =>
        {
            spriteBatch.DrawString(_genericFont,header,new Vector2(positionX[controllerI],160),Color.Yellow);
            controllerI++;

        });
    }

    private void DrawLeaderBoards()
    {
        List<User> usersForDataBase = LocalStorage.GetUsersPaginator(10, _page);
        int positionY = 220;
        
        usersForDataBase.ForEach(user =>
        {
            DrawUsers(user,_colors.RandomColor(usersForDataBase.IndexOf(user)) , positionY, usersForDataBase.IndexOf(user));
            positionY += 50;
        });
    }

    private void DrawUsers(User user, Color color, int y,int positionUserForLeaderBoardsScreen)
    {
        float[] positionX = new[] { graphics.PreferredBackBufferWidth / 2 - 600, 
            graphics.PreferredBackBufferWidth / 2 - (_genericFont.MeasureString("SCORE").X / 2),
            graphics.PreferredBackBufferWidth - 300
        };

        string[] dataForUsers = new[] { $"{positionUserForLeaderBoardsScreen + 1}", $"{user.Score}", $"{user.Name}" };
        
        for (int i = 0; i < positionX.Length; i++)
        {
            spriteBatch.DrawString(_genericFont,dataForUsers[i],new Vector2(positionX[i],y),color);
        }
        
    }
}