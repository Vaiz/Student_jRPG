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

        //SpriteFont font;        // шрифт текста       
        Texture2D textureFon;   // текстуры фона
        int windowHeigth;               // высота окна
        int windowWidth;
        Button buttonOk;
        Label label;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public EventMessage()
        {
            buttonOk = new Button(new Vector2(0, 0), 0, 0, "Ок");
            label = new Label();

            label.X = 70;
            label.Y = 150;
            label.Color = Color.Yellow;
        }

        public Texture2D Fon
        {
            get { return textureFon; }
            set { textureFon = value; }
        }

        public SpriteFont Font
        {
            set
            {
                label.Font = value;
                buttonOk.Font = value;
            }
        }

        public int WindowHeigth
        {
            get { return windowHeigth; }
            set 
            {
                windowHeigth = value;
            }
        }

        public int WindowWidth
        {
            get { return windowWidth; }
            set 
            {
                windowWidth = value;
            }
        }

        public string Text
        {
            set
            {
                label.Text = value;            
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
            buttonOk.Y = 160 + (int)label.GetStringHeigth();
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

            label.Draw(bath);
            buttonOk.Draw(bath);
        }

        #endregion

    }
}
