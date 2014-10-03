using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    class Map
    {
        #region Переменные

        byte[,] map;
        int sizeX, sizeY;
        Texture2D floor;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public Map()
        {
            sizeX = 60;
            sizeY = 100;
            map = new byte[sizeX, sizeY];

            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    map[i,j] = 0;
                }
            }

        }        

        public Texture2D Texture
        {
            get { return floor; }
            set 
            {
                floor = value;           
            }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {

        }

        public void Draw(SpriteBatch bath)
        {

        }

        #endregion
        //==============================================================================================
        #region Другие функции



        #endregion
    }
}
