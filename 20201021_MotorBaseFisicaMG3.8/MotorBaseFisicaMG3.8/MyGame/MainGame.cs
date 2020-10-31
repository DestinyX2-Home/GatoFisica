using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MotorBaseFisicaMG38.MyGame.Components;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;
using MotorBaseFisicaMG38.SistemaGameObject;
using MotorBaseFisicaMG38.SistemaGO;

namespace MotorBaseFisicaMG38.MyGame
{
    class MainGame : Escena
    {
        Camara camara;
        MouseState _currentState;
        MouseState _previousState;
        float rotationScale = .5f;
        float cameraMovementSpeed = 400f;
        UTGameObject circulo;
        public UTGameObject superior;
        public UTGameObject inferior;
        public UTGameObject izquierdo;
        public UTGameObject derecho;
        public bool spawnedCookie = false;

        public MainGame()
        {
            camara = new Camara(new Vector2(400 / 300), 1, 0);
            camara.HacerActiva();
            circulo = new UTGameObject("meow_cookie", new Vector2(400, 200), .2f, UTGameObject.FF_form.Circulo);
            circulo.objetoFisico.absorcionChoque = .5f;
            UTGameObject platform = new UTGameObject("Muro", new Vector2(400, 255),.5f, UTGameObject.FF_form.Rectangulo, true);
            platform.objetoFisico.absorcionChoque = .5f;
            platform.rot = 45 * 2 * (float)Math.PI / 360;
            superior = new UTGameObject("Colsion1", new Vector2(150, -10), 3f, UTGameObject.FF_form.Rectangulo, true);
            inferior = new UTGameObject("Colsion1", new Vector2(150, 450), 3f, UTGameObject.FF_form.Rectangulo, true);
            inferior.objetoFisico.absorcionChoque = 0f;
            izquierdo = new UTGameObject("Collision2", new Vector2(20, 220), 1f, UTGameObject.FF_form.Rectangulo, true);
            derecho = new UTGameObject("Collision2", new Vector2(780, 220), 1f, UTGameObject.FF_form.Rectangulo, true);
            Resorte resorte = new Resorte(.1f, "resorte2", new Vector2(500, 350), .3f, UTGameObject.FF_form.Rectangulo, true);

            //position1 = new Vector2(0, 0);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _previousState = _currentState;
            _currentState = Mouse.GetState();
            if (_currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed)
            {
                MouseForce(gameTime);
            }
            //update de los botones y el display de fuerzas, no tocar
            foreach (Component cp in components)
            {
                cp.Update(gameTime);
            }
            UpdateVelocimetro();
            #region Move Camera dont touch
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camara.pos += new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * cameraMovementSpeed, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camara.pos -= new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * cameraMovementSpeed, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camara.pos -= new Vector2(0, (float)gameTime.ElapsedGameTime.TotalSeconds * cameraMovementSpeed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camara.pos += new Vector2(0, (float)gameTime.ElapsedGameTime.TotalSeconds * cameraMovementSpeed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                camara.escala *= 1.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                camara.escala *= .9f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                camara.rot -= (float)gameTime.ElapsedGameTime.TotalSeconds * rotationScale;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                camara.rot += (float)gameTime.ElapsedGameTime.TotalSeconds * rotationScale;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.G) && !spawnedCookie)
            {
                new UTGameObject("meow_cookie", camara.PosMouseEnCamara(), .2f, UTGameObject.FF_form.Circulo);
                spawnedCookie = true;               
            }
            if (Keyboard.GetState().IsKeyUp(Keys.G))
            {
                spawnedCookie = false;
            }
            #endregion

        }

        public void UpdateVelocimetro()
        {
            TextDisplayer text = components[^1] as TextDisplayer;

            Vector2 distance = new Vector2((int)(camara.PosMouseEnCamara().X - circulo.objetoFisico.pos.X), (int)(camara.PosMouseEnCamara().Y - circulo.objetoFisico.pos.Y));

            Vector2 vec = new Vector2((float)Math.Round(circulo.objetoFisico.vel.X, 3), (float)Math.Round(circulo.objetoFisico.vel.Y, 3));

            Vector2 dir = new Vector2((float)Math.Round(Vector2.Normalize(circulo.objetoFisico.vel).X, 3), (float)Math.Round(Vector2.Normalize(circulo.objetoFisico.vel).Y, 3));

            double mag = Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));

            text.ChangeText("Velocidad: " + vec + "\nMagnitud: " + Math.Round(mag, 3) + "\nDireccion: " + dir + "\nRoce: " + MotorFisico.Roce);

            TextDisplayer txt = components[0] as TextDisplayer;
            txt.Position = _currentState.Position.ToVector2() - new Vector2(txt.stringSize().X / 2, 0);
            txt.ChangeText("Fuerza: " + distance);
        }

        public void MouseForce(GameTime time)
        {
            Vector2 distance;
            distance = new Vector2((int)(camara.PosMouseEnCamara().X - circulo.objetoFisico.pos.X), (int)(camara.PosMouseEnCamara().Y - circulo.objetoFisico.pos.Y));
            circulo.objetoFisico.AplicaFuerza(-distance, 1);
        }
    }
}