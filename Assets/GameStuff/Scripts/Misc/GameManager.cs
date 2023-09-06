using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool ArrayContainsEveryElement<T>(T[] collection, T[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (!collection.Contains(elements[i]))
            {
                return false;
            }
        }
        return true;
    }
}
