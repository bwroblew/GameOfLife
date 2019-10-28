using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class GameManager
    {
        private static GameManager instance = null;
        public static GameManager getInstance()
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }

        public World world;

        private GameManager()
        {
            InitializeGame();
        }

        public void InitializeGame()
        {
            world = new World();
        }

        public bool Save()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@".\save.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, world);
                stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool Load()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@".\save.bin", FileMode.Open, FileAccess.Read);
                world = (World)formatter.Deserialize(stream);
                world.Resize(GameSettings.getInstance().BoardHeight,
                             GameSettings.getInstance().BoardWidth);
                stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
