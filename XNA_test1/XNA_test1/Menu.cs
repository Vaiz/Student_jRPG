using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_test1
{
    class Menu
    {
        #region Переменные

        Texture2D textureBackGround;          //фоновое изображения для меню
        Button buttonContinue;
        Button buttonNewGame;
        Button buttonRecords;
        Button buttonMapEditor;
        Button buttonChangeResolution;
        Button buttonFullScreen;
        Button buttonExit;
        int windowHeigth;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public Menu()
        {
            int x, y;
            x = 10;
            y = 10;

            buttonContinue =            new Button(new Vector2(x, y), 260, 40, "Продолжить игру");
            y += 50;
            
            buttonNewGame =             new Button(new Vector2(x, y), 260, 40, "Новая игра");
            y += 50;

            buttonRecords =             new Button(new Vector2(x, y), 260, 40, "Рекорды");
            y += 50;

            buttonMapEditor =           new Button(new Vector2(x, y), 260, 40, "Редактировать карту");
            y += 50;
            
            buttonChangeResolution =    new Button(new Vector2(x, y), 260, 40, "Изменить разрешение");
            y += 50;
            
            buttonFullScreen =          new Button(new Vector2(x, y), 260, 40, "На полный экран");
            y += 50;
            
            buttonExit =                new Button(new Vector2(x, y), 260, 40, "Выход");
            y += 50;
        }

        public void LoadContent(ContentManager content)
        {
            Dictionary<VisibleState, Texture2D> buttonTextures;

            textureBackGround = content.Load<Texture2D>("fon1_16_9"); //загрузка фонового изображения для меню   

            buttonContinue.Font = content.Load<SpriteFont>("font\\button_font");
            buttonNewGame.Font = content.Load<SpriteFont>("font\\button_font");
            buttonRecords.Font = content.Load<SpriteFont>("font\\button_font");
            buttonMapEditor.Font = content.Load<SpriteFont>("font\\button_font");
            buttonChangeResolution.Font = content.Load<SpriteFont>("font\\button_font");
            buttonFullScreen.Font = content.Load<SpriteFont>("font\\button_font");
            buttonExit.Font = content.Load<SpriteFont>("font\\button_font"); 

            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button2_norm"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button2_hover"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button2_pressed"));

            buttonContinue.Textures = buttonTextures;
            buttonNewGame.Textures = buttonTextures;
            buttonRecords.Textures = buttonTextures;
            buttonMapEditor.Textures = buttonTextures;
            buttonChangeResolution.Textures = buttonTextures;
            buttonFullScreen.Textures = buttonTextures;
            buttonExit.Textures = buttonTextures;

        }

        public int WindowHeigth
        {
            get { return windowHeigth; }
            set { windowHeigth = value; }
        }

        public bool GameStarted
        {
            set
            {
                if(value)
                {
                    buttonContinue.Enabled = true;
                }
                else
                {
                    buttonContinue.Enabled = false;
                }
            }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            buttonContinue.Update(time);
            buttonNewGame.Update(time);
            buttonRecords.Update(time);
            buttonMapEditor.Update(time);
            buttonChangeResolution.Update(time);
            buttonFullScreen.Update(time);
            buttonExit.Update(time);
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rectWindFon;  //область экрана для рисования спрайта
            float scaleFone;        // отношение высоты экрана к высоте рисунка.

            scaleFone = (float)windowHeigth / textureBackGround.Height;
            rectWindFon = new Rectangle(0, 0, (int)(textureBackGround.Width * scaleFone), windowHeigth);

            bath.Draw(textureBackGround, rectWindFon, Color.White);

            buttonContinue.Draw(bath);
            buttonNewGame.Draw(bath);
            buttonRecords.Draw(bath);
            buttonMapEditor.Draw(bath);
            buttonChangeResolution.Draw(bath);
            buttonFullScreen.Draw(bath);
            buttonExit.Draw(bath);
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        public EventHandler ButtonContinue_OnClick
        {
            set { buttonContinue.MouseClick += value; }
        }

        public EventHandler ButtonNewGame_OnClick
        {
            set { buttonNewGame.MouseClick += value; }
        }

        public EventHandler ButtonRecords_OnClick
        {
            set { buttonRecords.MouseClick += value; }
        }

        public EventHandler ButtonMapEditor_OnClick
        {
            set { buttonMapEditor.MouseClick += value; }
        }

        public EventHandler ButtonChangeResolution_OnClick
        {
            set { buttonChangeResolution.MouseClick += value; }
        }

        public EventHandler ButtonFullScreen_OnClick
        {
            set { buttonFullScreen.MouseClick += value; }
        }

        public EventHandler ButtonExit_OnClick
        {
            set { buttonExit.MouseClick += value; }
        }

        #endregion
    }
}
