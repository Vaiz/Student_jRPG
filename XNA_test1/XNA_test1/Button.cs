using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_test1
{
    public enum VisibleState
    {
        Normal,     // нормальное или обычное состояние
        Pressed,    // состояние кнопки во время нажатия
        Hover       // состояние кнопки при наведении курсора
    }

    class Button
    {
        #region Переменные

        private Vector2 textPosition;       // позиция кнопки
        private Rectangle imageRectangle;    // позиция кнопки        
        private string buttonText;      // текст на кнопке
        private SpriteFont font;        // шрифт текста        
        private VisibleState currentState;  // текущее состояние кнопки
        private VisibleState previousState; // предыдущее состояние кнопки
        private Dictionary<VisibleState, Texture2D> textures;   // текстуры кнопки
        private MouseState currentMouseState;   // текущее состояние мышки
        private MouseState previousMouseState;  // предыдущее состояние мышки
        bool enabled;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Button(Vector2 position, int width, int height, string buttonText)
        {
            this.buttonText = buttonText;
            this.imageRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            currentState = VisibleState.Normal;
            previousState = VisibleState.Normal;
            enabled = true;
        }

        public SpriteFont Font
        {
            get { return font; }
            set 
            { 
                font = value;
                UpdateTextPosition();
            }
        }

        public Dictionary<VisibleState, Texture2D> Textures
        {
            get { return textures; }
            set { textures = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public int X
        {
            get { return imageRectangle.X; }
            set
            { 
                imageRectangle.X = value;
                UpdateTextPosition();
            }
        }

        public int Y
        {
            get { return imageRectangle.Y; }
            set 
            { 
                imageRectangle.Y = value;
                UpdateTextPosition();
            }
        }

        public int Heigth
        {
            get { return imageRectangle.Height; }
            set             
            { 
                imageRectangle.Height = value;
                UpdateTextPosition();
            }
        }

        public int Width
        {
            get { return imageRectangle.Width; }
            set
            { 
                imageRectangle.Width = value;
                UpdateTextPosition();
            }
        }
        
        public void UpdateTextPosition()
        {
            textPosition = new Vector2(imageRectangle.X + (imageRectangle.Width - font.MeasureString(buttonText).X) / 2, 
                imageRectangle.Y + (imageRectangle.Height - font.MeasureString(buttonText).Y) / 2);
        }

        public string Text
        {
            set 
            {
                buttonText = value;
                UpdateTextPosition();
            }
        }

        #endregion
        //==============================================================================================
        #region Events

        public event EventHandler MouseClick;
        public event EventHandler MouseDown;
        public event EventHandler MouseUp;
        public event EventHandler MouseOut;

        private void OnMouseOut(EventArgs e)
        {
            EventHandler handler = MouseOut;
            if (handler != null) handler(this, e);
        }

        private void OnMouseUp(EventArgs e)
        {
            EventHandler handler = MouseUp;
            if (handler != null) handler(this, e);
        }

        private void OnMouseDown(EventArgs e)
        {
            EventHandler handler = MouseDown;
            if (handler != null) handler(this, e);
        }

        private void OnMouseClick(EventArgs e)
        {
            EventHandler handler = MouseClick;
            if (handler != null) handler(this, e);
        }
        
        #endregion
        //==============================================================================================
        #region Основные потоки
        
        public void Update(GameTime time)
        {
            if (enabled)
            {
                previousState = currentState;

                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();

                if (imageRectangle.Contains(currentMouseState.X, currentMouseState.Y))
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (previousMouseState.LeftButton == ButtonState.Released)
                        {
                            OnMouseDown(EventArgs.Empty);
                            currentState = VisibleState.Pressed;
                        }
                        else if (currentState != VisibleState.Pressed)
                        {
                            currentState = VisibleState.Hover;
                        }
                    }
                    else
                    {
                        if (previousState == VisibleState.Pressed)
                        {
                            OnMouseClick(EventArgs.Empty);
                        }
                        currentState = VisibleState.Hover;
                    }
                }
                else
                {
                    if (previousState == VisibleState.Hover || previousState == VisibleState.Pressed)
                    {
                        OnMouseOut(EventArgs.Empty);
                    }
                    currentState = VisibleState.Normal;
                }

                if (currentMouseState.LeftButton == ButtonState.Released
                    && previousState == VisibleState.Pressed)
                {
                    OnMouseUp(EventArgs.Empty);
                }
            }
        }

        public void Draw(SpriteBatch bath)
        {
            if (enabled)
            {
                bath.Draw(textures[currentState], imageRectangle, Color.White);
                bath.DrawString(font, buttonText, textPosition, Color.White);
            }
        }

        #endregion
    }
}
