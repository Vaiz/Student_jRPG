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
        enum Situation
        {
            EVENT_MESSAGE,
            WALK_ON_MAP,
            FIGTH
        };
        #region Переменные

        EventMessage eventMessage;
        CharacterMove player;
        CharacterInfo characterInfo;
        List<Map> listMaps;
        int currentMap;
        Fight fight;
        Song song;
        int stage;  // прогресс игры
        Situation situation;  // текущая ситуация в игре
        int mobNumber;  // id бота, с которым происходит бой
        bool showCharacterInfo; 
        bool keyCDown1, keyCDown2;
        bool keyQDown1, keyQDown2;
        Texture2D textureZombie1;

        string[] quest;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Game()
        {
            eventMessage = new EventMessage();
            eventMessage.ButtonOk_OnClick = ButtonEventMessageOk_OnClick;
            
            quest = new string[10];

            player = new CharacterMove();
            characterInfo = new CharacterInfo();
            listMaps = new List<Map>();
            listMaps.Add(new Map("level1.bin", new Vector2(48, 94)));
            currentMap = 0;
            fight = new Fight();
            
            stage = 1;
            situation = Situation.EVENT_MESSAGE;
            listMaps[0].QuestNumber = stage;
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

            for(int i = 0; i < listMaps.Count; i++)
                listMaps[i].LoadContent(content);
            
            listMaps[0].AddNPC(42, 85, new List<int>(), content.Load<Texture2D>("character\\Gud1"));
            listMaps[0].AddNPC(45, 85, new List<int>(), content.Load<Texture2D>("character\\Gud2"));
            listMaps[0].AddNPC(42, 87, new List<int>(), content.Load<Texture2D>("character\\Bojd"));
            listMaps[0].AddNPC(54, 73, new List<int>(), content.Load<Texture2D>("character\\Bur"));
            listMaps[0].AddNPC(54, 76, new List<int>(), content.Load<Texture2D>("character\\Bersh"));

            List<int> questListVV = new List<int>();
            questListVV.Add(1);
            questListVV.Add(3);
            listMaps[0].AddNPC(52, 88, questListVV, content.Load<Texture2D>("character\\VV"));           

            fight.LoadContent(content);

            song = content.Load<Song>("music\\Tetris");
            textureZombie1 = content.Load<Texture2D>("character\\zombie2");
        }

        public int WindowHeigth
        {
            set 
            {
                eventMessage.WindowHeigth = value;
                player.WindowHeigth = value;
                listMaps[currentMap].WindowHeigth = value;
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
                listMaps[currentMap].WindowWidth = value;
                fight.WindowWidth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        public void Init()
        {
            player.Init();
            characterInfo.Init();
            listMaps[currentMap].Init("level1.bin", new Vector2(48, 94));
            
            stage = 1;
            situation = 0;
            listMaps[currentMap].QuestNumber = stage;
            showCharacterInfo = false;
            keyCDown1 = false;
            keyCDown2 = false;
            keyQDown1 = false;
            keyQDown2 = false;

            eventMessage.Text = quest[stage - 1];
            eventMessage.UpdateButtonPosition();

            Character.CharacterIndex mobIndex;
            mobIndex.hp = 100;
            mobIndex.mana = 100;
            mobIndex.atackMin = 20;
            mobIndex.atackMax = 25;
            mobIndex.defense = 5;

            listMaps[currentMap].AddMob(44, 64, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(13, 81, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(48, 60, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(48, 42, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(48, 14, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(11, 53, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(10, 69, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(25, 69, 10, textureZombie1, mobIndex);
            listMaps[currentMap].AddMob(43, 70, 10, textureZombie1, mobIndex);

            listMaps[currentMap].AddHealPotion(42, 59);
            listMaps[currentMap].AddHealPotion(20, 67);
            listMaps[currentMap].AddHealPotion(6, 83);
            listMaps[currentMap].AddHealPotion(42, 76);
            listMaps[currentMap].AddHealPotion(42, 26);

            listMaps[currentMap].AddManaPotion(7, 65);
            listMaps[currentMap].AddManaPotion(6, 80);
            listMaps[currentMap].AddManaPotion(42, 73);
            listMaps[currentMap].AddManaPotion(51, 65);
            listMaps[currentMap].AddManaPotion(54, 26);

            listMaps[currentMap].AddPortal(53, 69);
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
                if (situation == Situation.EVENT_MESSAGE) situation = Situation.WALK_ON_MAP;
                else situation = Situation.EVENT_MESSAGE;
            }
            else if (situation == Situation.EVENT_MESSAGE)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    situation = Situation.WALK_ON_MAP;
                }
            }

            keyQDown2 = keyQDown1;

            #endregion

            switch (situation)
            {
                case Situation.EVENT_MESSAGE:
                    eventMessage.Update(time);
                    MediaPlayer.Stop();
                    break;

                case Situation.WALK_ON_MAP:
                    {
                        if (MediaPlayer.Queue.ActiveSong == null || MediaPlayer.State == MediaState.Stopped)
                        {
                            MediaPlayer.Play(song);
                        }
                        
                        listMaps[currentMap].Update(time);
                        player.Update(time);

                        mobNumber = listMaps[currentMap].MobConnected();

                        if (mobNumber != -1)
                        {
                            situation = Situation.FIGTH;
                            MediaPlayer.Stop();
                            fight.Init(characterInfo.characterIndexFull, characterInfo.characterIndexCurrent, listMaps[currentMap].GetMobIndex(mobNumber));                           
                        }                      
                    }

                    if (listMaps[currentMap].QuestNPC())
                    {
                        if(stage == 1)
                        {
                            listMaps[currentMap].OpenMap();
                        }
                        StageInc();
                    }

                    if (characterInfo.characterIndexCurrent.hp < characterInfo.characterIndexFull.hp)
                    {
                        if(listMaps[currentMap].HealPotionConnected())
                        {
                            characterInfo.RestoreHP();
                        }
                    }

                    if (characterInfo.characterIndexCurrent.mana < characterInfo.characterIndexFull.mana)
                    {
                        if (listMaps[currentMap].ManaPotionConnected())
                        {
                            characterInfo.RestoreMana();
                        }
                    }


                    break;

                case Situation.FIGTH:
                    fight.Update(time);
                    if(fight.win)
                    {
                        characterInfo.characterIndexCurrent = fight.currentCharacterIndex;
                        characterInfo.AddExperience(listMaps[currentMap].KickMob(mobNumber));

                        situation = Situation.WALK_ON_MAP;

                        if (listMaps[currentMap].MobCnt == 0)
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
                case Situation.EVENT_MESSAGE:
                    eventMessage.Draw(bath);
                    break;

                case Situation.WALK_ON_MAP:
                    listMaps[currentMap].Draw(bath);
                    player.Draw(bath);
                    if (showCharacterInfo)
                    {
                        characterInfo.Draw(bath);
                    }
                    break;

                case Situation.FIGTH:
                    fight.Draw(bath);
                    break;
            }           
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonEventMessageOk_OnClick(object sender, EventArgs e)
        {
            situation = Situation.WALK_ON_MAP;
        }

        #endregion
        //==============================================================================================
        #region Другие функции

        private void StageInc()
        {
            stage++;
            listMaps[currentMap].QuestNumber = stage;

            eventMessage.Text = quest[stage - 1];
            eventMessage.UpdateButtonPosition();

            situation = 0;
        }

        #endregion
    }
}
