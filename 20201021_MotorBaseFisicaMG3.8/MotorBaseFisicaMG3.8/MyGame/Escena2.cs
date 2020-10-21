using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.MyGame
{
    public class Escena2 : Escena
    {
        bool click;

        public Escena2()
        {
            UTGameObjectsManager.Init();
            new Gato();
            //new UTGameObject("meow_cookie", new Vector2(700, 500), 1, UTGameObject.FF_form.Circulo);
            new UTGameObject("Muro", new Vector2(1000, 500), 1, UTGameObject.FF_form.Rectangulo, false);
            //new UTGameObject("Muro", new Vector2(500, 800), 1, UTGameObject.FF_form.Rectangulo, true);
            new UTGameObject("Muro", new Vector2(200, 500), 1, UTGameObject.FF_form.Rectangulo, true);
            new Camara(new Vector2(0, 0), .5f, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                new EscenaInicial();
            }
            if (!click && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                click = true;
                new Coleccionable("meow_cookie", Camara.ActiveCamera.PosMouseEnCamara(), .5f, UTGameObject.FF_form.Circulo);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                click = false;
            }
        }
    }
}
