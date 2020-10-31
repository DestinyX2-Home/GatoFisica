using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.SistemaGO
{
    class Resorte :UTGameObject
    {
        float longitud;
        float deformacion;
        float fuerza;
        float constante;
        Vector2 position;


        public Resorte(float cons, string imagen, Vector2 pos, float escala, FF_form forma, bool isStatic = false) 
            : base(imagen,pos,escala,forma,isStatic)
        {
            longitud = base.dibujable.alto;
            Debug.WriteLine(longitud);
            position = pos;
            constante = cons;
            base.objetoFisico.absorcionChoque = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public Vector2 FuerzaResorte(Vector2 obj)
        {
            Vector2 force = Vector2.Subtract(position, obj);
            float current = force.Length();
            deformacion = current - longitud;
            force.Normalize();
            force = force * constante * deformacion * -1;

            return force;
        }
        public override void OnCollision(UTGameObject other)
        {
            base.OnCollision(other);
            if (objetoFisico.lastCollision != null)
            {
                objetoFisico.lastCollision.isStatic = true;
                objetoFisico.lastCollision.vel = Vector2.Zero;
            }
        }
    }
}
