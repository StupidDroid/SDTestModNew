using Assets.Script.Util.Extensions;
using DLCPackage;
using HarmonyLib;
using MonomiPark.SlimeRancher.DataModel;
using MonomiPark.SlimeRancher.Regions;
using SDTestModNew;
using SRML;
using SRML.SR;
using SRML.SR.Templates.Identifiables;
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
            TranslationPatcher.AddActorTranslation("l." + ModdedIds.MAPLE_SLIME.ToString().ToLower(), "Maple Slime");
            TranslationPatcher.AddActorTranslation("l." + ModdedIds.MAPLE_PLORT.ToString().ToLower(), "Maple Plort");
            TranslationPatcher.AddActorTranslation("l." + ModdedIds.TEST_CRATE_01.ToString().ToLower(), "Test Crate");
            new SlimePediaEntryTranslation(ModdedIds.MAPLE_SLIMES).SetTitleTranslation("Maple Slimes").SetIntroTranslation("Pancakes, anyone?").SetSlimeologyTranslation("A syrupy relative of the Honey Slime, the Maple Slimes are a fall favorite which can be found in the Moss Blanket. It is rumored that they were created in a collaboration between Ogden Ortiz and Viktor Humphries and then got loose. Maybe someday they'll make Butter Slimes too...").SetDietTranslation("Fruit").SetFavoriteTranslation("Pogofruit").SetRisksTranslation("These slimes produce very sticky plorts. If they get loose, you'll have a very sticky situation on your very sticky hands. Best not let that happen.").SetPlortonomicsTranslation("These delicious morsels are just about the best thing since sliced... anything! They're highly prized by top chefs and breakfast enthusiasts alike. There's nothing like the smell of warm syrup to wake up a hungry rancher.");
            new SlimePediaEntryTranslation(ModdedIds.CARAMEL_APPLES).SetTitleTranslation("Caramel Apples").SetIntroTranslation("Intro TK").SetDescriptionTranslation("Description TK");
            PlortRegistry.AddPlortEntry(ModdedIds.MAPLE_PLORT, new ProgressDirector.ProgressType[] {
                ProgressDirector.ProgressType.UNLOCK_MOSS
            });
            DataModelRegistry.RegisterCustomActorModel(ModdedIds.MAPLE_SLIME, (long x, Identifiable.Id y, RegionRegistry.RegionSetId z, GameObject w) => new SlimeModel(x, y, z, w.transform));
            PediaRegistry.RegisterIdentifiableMapping(PediaDirector.Id.PLORTS, ModdedIds.MAPLE_PLORT);
            PediaRegistry.RegisterIdentifiableMapping(ModdedIds.MAPLE_SLIMES, ModdedIds.MAPLE_SLIME);
            PediaRegistry.RegisterIdentifiableMapping(ModdedIds.CARAMEL_APPLES, ModdedIds.CARAMEL_APPLE_FRUIT);
            PediaRegistry.SetPediaCategory(ModdedIds.MAPLE_SLIMES, PediaRegistry.PediaCategory.SLIMES);
            PediaRegistry.SetPediaCategory(ModdedIds.CARAMEL_APPLES, PediaRegistry.PediaCategory.RESOURCES);
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
                    material.SetColor(topColorNameId, new Color(87.0f / 255.0f, 32.0f / 255.0f, 0.0f / 255.0f)); // new Color(216.0f / 255.0f, 120.0f / 255.0f , 63.0f / 255.0f)
                    material.SetColor(middleColorNameId, new Color(164.0f / 255.0f, 75.0f / 255.0f, 23.0f / 255.0f)); // new Color(194.0f / 255.0f, 99.0f / 255.0f, 44.0f / 255.0f)
                    material.SetColor(bottomColorNameId, new Color(226.0f / 255.0f, 123.0f / 255.0f, 63.0f / 255.0f)); // new Color(182.0f / 255.0f, 86.0f / 255.0f, 33.0f / 255.0f)
                    material.SetColor("_SpecColor", Color.red);
                    material.SetFloat("_Shininess", 5f);
                    material.SetFloat("_Gloss", 5f);
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
            }
            foreach(SlimeExpressionFace slimeExpressionFace in mapleAppearance.Face.ExpressionFaces)
            {
                bool flag2 = slimeExpressionFace.Mouth;
                if (flag2)
                { 
                    slimeExpressionFace.Mouth.SetColor("_MouthBot", new Color(145.0f / 255.0f, 17.0f / 255.0f, 17.0f / 255.0f));
                    slimeExpressionFace.Mouth.SetColor("_MouthMid", new Color(145.0f / 255.0f, 17.0f / 255.0f, 17.0f / 255.0f));
                    slimeExpressionFace.Mouth.SetColor("_MouthTop", new Color(145.0f / 255.0f, 17.0f / 255.0f, 17.0f / 255.0f));
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
                Bottom = new Color(182.0f / 255.0f, 86.0f / 255.0f, 33.0f / 255.0f),
                Middle = new Color(194.0f / 255.0f, 99.0f / 255.0f, 44.0f / 255.0f),
                Top = new Color(216.0f / 255.0f, 120.0f / 255.0f, 63.0f / 255.0f),
                Ammo = new Color(194.0f / 255.0f, 99.0f / 255.0f, 44.0f / 255.0f)
            };
            Sprite mapleSlimeSprite = IMG2Sprite.LoadNewSprite("SDTestModNew.MapleSlime.png");
            mapleAppearance.Icon = mapleSlimeSprite;
            mapleDefinition.AppearancesDefault = new SlimeAppearance[]
            {
                mapleAppearance
            };
            mapleGameObject.GetComponent<Identifiable>().id = ModdedIds.MAPLE_SLIME;
            LookupRegistry.RegisterIdentifiablePrefab(mapleGameObject);
            SlimeRegistry.RegisterSlimeDefinition(mapleDefinition);
            Material maplePlortMaterial = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT).GetComponentInChildren<MeshRenderer>().material;
            GameObject maplePlortObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT));
            maplePlortObject.GetComponent<Identifiable>().id = ModdedIds.MAPLE_PLORT;
            maplePlortObject.name = "maplePlort";
            maplePlortMaterial = UnityEngine.Object.Instantiate<Material>(maplePlortMaterial);
            maplePlortMaterial.SetColor(topColorNameId, new Color(182.0f / 255.0f, 86.0f / 255.0f, 33.0f / 255.0f));
            maplePlortMaterial.SetColor(middleColorNameId, new Color(87.0f / 255.0f, 32.0f / 255.0f, 0.0f / 255.0f));
            maplePlortMaterial.SetColor(bottomColorNameId, new Color(182.0f / 255.0f, 86.0f / 255.0f, 33.0f / 255.0f));
            maplePlortObject.GetComponentInChildren<MeshRenderer>().material = maplePlortMaterial;
            LookupRegistry.RegisterIdentifiablePrefab(maplePlortObject);
            Sprite maplePlortSprite = IMG2Sprite.LoadNewSprite("SDTestModNew.MaplePlort.png");
            LookupRegistry.RegisterVacEntry(ModdedIds.MAPLE_PLORT, new Color(182.0f / 255.0f, 86.0f / 255.0f, 33.0f / 255.0f), maplePlortSprite);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, maplePlortObject);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, mapleGameObject);
            PediaRegistry.RegisterIdEntry(ModdedIds.MAPLE_SLIMES, mapleSlimeSprite);
            PlortRegistry.AddEconomyEntry(ModdedIds.MAPLE_PLORT, 35f, 50f);
            DroneRegistry.RegisterBasicTarget(ModdedIds.MAPLE_PLORT);

            Mesh pogoMesh = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.POGO_FRUIT)).GetComponent<Mesh>();
            MeshRenderer[] pogoMeshRenderers = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.POGO_FRUIT)).GetComponentsInChildren<MeshRenderer>();
            IEnumerable<Material> pogoMaterials = new List<Material>();
            foreach(MeshRenderer renderer in pogoMeshRenderers)
            {
                foreach(Material material in renderer.materials)
                {
                    pogoMaterials = pogoMaterials.Append(material);
                }
            }
            SRML.Console.Console.Log("pogoMaterials.Count(): " + pogoMaterials.Count().ToString());
            SRML.Console.Console.Log("pogoMaterials.ToArray().Length: " + pogoMaterials.ToArray().Length.ToString());
            TestingUtils.DumpChildComponents(Identifiable.Id.POGO_FRUIT);
            FoodTemplate caramelAppleTemplate = new FoodTemplate("caramelApple", ModdedIds.CARAMEL_APPLE_FRUIT, ModdedIds.CARAMEL_APPLES, FoodTemplate.Type.FRUIT, pogoMesh, pogoMaterials.ToArray());
            caramelAppleTemplate = caramelAppleTemplate.SetTranslation("Caramel Apple");
            GameObject caramelAppleObject = caramelAppleTemplate.Create().ToPrefab();
            caramelAppleObject.GetComponent<Identifiable>().id = ModdedIds.CARAMEL_APPLE_FRUIT;
            caramelAppleObject.GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;
            caramelAppleObject.name = "caramelApple";
            LookupRegistry.RegisterIdentifiablePrefab(caramelAppleObject);
            Sprite caramelAppleSprite = IMG2Sprite.LoadNewSprite("SDTestModNew.CaramelApple.png");
            LookupRegistry.RegisterVacEntry(ModdedIds.CARAMEL_APPLE_FRUIT, Color.green, caramelAppleSprite);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, caramelAppleObject);
            PediaRegistry.RegisterIdEntry(ModdedIds.CARAMEL_APPLES, caramelAppleSprite);
            DroneRegistry.RegisterBasicTarget(ModdedIds.CARAMEL_APPLE_FRUIT);
            TestingUtils.DumpChildComponents(ModdedIds.CARAMEL_APPLE_FRUIT);

            CrateTemplate testCrateTemplate = new CrateTemplate("testCrate", ModdedIds.TEST_CRATE_01);
            testCrateTemplate = testCrateTemplate.SetSpawnInfo(3, 5);
            List<BreakOnImpact.SpawnOption> testCrateSpawnOptions = new List<BreakOnImpact.SpawnOption>();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.MANGO_FRUIT, 1.5f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.POGO_FRUIT, 3.0f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.CARROT_VEGGIE, 3.0f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.OCAOCA_VEGGIE, 1.5f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.PEAR_FRUIT, 0.25f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.PARSNIP_VEGGIE, 0.25f)).ToList();
            testCrateSpawnOptions = testCrateSpawnOptions.Append(SpawnOptionByID(Identifiable.Id.GINGER_VEGGIE, 0.0001f)).ToList();
            testCrateTemplate.SetSpawnOptions(testCrateSpawnOptions);
            GameObject testCrateObject = testCrateTemplate.Create().ToPrefab();
            LookupRegistry.RegisterIdentifiablePrefab(testCrateObject);
        }

        // Called after all mods Load's have been called
        // Used for editing existing assets in the game, not a registry step
        public override void PostLoad()
        {
        }

        private static int topColorNameId = Shader.PropertyToID("_TopColor");
        private static int middleColorNameId = Shader.PropertyToID("_MiddleColor");
        private static int bottomColorNameId = Shader.PropertyToID("_BottomColor");

        private BreakOnImpact.SpawnOption SpawnOptionByID(Identifiable.Id id, float weight)
        {
            BreakOnImpact.SpawnOption spawnOption = new BreakOnImpact.SpawnOption();
            spawnOption.spawn = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(id);
            spawnOption.weight = weight;
            return spawnOption;
        }

    }
}