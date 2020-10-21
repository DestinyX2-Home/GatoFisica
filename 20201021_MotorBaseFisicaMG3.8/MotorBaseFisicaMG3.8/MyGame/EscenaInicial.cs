using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorBaseFisicaMG38.SistemaAudio;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.MyGame
{
    public class EscenaInicial:Escena
    {        
        UTGameObject playerUTG;
        Camara camara;
        Camara camara2;
        bool click = false;
        bool seeC2;
        bool spacePressed;
        public EscenaInicial()
        {            
            UTGameObject auto = new Gato();       

            new UTGameObject("meow_cookie", new Vector2(1200, 500), 1, UTGameObject.FF_form.Circulo);
            camara = new Camara(new Vector2(0, 0), .5f, 0);
            camara2 = new Camara(new Vector2(200, 200), 1, 0);
            //camara.centro = auto.dibujable;
            //camara2.centro = auto.dibujable;            

            //AudioManager.PlaySong("Locations_Happy Village (loop)", loop:true);
        }
        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);           
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camara.pos += new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * 100f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camara.pos += new Vector2(-(float)gameTime.ElapsedGameTime.TotalSeconds * 100f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                camara.escala *= 1.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                camara.escala *= .9f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                camara.rot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (!spacePressed)
                {
                    spacePressed = true;
                    if (camara.EsCamaraActiva())
                    {
                        camara2.HacerActiva();
                    }
                    else
                    {
                        camara.HacerActiva();
                    }
                }
            }
            else
            {
                spacePressed = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                new Escena2();
            }
            if(!click && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                click = true;
                new Coleccionable("meow_cookie", Camara.ActiveCamera.PosMouseEnCamara(), .5f, UTGameObject.FF_form.Circulo);
            }
            if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                click = false;
            }
        }
    }
}
