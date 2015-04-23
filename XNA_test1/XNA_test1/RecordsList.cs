using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    struct Record : IComparable<Record>
    {
        public string name;
        public int score;

        public int CompareTo(Record other)
        {
            // If other is not a valid object reference, this instance is greater.
            //if (other == null) return 1;

            // The temperature comparison depends on the comparison of 
            // the underlying Double values. 
            return other.score.CompareTo(score);
        }
    };
    class RecordsList
    {
        #region Переменные

        Texture2D textureBackGround;          //фоновое изображения для меню
        List<Label> records;
        Rectangle rectFon;  //область экрана для рисования спрайта
        int windowHeigth;

        #endregion
        //==============================================================================================
        #region Инициализация

        public RecordsList()
        {
            records = new List<Label>();
        }

        public void LoadContent(ContentManager content)
        {
            textureBackGround = content.Load<Texture2D>("background\\records"); //загрузка фонового изображения для меню   
        }

        public int WindowHeigth
        {
            get { return windowHeigth; }
            set 
            { 
                windowHeigth = value;

                float scaleFone;        // отношение высоты экрана к высоте рисунка.
                scaleFone = (float)windowHeigth / textureBackGround.Height;
                rectFon = new Rectangle(0, 0, (int)(textureBackGround.Width * scaleFone), windowHeigth);
            }
        }

        public void SetRecords(List<Record> in_Records, SpriteFont font)
        {
            records.Clear();

            int x0 = 400;
            int x1 = 450, y = 250;
            int x2 = 700;

            Label tmpLabel = new Label();
            tmpLabel.Font = font;
            tmpLabel.Text = "Имя";
            tmpLabel.Color = Color.Aqua;
            tmpLabel.X = x1;
            tmpLabel.Y = y;

            records.Add(tmpLabel);

            tmpLabel = new Label();
            tmpLabel.Font = font;
            tmpLabel.Text = "Очки";
            tmpLabel.Color = Color.Aqua;
            tmpLabel.X = x2;
            tmpLabel.Y = y;

            records.Add(tmpLabel);
            y += 50;

            for(int i = 0; i < in_Records.Count; i++)
            {
                tmpLabel = new Label();
                tmpLabel.Font = font;
                tmpLabel.Text = "" + (i + 1);
                tmpLabel.Color = Color.Aqua;
                tmpLabel.X = x0;
                tmpLabel.Y = y;

                records.Add(tmpLabel);

                tmpLabel = new Label();
                tmpLabel.Font = font;
                tmpLabel.Text = in_Records[i].name;
                tmpLabel.Color = Color.Aqua;
                tmpLabel.X = x1;
                tmpLabel.Y = y;

                records.Add(tmpLabel);

                tmpLabel = new Label();
                tmpLabel.Font = font;
                tmpLabel.Text = Convert.ToString(in_Records[i].score);
                tmpLabel.Color = Color.Aqua;
                tmpLabel.X = x2;
                tmpLabel.Y = y;

                records.Add(tmpLabel);
                y += 50;
            }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Draw(SpriteBatch bath)
        {
            bath.Draw(textureBackGround, rectFon, Color.White);

            for (int i = 0; i < records.Count; i++)
                records[i].Draw(bath);
        }

        #endregion

    }
}
