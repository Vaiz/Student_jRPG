using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    class Label
    {
        #region Переменные

        string text;            // текст сообщения
        Color color;
        SpriteFont font;        // шрифт текста       
        int x;              
        int y;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public Label()
        {
            color = Color.Black;
        }
       
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
            }
        }

        public int X
        {
            get { return x; }
            set 
            {
                x = value;
            }
        }

        public int Y
        {
            get { return y; }
            set 
            {
                y = value;
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

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            
        }

        public void Draw(SpriteBatch bath)
        {
            bath.DrawString(font, text, new Vector2(x, y), color);
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        public float GetStringHeigth()
        {
            return font.MeasureString(text).Y;
        }

        #endregion
    }
}
