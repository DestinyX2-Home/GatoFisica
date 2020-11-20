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
using Microsoft.Xna.Framework.Graphics;

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
        public Resorte resorte;
        public bool spawnedCookie = false;
        public Eslabon eslabon;

        public MainGame(Texture2D lb1,Texture2D lb2,SpriteBatch batch)
        {
            camara = new Camara(new Vector2(400 / 300), 1, 0);
            camara.HacerActiva();
            circulo = new UTGameObject("meow_cookie", new Vector2(300, 80), .2f, UTGameObject.FF_form.Circulo);
            circulo.objetoFisico.absorcionChoque = .5f;
            UTGameObject platform = new UTGameObject("Muro", new Vector2(300, 155),.5f, UTGameObject.FF_form.Rectangulo, true);
            platform.objetoFisico.absorcionChoque = .6f;
            platform.rot = 30 * 2 * (float)Math.PI / 360;

            UTGameObject platform2 = new UTGameObject("Muro", new Vector2(700, 301), .5f, UTGameObject.FF_form.Rectangulo, true);
            platform2.objetoFisico.absorcionChoque = .6f;


            resorte = new Resorte(.1f, "resorte2", new Vector2(380, 400), .3f, UTGameObject.FF_form.Rectangulo, true);
            resorte.rot = 20 * 2 * (float)Math.PI / 360;

            eslabon = new Eslabon(new Vector2(100, 100), new Vector2(100, 140),"eslabon1",true);
            eslabon.CreateChain(4);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _previousState = _currentState;
            _currentState = Mouse.GetState();
            eslabon.Update(gameTime);
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
            if(Keyboard.GetState().IsKeyDown(Keys.F))
            {
                resorte.Launch();
            }
            #endregion

        }

        public void UpdateVelocimetro()
        {
            TextDisplayer text = components[^1] as TextDisplayer;


            Vector2 vec = new Vector2((float)Math.Round(circulo.objetoFisico.vel.X, 3), (float)Math.Round(circulo.objetoFisico.vel.Y, 3));

            Vector2 dir = new Vector2((float)Math.Round(Vector2.Normalize(circulo.objetoFisico.vel).X, 3), (float)Math.Round(Vector2.Normalize(circulo.objetoFisico.vel).Y, 3));

            double mag = Math.Sqrt(Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2));

            text.ChangeText("Velocidad: " + vec + "\nMagnitud: " + Math.Round(mag, 3) + "\nDireccion: " + dir + "\nRoce: " + MotorFisico.Roce);
        }

        public void MouseForce(GameTime time)
        {
            Vector2 distance;
            distance = new Vector2((int)(camara.PosMouseEnCamara().X - circulo.objetoFisico.pos.X), (int)(camara.PosMouseEnCamara().Y - circulo.objetoFisico.pos.Y));
            circulo.objetoFisico.AplicaFuerza(-distance, 1);
        }
    }
}