using Harmony;
using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsPlus {

    [HarmonyPatch(typeof(Event))]
    [HarmonyPatch("checkForNextCommand")]
    [HarmonyPatch(new Type[] { typeof(GameLocation), typeof(GameTime) })]
    class NextCommand {

        static void Postfix(GameLocation location, GameTime time, Event __instance) {
            if (__instance.skipped || Game1.farmEvent != null)
                return;

            Executor.ExecuteCommands(location, time, __instance, 
                                     __instance.eventCommands[Math.Min(__instance.eventCommands.Length - 1, 
                                                                       __instance.CurrentCommand)].Split(' '));
        }
    }
}
