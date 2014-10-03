using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_test1.Character;
using Microsoft.Xna.Framework.Input;

namespace XNA_test1
{
    class Game
    {
        #region Переменные

        EventMessage eventMessage;
        CharacterMove player;
        CharacterInfo characterInfo;
        int stage;  // прогресс игры
        bool showCharacterInfo;
        bool keyCDown1;
        bool keyCDown2;

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
            
            stage = 1;
            showCharacterInfo = false;
            keyCDown1 = false;
            keyCDown2 = false;
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
        }

        public int WindowHeigth
        {
            set 
            {
                eventMessage.WindowHeigth = value;
                player.WindowHeigth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        public int WindowWidth
        {
            set 
            { 
                eventMessage.WindowWidth = value;
                player.WindowWidth = value;
                eventMessage.UpdateButtonPosition();
            }
        }

        #endregion
        //==============================================================================================
        #region Основные потоки

        public void Update(GameTime time)
        {
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

            switch (stage)
            {
                case 1:
                    eventMessage.Update(time);
                    break;

                case 2:
                    player.Update(time);
                    break;
            }            
        }

        public void Draw(SpriteBatch bath)
        {
            switch (stage)
            {
                case 1:
                    eventMessage.Draw(bath);
                    break;

                case 2:
                    player.Draw(bath);
                    if(showCharacterInfo)
                    {
                        characterInfo.Draw(bath);
                    }
                    break;
            } 
        }

        #endregion
        //==============================================================================================
        #region События кнопок при нажатии

        private void ButtonEventMessageOk_OnClick(object sender, EventArgs e)
        {
            stage = 2;
        }

        #endregion
    }
}
