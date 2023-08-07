using UnityEngine;

namespace Project.Scripts
{
    public abstract class TargetModifier
    {
        public abstract float GetSpeed();
        public abstract float GetAmplitude();

        public abstract float GetFrequency();
    }

    public class GreyModifier : TargetModifier
    {
        private const float Speed = 0.05f;
        private const float amplitude = 1.0f;
        private const float frequency = 1.0f;

        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;
    }
    public class GreenModifier : TargetModifier
    {
        private const float Speed = 0.07f;
        private const float amplitude = 1.0f;
        private const float frequency = 1.0f;

        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;
    }
    public class OrangeModifier : TargetModifier
    {
        private const float Speed = 1.25f; //TODO: make the values smaller = to like grey mod
        private const float amplitude = 1.25f;
        private const float frequency = 1.25f;
        
        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;
        
    }
    public class RedModifier : TargetModifier
    {
        private const float Speed = 1.50f;
        private const float amplitude = 1.50f;
        private const float frequency = 1.50f;
        
        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;
        
       
    }
    public class PurpleModifier : TargetModifier
    {
        private const float Speed = 1.75f;
        private const float amplitude = 1.75f;
        private const float frequency = 1.75f;
        
        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;
        
      
    }
    public class BrownModifier : TargetModifier
    {
        private const float Speed = 2.0f;
        private const float amplitude = 2.0f;
        private const float frequency = 1.75f;
        
        public override float GetSpeed() => Speed;

        public override float GetAmplitude() => amplitude;
        public override float GetFrequency() => frequency;

    }
}