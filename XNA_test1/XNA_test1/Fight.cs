using Microsoft.Xna.Framework;
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
        Texture2D texturePlayerAttackHand;
        Texture2D texturePlayerAttackLeg;
        Texture2D texturePlayerAttackSpike;
        Texture2D texturePlayerDead;
        Texture2D textureZombie;
        Texture2D textureZombieAttack;
        Texture2D textureGameOver;
        Texture2D textureBattleLog;
        Rectangle rectPlayer;
        Rectangle rectPlayerAttack;
        Rectangle rectZombieAttack;
        Button buttonHandAttack;
        Button buttonLegAttack;
        Button buttonSpikeAttack;
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
        bool level2;
        int playerBleeding;
        int zombieShield;

        public event EventHandler onEnd;
        bool dead = false;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Fight()
        {
            rectPlayer = new Rectangle(0, 0, 80, 80);
            buttonHandAttack = new Button(new Vector2(0, 0), 64, 64, "");
            buttonHandAttack.MouseClick += ButtonHandAttack_OnClick;
            buttonLegAttack = new Button(new Vector2(0, 0), 64, 64, "");
            buttonLegAttack.MouseClick += ButtonLegAttack_OnClick;
            buttonSpikeAttack = new Button(new Vector2(0, 0), 64, 64, "");
            buttonSpikeAttack.MouseClick += ButtonSpikeAttack_OnClick;
            buttonHeal = new Button(new Vector2(0, 0), 64, 64, "");
            buttonHeal.MouseClick += ButtonHeal_OnClick;
            timeElapsed = 0;
            win = false;
            dead = false;
            playerMove = true;
            situation = 0;

            int x1, x2;
            int y, dy;

            labelBattleLog = new Label[10];
            for (int i = 0; i < 10; i++)
            {
                labelBattleLog[i] = new Label();
                labelBattleLog[i].Text = "" + i;
                labelBattleLog[i].X = 430;
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
            texturePlayerAttackHand = content.Load<Texture2D>("fight\\edward_elric_hand_atack");
            texturePlayerAttackLeg = content.Load<Texture2D>("fight\\edward_elric_leg_attack");
            texturePlayerAttackSpike = content.Load<Texture2D>("fight\\edward_elric_spike_atack1");
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
            buttonLegAttack.Hint = "Удар ногой. Шанс 30% оглушить\nврага на один ход.";
            buttonLegAttack.hintTexture = content.Load<Texture2D>("text_fon\\black_glass_rect");
            buttonLegAttack.textColor = Color.Aqua;
            buttonLegAttack.Enabled = false;

            buttonTextures = null;
            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button_hand"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button_hand"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button_hand"));
            buttonHandAttack.Textures = buttonTextures;
            buttonHandAttack.Font = content.Load<SpriteFont>("font\\character_info");
            buttonHandAttack.Hint = "Удар рукой.\nОткрывает удар ногой.";
            buttonHandAttack.hintTexture = content.Load<Texture2D>("text_fon\\black_glass_rect");
            buttonHandAttack.textColor = Color.Aqua;

            buttonTextures = null;
            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button_spike"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button_spike"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button_spike"));
            buttonSpikeAttack.Textures = buttonTextures;
            buttonSpikeAttack.Font = content.Load<SpriteFont>("font\\character_info");
            buttonSpikeAttack.Hint = "Удар шипом. Игнорирует броню соперника.\nСтоимость: 20 маны";
            buttonSpikeAttack.hintTexture = content.Load<Texture2D>("text_fon\\black_glass_rect");
            buttonSpikeAttack.textColor = Color.Aqua;

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
                buttonHandAttack.X = (int)(500 * ((float)value / 1920f) + 120);
                buttonHandAttack.Y = (int)(800 * ((float)value / 1920f) - 150);
                buttonLegAttack.X = (int)(500 * ((float)value / 1920f) + 120);
                buttonLegAttack.Y = (int)(800 * ((float)value / 1920f) - 150);
                buttonSpikeAttack.X = (int)(500 * ((float)value / 1920f) + 50);
                buttonSpikeAttack.Y = (int)(800 * ((float)value / 1920f) - 250);
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
            playerBleeding = 0;
            zombieShield = 0;
            situation = 0;

            buttonLegAttack.Enabled = false;
            buttonHandAttack.Enabled = true;
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

        public void Level2On()
        {
            level2 = true;
        }

        public void Level2Off()
        {
            level2 = false;
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
                if(dead == false)
                {
                    dead = true;
                    onEnd(this, null);
                }
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
                        buttonHandAttack.Update(time);
                        buttonLegAttack.Update(time);
                        buttonSpikeAttack.Update(time);
                        buttonHeal.Update(time);

                        if(situation != 0)
                        {
                            if (playerBleeding > 0)
                            {
                                playerBleeding--;
                                AddMessageToLog("Студент теряет 6 хп из-за кровотечения.");
                                currentCharacterIndex.hp -= 6;
                            }
                        }
                        break;

                    case 1: // Удар ногой
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 200)
                        {
                            timeElapsed = 0;
                            rectPlayerAttack.X += 80;
                            if (rectPlayerAttack.X == 240)
                            {
                                if (playerMove) situation = 0;
                                else situation = 2;
                                if (currentMobIndex.hp <= 0) 
                                    win = true;
                            }
                        }
                        break;

                    case 2: // Пауза, удар зомби
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 500) // Удар зомби
                        {
                            if(zombieShield > 0)
                            {
                                zombieShield -= 1;
                            }

                            timeElapsed = 0;
                            situation = 3;
                            rectZombieAttack = new Rectangle(0, 0, 400, 260);                         

                            if(currentMobIndex.hp <= maxMobIndex.hp / 2 && currentMobIndex.mana >= 50)
                            {
                                int damage = new Random().Next(currentMobIndex.atackMin, currentMobIndex.atackMax);
                                currentCharacterIndex.hp -= damage - currentCharacterIndex.defense;

                                currentMobIndex.mana -= 50;
                                currentMobIndex.hp += damage - currentCharacterIndex.defense;
                                AddMessageToLog("Зомби похитил у студента  " + (damage - currentCharacterIndex.defense) + " единиц здоровья");
                            }
                            else if (level2 
                                && playerBleeding == 0
                                && currentMobIndex.mana >= 20)
                            {
                                int damage = new Random().Next(currentMobIndex.atackMin, currentMobIndex.atackMax);
                                currentCharacterIndex.hp -= damage - currentCharacterIndex.defense;

                                playerBleeding = 3;
                                currentMobIndex.mana -= 20;
                                AddMessageToLog("Зомби нанес студенту кровоточащую рану.");
                                AddMessageToLog("Урон: " + damage + " - " + currentCharacterIndex.defense);
                            }
                            else if(level2
                                && zombieShield == 0
                                && currentMobIndex.mana >= 40)
                            {
                                zombieShield = 3;
                                currentMobIndex.mana -= 40;
                                AddMessageToLog("Зомби воспользовался заклинанием щит.");
                            }
                            else
                            {
                                int damage = new Random().Next(currentMobIndex.atackMin, currentMobIndex.atackMax);
                                currentCharacterIndex.hp -= damage - currentCharacterIndex.defense;
                                
                                AddMessageToLog("Зомби укусил студента. Урон: " + damage + " - " + currentCharacterIndex.defense);
                            }

                            UpdateLabel();
                            
                            MediaPlayer.Play(songBrains);
                        }
                        break;

                    case 3: // Удар зомби, анимация
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

                    case 4: // Удар шипом
                        timeElapsed += time.ElapsedGameTime.Milliseconds;
                        if (timeElapsed >= 200)
                        {
                            timeElapsed = 0;
                            rectPlayerAttack.X += 160;
                            if (rectPlayerAttack.X == 320)
                            {
                                situation = 2;
                                if (currentMobIndex.hp <= 0) win = true;
                            }
                        }
                        break;

                    case 5: // Удар рукой
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

            bath.Draw(textureBattleLog, new Rectangle(400, 0, 540, 240), Color.White);

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
                Color playerColor;
                if (playerBleeding == 0)
                    playerColor = Color.White;
                else
                    playerColor = Color.Red;

                Color zombieColor;
                if (zombieShield == 0)
                    zombieColor = Color.White;
                else
                    zombieColor = Color.Blue;

                switch (situation)
                {
                    case 0: // Ожидание выбора игрока
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, playerColor);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, zombieColor);

                        buttonSpikeAttack.Draw(bath);
                        buttonHeal.Draw(bath);
                        buttonHandAttack.Draw(bath);
                        buttonLegAttack.Draw(bath);
                        
                        break;

                    case 1:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayerAttackLeg, rectSprite, rectPlayerAttack, playerColor);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, zombieColor);
                        break;

                    case 2:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, playerColor);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, zombieColor);
                        break;

                    case 3:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayer, rectSprite, rectPlayer, playerColor);

                        rectSprite = new Rectangle(windowWidth / 2 - 200, windowHeigth / 2 - 130, 400, 260);
                        bath.Draw(textureZombieAttack, rectSprite, rectZombieAttack, Color.White);
                        break;

                    case 4:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 640, 320);
                        bath.Draw(texturePlayerAttackSpike, rectSprite, rectPlayerAttack, playerColor);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, zombieColor);
                        break;

                    case 5:
                        rectSprite = new Rectangle((int)(500 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(texturePlayerAttackHand, rectSprite, rectPlayerAttack, playerColor);

                        rectSprite = new Rectangle((int)(1300 * scale) - 160, (int)(800 * scale) - 160, 320, 320);
                        bath.Draw(textureZombie, rectSprite, zombieColor);
                        break;

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
                if (zombieShield > 0)
                {
                    damage /= 2;
                }
                AddMessageToLog("Студент нанес удар ногой. Урон: " + damage + " - " + currentMobIndex.defense);
                currentMobIndex.hp -= damage - currentMobIndex.defense;
                UpdateLabel();                    
                buttonLegAttack.Enabled = false;
                buttonHandAttack.Enabled = true;

                if(new Random().Next(0, 99) < 30)
                {
                    AddMessageToLog("Зомби пропускает ход");
                    playerMove = true;
                }
            }
        }

        private void ButtonHandAttack_OnClick(object sender, EventArgs e)
        {
            if (playerMove)
            {
                int damage;
                situation = 5;
                MediaPlayer.Play(songAttack);
                playerMove = false;
                timeElapsed = 0;
                rectPlayerAttack = new Rectangle(0, 0, 80, 80);
                damage = new Random().Next(currentCharacterIndex.atackMin, currentCharacterIndex.atackMax);
                

                if (zombieShield > 0)
                {
                    damage /= 2;
                }
                
                UpdateLabel();
                AddMessageToLog("Студент нанес удар рукой. Урон: " + damage + " - " + currentMobIndex.defense);
                currentMobIndex.hp -= damage - currentMobIndex.defense;

                buttonLegAttack.Enabled = true;
                buttonHandAttack.Enabled = false;
            }
        }

        private void ButtonSpikeAttack_OnClick(object sender, EventArgs e)
        {
            if (playerMove && currentCharacterIndex.mana >= 20)
            {
                int damage;
                situation = 4;
                MediaPlayer.Play(songAttack);
                playerMove = false;
                timeElapsed = 0;
                rectPlayerAttack = new Rectangle(0, 0, 160, 80);
                damage = new Random().Next(currentCharacterIndex.atackMin, currentCharacterIndex.atackMax);
                currentMobIndex.hp -= damage;
                currentCharacterIndex.mana -= 20;
                UpdateLabel();
                AddMessageToLog("Студент нанес удар шипом. Урон: " + damage);

                buttonLegAttack.Enabled = false;
                buttonHandAttack.Enabled = true;
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

                    buttonLegAttack.Enabled = false;
                    buttonHandAttack.Enabled = true;
                }
            }
        }

        #endregion

    }
}
