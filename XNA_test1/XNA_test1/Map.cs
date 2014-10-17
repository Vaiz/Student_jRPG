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
        public List<int> questList;    // номер квеста, когда NPC активен
        public bool questActive;
        public Texture2D texture;  // текстура моба

        public NPC(int x, int y, List<int> questList, Texture2D texture)
        {
            position = new Vector2(x, y);
            questActive = false;
            this.questList = questList;
            this.texture = texture;
        }              
    };

    struct Mob
    {
        public Vector2 position;    // позиция моба на карте
        public Texture2D texture;   // текстура моба
        public Rectangle rect;      // прямоугольник текстуры
        public bool active;         // активен или нет
        public Vector2 vectorMove;  // вектор направления движения
        public int exp;             // опыт за моба
        public Character.CharacterIndex mobIndex;

        public Mob(int x, int y, int exp, Texture2D texture, Character.CharacterIndex mobIndex)
        {
            position = new Vector2(x, y);
            this.exp = exp;
            this.texture = texture;
            this.mobIndex = mobIndex;
            rect = new Rectangle(0, 0, 32, 48);
            active = true;
            vectorMove = new Vector2(0, 1);
        }

        public Vector2 VectorMove
        {
            set { vectorMove = value; }
        }
    }

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
        List<NPC> listNPC;
        List<Mob> listMobs;
        int windowWidth;
        int windowHeigth;
        int x0, y0;         // точка начала отрисовки центральной плитки карты в пикселях
        int x, y;           // количество отрисовываемых плиток
        int speed;          // количество миллисекунд для смены кадра на новый
        int timeFromLastFrame;    // время, которое прошло с момента смены последнего кадра в милиссекундах
        int timeFromLastMobsMove;    // время, которое прошло с момента последнего передвижения ботов
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
            listNPC = new List<NPC>();
            listMobs = new List<Mob>();

            timeFromLastMobsMove = 0;

            speed = 200;
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

        public void AddNPC(int x, int y, List<int> questList, Texture2D texture)
        {
            if (questList.Count > 0)
            {
                listQuestNPC.Add(new NPC(x, y, questList, texture));
            }
            else
            {
                listNPC.Add(new NPC(x, y, questList, texture));
            }
        }

        public void AddMob(int x, int y, int exp, Texture2D texture, Character.CharacterIndex mobIndex)
        {
            listMobs.Add(new Mob(x, y, exp, texture, mobIndex));
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

            #region Движение карты

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
            #endregion

            #region Движение мобов

            Mob tmpMob;

            timeFromLastMobsMove += time.ElapsedGameTime.Milliseconds;
            if (timeFromLastMobsMove > speed)
            {
                timeFromLastMobsMove = 0;
                for (int i = 0; i < listMobs.Count; i++)
                {
                    if (listMobs[i].active)
                    {
                        k = (int)listMobs[i].position.Y * sizeX + (int)listMobs[i].position.X;

                        tmpMob = listMobs[i];

                        if ((int)(new Random().Next(4)) == 0 && listMobs[i].position.X == (int)listMobs[i].position.X && listMobs[i].position.Y == (int)listMobs[i].position.Y)
                        {
                            switch ((int)(new Random().Next(4)))
                            {
                                case 0:
                                    tmpMob.VectorMove = new Vector2(1, 0);
                                    tmpMob.rect.Y = 48 * 2;
                                    break;

                                case 1:
                                    tmpMob.VectorMove = new Vector2(-1, 0);
                                    tmpMob.rect.Y = 48 * 1;
                                    break;

                                case 2:
                                    tmpMob.VectorMove = new Vector2(0, 1);
                                    tmpMob.rect.Y = 48 * 0;
                                    break;

                                case 3:
                                    tmpMob.VectorMove = new Vector2(0, -1);
                                    tmpMob.rect.Y = 48 * 3;
                                    break;
                            }
                        }

                        while (true)
                        {
                            if (tmpMob.vectorMove.X == 1 && (map[k] & (1 << 4)) == 0)
                            {
                                break;
                            }
                            if (tmpMob.vectorMove.X == -1 && (map[k] & (1 << 5)) == 0)
                            {
                                break;
                            }
                            if (tmpMob.vectorMove.Y == 1 && (map[k] & (1 << 6)) == 0)
                            {
                                break;
                            }
                            if (tmpMob.vectorMove.Y == -1 && (map[k] & (1 << 7)) == 0)
                            {
                                break;
                            }

                            switch ((int)(new Random().Next(4)))
                            {
                                case 0:
                                    tmpMob.VectorMove = new Vector2(1, 0);
                                    tmpMob.rect.Y = 48 * 2;
                                    break;

                                case 1:
                                    tmpMob.VectorMove = new Vector2(-1, 0);
                                    tmpMob.rect.Y = 48 * 1;
                                    break;

                                case 2:
                                    tmpMob.VectorMove = new Vector2(0, 1);
                                    tmpMob.rect.Y = 48 * 0;
                                    break;

                                case 3:
                                    tmpMob.VectorMove = new Vector2(0, -1);
                                    tmpMob.rect.Y = 48 * 3;
                                    break;
                            }
                        }

                        tmpMob.position += tmpMob.vectorMove / 4;
                        tmpMob.rect.X += 32;
                        tmpMob.rect.X %= 128;
                        listMobs[i] = tmpMob;
                    }
                }
            }

            #endregion
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
                    if(listQuestNPC[i].questList.Contains(questNumber))
                    {
                        rect.Y -= 32;
                        rect.Height = 32;
                        bath.Draw(textureQuest, rect, Color.White);
                    }
                }
            }

            for (int i = 0; i < listNPC.Count; i++)
            {
                if (listNPC[i].position.X >= position.X - x && listNPC[i].position.X <= position.X + x &&
                    listNPC[i].position.Y >= position.Y - y && listNPC[i].position.Y <= position.Y + y)
                {
                    rect.X = x0 + (int)(listNPC[i].position.X - position.X) * 32;
                    rect.Y = y0 + (int)(listNPC[i].position.Y - position.Y) * 32 - 24;
                    rect.Width = 32;
                    rect.Height = 48;
                    bath.Draw(listNPC[i].texture, rect, new Rectangle(0, 0, 32, 48), Color.White);
                }
            }

            for (int i = 0; i < listMobs.Count; i++)
            {
                if (listMobs[i].position.X >= position.X - x && listMobs[i].position.X <= position.X + x &&
                    listMobs[i].position.Y >= position.Y - y && listMobs[i].position.Y <= position.Y + y)
                {
                    rect.X = x0 + (int)((listMobs[i].position.X - position.X) * 32);
                    rect.Y = y0 + (int)((listMobs[i].position.Y - position.Y) * 32) - 24;
                    rect.Width = 32;
                    rect.Height = 48;
                    bath.Draw(listMobs[i].texture, rect, listMobs[i].rect, Color.White);
                }
            }
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        public bool QuestNPC()      // столкнулся с квестовым неписем
        {
            for(int i = 0; i < listQuestNPC.Count; i++)
            {
                if(listQuestNPC[i].questList.Contains(questNumber))
                {
                    if((listQuestNPC[i].position - position).LengthSquared() <= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void OpenMap()   // Открыть выход с кафедры
        {
            map[72 * sizeX + 48] = 1;
            map[71 * sizeX + 48] = 1;
        }

        public int MobConnected()   // столкнулся с мобом
        {
            for (int i = 0; i < listMobs.Count; i++)
            {
                if (listMobs[i].active)
                {
                    if ((listMobs[i].position - position).LengthSquared() <= 0.5)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        
        public int MobCnt
        {
            get { return listMobs.Count; }
        }

        public Character.CharacterIndex GetMobIndex(int i)
        {
            return listMobs[i].mobIndex;
        }

        public int KickMob(int i)  // убить моба
        {
            int exp;
            exp = listMobs[i].exp;
            listMobs.RemoveAt(i);
            return exp;
        }

        #endregion
    }
}
