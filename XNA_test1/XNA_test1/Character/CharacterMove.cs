using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_test1.Character
{
    class CharacterMove
    {
        #region Переменные

        Texture2D texture;
        int windowWidth;
        int windowHeigth;
        int strNumber;      // номер строки, откуда брать картинку
        int positionNumber; // номер позиции откуда брать картинку
        Rectangle box;      // центр экрана, где находится перс
        int speed;          // количество миллисекунд для смены кадра на новый
        int timeFromLastFrame;    // время, которое прошло с момента смены последнего кадра в милиссекундах

        #endregion
        //==============================================================================================
        #region Инициализация

        public CharacterMove()
        {
            strNumber = 0;
            positionNumber = 0;
            speed = 100;
        }        

        public Texture2D Texture
        {
            get { return texture; }
            set 
            {
                texture = value;
                UpdateBox();
            }
        }

        public int WindowWidth
        {
            set 
            {
                windowWidth = value;
                UpdateBox();
            }
        }

        public int WindowHeigth
        {
            set 
            {
                windowHeigth = value;
                UpdateBox();
            }
        }

        public void UpdateBox()
        {
            box = new Rectangle(windowWidth / 2 - texture.Width / 8,
                                windowHeigth / 2 - texture.Height / 8,
                                texture.Width / 4, texture.Height / 4);
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            bool move;

            move = false;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                move = true;
                strNumber = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                move = true;
                strNumber = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                move = true;
                strNumber = 2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                move = true;
                strNumber = 3;
            }
            else
            {
                timeFromLastFrame = 0;
                positionNumber = 0;
            }

            if(move)
            {
                timeFromLastFrame += time.ElapsedGameTime.Milliseconds;
                if (timeFromLastFrame > speed)
                {
                    timeFromLastFrame = 0;
                    positionNumber++;
                    positionNumber %= 4;
                }
            }
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle sourceRectangle;

            sourceRectangle = new Rectangle(texture.Width / 4 * positionNumber, texture.Height / 4 * strNumber,
                                            texture.Width / 4 , texture.Height / 4);

            bath.Draw(texture, box, sourceRectangle, Color.White);
        }

        #endregion
    }
}
