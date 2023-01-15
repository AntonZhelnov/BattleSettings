using System;

namespace Gameplay.Data
{
    [Serializable]
    public class Settings
    {
        public CameraSettings cameraSettings;
        public GameSettings gameSettings;
        public Stat[] stats;
        public Buff[] buffs;
    }

    [Serializable]
    public class CameraSettings
    {
        public float roundDuration;
        public float roundRadius;
        public float height;
        public float lookAtHeight;
        public float roamingRadius;
        public float roamingDuration;
        public float fovMin;
        public float fovMax;
        public float fovDelay;
        public float fovDuration;
    }

    [Serializable]
    public class GameSettings
    {
        public int buffCountMin;
        public int buffCountMax;
        public bool allowDuplicateBuffs;
    }

    [Serializable]
    public class Stat
    {
        public int id;
        public string icon;
        public float value;
    }

    [Serializable]
    public class BuffStat
    {
        public int statId;
        public float value;
    }

    [Serializable]
    public class Buff
    {
        public string title;
        public string icon;
        public BuffStat[] stats;
    }
}