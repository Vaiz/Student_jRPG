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
using System.IO;

namespace XNA_test1
{
    public class main : Microsoft.Xna.Framework.Game
    {
        #region Переменные

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int situation;

        Menu menu;
        Game game;
        MapEditor mapEditor;
        Texture2D cursor;               //Спрайт с изображением-курсором
        Rectangle cursorPosition;       //Текущая позиция курсора

        Vector2 []resolution;
        int resolutionNumber;
        int resolutionCnt;

        List<Record> records;
        RecordsList recordsList;

        #endregion
        //==============================================================================================       
        #region Инициализация
        public main()   // конструктор
        {
            situation = 1;  // Мы в меню

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;    //ширина экрана
            graphics.PreferredBackBufferHeight = 720;   //высота экрана            
            //graphics.IsFullScreen = true; //полноэкранный режим 

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

            records = new List<Record>();

            if (File.Exists("records.txt"))
            {
                StreamReader sr = File.OpenText("records.txt");
                Record readRecord;
                
                while (records.Count() < 20)
                {
                    readRecord.name = sr.ReadLine();
                    if (readRecord.name == null) 
                        break;
                    
                    readRecord.score = Convert.ToInt32(sr.ReadLine());
                    records.Add(readRecord);
                }
            }

            recordsList = new RecordsList();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menu = new Menu();
            game = new Game();
            mapEditor = new MapEditor();
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

            menu.ButtonContinue_OnClick = ButtonContinue_OnClick;
            menu.ButtonNewGame_OnClick = ButtonNewGame_OnClick;
            menu.ButtonRecords_OnClick = ButtonRecords_OnClick;
            menu.ButtonMapEditor_OnClick = ButtonMapEditor_OnClick;
            menu.ButtonChangeResolution_OnClick = ButtonChangeResolution_OnClick;
            menu.ButtonFullScreen_OnClick = ButtonFullScreen_OnClick;
            menu.ButtonExit_OnClick = ButtonExit_OnClick;
            menu.GameStarted = false;

            game.LoadContent(Content);
            game.WindowHeigth = graphics.PreferredBackBufferHeight;
            game.WindowWidth = graphics.PreferredBackBufferWidth;

            mapEditor.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        #endregion
        //==============================================================================================
        #region Основные потоки
        
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                situation = 1;
            }

            switch (situation)
            {
                case 1:
                    menu.Update(gameTime);
                    break;

                case 2:
                    game.Update(gameTime);
                    break;

                case 3:
                    mapEditor.Update(gameTime);
                    break;

                case 4:
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
                    MediaPlayer.Stop();
                    break;

                case 2:
                    game.Draw(spriteBatch);
                    break;

                case 3:
                    mapEditor.Draw(spriteBatch);
                    MediaPlayer.Stop();
                    break;

                case 4:
                    recordsList.Draw(spriteBatch);
                    break;
            }


            spriteBatch.Draw(cursor, cursorPosition, Color.White);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonContinue_OnClick(object sender, EventArgs e)
        {
            situation = 2;
        }
        
        private void ButtonNewGame_OnClick(object sender, EventArgs e)
        {
            situation = 2;
            menu.GameStarted = true;
            game.Init();
        }

        private void ButtonRecords_OnClick(object sender, EventArgs e)
        {
            recordsList.SetRecords(records, Content.Load<SpriteFont>("font\\button_font"));
            situation = 4;
        }

        private void ButtonMapEditor_OnClick(object sender, EventArgs e)
        {
            situation = 3;
        }

        private void ButtonChangeResolution_OnClick(object sender, EventArgs e)
        {
            resolutionNumber++;
            if (resolutionNumber == resolutionCnt) resolutionNumber = 0; 
            graphics.PreferredBackBufferWidth = (int) resolution[resolutionNumber].X;
            graphics.PreferredBackBufferHeight = (int) resolution[resolutionNumber].Y;   //высота экрана      
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
