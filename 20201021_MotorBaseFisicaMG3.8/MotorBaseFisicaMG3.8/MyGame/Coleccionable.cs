using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.MyGame
{
    class Coleccionable : UTGameObject
    {
        public Coleccionable(string imagen, Vector2 pos, float escala, FF_form forma, bool isStatic = false) : base(imagen, pos, escala, forma, isStatic)
        {
            objetoFisico.isTrigger = true;
        }
    }
}
