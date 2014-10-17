using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_test1.Character;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_test1
{
    class Game
    {
        #region Переменные

        EventMessage eventMessage;
        CharacterMove player;
        CharacterInfo characterInfo;
        Map map;
        Fight fight;
        Song song;
        int stage;  // прогресс игры
        int situation;  // текущая ситуация в игре
        int mobNumber;
        bool showCharacterInfo;
        bool keyCDown1, keyCDown2;
        bool keyQDown1, keyQDown2;

        string[] quest;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Game()
        {
            eventMessage = new EventMessage();
            eventMessage.ButtonOk_OnClick = ButtonEventMessageOk_OnClick;
            //eventMessage.Text = "Игра началась!\nПервый квест: сдать все лабы В.В.";
            quest = new string[10];

            player = new CharacterMove();
            characterInfo = new CharacterInfo();
            map = new Map();
            fight = new Fight();
            
            stage = 1;
            situation = 0;
            map.QuestNumber = stage;
            showCharacterInfo = false;
            keyCDown1 = false;
            keyCDown2 = false;
            keyQDown1 = false;
            keyQDown2 = false;

            
        }

        public void LoadContent(ContentManager content)
        {
            Dictionary<VisibleState, Texture2D> buttonTextures;

            quest[0] = content.Load<string>("quest\\quest1");
            quest[1] = content.Load<string>("quest\\quest2");
            quest[2] = content.Load<string>("quest\\quest3");
            quest[3] = content.Load<string>("quest\\quest4");
            
            eventMessage.Fon = content.Load<Texture2D>("text_fon\\fallout_1920x1080");
            eventMessage.Font = content.Load<SpriteFont>("font\\fallout_font");
            eventMessage.Text = quest[0];

            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button3_norm"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button3_hover"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button3_pressed"));
            eventMessage.ButtonTexture = buttonTextures;

            player.Texture = content.Load<Texture2D>("character\\edward_elric_1");
            characterInfo.Texture = content.Load<Texture2D>("text_fon\\paper1_cr");
            characterInfo.Font = content.Load<SpriteFont>("font\\character_info");

            map.LoadContent(content);

            List<int> questList;
            questList = new List<int>(); 
            map.AddNPC(42, 85, questList, content.Load<Texture2D>("character\\Gud1"));
            map.AddNPC(45, 85, questList, content.Load<Texture2D>("character\\Gud2"));
            map.AddNPC(42, 87, questList, content.Load<Texture2D>("character\\Bojd"));
            map.AddNPC(54, 73, questList, content.Load<Texture2D>("character\\Bur"));
            map.AddNPC(54, 76, questList, content.Load<Texture2D>("character\\Bersh"));
            questList.Add(1);
            questList.Add(3);
            map.AddNPC(52, 88, questList, content.Load<Texture2D>("character\\VV"));

            Character.CharacterIndex mobIndex;
            mobIndex.hp = 100;
            mobIndex.mana = 0;
            mobIndex.atackMin = 20;
            mobIndex.atackMax = 25;
            mobIndex.defense = 5;

            map.AddMob(44, 64, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(13, 81, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(48, 60, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(48, 42, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(48, 14, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(11, 53, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(10, 69, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(25, 69, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);
            map.AddMob(43, 70, 10, content.Load<Texture2D>("character\\zombie2"), mobIndex);

            fight.LoadContent(content);

            song = content.Load<Song>("music\\Tetris");
        }

        public int WindowHeigth
        {
            set 
            {
                eventMessage.WindowHeigth = value;
                player.WindowHeigth = value;
                map.WindowHeigth = value;
                fight.WindowHeigth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        public int WindowWidth
        {
            set 
            { 
                eventMessage.WindowWidth = value;
                player.WindowWidth = value;
                map.WindowWidth = value;
                fight.WindowWidth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
            #region Нажатие C

            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                keyCDown1 = true;
            }
            else
            {
                keyCDown1 = false;
            }
           
            if (!keyCDown1 && keyCDown2)
            {
                showCharacterInfo = !showCharacterInfo;
            }

            keyCDown2 = keyCDown1;

            #endregion

            #region Нажатие Q

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                keyQDown1 = true;
            }
            else
            {
                keyQDown1 = false;
            }

            if (!keyQDown1 && keyQDown2)
            {
                if (situation == 0) situation = 1;
                else situation = 0;
            }
            else if (situation == 0)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    situation = 1;
                }
            }

            keyQDown2 = keyQDown1;

            #endregion

            switch (situation)
            {
                case 0:
                    eventMessage.Update(time);
                    MediaPlayer.Stop();
                    break;

                case 1:
                    {
                        if (MediaPlayer.Queue.ActiveSong == null || MediaPlayer.State == MediaState.Stopped)
                        {
                            MediaPlayer.Play(song);
                        }
                        
                        map.Update(time);
                        player.Update(time);

                        mobNumber = map.MobConnected();

                        if (mobNumber != -1)
                        {
                            situation = 2;
                            MediaPlayer.Stop();
                            fight.Init(characterInfo.characterIndex, map.GetMobIndex(mobNumber));                           
                        }                      
                    }

                    if (map.QuestNPC())
                    {
                        if(stage == 1)
                        {
                            map.OpenMap();
                        }
                        StageInc();
                    }

                    break;

                case 2:
                    fight.Update(time);
                    if(fight.win)
                    {
                        characterInfo.AddExperience(map.KickMob(mobNumber));
                        situation = 1;

                        if (map.MobCnt == 0)
                        {
                            StageInc();
                        }
                    }
                    
                    break;

            }                  
        }

        public void Draw(SpriteBatch bath)
        {
            switch(situation)
            {
                case 0:
                    eventMessage.Draw(bath);
                    break;

                case 1:
                    map.Draw(bath);
                    player.Draw(bath);
                    if (showCharacterInfo)
                    {
                        characterInfo.Draw(bath);
                    }
                    break;

                case 2:
                    fight.Draw(bath);
                    break;
            }           
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonEventMessageOk_OnClick(object sender, EventArgs e)
        {
            situation = 1;
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        private void StageInc()
        {
            stage++;
            map.QuestNumber = stage;

            eventMessage.Text = quest[stage - 1];
            eventMessage.UpdateButtonPosition();

            situation = 0;
        }

        #endregion
    }
}
