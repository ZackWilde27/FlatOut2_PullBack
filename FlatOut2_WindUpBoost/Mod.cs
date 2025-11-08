using FlatOut2_WindUpBoost.Configuration;
using FlatOut2_WindUpBoost.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using FlatOut2.SDK;
using FlatOut2.SDK.Enums;
using Reloaded.Mod.Interfaces;
using FlatOut2.SDK.API;
using FlatOut2.SDK.Structs;
using System.Numerics;

namespace FlatOut2_WindUpBoost
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public unsafe class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private static Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private static bool StartedWindUp = false;
        private static Vector3 Forward = new(0);
        private static Quaternion Rotation = new();
        private static float Power = 0;
        private static bool LockedOutOfWindUp = false;

        private static void Shoot(Car* car)
        {
            car->Velocity = Forward * Power;
            Power = 0;
            StartedWindUp = false;
        }

        private static void PerFrame()
        {
            var car = ((Player*)Info.Race.GetPlayers()[0])->Car;

            car->Nitro = 0;

            if (Info.Controller.IsKeyHeld(KeyboardKeys.Control))
            {
                if (!LockedOutOfWindUp)
                {
                    if (!StartedWindUp)
                    {
                        Forward = Vector3.Normalize(car->Velocity);
                        Rotation = car->Rotation;
                        StartedWindUp = true;
                    }

                    car->RotationalVelocity *= 0.5f;
                    //car->Rotation = Rotation;
                    car->Velocity += -Forward;

                    if (Vector3.Dot(car->Velocity, Forward) < 0)
                    {
                        float length = car->Velocity.Length();
                        if (length > _configuration.WindUpSpeed)
                            car->Velocity = (car->Velocity / length) * _configuration.WindUpSpeed;

                        if (Power < _configuration.MaxWindUp)
                            Power += _configuration.WindUpGain;
                        else
                        {
                            LockedOutOfWindUp = true;
                            Shoot(car);
                        }
                    }
                }
            }
            else
            {
                if (StartedWindUp)
                    Shoot(car);

                LockedOutOfWindUp = false;
            }
        }

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;


            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic

            SDK.Init(_hooks!);
            Helpers.HookPerFrame(PerFrame);
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}