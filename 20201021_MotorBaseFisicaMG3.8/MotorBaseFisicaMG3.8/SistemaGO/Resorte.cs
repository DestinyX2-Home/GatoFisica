using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MotorBaseFisicaMG38.SistemaGO
{
    class Resorte
    {
        float longitud;
        float deformacion;
        float fuerza;
        float constante;
        Vector2 position;

        public Resorte(float lenght, float cons)
        {
            longitud = lenght;
            constante = cons;
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
    }
}
