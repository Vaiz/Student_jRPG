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
    struct NPC
    {
        public Vector2 position;   // позиция моба на карте
        public int questNumber;    // номер квеста, когда NPC активен
        public bool questActive;
        public Texture2D texture;  // текстура моба

        public NPC(int x, int y, int questNumber, Texture2D texture)
        {
            position = new Vector2(x, y);
            questActive = false;
            this.questNumber = questNumber;
            this.texture = texture;
        }      
    };

    class Map
    {
        #region Переменные

        byte[] map;
        int sizeX, sizeY;
        Texture2D textureFloor;
        Texture2D textureWall;
        Texture2D textureQuest;
        Vector2 position;   // центр карты в координатах карты
        List<NPC> listQuestNPC;
        int windowWidth;
        int windowHeigth;
        int x0, y0;         // точка начала отрисовки центральной плитки карты в пикселях
        int x, y;           // количество отрисовываемых плиток
        int speed;          // количество миллисекунд для смены кадра на новый
        int timeFromLastFrame;    // время, которое прошло с момента смены последнего кадра в милиссекундах
        int questNumber;    // номер текущего квеста

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
            listQuestNPC = new List<NPC>();
            
            speed = 100;
        }

        public void LoadContent(ContentManager content)
        {
            textureFloor = content.Load<Texture2D>("floor\\floor1");
            textureWall = content.Load<Texture2D>("wall\\wall");
            textureQuest = content.Load<Texture2D>("quest\\quest_icon_blue");
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

        public void AddNPC(int x, int y, int questNumber, Texture2D texture)
        {
            listQuestNPC.Add(new NPC(x, y, questNumber, texture));
        }

        public int QuestNumber
        {
            set
            {
                questNumber = value;
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
                            bath.Draw(textureFloor, rect, new Rectangle(96, 0, 32, 32), Color.White);
                            if ((map[k] & (1 << 4)) != 0)
                            {
                                bath.Draw(textureWall, rect, new Rectangle(32, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 5)) != 0)
                            {
                                bath.Draw(textureWall, rect, new Rectangle(64, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 6)) != 0)
                            {
                                bath.Draw(textureWall, rect, new Rectangle(96, 0, 32, 32), Color.White);
                            }
                            if ((map[k] & (1 << 7)) != 0)
                            {
                                bath.Draw(textureWall, rect, new Rectangle(128, 0, 32, 32), Color.White);
                            }
                        }
                    }
                }
            }

            for(int i = 0; i < listQuestNPC.Count; i++)
            {
                if (listQuestNPC[i].position.X >= position.X - x && listQuestNPC[i].position.X <= position.X + x &&
                    listQuestNPC[i].position.Y >= position.Y - y && listQuestNPC[i].position.Y <= position.Y + y)
                {
                    rect.X = x0 + (int)(listQuestNPC[i].position.X - position.X) * 32;
                    rect.Y = y0 + (int)(listQuestNPC[i].position.Y - position.Y) * 32 - 24;
                    rect.Width = 32;
                    rect.Height = 48;
                    bath.Draw(listQuestNPC[i].texture, rect, new Rectangle(0, 0, 32, 48), Color.White);
                    if(listQuestNPC[i].questNumber == questNumber)
                    {
                        rect.Y -= 32;
                        rect.Height = 32;
                        bath.Draw(textureQuest, rect, Color.White);
                    }
                }
            }
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        public bool QuestNPC()
        {
            for(int i = 0; i < listQuestNPC.Count; i++)
            {
                if(listQuestNPC[i].questNumber == questNumber)
                {
                    if((listQuestNPC[i].position - position).LengthSquared() <= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
