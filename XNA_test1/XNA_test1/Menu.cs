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

        Texture2D mainMenuFon;          //фоновое изображения для меню
        Button buttonNewGame;
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
            
            buttonNewGame =             new Button(new Vector2(x, y), 260, 40, "Новая игра");
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

            mainMenuFon = content.Load<Texture2D>("fon1_16_9"); //загрузка фонового изображения для меню   

            buttonNewGame.Font = content.Load<SpriteFont>("font\\button_font");
            buttonChangeResolution.Font = content.Load<SpriteFont>("font\\button_font");
            buttonFullScreen.Font = content.Load<SpriteFont>("font\\button_font");
            buttonExit.Font = content.Load<SpriteFont>("font\\button_font"); 

            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button2_norm"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button2_hover"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button2_pressed"));

            buttonNewGame.Textures = buttonTextures;
            buttonChangeResolution.Textures = buttonTextures;
            buttonFullScreen.Textures = buttonTextures;
            buttonExit.Textures = buttonTextures;

        }

        public int WindowHeigth
        {
            get { return windowHeigth; }
            set { windowHeigth = value; }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            buttonNewGame.Update(time);
            buttonChangeResolution.Update(time);
            buttonFullScreen.Update(time);
            buttonExit.Update(time);
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rectWindFon;  //область экрана для рисования спрайта
            float scaleFone;        // отношение высоты экрана к высоте рисунка.

            scaleFone = (float)windowHeigth / mainMenuFon.Height;
            rectWindFon = new Rectangle(0, 0, (int)(mainMenuFon.Width * scaleFone), windowHeigth);

            bath.Draw(mainMenuFon, rectWindFon, Color.White);            

            buttonNewGame.Draw(bath);
            buttonChangeResolution.Draw(bath);
            buttonFullScreen.Draw(bath);
            buttonExit.Draw(bath);
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        public EventHandler ButtonNewGame_OnClick
        {
            set { buttonNewGame.MouseClick += value; }
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
