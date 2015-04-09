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

        bool win;
        Label labelWin;

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
            listMaps.Add(new Map("level2.bin", new Vector2(35, 43)));
            
            fight = new Fight();

            labelWin = new Label();
            labelWin.Color = Color.Red;
            labelWin.Text = "Вы сдали курсач!!!";
            labelWin.X = 550;
            labelWin.Y = 300;
        }

        public void LoadContent(ContentManager content)
        {
            Dictionary<VisibleState, Texture2D> buttonTextures;

            quest[0] = content.Load<string>("quest\\quest1");
            quest[1] = content.Load<string>("quest\\quest2");
            quest[2] = content.Load<string>("quest\\quest3");
            quest[3] = content.Load<string>("quest\\quest4");
            quest[4] = content.Load<string>("quest\\quest5");
            quest[5] = content.Load<string>("quest\\quest6");
            quest[6] = content.Load<string>("quest\\quest7");
            
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
            questListVV.Add(6);
            listMaps[0].AddNPC(52, 88, questListVV, content.Load<Texture2D>("character\\VV"));

            List<int> questListLab = new List<int>();
            questListLab.Add(5);
            listMaps[1].AddNPC(29, 17, questListLab, content.Load<Texture2D>("character\\lab"));

            fight.LoadContent(content);

            song = content.Load<Song>("music\\Tetris");
            textureZombie1 = content.Load<Texture2D>("character\\zombie2");

            labelWin.Font = content.Load<SpriteFont>("font\\fallout_font");
        }

        public int WindowHeigth
        {
            set 
            {
                eventMessage.WindowHeigth = value;
                player.WindowHeigth = value;
                for (int i = 0; i < listMaps.Count; i++)
                    listMaps[i].WindowHeigth = value;
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
                for (int i = 0; i < listMaps.Count; i++)
                    listMaps[i].WindowWidth = value;
                fight.WindowWidth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        public void Init()
        {
            player.Init();
            characterInfo.Init();
            listMaps[0].Init("level1.bin", new Vector2(48, 94));
            listMaps[1].Init("level2.bin", new Vector2(36, 43));
            currentMap = 0;

            stage = 1;
            situation = 0;
            Map.QuestNumber = stage;
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

            /*listMaps[0].AddMob(44, 64, 35, textureZombie1, mobIndex);
            listMaps[0].AddMob(13, 81, 35, textureZombie1, mobIndex);*/
            listMaps[0].AddMob(48, 60, 35, textureZombie1, mobIndex);

            characterInfo.AddExperience(70);

            /*listMaps[0].AddMob(48, 42, 10, textureZombie1, mobIndex);
            listMaps[0].AddMob(48, 14, 10, textureZombie1, mobIndex);
            listMaps[0].AddMob(11, 53, 10, textureZombie1, mobIndex);
            listMaps[0].AddMob(10, 69, 10, textureZombie1, mobIndex);
            listMaps[0].AddMob(25, 69, 10, textureZombie1, mobIndex);
            listMaps[0].AddMob(43, 70, 10, textureZombie1, mobIndex);*/

            listMaps[0].AddHealPotion(42, 59);
            listMaps[0].AddHealPotion(20, 67);
            listMaps[0].AddHealPotion(6, 83);
            listMaps[0].AddHealPotion(42, 76);
            listMaps[0].AddHealPotion(42, 26);

            listMaps[0].AddManaPotion(7, 65);
            listMaps[0].AddManaPotion(6, 80);
            listMaps[0].AddManaPotion(42, 73);
            listMaps[0].AddManaPotion(51, 65);
            listMaps[0].AddManaPotion(54, 26);

            listMaps[0].AddPortal(53, 69);

            mobIndex.hp = 150;
            mobIndex.mana = 400;
            mobIndex.atackMin = 25;
            mobIndex.atackMax = 30;
            mobIndex.defense = 10;

            listMaps[1].AddMob(26, 44, 70, textureZombie1, mobIndex);
            listMaps[1].AddMob(25, 32, 70, textureZombie1, mobIndex);
            listMaps[1].AddMob(30, 23, 70, textureZombie1, mobIndex);

            listMaps[1].AddPortal(37, 43);

            listMaps[1].AddHealPotion(36, 41);
            listMaps[1].AddHealPotion(36, 45);

            listMaps[1].AddManaPotion(35, 41);
            listMaps[1].AddManaPotion(35, 45);

            fight.Level2Off();

            win = false;            
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
                    

                    if (listMaps[currentMap].QuestNPC())
                    {
                        StageInc();                       
                    }

                    if (characterInfo.characterIndexCurrent.hp < characterInfo.characterIndexFull.hp)
                    {
                        int index = listMaps[currentMap].HealPotionConnected();
                        if(index != -1)
                        {
                            characterInfo.RestoreHP();
                            listMaps[currentMap].RemoveHealPotion(index);
                        }
                    }

                    if (characterInfo.characterIndexCurrent.mana < characterInfo.characterIndexFull.mana)
                    {
                        int index = listMaps[currentMap].ManaPotionConnected();
                        if (index != -1)
                        {
                            characterInfo.RestoreMana();
                            listMaps[currentMap].RemoveManaPotion(index);
                        }
                    }

                    if (listMaps[currentMap].PortalsConnected())
                    {
                        currentMap = (currentMap + 1) % 2;
                        listMaps[currentMap].GoGoGo();
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
                    if (win)
                        labelWin.Draw(bath);
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
            Map.QuestNumber = stage;

            eventMessage.Text = quest[stage - 1];
            eventMessage.UpdateButtonPosition();

            situation = 0;

            switch (stage)
            {
                case 2:
                    listMaps[0].OpenMap(48, 71);
                    listMaps[0].OpenMap(48, 72);
                    break;

                case 4:
                    listMaps[0].OpenMap(49, 69);
                    listMaps[0].OpenMap(50, 69);
                    characterInfo.AddExperience(500);
                    fight.Level2On();
                    break;

                case 5:
                    listMaps[1].OpenMap(29, 19);
                    listMaps[1].OpenMap(29, 20);
                    break;

                case 7:
                    win = true;
                    break;
            }
        }

        #endregion
    }
}
