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
        Vector2 deformacion;
        float constante;
        Vector2 position;
        Vector2 impactForce;
        ObjetoFisico toLaunch;


        public Resorte(float cons, string imagen, Vector2 pos, float escala, FF_form forma, bool isStatic = false) 
            : base(imagen,pos,escala,forma,isStatic)
        {
            longitud = base.dibujable.alto;
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
            Vector2 lambdaX = Vector2.Subtract(position, deformacion);
            Vector2 launchAngle = new Vector2(MathF.Sin(toLaunch.rot), MathF.Cos(toLaunch.rot));
            return launchAngle * lambdaX *-1;
        }
        public override void OnCollision(UTGameObject other)
        {
            base.OnCollision(other);

            if(other.objetoFisico!=null)
                toLaunch = other.objetoFisico;

            if (toLaunch != null)
            {
                impactForce = toLaunch.vel * toLaunch.masa;
                deformacion = impactForce / -constante;
                toLaunch.isStatic = true;
                toLaunch.vel = Vector2.Zero;
            }
        }

        public void Launch()
        {
            if (toLaunch != null)
            {
                toLaunch.pos = toLaunch.pos + new Vector2(0, -5f);
                toLaunch.AplicaFuerza(FuerzaResorte(toLaunch.pos), 1);   
                toLaunch.isStatic = false;
                toLaunch = null;
            }
        }
    }
}
