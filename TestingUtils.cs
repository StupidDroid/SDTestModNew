using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDTestModNew
{
    static class TestingUtils
    {
        public static void DumpChildComponents(Identifiable.Id id)
        {
            UnityEngine.Object[] components = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(id).GetComponentsInChildren<UnityEngine.Object>();
            SRML.Console.Console.Log("Printing child components for: " + SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(id).name, true);
            foreach(UnityEngine.Object obj in components)
            {
                SRML.Console.Console.Log("\tName: " + obj.name, true);
                SRML.Console.Console.Log("\tType: " + obj.GetType().ToString(), true);
            }
        }
    }
}
