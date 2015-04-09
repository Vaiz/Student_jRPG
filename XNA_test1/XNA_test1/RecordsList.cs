using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    struct Record
    {
        public string name;
        public int score;
    };
    class RecordsList
    {
        #region Переменные

        List<Label> records;

        #endregion
        //==============================================================================================
        #region Инициализация

        public RecordsList()
        {
            records = new List<Label>();
        }

        public void SetRecords(List<Record> in_Records, SpriteFont font)
        {
            records.Clear();

            int x1 = 150, y = 50;
            int x2 = 400;

            Label tmpLabel = new Label();
            tmpLabel.Font = font;
            tmpLabel.Text = "Имя";
            tmpLabel.X = x1;
            tmpLabel.Y = y;

            records.Add(tmpLabel);

            tmpLabel = new Label();
            tmpLabel.Font = font;
            tmpLabel.Text = "Очки";
            tmpLabel.X = x2;
            tmpLabel.Y = y;

            records.Add(tmpLabel);
            y += 50;

            for(int i = 0; i < in_Records.Count; i++)
            {
                tmpLabel = new Label();
                tmpLabel.Font = font;
                tmpLabel.Text = in_Records[i].name;
                tmpLabel.X = x1;
                tmpLabel.Y = y;

                records.Add(tmpLabel);

                tmpLabel = new Label();
                tmpLabel.Font = font;
                tmpLabel.Text = Convert.ToString(in_Records[i].score);
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
            for (int i = 0; i < records.Count; i++)
                records[i].Draw(bath);
        }

        #endregion

    }
}
