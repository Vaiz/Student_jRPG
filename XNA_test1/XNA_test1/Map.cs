using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    class Map
    {
        #region Переменные

        byte[] map;
        int sizeX, sizeY;
        Texture2D floor;
        Texture2D wall;
        Vector2 position;   // центр карты в координатах карты
        //Vector2 resolution;
        int windowWidth;
        int windowHeigth;
        int x0, y0;         // точка начала отрисовки центральной плитки карты
        int x, y;           // количество отрисовываемых плиток
        int speed;          // количество миллисекунд для смены кадра на новый
        int timeFromLastFrame;    // время, которое прошло с момента смены последнего кадра в милиссекундах

        #endregion
        //==============================================================================================
        #region Инициализация

        public Map()
        {
            sizeX = 60;
            sizeY = 100;

            map = new byte[sizeX * sizeY];
            map = File.ReadAllBytes("map.bin");

            position = new Vector2(48, 94);
            speed = 100;
        }

        public void LoadContent(ContentManager content)
        {
            floor = content.Load<Texture2D>("floor\\floor1");
            wall = content.Load<Texture2D>("wall\\wall");
        }

        public int WindowHeigth
        {
            set
            {
                windowHeigth = value;
                y0 = windowHeigth / 2 - 16;
                y = (windowHeigth - y0) / 32 + 1;
            }
        }

        public int WindowWidth
        {
            set
            {
                windowWidth = value;
                x0 = windowWidth / 2 - 16;
                x = (windowWidth - x0) / 32 + 1;
            }
        }


        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            bool move;
            Vector2 vectorMove = new Vector2(0,0);
            int k;

            move = false;
            k = (int)position.Y * sizeX + (int)position.X;

            if (Keyboard.GetState().IsKeyDown(Keys.D) && (map[k] & (1 << 4)) == 0)
            {
                move = true;
                vectorMove.X++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) && (map[k] & (1 << 5)) == 0)
            {
                move = true;
                vectorMove.X--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && (map[k] & (1 << 6)) == 0)
            {
                move = true;
                vectorMove.Y++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && (map[k] & (1 << 7)) == 0)
            {
                move = true;
                vectorMove.Y--;
            }
                       
            if (move)
            {
                timeFromLastFrame += time.ElapsedGameTime.Milliseconds;
                if (timeFromLastFrame > speed)
                {
                    timeFromLastFrame = 0;
                    position += vectorMove;
                }
            }
            else
            {
                timeFromLastFrame = 0;
            }
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rect;
            rect = new Rectangle(0, 0, 32, 32);

            int k;

            for (int i = -y; i <= y; i++)
            {
                for (int j = -x; j <= x; j++)
                {
                    if (j + (int)position.X >= sizeX || i + (int)position.Y >= sizeY ||
                        j + (int)position.X < 0 || i + (int)position.Y < 0)
                    { }
                    else
                    {
                        k = (i + (int)position.Y) * sizeX + (j + (int)position.X);
                        if ((map[k] & 0x0f) != 0)
                        {
                            rect.X = x0 + j * 32;
                            rect.Y = y0 + i * 32;
                            bath.Draw(floor, rect, new Rectangle(96, 0, 32, 32), Color.White);
                            if ((map[k] & (1 << 4)) != 0)
                            {
                                bath.Draw(wall, rect, new Rectangle(32, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 5)) != 0)
                            {
                                bath.Draw(wall, rect, new Rectangle(64, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 6)) != 0)
                            {
                                bath.Draw(wall, rect, new Rectangle(96, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 7)) != 0)
                            {
                                bath.Draw(wall, rect, new Rectangle(128, 0, 32, 32), Color.White);
                            }
                        }
                    }
                }
            }
        }

        #endregion
        //==============================================================================================
        #region Другие функции



        #endregion
    }
}
