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
        Song song;
        int stage;  // прогресс игры
        bool showQuest;
        bool showCharacterInfo;
        bool keyCDown1, keyCDown2;
        bool keyQDown1, keyQDown2;

        #endregion
        //==============================================================================================
        #region Инициализация

        public Game()
        {
            eventMessage = new EventMessage();
            eventMessage.ButtonOk_OnClick = ButtonEventMessageOk_OnClick;
            eventMessage.Text = "Игра началась!\nПервый квест: сдать все лабы В.В.";

            player = new CharacterMove();
            characterInfo = new CharacterInfo();
            map = new Map();
            
            stage = 1;
            showQuest = true;
            showCharacterInfo = false;
            keyCDown1 = false;
            keyCDown2 = false;
            keyQDown1 = false;
            keyQDown2 = false;
        }

        public void LoadContent(ContentManager content)
        {
            Dictionary<VisibleState, Texture2D> buttonTextures;

            eventMessage.Fon = content.Load<Texture2D>("text_fon\\fallout_1920x1080");
            eventMessage.Font = content.Load<SpriteFont>("font\\fallout_font");

            buttonTextures = new Dictionary<VisibleState, Texture2D>();
            buttonTextures.Add(VisibleState.Normal, content.Load<Texture2D>("button\\button3_norm"));
            buttonTextures.Add(VisibleState.Hover, content.Load<Texture2D>("button\\button3_hover"));
            buttonTextures.Add(VisibleState.Pressed, content.Load<Texture2D>("button\\button3_pressed"));
            eventMessage.ButtonTexture = buttonTextures;

            player.Texture = content.Load<Texture2D>("character\\edward_elric_1");
            characterInfo.Texture = content.Load<Texture2D>("text_fon\\paper1_cr");
            characterInfo.Font = content.Load<SpriteFont>("font\\character_info");

            map.LoadContent(content);
            song = content.Load<Song>("music\\Tetris");
        }

        public int WindowHeigth
        {
            set 
            {
                eventMessage.WindowHeigth = value;
                player.WindowHeigth = value;
                map.WindowHeigth = value;
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
                characterInfo.AddExperience(100);
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
                showQuest = !showQuest;
            }

            keyQDown2 = keyQDown1;

            #endregion

            switch (stage)
            {
                case 1:
                    if (showQuest)
                    {
                        eventMessage.Update(time);
                        MediaPlayer.Stop();
                    }
                    else
                    {
                        map.Update(time);
                        player.Update(time);

                        if (MediaPlayer.Queue.ActiveSong == null || MediaPlayer.State == MediaState.Stopped)
                        {
                            MediaPlayer.Play(song);
                        }
                    }
                    break;

                case 2:
                    
                    break;

                default:
                    MediaPlayer.Stop();
                    break;

            }            
        }

        public void Draw(SpriteBatch bath)
        {
            switch (stage)
            {
                case 1:
                    if (showQuest)
                    {
                        eventMessage.Draw(bath);
                    }
                    else
                    {
                        map.Draw(bath);
                        player.Draw(bath);
                        if (showCharacterInfo)
                        {
                            characterInfo.Draw(bath);
                        }
                    }
                    break;              

                case 2:
                    
                    break;
            } 
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonEventMessageOk_OnClick(object sender, EventArgs e)
        {
            showQuest = false;
        }

        #endregion
    }
}
