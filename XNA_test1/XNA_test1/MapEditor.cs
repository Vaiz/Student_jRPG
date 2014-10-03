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
    class MapEditor
    {
        #region Переменные

        byte[] map;
        int sizeX, sizeY;
        Texture2D floor;
        Texture2D wall;
        Button buttonSave;
        Rectangle rectFloor;
        Rectangle[] rectWall;
        byte currentType;
        MouseState currentMouseState;   // текущее состояние мышки
        MouseState previousMouseState;  // предыдущее состояние мышки
        Label labelPoint;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public MapEditor()
        {
            buttonSave = new Button(new Vector2(500, 300), 260, 40, "Сохранить карту");
            labelPoint = new Label();

            sizeX = 60;
            sizeY = 100;

            currentType = 0;
            rectFloor = new Rectangle(500, 50, 32, 32);
            rectWall = new Rectangle[4];
            rectWall[0] = new Rectangle(500, 100, 32, 32);
            rectWall[1] = new Rectangle(550, 100, 32, 32);
            rectWall[2] = new Rectangle(600, 100, 32, 32);
            rectWall[3] = new Rectangle(650, 100, 32, 32);

            map = new byte[sizeX * sizeY];
            
            if (File.Exists("map.bin"))
            {
                map = File.ReadAllBytes("map.bin");
            }
            else
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        map[i * sizeX + j] = 0;
                    }
                }
            }

        }

        public void LoadContent(ContentManager content)
        {           
            floor = content.Load<Texture2D>("floor\\floor1");
            wall = content.Load<Texture2D>("wall\\wall");

            Dictionary<VisibleState, Texture2D> buttonTextures;
            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button3_norm"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button3_hover"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button3_pressed"));

            buttonSave.Textures = buttonTextures;
            buttonSave.Font = content.Load<SpriteFont>("font\\button_font");
            buttonSave.MouseClick += ButtonSave_OnClick;

            labelPoint.Font = content.Load<SpriteFont>("font\\button_font");
            labelPoint.Text = 0 + "," + 0;
            labelPoint.X = 0;
            labelPoint.Y = 0;
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            labelPoint.Text = (currentMouseState.X / 8) + "," + (currentMouseState.Y / 8);
            labelPoint.X = currentMouseState.X + 32;
            labelPoint.Y = currentMouseState.Y;

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (rectFloor.Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        currentType = 0x01;
                    }
                    else if (rectWall[0].Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        currentType = 0x11;
                    }
                    else if (rectWall[1].Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        currentType = 0x21;
                    }
                    else if (rectWall[2].Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        currentType = 0x41;
                    }
                    else if (rectWall[3].Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        currentType = 0x81;
                    }

                    else if(new Rectangle(0, 0, 480, 800).Contains(currentMouseState.X, currentMouseState.Y))
                    {
                        if (currentType == 0x01 && map[currentMouseState.Y / 8 * sizeX + currentMouseState.X / 8] > 0)
                        {
                            map[currentMouseState.Y / 8 * sizeX + currentMouseState.X / 8] = 0;
                        }
                        else
                        {
                            map[currentMouseState.Y / 8 * sizeX + currentMouseState.X / 8] |= currentType;
                        }
                    }
                }
            }


            buttonSave.Update(time);
            labelPoint.Update(time);
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rect;           
            rect = new Rectangle(0, 0, 8, 8);
            
            for(int i = 0; i < sizeY; i++)
            {
                for(int j = 0; j < sizeX; j++)
                {
                    if ((map[i * sizeX + j] & 0x0f) != 0)
                    {
                        rect.X = j * 8;
                        rect.Y = i * 8;
                        bath.Draw(floor, rect, new Rectangle(96, 0, 32, 32), Color.White);
                        if ((map[i * sizeX + j] & (1 << 4)) != 0)
                        {
                            bath.Draw(wall, rect, new Rectangle(32, 0, 32, 32), Color.White);
                        }
                        if ((map[i * sizeX + j] & (1 << 5)) != 0)
                        {
                            bath.Draw(wall, rect, new Rectangle(64, 0, 32, 32), Color.White);
                        }
                        if ((map[i * sizeX + j] & (1 << 6)) != 0)
                        {
                            bath.Draw(wall, rect, new Rectangle(96, 0, 32, 32), Color.White);
                        }
                        if ((map[i * sizeX + j] & (1 << 7)) != 0)
                        {
                            bath.Draw(wall, rect, new Rectangle(128, 0, 32, 32), Color.White);
                        }
                    }                    
                }
            }

            bath.Draw(floor, rectFloor, new Rectangle(96, 0, 32, 32), Color.White);

            bath.Draw(floor, rectWall[0], new Rectangle(96, 0, 32, 32), Color.White);
            bath.Draw(wall, rectWall[0], new Rectangle(32, 0, 32, 32), Color.White);
            bath.Draw(floor, rectWall[1], new Rectangle(96, 0, 32, 32), Color.White);
            bath.Draw(wall, rectWall[1], new Rectangle(64, 0, 32, 32), Color.White);
            bath.Draw(floor, rectWall[2], new Rectangle(96, 0, 32, 32), Color.White);
            bath.Draw(wall, rectWall[2], new Rectangle(96, 0, 32, 32), Color.White);
            bath.Draw(floor, rectWall[3], new Rectangle(96, 0, 32, 32), Color.White);
            bath.Draw(wall, rectWall[3], new Rectangle(128, 0, 32, 32), Color.White);

            buttonSave.Draw(bath);
            labelPoint.Draw(bath);
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        private void ButtonSave_OnClick(object sender, EventArgs e)
        {
            File.WriteAllBytes("map.bin", map);
        }

        #endregion
    }
}
