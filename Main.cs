﻿using DLCPackage;
using MonomiPark.SlimeRancher.DataModel;
using MonomiPark.SlimeRancher.Regions;
using SDTestModNew;
using SRML;
using SRML.SR;
using SRML.SR.Translation;
using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SDTestModNew
{
    public class Main : ModEntryPoint
    {
        // Called before GameContext.Awake
        // You want to register new things and enum values here, as well as do all your harmony patching
        public override void PreLoad()
        {
            HarmonyInstance.PatchAll();
            TranslationPatcher.AddActorTranslation("1." + ModdedIds.MAPLE_SLIME.ToString().ToLower(), "Maple Slime");
            TranslationPatcher.AddActorTranslation("1." + ModdedIds.MAPLE_PLORT.ToString().ToLower(), "Maple Plort");
            new SlimePediaEntryTranslation(ModdedIds.MAPLE_SLIMES).SetTitleTranslation("Maple Slimes").SetIntroTranslation("Intro Translation").SetSlimeologyTranslation("Slimeology Translation").SetDietTranslation("Fruit").SetFavoriteTranslation("Favorite Food").SetRisksTranslation("Risks").SetPlortonomicsTranslation("Plortonomics");
            PlortRegistry.AddPlortEntry(ModdedIds.MAPLE_PLORT, new ProgressDirector.ProgressType[] {
                ProgressDirector.ProgressType.NONE
            });
            // Might need something to do with SaveRegistry here, I have literally no idea.
            DataModelRegistry.RegisterCustomActorModel(ModdedIds.MAPLE_SLIME, (long x, Identifiable.Id y, RegionRegistry.RegionSetId z, GameObject w) => new SlimeModel(x, y, z, w.transform));
            PediaRegistry.RegisterIdentifiableMapping(PediaDirector.Id.PLORTS, ModdedIds.MAPLE_PLORT);
            PediaRegistry.RegisterIdentifiableMapping(ModdedIds.MAPLE_SLIMES, ModdedIds.MAPLE_SLIME);
            PediaRegistry.SetPediaCategory(ModdedIds.MAPLE_SLIMES, PediaRegistry.PediaCategory.SLIMES);
        }


        // Called before GameContext.Start
        // Used for registering things that require a loaded gamecontext
        public override void Load()
        {
            SlimeDefinition mapleDefinition = PrefabUtils.DeepCopyObject(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PINK_SLIME)) as SlimeDefinition;
            mapleDefinition.AppearancesDefault = new SlimeAppearance[1];
            mapleDefinition.Diet.Produces = new Identifiable.Id[]
            {
                ModdedIds.MAPLE_PLORT
            };
            mapleDefinition.Diet.MajorFoodGroups = new SlimeEat.FoodGroup[]
            {
                SlimeEat.FoodGroup.FRUIT
            };
            mapleDefinition.Diet.AdditionalFoods = new Identifiable.Id[0];
            mapleDefinition.Diet.Favorites = new Identifiable.Id[]
            {
                Identifiable.Id.POGO_FRUIT
            };
            List<SlimeDiet.EatMapEntry> eatMap = mapleDefinition.Diet.EatMap;
            if (eatMap != null)
            {
                eatMap.Clear();
            }
            mapleDefinition.CanLargofy = false;
            mapleDefinition.FavoriteToys = new Identifiable.Id[0];
            mapleDefinition.Name = "Maple";
            mapleDefinition.IdentifiableId = ModdedIds.MAPLE_SLIME;
            SlimeDefinition pinkSlimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PINK_SLIME);
            GameObject mapleGameObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME));
            mapleGameObject.name = "mapleSlime";
            mapleGameObject.GetComponent<PlayWithToys>().slimeDefinition = mapleDefinition;
            mapleGameObject.GetComponent<SlimeAppearanceApplicator>().SlimeDefinition = mapleDefinition;
            mapleGameObject.GetComponent<SlimeEat>().slimeDefinition = mapleDefinition;
            SlimeAppearance mapleAppearance = PrefabUtils.DeepCopyObject(pinkSlimeDefinition.AppearancesDefault[0]) as SlimeAppearance;
            foreach (SlimeAppearanceStructure slimeAppearanceStructure in mapleAppearance.Structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                bool flag = ((defaultMaterials != null) ? defaultMaterials.Length : 0) == 0;
                if (!flag)
                {
                    Material material = UnityEngine.Object.Instantiate<Material>(slimeAppearanceStructure.DefaultMaterials[0]);
                    material.SetColor(topColorNameId, Color.red);
                    material.SetColor(middleColorNameId, Color.red);
                    material.SetColor(bottomColorNameId, Color.red);
                    material.SetColor("_SpecColor", Color.red);
                    material.SetFloat("_Shininess", 50f);
                    material.SetFloat("_Gloss", 50f);
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
            }
            foreach(SlimeExpressionFace slimeExpressionFace in mapleAppearance.Face.ExpressionFaces)
            {
                bool flag2 = slimeExpressionFace.Mouth;
                if (flag2)
                {
                    slimeExpressionFace.Mouth.SetColor("_MouthBot", Color.black);
                    slimeExpressionFace.Mouth.SetColor("_MouthMid", Color.black);
                    slimeExpressionFace.Mouth.SetColor("_MouthTop", Color.black);
                }

                bool flag3 = slimeExpressionFace.Eyes;
                if (flag3)
                {
                    slimeExpressionFace.Eyes.SetColor("_EyeBlue", Color.black);
                    slimeExpressionFace.Eyes.SetColor("_EyeGreen", Color.black);
                    slimeExpressionFace.Eyes.SetColor("_EyeRed", Color.black);
                    slimeExpressionFace.Eyes.SetColor("_GlowColor", Color.black);
                }
            }
            mapleAppearance.Face.OnEnable();
            mapleAppearance.ColorPalette = new SlimeAppearance.Palette
            {
                Bottom = Color.red,
                Middle = Color.red,
                Top = Color.red
            };
            mapleGameObject.GetComponent<Identifiable>().id = ModdedIds.MAPLE_SLIME;
            LookupRegistry.RegisterIdentifiablePrefab(mapleGameObject);
            SlimeRegistry.RegisterSlimeDefinition(mapleDefinition);
            Material maplePlortMaterial = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT).GetComponentInChildren<MeshRenderer>().material;
            GameObject maplePlortObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT));
            maplePlortObject.GetComponent<Identifiable>().id = ModdedIds.MAPLE_PLORT;
            maplePlortObject.name = "maplePlort";
            maplePlortMaterial = UnityEngine.Object.Instantiate<Material>(maplePlortMaterial);
            maplePlortMaterial.SetColor(topColorNameId, new Color(182f, 35f, 35f));
            maplePlortMaterial.SetColor(middleColorNameId, new Color(182f, 35f, 35f));
            maplePlortMaterial.SetColor(bottomColorNameId, new Color(182f, 35f, 35f));
            maplePlortObject.GetComponent<MeshRenderer>().material = maplePlortMaterial;
            LookupRegistry.RegisterIdentifiablePrefab(maplePlortObject);
            Sprite maplePlortSprite = IMG2Sprite.LoadNewSprite("SDTestModNew.MaplePlort.png");
            LookupRegistry.RegisterVacEntry(ModdedIds.MAPLE_PLORT, new Color(182f, 35f, 35f), maplePlortSprite);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, maplePlortObject);
            Sprite mapleSlimeSprite = IMG2Sprite.LoadNewSprite("SDTestModNew.MapleSlime.png");
            PediaRegistry.RegisterIdEntry(ModdedIds.MAPLE_SLIMES, mapleSlimeSprite);
            PlortRegistry.AddEconomyEntry(ModdedIds.MAPLE_PLORT, 10f, 40f);
            DroneRegistry.RegisterBasicTarget(ModdedIds.MAPLE_PLORT);
        }

        // Called after all mods Load's have been called
        // Used for editing existing assets in the game, not a registry step
        public override void PostLoad()
        {

        }

        private static int topColorNameId = Shader.PropertyToID("_TopColor");
        private static int middleColorNameId = Shader.PropertyToID("_MiddleColor");
        private static int bottomColorNameId = Shader.PropertyToID("_BottomColor");

    }
}