using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorBaseFisicaMG38.SistemaFisico
{
    public abstract class FormaFisica
    {
        public Vector2 pos;
        public abstract bool colisiona(FormaFisica otra, out Vector2 collisionPoint);
    }
}
