using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_test1
{
    public class main : Microsoft.Xna.Framework.Game
    {
        #region ����������

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int situation;

        Menu menu;
        Game game;
        Texture2D cursor;               //������ � ������������-��������
        Rectangle cursorPosition;       //������� ������� �������

        Vector2 []resolution;
        int resolutionNumber;
        int resolutionCnt;
        
        #endregion
        //==============================================================================================
        
        #region �������������
        public main()   // �����������
        {
            situation = 1;  // �� � ����

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;    //������ ������
            graphics.PreferredBackBufferHeight = 720;   //������ ������            
            //graphics.IsFullScreen = true; //������������� ����� 

            resolution = new Vector2[4];
            resolution[0].X = 1280;
            resolution[0].Y = 720;

            resolution[1].X = 1366;
            resolution[1].Y = 768;

            resolution[2].X = 1600;
            resolution[2].Y = 900;

            resolution[3].X = 1920;
            resolution[3].Y = 1080;

            resolutionNumber = 0;
            resolutionCnt = 4;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menu = new Menu();
            game = new Game();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            cursor = Content.Load<Texture2D>("cursor\\cursor_sword");

            menu.LoadContent(Content);
            menu.WindowHeigth = graphics.PreferredBackBufferHeight;

            menu.ButtonNewGame_OnClick = ButtonNewGame_OnClick;
            menu.ButtonChangeResolution_OnClick = ButtonChangeResolution_OnClick;
            menu.ButtonFullScreen_OnClick = ButtonFullScreen_OnClick;
            menu.ButtonExit_OnClick = ButtonExit_OnClick;

            game.LoadContent(Content);
            game.WindowHeigth = graphics.PreferredBackBufferHeight;
            game.WindowWidth = graphics.PreferredBackBufferWidth;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        #endregion
        //==============================================================================================
        #region �������� ������
        
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (situation)
            {
                case 1:
                    menu.Update(gameTime);
                    break;

                case 2:
                    game.Update(gameTime);
                    break;
            }
            
            cursorPosition = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, cursor.Width, cursor.Height);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch(situation)
            { 
                case 1:
                    menu.Draw(spriteBatch);
                    break;

                case 2:
                    game.Draw(spriteBatch);
                    break;
            }


            spriteBatch.Draw(cursor, cursorPosition, Color.White);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        #endregion
        //==============================================================================================
        #region ������� ������ ��� �������

        private void ButtonNewGame_OnClick(object sender, EventArgs e)
        {
            situation = 2;
        }

        private void ButtonChangeResolution_OnClick(object sender, EventArgs e)
        {
            resolutionNumber++;
            if (resolutionNumber == resolutionCnt) resolutionNumber = 0; 
            graphics.PreferredBackBufferWidth = (int) resolution[resolutionNumber].X;
            graphics.PreferredBackBufferHeight = (int) resolution[resolutionNumber].Y;   //������ ������      
            graphics.ApplyChanges();

            menu.WindowHeigth = graphics.PreferredBackBufferHeight;

            game.WindowHeigth = graphics.PreferredBackBufferHeight;
            game.WindowWidth = graphics.PreferredBackBufferWidth;
        }

        private void ButtonFullScreen_OnClick(object sender, EventArgs e)
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
        }

        private void ButtonExit_OnClick(object sender, EventArgs e)
        {
            this.Exit();
        }

        #endregion
    }
}