using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kontroll
{
    class GameObjectManager
    {
        static public List<GameObject> gameObjects = new List<GameObject>();
        static private List<GameObject> gameObjectsToRemove = new List<GameObject>();
        static private List<GameObject> gameObjectsToAdd = new List<GameObject>();

        static public void Update()
        {
            foreach (GameObject g in gameObjectsToRemove)
            {
                gameObjects.Remove(g);
            }
            gameObjectsToRemove.Clear();
            foreach (GameObject g in gameObjectsToAdd)
            {
                gameObjects.Add(g);
            }
            gameObjectsToAdd.Clear();

            foreach (GameObject g in gameObjects)
            {
                g.Update();
            }
        }

        static public void add(GameObject g)
        {
            gameObjectsToAdd.Add(g);
        }

        static public void remove(GameObject g)
        {
            gameObjectsToRemove.Add(g);
        }
    }
}
