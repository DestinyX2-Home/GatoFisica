using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorBaseFisicaMG38.SistemaGameObject
{
    public static class UTGameObjectsManager
    {
        static List<UTGameObject> AllUTGameObjects = new List<UTGameObject>();
        static List<UTGameObject> DestroyedUTGameObjects = new List<UTGameObject>();
        public static void Init()
        {
            foreach(UTGameObject utg in AllUTGameObjects)
            {
                utg.Destroy();
            }
            AllUTGameObjects = new List<UTGameObject>();            
        }
        public static void DestruirObjeto(UTGameObject utg)
        {
            utg.OnDestroy();
            DestroyedUTGameObjects.Add(utg);
        }
        public static void suscribirObjeto(UTGameObject utg)
        {
            AllUTGameObjects.Add(utg);
        }
        public static void Update(GameTime gameTime)
        {
            foreach(UTGameObject utg in AllUTGameObjects)
            {
                utg.Update(gameTime);
            }
            foreach(UTGameObject utg in DestroyedUTGameObjects)
            {
                AllUTGameObjects.Remove(utg);
            }
            DestroyedUTGameObjects = new List<UTGameObject>();
        }

    }
}
