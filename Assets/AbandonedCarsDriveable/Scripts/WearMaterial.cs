using System;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    [Serializable]
    public class WearMaterial
    {
        public Material BodyMaterial;
        public Material WheelsMaterial;
        public Material InteriorMaterial;
        public Material RailingsMaterial;
        public Material EngineMaterial;
        public Material[] FrontLightMaterialArray; 
        public Material RearLightMaterial;
        public Material[] GlassBodyMaterial;


        public WearMaterial(Material originalBodyMaterial,
            Material originalWheelsMaterial,
            Material originalInteriorMaterial,
            Material originalRailingsMaterial,
            Material originalEngineMaterial,
            Material[] originalFrontLightMaterialArray, 
            Material originalRearLightMaterial,
            Material[] originalGlassBodyMaterial)
        {
            BodyMaterial = originalBodyMaterial;
            WheelsMaterial = originalWheelsMaterial;
            InteriorMaterial = originalInteriorMaterial;
            RailingsMaterial = originalRailingsMaterial;
            EngineMaterial = originalEngineMaterial;
            FrontLightMaterialArray = originalFrontLightMaterialArray; 
            RearLightMaterial = originalRearLightMaterial;
            GlassBodyMaterial = originalGlassBodyMaterial;
        }

        public WearMaterial()
        {
        }
    }

    public enum CarPart
    {
        Body = 0,
        Wheel = 1,
        Engine = 2,
        Railing = 3,
        Interior = 4,
        Light = 5,
        GlassBody = 6,
        Glass
    }
}