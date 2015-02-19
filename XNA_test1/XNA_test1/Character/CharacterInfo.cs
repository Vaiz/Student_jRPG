using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1.Character
{

    struct CharacterIndex
    {
        public int mana;
        public int hp;
        public int atackMin, atackMax;
        public int defense;
    };

    class CharacterInfo
    {
        #region Переменные

        Texture2D texture;

        int level;
        int experience;
        const int maxLevel = 9;
        int[] levelExperience = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 0};
        public CharacterIndex characterIndexFull;
        public CharacterIndex characterIndexCurrent;

        Label labelInfo;
        Label labelNickName1, labelNickName2;
        Label labelLevel1, labelLevel2;
        Label labelExperience1, labelExperience2;
        Label labelHP1, labelHP2;
        Label labelMana1, labelMana2;
        Label labelAtack1, labelAtack2;
        Label labelDefense1, labelDefense2;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public CharacterInfo()
        {
            int x1, x2;
            int y, dy;

            x1 = 100; x2 = 260;
            y = 115; dy = 55;

            labelInfo = new Label();
            labelInfo.Text = "Информация о персонаже";
            labelInfo.X = 140;
            labelInfo.Y = 60;

            labelNickName1 = new Label();
            labelNickName1.Text = "Имя:";
            labelNickName1.X = x1;
            labelNickName1.Y = y;

            labelNickName2 = new Label();
            labelNickName2.Text = "Студент";
            labelNickName2.X = x2;
            labelNickName2.Y = y;

            y += dy;

            labelLevel1 = new Label();
            labelLevel1.Text = "Уровень:";
            labelLevel1.X = x1;
            labelLevel1.Y = y;

            labelLevel2 = new Label();
            labelLevel2.X = x2;
            labelLevel2.Y = y;

            y += dy;

            labelExperience1 = new Label();
            labelExperience1.Text = "Опыт:";
            labelExperience1.X = x1;
            labelExperience1.Y = y;

            labelExperience2 = new Label();
            labelExperience2.X = x2;
            labelExperience2.Y = y;

            y += dy;

            labelHP1 = new Label();
            labelHP1.Text = "Здоровье:";
            labelHP1.X = x1;
            labelHP1.Y = y;

            labelHP2 = new Label();
            labelHP2.X = x2;
            labelHP2.Y = y;

            y += dy;

            labelMana1 = new Label();
            labelMana1.Text = "Мана:";
            labelMana1.X = x1;
            labelMana1.Y = y;

            labelMana2 = new Label();
            labelMana2.X = x2;
            labelMana2.Y = y;

            y += dy;

            labelAtack1 = new Label();
            labelAtack1.Text = "Урон:";
            labelAtack1.X = x1;
            labelAtack1.Y = y;

            labelAtack2 = new Label();
            labelAtack2.X = x2;
            labelAtack2.Y = y;

            y += dy;

            labelDefense1 = new Label();
            labelDefense1.Text = "Защита:";
            labelDefense1.X = x1;
            labelDefense1.Y = y;

            labelDefense2 = new Label();
            labelDefense2.X = x2;
            labelDefense2.Y = y;

            Init();    
        }        

        public Texture2D Texture
        {
            get { return texture; }
            set 
            {
                texture = value;           
            }
        }

        public SpriteFont Font
        {
            set
            {
                labelInfo.Font = value;
                labelNickName1.Font = value;
                labelNickName2.Font = value;
                labelLevel1.Font = value;
                labelLevel2.Font = value;
                labelExperience1.Font = value;
                labelExperience2.Font = value;
                labelHP1.Font = value;
                labelHP2.Font = value;
                labelMana1.Font = value;
                labelMana2.Font = value;
                labelAtack1.Font = value; 
                labelAtack2.Font = value;
                labelDefense1.Font = value; 
                labelDefense2.Font = value;
            }
        }

        public void Init()
        {
            level = 0;
            experience = 0;
            characterIndexFull.mana = 200;
            characterIndexFull.hp = 100;
            characterIndexFull.atackMin = 17;
            characterIndexFull.atackMax = 20;
            characterIndexFull.defense = 6;

            characterIndexCurrent = characterIndexFull;

            UpdateLabel();
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {

        }

        public void Draw(SpriteBatch bath)
        {
            bath.Draw(texture, new Vector2(0,0), Color.White);

            labelInfo.Draw(bath);
            labelNickName1.Draw(bath);
            labelNickName2.Draw(bath);
            labelLevel1.Draw(bath);
            labelLevel2.Draw(bath);
            labelExperience1.Draw(bath);
            labelExperience2.Draw(bath);
            labelHP1.Draw(bath);
            labelHP2.Draw(bath);
            labelMana1.Draw(bath);
            labelMana2.Draw(bath);
            labelAtack1.Draw(bath);
            labelAtack2.Draw(bath);
            labelDefense1.Draw(bath);
            labelDefense2.Draw(bath);
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        public void AddExperience(int exp)
        {
            experience += exp;
            while (experience >= levelExperience[level] && level < maxLevel)
            {
                experience -= levelExperience[level];
                level++;
                characterIndexFull.hp += 10;
                characterIndexFull.mana += 20;
                characterIndexFull.atackMin += 3;
                characterIndexFull.atackMax += 4;
                characterIndexFull.defense += 2;

                characterIndexCurrent = characterIndexFull;
            }

            UpdateLabel();
        }

        void UpdateLabel()
        {
            labelLevel2.Text = "" + (level + 1);
            labelExperience2.Text = experience + " / " + levelExperience[level];

            labelHP2.Text = "" + characterIndexCurrent.hp + " / " + characterIndexFull.hp;
            labelMana2.Text = "" + characterIndexCurrent.mana + " / " + characterIndexFull.mana;
            labelAtack2.Text = "" + characterIndexFull.atackMin + "-" + characterIndexFull.atackMax;
            labelDefense2.Text = " " + characterIndexFull.defense;
        }

        public void RestoreHP()
        {
            characterIndexCurrent.hp = characterIndexFull.hp;
            UpdateLabel();
        }

        public void RestoreMana()
        {
            characterIndexCurrent.mana = characterIndexFull.mana;
            UpdateLabel();
        }


        #endregion
    }
}
