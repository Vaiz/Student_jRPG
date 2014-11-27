﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_test1
{
    class Fight
    {
        #region Переменные

        Texture2D textureBackGround;
        Texture2D texturePlayer;
        Texture2D texturePlayerAttack;
        Texture2D texturePlayerDead;
        Texture2D textureZombie;
        Texture2D textureZombieAttack;
        Texture2D textureGameOver;
        Texture2D textureBattleLog;
        Rectangle rectPlayer;
        Rectangle rectPlayerAttack;
        Rectangle rectZombieAttack;
        Button buttonLegAttack;
        Button buttonHeal;
        //Button buttonShield;
        Song songBrains;
        Song songAttack;
        Song songDead;

        public Character.CharacterIndex maxCharacterIndex;
        public Character.CharacterIndex currentCharacterIndex;
        public Character.CharacterIndex maxMobIndex;
        Character.CharacterIndex currentMobIndex;

        Label labelCharHP1, labelCharHP2;
        Label labelCharMana1, labelCharMana2;
        Label labelCharAtack1, labelCharAtack2;
        Label labelCharDefense1, labelCharDefense2;

        Label labelMobHP1, labelMobHP2;
        Label labelMobMana1, labelMobMana2;
        Label labelMobAtack1, labelMobAtack2;
        Label labelMobDefense1, labelMobDefense2;

        Label[] labelBattleLog;

        int windowHeigth, windowWidth;
        int timeElapsed;
        int situation;

        public bool win;
        bool playerMove;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Fight()
        {
            rectPlayer = new Rectangle(0, 0, 80, 80);
            buttonLegAttack = new Button(new Vector2(0,0), 64, 64, "");
            buttonLegAttack.MouseClick += ButtonLegAttack_OnClick;
            buttonHeal = new Button(new Vector2(0, 0), 64, 64, "");
            buttonHeal.MouseClick += ButtonHeal_OnClick;
            timeElapsed = 0;
            win = false;
            playerMove = true;
            situation = 0;

            int x1, x2;
            int y, dy;

            labelBattleLog = new Label[10];
            for (int i = 0; i < 10; i++)
            {
                labelBattleLog[i] = new Label();
                labelBattleLog[i].Text = "" + i;
                labelBattleLog[i].X = 450;
                labelBattleLog[i].Y = 10 + i * 20;
            }


            #region CharLabel

            x1 = 10; x2 = 120;
            y = 10; dy = 20;

            labelCharHP1 = new Label();
            labelCharHP1.Text = "Здоровье:";
            labelCharHP1.X = x1;
            labelCharHP1.Y = y;

            labelCharHP2 = new Label();
            labelCharHP2.X = x2;
            labelCharHP2.Y = y;

            y += dy;

            labelCharMana1 = new Label();
            labelCharMana1.Text = "Мана:";
            labelCharMana1.X = x1;
            labelCharMana1.Y = y;

            labelCharMana2 = new Label();
            labelCharMana2.X = x2;
            labelCharMana2.Y = y;

            y += dy;

            labelCharAtack1 = new Label();
            labelCharAtack1.Text = "Урон:";
            labelCharAtack1.X = x1;
            labelCharAtack1.Y = y;

            labelCharAtack2 = new Label();
            labelCharAtack2.X = x2;
            labelCharAtack2.Y = y;

            y += dy;

            labelCharDefense1 = new Label();
            labelCharDefense1.Text = "Защита:";
            labelCharDefense1.X = x1;
            labelCharDefense1.Y = y;

            labelCharDefense2 = new Label();
            labelCharDefense2.X = x2;
            labelCharDefense2.Y = y;

            #endregion

            #region MobLabel

            x1 = 1000; x2 = 1120;
            y = 10; dy = 20;

            labelMobHP1 = new Label();
            labelMobHP1.Text = "Здоровье:";
            labelMobHP1.X = x1;
            labelMobHP1.Y = y;

            labelMobHP2 = new Label();
            labelMobHP2.X = x2;
            labelMobHP2.Y = y;

            y += dy;

            labelMobMana1 = new Label();
            labelMobMana1.Text = "Мана:";
            labelMobMana1.X = x1;
            labelMobMana1.Y = y;

            labelMobMana2 = new Label();
            labelMobMana2.X = x2;
            labelMobMana2.Y = y;

            y += dy;

            labelMobAtack1 = new Label();
            labelMobAtack1.Text = "Урон:";
            labelMobAtack1.X = x1;
            labelMobAtack1.Y = y;

            labelMobAtack2 = new Label();
            labelMobAtack2.X = x2;
            labelMobAtack2.Y = y;

            y += dy;

            labelMobDefense1 = new Label();
            labelMobDefense1.Text = "Защита:";
            labelMobDefense1.X = x1;
            labelMobDefense1.Y = y;

            labelMobDefense2 = new Label();
            labelMobDefense2.X = x2;
            labelMobDefense2.Y = y;

            #endregion
        }

        public void LoadContent(ContentManager content)
        {
            textureBackGround = content.Load<Texture2D>("background\\Thenewkid");
            texturePlayer = content.Load<Texture2D>("fight\\edward_elric_figth1");
            texturePlayerAttack = content.Load<Texture2D>("fight\\edward_elric_leg_attack");
            texturePlayerDead = content.Load<Texture2D>("fight\\edward_dead");
            textureZombie = content.Load<Texture2D>("fight\\zomb_1_cr");
            textureZombieAttack = content.Load<Texture2D>("fight\\zombie_attack");
            textureGameOver = content.Load<Texture2D>("fight\\game_over");
            textureBattleLog = content.Load<Texture2D>("text_fon\\black_glass_rect");
            songBrains = content.Load<Song>("music\\pvz_brainzzz");
            songAttack = content.Load<Song>("music\\attack2");
            songDead = content.Load<Song>("music\\marsh");

            Dictionary<VisibleState, Texture2D> buttonTextures;
            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button_chiken_leg"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button_chiken_leg"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button_chiken_leg"));
            buttonLegAttack.Textures = buttonTextures;
            buttonLegAttack.Font = content.Load<SpriteFont>("font\\character_info");
            buttonLegAttack.Hint = "Удар ногой";
            buttonLegAttack.hintTexture = content.Load<Texture2D>("text_fon\\black_glass_rect");
            buttonLegAttack.textColor = Color.Aqua;

            buttonTextures = null;
            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button_heal"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button_heal"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button_heal"));
            buttonHeal.Textures = buttonTextures;
            buttonHeal.Font = content.Load<SpriteFont>("font\\character_info");
            buttonHeal.Hint = "Восстанавливает 30хп\nСтоимость: 50 маны";
            buttonHeal.hintTexture = content.Load<Texture2D>("text_fon\\black_glass_rect");
            buttonHeal.textColor = Color.Aqua;

            labelCharHP1.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharHP2.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharMana1.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharMana2.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharAtack1.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharAtack2.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharDefense1.Font = content.Load<SpriteFont>("font\\character_info");
            labelCharDefense2.Font = content.Load<SpriteFont>("font\\character_info");

            labelCharHP1.Color = Color.Red;
            labelCharHP2.Color = Color.Red;
            labelCharMana1.Color = Color.Blue;
            labelCharMana2.Color = Color.Blue;
            labelCharAtack1.Color = Color.Red;
            labelCharAtack2.Color = Color.Red;
            labelCharDefense1.Color = Color.Blue;
            labelCharDefense2.Color = Color.Blue;

            labelMobHP1.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobHP2.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobMana1.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobMana2.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobAtack1.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobAtack2.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobDefense1.Font = content.Load<SpriteFont>("font\\character_info");
            labelMobDefense2.Font = content.Load<SpriteFont>("font\\character_info");

            labelMobHP1.Color = Color.Red;
            labelMobHP2.Color = Color.Red;
            labelMobMana1.Color = Color.Blue;
            labelMobMana2.Color = Color.Blue;
            labelMobAtack1.Color = Color.Red;
            labelMobAtack2.Color = Color.Red;
            labelMobDefense1.Color = Color.Blue;
            labelMobDefense2.Color = Color.Blue;

            for(int i = 0; i < 10; i++)
            {
                labelBattleLog[i].Font = content.Load<SpriteFont>("font\\character_info");
                labelBattleLog[i].Color = Color.Red;
            }
        }

        public int WindowHeigth
        {
            set 
            {
                windowHeigth = value;                
            }
        }

        public int WindowWidth
        {
            set 
            {
                windowWidth = value;
                buttonLegAttack.X = (int)(500 * ((float)value / 1920f) + 100);
                buttonLegAttack.Y = (int)(800 * ((float)value / 1920f) - 200);
                buttonHeal.X = (int)(500 * ((float)value / 1920f) - 180);
                buttonHeal.Y = (int)(800 * ((float)value / 1920f) - 200);
            }
        }

        public void Init(Character.CharacterIndex characterIndexFull, Character.CharacterIndex characterIndexCurrent, Character.CharacterIndex mobIndex)
        {
            maxCharacterIndex = characterIndexFull;
            currentCharacterIndex = characterIndexCurrent;
            maxMobIndex = mobIndex;
            currentMobIndex = mobIndex;

            UpdateLabel();

            for (int i = 0; i < 10; i++)
            {
                labelBattleLog[i].Text = "";
            }

            rectPlayer = new Rectangle(0, 0, 80, 80);
            timeElapsed = 0;
            win = false;
            playerMove = true;
            situation = 0;
        }

        void UpdateLabel()
        {
            labelCharHP2.Text = "" + currentCharacterIndex.hp + " / " + maxCharacterIndex.hp;
            labelCharMana2.Text = "" + currentCharacterIndex.mana + " / " + maxCharacterIndex.mana;            
            labelCharAtack2.Text = "" + currentCharacterIndex.atackMin + "-" + currentCharacterIndex.atackMax;
            labelCharDefense2.Text = "" + currentCharacterIndex.defense;

            labelMobHP2.Text = "" + currentMobIndex.hp + " / " + maxMobIndex.hp;
            labelMobMana2.Text = "" + currentMobIndex.mana + " / " + maxMobIndex.mana;
            labelMobAtack2.Text = "" + currentMobIndex.atackMin + "-" + currentMobIndex.atackMax;
            labelMobDefense2.Text = "" + currentMobIndex.defense;
        }

        void AddMessageToLog(string text)
        {
            for(int i = 0; i < 9; i++)
            {
                labelBattleLog[i].Text = labelBattleLog[i + 1].Text;
            }
            labelBattleLog[9].Text = text;
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            if (rectPlayer.X != 560)
            {
                timeElapsed += time.ElapsedGameTime.Milliseconds;
                if (timeElapsed >= 200)
                {
                    timeElapsed = 0;
                    rectPlayer.X += 80;
                }
            }
            else if (currentCharacterIndex.hp <= 0 && situation != 3)
            {
                if (MediaPlayer.Queue.ActiveSong == null || MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(songDead);
                }
            }
            else
            {
                switch (situation)
                {
                    case 0: // Ожидание выбора игрока
                        buttonLegAttack.Update(time);
                        buttonHeal.Update(time);
                        break;

                    case 1: // Удар ногой
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 200)
                        {
                            timeElapsed = 0;
                            rectPlayerAttack.X += 80;
                            if (rectPlayerAttack.X == 240)
                            {
                                situation = 2;
                                if (currentMobIndex.hp <= 0) win = true;
                            }
                        }
                        break;

                    case 2: // Пауза
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 500)
                        {
                            int damage;
                            timeElapsed = 0;
                            situation = 3;
                            rectZombieAttack = new Rectangle(0, 0, 400, 260);
                            damage = new Random().Next(currentMobIndex.atackMin, currentMobIndex.atackMax);
                            currentCharacterIndex.hp -= damage - currentCharacterIndex.defense;
                            UpdateLabel();
                            AddMessageToLog("Зомби укусил студента. Урон: " + damage + " - " + currentCharacterIndex.defense);
                            MediaPlayer.Play(songBrains);
                        }
                        break;

                    case 3: // Удар зомби
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 100)
                        {
                            timeElapsed = 0;
                            rectZombieAttack.X += 400;
                            if (rectZombieAttack.X == 2000)
                            {
                                if (rectZombieAttack.Y == 0)
                                {
                                    rectZombieAttack.Y = 260;
                                    rectZombieAttack.X = 0;
                                }
                                else
                                {
                                    situation = 0;
                                    playerMove = true;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch bath)
        {
            Rectangle rectSprite;  // область экрана для рисования спрайта
            float scale;        // отношение высоты экрана к высоте рисунка.

            scale = (float)windowHeigth / textureBackGround.Height;
            rectSprite = new Rectangle(0, 0, (int)(textureBackGround.Width * scale), windowHeigth);

            bath.Draw(textureBackGround, rectSprite, Color.White);

            bath.Draw(textureBattleLog, new Rectangle(420, 0, 500, 240), Color.White);

            for (int i = 0; i < 10; i++)
            {
                labelBattleLog[i].Draw(bath);
            }

            scale = (float)windowWidth / 1920f;
            if (currentCharacterIndex.hp <= 0 && situation != 3)
            {
                rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                bath.Draw(texturePlayerDead, rectSprite, Color.White);

                rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                bath.Draw(textureZombie, rectSprite, Color.White);

                rectSprite = new Rectangle(windowWidth / 2 - textureGameOver.Width / 2, windowHeigth / 2 - textureGameOver.Height / 2, textureGameOver.Width, textureGameOver.Height);
                bath.Draw(textureGameOver, rectSprite, Color.White);
            }
            else
            {
                switch (situation)
                {
                    case 0:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, Color.White);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, Color.White);
                        break;

                    case 1:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayerAttack, rectSprite, rectPlayerAttack, Color.White);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, Color.White);
                        break;

                    case 2:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, Color.White);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, Color.White);
                        break;

                    case 3:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, Color.White);

                        rectSprite = new Rectangle(windowWidth / 2 - 200, windowHeigth / 2 - 130, 400, 260);
                        bath.Draw(textureZombieAttack, rectSprite, rectZombieAttack, Color.White);

                        break;
                }

                if (playerMove)
                {
                    buttonLegAttack.Draw(bath);
                    buttonHeal.Draw(bath);
                }
            }
            labelCharHP1.Draw(bath);
            labelCharHP2.Draw(bath);
            labelCharMana1.Draw(bath);
            labelCharMana2.Draw(bath);
            labelCharAtack1.Draw(bath);
            labelCharAtack2.Draw(bath);
            labelCharDefense1.Draw(bath);
            labelCharDefense2.Draw(bath);

            labelMobHP1.Draw(bath);
            labelMobHP2.Draw(bath);
            labelMobMana1.Draw(bath);
            labelMobMana2.Draw(bath);
            labelMobAtack1.Draw(bath);
            labelMobAtack2.Draw(bath);
            labelMobDefense1.Draw(bath);
            labelMobDefense2.Draw(bath);
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonLegAttack_OnClick(object sender, EventArgs e)
        {
            if(playerMove)
            {
                int damage;
                situation = 1;
                MediaPlayer.Play(songAttack);
                playerMove = false;
                timeElapsed = 0;
                rectPlayerAttack = new Rectangle(0, 0, 80, 80);
                damage = new Random().Next(currentCharacterIndex.atackMin, currentCharacterIndex.atackMax);
                currentMobIndex.hp -= damage - currentMobIndex.defense;
                UpdateLabel();
                AddMessageToLog("Студент нанес удар ногой. Урон: " + damage + " - " + currentMobIndex.defense);
            }
        }

        private void ButtonHeal_OnClick(object sender, EventArgs e)
        {
            if (playerMove)
            {
                if (currentCharacterIndex.mana >= 50)
                {
                    playerMove = false;
                    currentCharacterIndex.mana -= 50;
                    currentCharacterIndex.hp += 30;
                    if (currentCharacterIndex.hp > maxCharacterIndex.hp) currentCharacterIndex.hp = maxCharacterIndex.hp;
                    UpdateLabel();
                    AddMessageToLog("Студент восстановил 30 хп.");
                    situation = 2;
                }

            }
        }

        #endregion

    }
}
