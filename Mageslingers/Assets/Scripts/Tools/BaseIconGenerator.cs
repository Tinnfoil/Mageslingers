using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
public class BaseIconGenerator : MonoBehaviour
{
    public static BaseIconGenerator Instance
    {
        get
        {
            if (_instance)
            {
                return _instance;
            }
            else
            {
                BaseIconGenerator gen = FindObjectOfType<BaseIconGenerator>();
                if (gen)
                {
                    _instance = gen;
                }
                return gen;
            }
        }
    }

    public static BaseIconGenerator _instance;

    public Camera RenderCamera;
    public Transform CameraRig;
    public Transform DummyPosition;
    public MeshRenderer DummyRenderer;
    [Tooltip("How much space to leave in between the edge of the icon and the object.")]
    public float IconMargin;
    public string Prefix;
    public string Suffix;

    public string TargetFilePath;

    public string IconName;
    public int IconWidth = 256;
    public int IconHeight = 256;
    public int IconPixelsPerUnit = 1;
    public float ChromaKeyError;

    Vector3 originalCameraPosition;
    Quaternion originalCameraRotation;
    float originalCameraSize;

    public GameObject Target;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

    }

    public Sprite GenerateIconForPrefab(GameObject ObjectPrefab, int IconWidth, int IconHeight, float PixelsPerUnit, bool bSaveAsset = false)
    {
        Sprite IconSprite = null;
        if (ObjectPrefab)
        {
            GameObject ObjectInstance = Instantiate(ObjectPrefab, DummyPosition.position, Quaternion.identity);
            ObjectInstance.hideFlags = HideFlags.DontSave;
            ObjectInstance.transform.localPosition = Vector3.zero;
            ObjectInstance.transform.rotation = DummyPosition.rotation;
            DummyRenderer.enabled = false;
            originalCameraPosition = CameraRig.transform.position;
            originalCameraRotation = CameraRig.transform.rotation;
            if (RenderCamera.orthographic)
            {
                originalCameraSize = RenderCamera.orthographicSize;
                RenderCamera.aspect = 1f;
            }

            if (ObjectInstance)
            {
                MeshRenderer meshRend = ObjectInstance.GetComponentInChildren<MeshRenderer>();
                Material material = null;
                float originalOutline = 0f;
                float originalRimLight = 0f;
                if (meshRend)
                {
                    meshRend.transform.localRotation = Quaternion.identity;
                    material = meshRend.sharedMaterial;
                    if (material)
                    {
                        if (material.HasProperty("_OutlineWidth"))
                        {
                            originalOutline = material.GetFloat("_OutlineWidth");
                            material.SetFloat("_OutlineWidth", 0f);
                        }

                        if (material.HasProperty("_RimLigInt"))
                        {
                            originalRimLight = material.GetFloat("_RimLigInt");
                            material.SetFloat("_RimLigInt", 0f);
                        }
                    }
                    else
                    {
                        Debug.LogError("Icon generator error: object instance: " + ObjectInstance.name + " has no material.");
                        DestroyImmediate(ObjectInstance);

                        DummyRenderer.enabled = true;
                        if (CameraRig)
                        {
                            CameraRig.transform.SetPositionAndRotation(originalCameraPosition, originalCameraRotation);
                            if (RenderCamera.orthographic)
                            {
                                RenderCamera.orthographicSize = originalCameraSize;
                            }
                        }
                        return null;
                    }
                }

                BaseCameraFramer.PlaceCamera(RenderCamera, CameraRig, ObjectInstance, IconMargin);

                Texture2D NewTexture = GenerateTextureFromCamera(RenderCamera, IconWidth, IconHeight);
                IconSprite = GenerateSpriteFromTexture(NewTexture, IconWidth, IconHeight, PixelsPerUnit);
                if (bSaveAsset && IconSprite && TargetFilePath != "")
                {
                    string AssetPath = TargetFilePath + "/" + Prefix + ObjectPrefab.name + Suffix + ".png";
                    SaveSprite(AssetPath, ref IconSprite);
                }

                if (material)
                {
                    if (material.HasProperty("_OutlineWidth"))
                    {
                        material.SetFloat("_OutlineWidth", originalOutline);
                    }
                    if (material.HasProperty("_RimLigInt"))
                    {
                        material.SetFloat("_RimLigInt", originalRimLight);
                    }
                }

                DestroyImmediate(ObjectInstance);
            }
            DummyRenderer.enabled = true;
            if (CameraRig)
            {
                CameraRig.transform.SetPositionAndRotation(originalCameraPosition, originalCameraRotation);
                if (RenderCamera.orthographic)
                {
                    RenderCamera.orthographicSize = originalCameraSize;
                    RenderCamera.ResetAspect();
                }
            }
        }
        return IconSprite;
    }

    bool SaveSprite(string AssetPath, ref Sprite IconSprite)
    {
        string Dir = Path.GetDirectoryName(AssetPath);

        if (UnityEditor.VersionControl.Provider.isActive)
        {
            UnityEditor.VersionControl.Asset existingAsset = UnityEditor.VersionControl.Provider.GetAssetByPath(AssetPath);
            if (existingAsset != null)
            {
                UnityEditor.VersionControl.Task task = UnityEditor.VersionControl.Provider.Checkout(UnityEditor.VersionControl.Provider.GetAssetByPath(AssetPath), UnityEditor.VersionControl.CheckoutMode.Asset);
                task.Wait();
                if (!task.success)
                {
                    Debug.LogError("Could not check out existing icon file.");
                    IconSprite = null;
                    return false;
                }
            }
        }

        Directory.CreateDirectory(Dir);

        File.WriteAllBytes(AssetPath, IconSprite.texture.EncodeToPNG());
        AssetDatabase.Refresh();
        try
        {
            AssetDatabase.AddObjectToAsset(IconSprite, AssetPath);
        }
        catch
        {
            // lol
        }

        TextureImporter TI = AssetImporter.GetAtPath(AssetPath) as TextureImporter;
        TI.textureType = TextureImporterType.Sprite;
        TI.spritePixelsPerUnit = IconSprite.pixelsPerUnit;
        TI.mipmapEnabled = false;
        EditorUtility.SetDirty(TI);
        TI.SaveAndReimport();
        IconSprite = AssetDatabase.LoadAssetAtPath(AssetPath, typeof(Sprite)) as Sprite;
        return true;
    }

    Sprite GenerateSpriteFromTexture(Texture2D InTexture, int InWidth, int InHeight, float PixelsPerUnit)
    {
        Rect Rectangle = new Rect(0, 0, InWidth, InHeight);
        Sprite NewSprite = Sprite.Create(InTexture, Rectangle, new Vector2(InWidth / 2, InHeight / 2), PixelsPerUnit);
        return NewSprite;
    }

    Texture2D GenerateTextureFromCamera(Camera InCamera, int InWidth, int InHeight)
    {
        Rect Rectangle = new Rect(0, 0, InWidth, InHeight);
        RenderTexture RendText = new RenderTexture(InWidth, InHeight, 32);
        Texture2D Capture = new Texture2D(InWidth, InHeight, TextureFormat.RGBA32, false);

        InCamera.targetTexture = RendText;
        InCamera.Render();

        RenderTexture.active = RendText;
        Capture.ReadPixels(Rectangle, 0, 0);
        Capture.Apply();

        Texture2D Edit = new Texture2D(InWidth, InHeight, TextureFormat.RGBA32, false);
        Color ChromaKey = Capture.GetPixel(0, 0); // first pixel will always be background color
        Color LastPixelCleared = new Color();
        for (int i = 0; i < Capture.width; ++i)
        {
            for (int k = 0; k < Capture.height; ++k)
            {
                Color pixel = Capture.GetPixel(i, k);

                // if background, make pixel clear. This override is necessary because enabling post-processing in camera
                // kills the output alpha value in render texture (dumb Unity optimization)
                if (Mathf.Abs(pixel.r - ChromaKey.r) <= ChromaKeyError && Mathf.Abs(pixel.g - ChromaKey.g) <= ChromaKeyError && Mathf.Abs(pixel.b - ChromaKey.b) <= ChromaKeyError)
                {
                    LastPixelCleared = pixel;
                    Edit.SetPixel(i, k, Color.clear);
                }
                else
                {
                    Edit.SetPixel(i, k, pixel);
                }
            }
        }
        Edit.Apply();

        InCamera.targetTexture = null;
        RenderTexture.active = null;

        return Edit;
    }

    public Sprite GenerateIconFromCameraView(string IconName, int IconWidth, int IconHeight, float PixelsPerUnit, bool bSaveAsset = false)
    {
        Texture2D NewTexture = GenerateTextureFromCamera(RenderCamera, IconWidth, IconHeight);
        Sprite IconSprite = GenerateSpriteFromTexture(NewTexture, IconWidth, IconHeight, PixelsPerUnit);
        if (bSaveAsset && IconSprite && TargetFilePath != "")
        {
            string AssetPath = TargetFilePath + "/" + IconName + ".png";
            string Dir = Path.GetDirectoryName(AssetPath);

            Directory.CreateDirectory(Dir);

            File.WriteAllBytes(AssetPath, IconSprite.texture.EncodeToPNG());
            AssetDatabase.Refresh();
            AssetDatabase.AddObjectToAsset(IconSprite, AssetPath);

            TextureImporter TI = AssetImporter.GetAtPath(AssetPath) as TextureImporter;
            TI.textureType = TextureImporterType.Sprite;
            TI.spritePixelsPerUnit = IconSprite.pixelsPerUnit;
            TI.mipmapEnabled = false;
            EditorUtility.SetDirty(TI);
            TI.SaveAndReimport();
            IconSprite = AssetDatabase.LoadAssetAtPath(AssetPath, typeof(Sprite)) as Sprite;
        }
        return IconSprite;
    }
}


[CustomEditor(typeof(BaseIconGenerator))]
public class BaseIconGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BaseIconGenerator myTarget = (BaseIconGenerator)target;
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Icon"))
        {
            myTarget.GenerateIconForPrefab(myTarget.Target, myTarget.IconWidth, myTarget.IconHeight, myTarget.IconPixelsPerUnit, true);
        }

    }
}
#endif
