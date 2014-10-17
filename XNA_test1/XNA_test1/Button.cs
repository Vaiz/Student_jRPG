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

        private bool enabled;
        private Rectangle imageRectangle;    // позиция кнопки        
        private Dictionary<VisibleState, Texture2D> textures;   // текстуры кнопки

        private SpriteFont font;        // шрифт текста        
        public  Color textColor;
        private Vector2 textPosition;       // позиция текста
        private string buttonText;      // текст на кнопке
        private bool showHint;
        private string hintText;
        private Vector2 hintPosition;
        public Texture2D hintTexture;
        private Rectangle hintTexturePosition;

        private VisibleState currentState;  // текущее состояние кнопки
        private VisibleState previousState; // предыдущее состояние кнопки
        private MouseState currentMouseState;   // текущее состояние мышки
        private MouseState previousMouseState;  // предыдущее состояние мышки
        
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public Button(Vector2 position, int width, int height, string buttonText)
        {
            this.buttonText = buttonText;
            this.imageRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            textColor = Color.White;
            currentState = VisibleState.Normal;
            previousState = VisibleState.Normal;
            enabled = true;
            showHint = false;
        }

        public SpriteFont Font
        {
            get { return font; }
            set 
            { 
                font = value;
                UpdatePosition();
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
                UpdatePosition();
            }
        }

        public int Y
        {
            get { return imageRectangle.Y; }
            set 
            { 
                imageRectangle.Y = value;
                UpdatePosition();
            }
        }

        public int Height
        {
            get { return imageRectangle.Height; }
            set             
            { 
                imageRectangle.Height = value;
                UpdatePosition();
            }
        }

        public int Width
        {
            get { return imageRectangle.Width; }
            set
            { 
                imageRectangle.Width = value;
                UpdatePosition();
            }
        }
        
        public void UpdatePosition()
        {
            textPosition = new Vector2(imageRectangle.X + (imageRectangle.Width - font.MeasureString(buttonText).X) / 2, 
                imageRectangle.Y + (imageRectangle.Height - font.MeasureString(buttonText).Y) / 2);

            if(hintText != null)
            {
                hintPosition = new Vector2(imageRectangle.X + imageRectangle.Width / 2 - font.MeasureString(hintText).X / 2, imageRectangle.Y - font.MeasureString(hintText).Y);
                hintTexturePosition.X = (int)hintPosition.X - 5;
                hintTexturePosition.Y = (int)hintPosition.Y - 5;
                hintTexturePosition.Width = (int)font.MeasureString(hintText).X + 10;
                hintTexturePosition.Height = (int)font.MeasureString(hintText).Y + 10;

            }
        }

        public string Text
        {
            set 
            {
                buttonText = value;
                UpdatePosition();
            }
        }

        public string Hint
        {
            set
            {
                hintText = value;               
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

                if(currentState == VisibleState.Hover)
                {
                    if (hintText != null)
                    {
                        showHint = true;
                    }
                }
                else
                {
                    if (hintText != null)
                    {
                        showHint = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch bath)
        {
            if (enabled)
            {
                bath.Draw(textures[currentState], imageRectangle, Color.White);
                bath.DrawString(font, buttonText, textPosition, textColor);

                if(showHint)
                {
                    bath.Draw(hintTexture, hintTexturePosition, Color.White);
                    bath.DrawString(font, hintText, hintPosition, textColor);
                }
            }
        }

        #endregion
    }
}
