using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMOD;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace Beluga.AudioShit
{
    // also quite a bit from the seal - thanks guys!
    public class AudioRegistrar
    {
        public const MODE k3DSoundModes = MODE.DEFAULT | MODE._3D | MODE.ACCURATETIME | MODE._3D_LINEARSQUAREROLLOFF;
        public const MODE k2DSoundModes = MODE.DEFAULT | MODE._2D | MODE.ACCURATETIME;
        public const MODE kStreamSoundModes = k2DSoundModes | MODE.CREATESTREAM;

        public static void RegisterAudio(AssetBundle bundle)
        {
            // SFX
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("undock"), "undock");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("cyclops_door_close"), "cyclops_door_close");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("cyclops_door_open"), "cyclops_door_open");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("docking_doors_close"), "docking_doors_close");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("docking_doors_open"), "docking_doors_open");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("explode"), "explode");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("buttonon"), "buttonon");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("buttonoff"), "buttonoff");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("ladderUp"), "ladderUp");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("ladderDown"), "ladderDown");

            // Engine
            AddWorldLoopingSoundEffect(bundle.LoadAsset<AudioClip>("AmbientLoop"), "AmbientLoop");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("PowerDown"), "PowerDown");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("PowerUp"), "PowerUp");
            AddWorldLoopingSoundEffect(bundle.LoadAsset<AudioClip>("DriveTheFuckinCar"), "DriveTheFuckinCar");
            AddWorldSoundEffect(bundle.LoadAsset<AudioClip>("bwompwomp"),"bwompwomp");

            // AIVoicelines
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("AbandonShip"), "AbandonShip");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("AgilityOff"), "AgilityOff");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("AgilityOn"), "AgilityOn");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("Autolevel"), "Autolevel"); // TODO
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("CreatureAttack"), "CreatureAttack"); // TODO
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DamageCritical"), "DamageCritical");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DamageModerate"), "DamageModerate");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DepthClose"), "DepthClose");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DepthMaximum"), "DepthMaximum");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DetectLeviathan"), "DetectLeviathan");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("DetectLifeform"), "DetectLifeform");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("EnginePowerDown"), "EnginePowerDown");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("EnginePowerUp"), "EnginePowerUp");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("PowerCritical"), "PowerCritical");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("PowerDepleting"), "PowerDepleting");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("PowerLow"), "PowerLow");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("PowerZero"), "PowerZero");
            AddSubVoiceLine(bundle.LoadAsset<AudioClip>("WelcomeAboard"), "WelcomeAboard");

            // PDA
            AddPDAVoiceline(bundle.LoadAsset<AudioClip>("FirstAgility"), "FirstAgility");
            AddPDAVoiceline(bundle.LoadAsset<AudioClip>("FirstCraft"), "FirstCraft");
        }

        public static void AddSubVoiceLine(AudioClip clip, string soundPath)
        {
            var sound = AudioUtils.CreateSound(clip, kStreamSoundModes);
            CustomSoundHandler.RegisterCustomSound(soundPath, sound, AudioUtils.BusPaths.VoiceOvers);
        }

        public static void AddWorldSoundEffect(AudioClip clip, string soundPath, float minDistance = 1f, float maxDistance = 100f, string overrideBus = null)
        {
            var sound = AudioUtils.CreateSound(clip, k3DSoundModes);
            if (maxDistance > 0f)
            {
                sound.set3DMinMaxDistance(minDistance, maxDistance);
            }
            CustomSoundHandler.RegisterCustomSound(soundPath, sound, string.IsNullOrEmpty(overrideBus) ? AudioUtils.BusPaths.PlayerSFXs : overrideBus);
        }

        public static void AddInterfaceSoundEffect(AudioClip clip, string soundPath)
        {
            var sound = AudioUtils.CreateSound(clip, k2DSoundModes);
            CustomSoundHandler.RegisterCustomSound(soundPath, sound, AudioUtils.BusPaths.PlayerSFXs);
        }

        public static void AddPDAVoiceline(AudioClip clip, string soundPath)
        {
            var sound = AudioUtils.CreateSound(clip, k2DSoundModes);
            CustomSoundHandler.RegisterCustomSound(soundPath, sound, AudioUtils.BusPaths.PDAVoice);
        }

        public static void AddWorldLoopingSoundEffect(AudioClip clip, string soundPath, float minDistance = 1f, float maxDistance = 100f, string overrideBus = null)
        {
            var sound = AudioUtils.CreateSound(clip, k3DSoundModes);
            if (maxDistance > 0f)
            {
                sound.set3DMinMaxDistance(minDistance, maxDistance);
            }
            sound.setMode(MODE.LOOP_NORMAL);
            CustomSoundHandler.RegisterCustomSound(soundPath, sound, string.IsNullOrEmpty(overrideBus) ? AudioUtils.BusPaths.PlayerSFXs : overrideBus);
        }
    }
}