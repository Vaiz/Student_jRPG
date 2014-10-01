using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1.Character
{
    class CharacterInfo
    {
        #region Переменные

        Texture2D texture;

        int level;
        int experience;
        const int maxLevel = 9;
        int[] levelExperience = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};

        Label labelInfo;
        Label labelNickName1, labelNickName2;
        Label labelLevel1, labelLevel2;
        Label labelExperience1, labelExperience2;
        
        #endregion
        //==============================================================================================
        #region Инициализация

        public CharacterInfo()
        {
            level = 0;
            experience = 0;

            labelInfo = new Label();
            labelInfo.Text = "Информация о персонаже";
            labelInfo.X = 140;
            labelInfo.Y = 60;

            labelNickName1 = new Label();
            labelNickName1.Text = "Имя:";
            labelNickName1.X = 100;
            labelNickName1.Y = 115;

            labelNickName2 = new Label();
            labelNickName2.Text = "Студент";
            labelNickName2.X = 260;
            labelNickName2.Y = 115;

            labelLevel1 = new Label();
            labelLevel1.Text = "Уровень:";
            labelLevel1.X = 100;
            labelLevel1.Y = 170;

            labelLevel2 = new Label();
            labelLevel2.Text = "" + (level + 1);
            labelLevel2.X = 260;
            labelLevel2.Y = 170;

            labelExperience1 = new Label();
            labelExperience1.Text = "Опыт:";
            labelExperience1.X = 100;
            labelExperience1.Y = 225;

            labelExperience2 = new Label();
            labelExperience2.Text = experience + " / " + levelExperience[level];
            labelExperience2.X = 260;
            labelExperience2.Y = 225;
                
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
            bath.Draw(texture, new Vector2(0,0), Color.White);

            labelInfo.Draw(bath);
            labelNickName1.Draw(bath);
            labelNickName2.Draw(bath);
            labelLevel1.Draw(bath);
            labelLevel2.Draw(bath);
            labelExperience1.Draw(bath);
            labelExperience2.Draw(bath);
        }

        #endregion

        #region Другие функции

        public void AddExperience(int exp)
        {
            experience += exp;
            while (experience > levelExperience[level] && level < maxLevel)
            {
                experience %= levelExperience[level];
                level++;              
            }

            labelLevel2.Text = "" + (level + 1);
            labelExperience2.Text = experience + " / " + levelExperience[level];
        }

        #endregion
    }
}
