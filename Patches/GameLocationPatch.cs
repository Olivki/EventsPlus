using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {

    [HarmonyPatch(typeof(GameLocation))]
    [HarmonyPatch("checkEventPrecondition")]
    [HarmonyPatch(new Type[] { typeof(string) })]
    class EventPrecondition {

        static int Postfix(int __result, string precondition, GameLocation __instance) {
            return Condtioner.Conditions(precondition.Split('/'), __instance, __result);
        }
    }
}
