using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    class EnterName
    {
        #region Переменные

        Texture2D textureFon;   // текстуры фона
        int windowHeigth;               // высота окна
        int windowWidth;
        Button buttonOk;
        Label label;
        Label name;
        List<Keys> keysPressedLastFrame;
        #endregion

        #region Инициализация

        public EnterName()
        {
            buttonOk = new Button(new Vector2(0, 0), 0, 0, "Ок");
            label = new Label();

            label.X = 70;
            label.Y = 150;
            label.Color = Color.Yellow;
            label.Text = "Введите ваше имя: ";

            name = new Label();

            name.X = 300;
            name.Y = 150;
            name.Color = Color.Yellow;
            name.Text = "Student";

            keysPressedLastFrame = new List<Keys>();
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
                name.Font = value;
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
            buttonOk.Height = 40;
        }

        #endregion

        #region Основные потоки

        public void Update(GameTime time)
        {
            buttonOk.Update(time);

            KeyboardState keyboardState = Keyboard.GetState();
            // Нажата ли клавиша Shift (проверяем оба, левый и правый)
            bool isShiftPressed =
                keyboardState.IsKeyDown(Keys.LeftShift) ||
                keyboardState.IsKeyDown(Keys.RightShift);

            

            // Перебираем все нажатые клавиши
            foreach (Keys pressedKey in keyboardState.GetPressedKeys())
            {    // Обрабатываем только те, которые не были нажаты в предыдущем кадре
                if (keysPressedLastFrame.Contains(pressedKey) == false)
                {
                    // Нет специальных клавиш?
                    if (IsSpecialKey(pressedKey) == false &&
                        // Допустимо не более 32 символов
                        name.Text.Length < 32)
                    {
                        // Добавляем букву к нашему inputText.
                        // Также проверяем состояние Shift!
                        char c = KeyToChar(pressedKey, isShiftPressed);
                        if (c != ' ')
                            name.Text += c; 
                    } // if (IsSpecialKey)
                    else if (pressedKey == Keys.Back &&
                        name.Text.Length > 0)
                    {
                        // Удаляем 1 символ в конце
                        name.Text = name.Text.Substring(0, name.Text.Length - 1);
                    } // else if
                } // foreach if (WasKeyPressedLastFrame)
            }
            keysPressedLastFrame = new List<Keys>(keyboardState.GetPressedKeys());
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rectWindFon;  // область экрана для рисования спрайта
            float scaleFone;        // отношение высоты экрана к высоте рисунка.

            scaleFone = (float)windowHeigth / textureFon.Height;
            rectWindFon = new Rectangle(0, 0, (int)(textureFon.Width * scaleFone), windowHeigth);

            bath.Draw(textureFon, rectWindFon, Color.White);

            label.Draw(bath);
            name.Draw(bath);
            buttonOk.Draw(bath);
        }

        #endregion

        #region Другие функции

        /// <summary>
        /// Вспомогательный метод преобразования клавиши в символ.
        /// Примечание: если расположение клавиш отличается от стандартной
        /// клавиатуры QWERTY, метод не будет правильно работать. Большинство
        /// клавиатур возвращают одни и те же значения для A-Z и 0-9,
        /// но специальные клавиши могут отличаться. Извините, легкого способа
        /// исправить это в XNA нет... Для игр с возможностью общения (окном)
        /// вы должны использовать для сбора ввода с клавиатуры соытия Windows,
        /// что гораздо лучше!
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>Символ</returns>
        public static char KeyToChar(Keys key, bool shiftPressed = false)
        {
          // Если клавиша не найдена, просто возвращаем пробел
            char ret = ' ';
            int keyNum = (int)key;
            if (keyNum >= (int)Keys.A && keyNum <= (int)Keys.Z)
            {
                if (shiftPressed)
                    ret = key.ToString()[0];
                else
                    ret = key.ToString().ToLower()[0];
            } // if (keyNum)
            else if (keyNum >= (int)Keys.D0 && keyNum <= (int)Keys.D9 &&
                       shiftPressed == false)
            {
                ret = (char)((int)'0' + (keyNum - Keys.D0));
            } // else if
            else if (key == Keys.D1 && shiftPressed)
                ret = '!';
            else if (key == Keys.D2 && shiftPressed)
                ret = '@';

          //[и т.д. еще около 20 проверок специальных клавиш]

          // Возвращаем результат
          return ret;
        } // KeyToChar(key)

        public bool IsSpecialKey(Keys key)
        {
            // All keys except A-Z, 0-9 and `-\[];',./= (and space) are special keys.
            // With shift pressed this also results in this keys:
            // ~_|{}:"<>? !@#$%^&*().

            int keyNum = (int)key;

            if ((keyNum >= (int)Keys.A && keyNum <= (int)Keys.Z) ||
             (keyNum >= (int)Keys.D0 && keyNum <= (int)Keys.D9) ||
             key == Keys.Space || // well, space ^^
             key == Keys.OemTilde || // `~
             key == Keys.OemMinus || // -_
             key == Keys.OemPipe || // \|
             key == Keys.OemOpenBrackets || // [{
             key == Keys.OemCloseBrackets || // ]}
             key == Keys.OemQuotes || // '"
             key == Keys.OemQuestion || // /?
             key == Keys.OemPlus) // =+
            {
                return false;
            }

            return true;
        }

        public string GetName()
        {
            return name.Text;
        }

        #endregion

    }
}
