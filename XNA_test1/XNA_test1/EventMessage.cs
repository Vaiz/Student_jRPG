using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_test1
{
    class EventMessage
    {
        #region Переменные

        private SpriteFont font;        // шрифт текста       
        private Texture2D textureFon;   // текстуры фона
        int windowHeigth;               // высота окна
        int windowWidth;
        private Button buttonOk;
        private string text;            // текст сообщения
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public EventMessage()
        {
            buttonOk = new Button(new Vector2(0, 0), 0, 0, "Ок");
        }

        public Texture2D Fon
        {
            get { return textureFon; }
            set { textureFon = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                buttonOk.Font = value;
                UpdateButtonPosition();
            }
        }

        public int WindowHeigth
        {
            get { return windowHeigth; }
            set 
            {
                windowHeigth = value;
                UpdateButtonPosition();
            }
        }

        public int WindowWidth
        {
            get { return windowWidth; }
            set 
            {
                windowWidth = value;
                UpdateButtonPosition();
            }
        }

        public string Text
        {
            set
            {
                text = value;
            //    UpdateButtonPosition();
            }
        }

        public Dictionary<VisibleState, Texture2D> ButtonTexture
        {
            set
            {
                buttonOk.Textures = value;
            }
        }

        public EventHandler ButtonOk_OnClick
        {
            set { buttonOk.MouseClick += value; }
        }

        public void UpdateButtonPosition()
        {
            buttonOk.X = windowWidth / 2 - 50;
            buttonOk.Y = 160 + (int)font.MeasureString(text).Y;
            buttonOk.Width = 100;
            buttonOk.Heigth = 40;
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            buttonOk.Update(time);
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rectWindFon;  // область экрана для рисования спрайта
            float scaleFone;        // отношение высоты экрана к высоте рисунка.

            scaleFone = (float)windowHeigth / textureFon.Height;
            rectWindFon = new Rectangle(0, 0, (int)(textureFon.Width * scaleFone), windowHeigth);

            bath.Draw(textureFon, rectWindFon, Color.White);

            bath.DrawString(font, text, new Vector2(70,150), Color.Green);
            buttonOk.Draw(bath);
        }

        #endregion

    }
}
