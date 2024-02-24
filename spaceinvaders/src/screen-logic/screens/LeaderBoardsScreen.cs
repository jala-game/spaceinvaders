using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using spaceinvaders.model;
using spaceinvaders.services;

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
    private EScreenMenuOptionsGameOver _chooseMenu = EScreenMenuOptionsGameOver.RightArrow;
    private float delayToPress = 10f;
    private int _placingManage = 0;
    private ColorLeaderBoards _colors = new();

    public override void Initialize() { }

    public override void LoadContent() { }

    public override void Update(GameTime gameTime)
    {
        var kstate = Keyboard.GetState();
        
        delayToPress--;
        if (delayToPress > 0) return;
        
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

    private void DrawUsers(User user, Color color, int y,int positionUser)
    {
        float[] positionX = new[] { graphics.PreferredBackBufferWidth / 2 - 600, 
            graphics.PreferredBackBufferWidth / 2 - (_genericFont.MeasureString("SCORE").X / 2),
            graphics.PreferredBackBufferWidth - 300
        };

        string[] placingOptions = new[] { "ST","ND","RD","TH"};
        string placingPosition = positionUser > 3 ? placingOptions[3] : placingOptions[positionUser];

        string[] dataForUser = new[] { $"{positionUser + 1 + _placingManage}{placingPosition}", $"{user.Score}", $"{user.Name}" };
        
        for (int i = 0; i < positionX.Length; i++)
        {
            spriteBatch.DrawString(_genericFont,dataForUser[i],new Vector2(positionX[i],y),color);
        }
        
    }

    private void DrawMenu()
    {
        string[] texts = new[] { "  Back" , "<",$"{_page + 1}",">"};
        float[] positionsX = new[] { 100, 
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"<").X / 2) - 70,
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"{_page + 1}").X / 2),
            graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($">").X / 2) + 70,
        };

        for (int i = 0; i < texts.Length; i++)
        {
            DrawMenuItem(positionsX[i],texts[i], null);
        }
        
    }

    private void DrawMenuItem(float x, string text, Color? color)
    {
        spriteBatch.DrawString(_genericFont,text,new Vector2(x,graphics.PreferredBackBufferHeight - 100),color ?? Color.White);
    }
    
    private void DrawMenuItemActive()
    {
        switch (_chooseMenu)
        {
            case  EScreenMenuOptionsGameOver.LeaveGame:
                DrawMenuItem(100, "> Back", Color.Green);
                break;
            case EScreenMenuOptionsGameOver.RightArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($">").X / 2) + 70, ">", Color.Green);
                break;
            case EScreenMenuOptionsGameOver.LeftArrow:
                DrawMenuItem(graphics.PreferredBackBufferWidth/2 - (_genericFont.MeasureString($"<").X / 2) - 70, "<", Color.Green);
                break;
        }
    }
    
    private void SendMenuOption(KeyboardState kstate)
    {
        delayToPress--;
        if (delayToPress > 0) return;
        if (!kstate.IsKeyDown(Keys.Enter)) return;
        switch (_chooseMenu)
        {
            case EScreenMenuOptionsGameOver.LeaveGame:
                LeaveTheGame();
                break;
            case EScreenMenuOptionsGameOver.LeftArrow:
                ReturnPages();
                delayToPress = 10;
                break;
            case EScreenMenuOptionsGameOver.RightArrow:
                NextPages();
                delayToPress = 10;
                break;
        }
    }
    
    private void LeaveTheGame()
    {
        MainScreen mainScreen = new MainScreen(game,graphics,contentManager,spriteBatch );
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
        List<User> usersForDataBase = LocalStorage.GetUsersPaginator(10, _page + 1);
        if (usersForDataBase.Count == 0) return;
        _page++;
        _placingManage += 10;
    }
    
    private void ModifyMenuSelection(KeyboardState kstate)
    {
        float resetDelay = 10;
        if (kstate.IsKeyDown(Keys.Left) && _chooseMenu > EScreenMenuOptionsGameOver.LeaveGame)
        {
            _chooseMenu--;
            delayToPress = resetDelay;
        }
        
        if (kstate.IsKeyDown(Keys.Right) && _chooseMenu < EScreenMenuOptionsGameOver.RightArrow)
        {
            _chooseMenu++;
            delayToPress = resetDelay;
        }
    }
}