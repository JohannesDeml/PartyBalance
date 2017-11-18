using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;

class KenneySpriteSheetBuilder : AssetPostprocessor
{

    #region Customization

    //
    // If you want to only process kenney spritesheets in a specific folder you can do it by setting 
    // this string to the name of the folder relative to your assets directory.
    //
    const string m_RequiredRootFolder = null;

    #endregion

    #region Internals

    // This is a brilliant method by unity form member numberkruncher
    // Here's the post that provided the guts of this method, not taken quite verbatum:
    // http://forum.unity3d.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/
    public static bool GetImageSize(TextureImporter importer, out int width, out int height)
    {
        if (importer != null)
        {
            object[] args = new object[2] { 0, 0 };
            MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(importer, args);

            width = (int)args[0];
            height = (int)args[1];

            return true;
        }

        height = width = 0;
        return false;
    }

    private static void HandleSpritesheet(TextureImporter importer, XmlDocument document, ref int totalSprites)
    {
        // don't over-write sprite sheets that have already been setup (either by hand, or previously).
        if (importer.spriteImportMode != SpriteImportMode.Multiple)
        {
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            importer.spriteImportMode = SpriteImportMode.Multiple;
            List<SpriteMetaData> sprites = new List<SpriteMetaData>();

            int textureWidth = 0,
                textureHeight = 0;
            if (!GetImageSize(importer, out textureWidth, out textureHeight))
            {
                Debug.LogWarning("Couldn't determine the texture dimensions for this asset. Asset path is\"" + importer.assetPath + "\"");
            }

            foreach (XmlNode xmlNode in document.DocumentElement.ChildNodes)
            {
                string childName = xmlNode.Name;
                if (childName == "SubTexture")
                {
                    string
                        name = null,
                        x = null,
                        y = null,
                        width = null,
                        height = null;

                    int _x = 0,
                        _y = 0,
                        _width = 0,
                        _height = 0;
                    foreach (XmlAttribute attr in xmlNode.Attributes)
                    {
                        switch (attr.Name)
                        {
                            case "name":
                                name = attr.Value;
                                break;
                            case "x":
                                x = attr.Value;
                                break;
                            case "y":
                                y = attr.Value;
                                break;
                            case "width":
                                width = attr.Value;
                                break;
                            case "height":
                                height = attr.Value;
                                break;
                            // It seems like we can ignore these attributes coming from the shoebox exporter.
                            // If they need to be considered in the future we can add support here.
                            case "frameX":
                            case "frameY":
                            case "frameWidth":
                            case "frameHeight":
                                break;
                            default:
                                Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture attr that was unexpected... It was \"" + attr.Name + "\"");
                                break;
                        }
                    }

                    if (name == null)
                    {
                        Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture without a name.");
                        continue;
                    }
                    if (x == null || !int.TryParse(x, out _x))
                    {
                        Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture without an x position. The name was \"" + name + "\"");
                        continue;
                    }
                    if (y == null || !int.TryParse(y, out _y))
                    {
                        Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture without a y position. The name was \"" + name + "\"");
                        continue;
                    }
                    if (width == null || !int.TryParse(width, out _width))
                    {
                        Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture without a width. The name was \"" + name + "\"");
                        continue;
                    }
                    if (height == null || !int.TryParse(height, out _height))
                    {
                        Debug.LogWarning("[Kenney] While loading a spritesheet, we found a subtexture without a height. The name was \"" + name + "\"");
                        continue;
                    }

                    SpriteMetaData meta = new SpriteMetaData();
                    meta.name = name;
                    meta.pivot = pivot;
                    // note the required modification to the y position to work with unity sprite co-ords
                    meta.rect = new Rect(_x, textureHeight - _y - _height, _width, _height);
                    sprites.Add(meta);
                }
            }
            if (sprites.Count > 0)
            {
                totalSprites += sprites.Count;
                importer.spritesheet = sprites.ToArray();
                // refresh the sprite sheet so it can apply our changes.
                AssetDatabase.ImportAsset(importer.assetPath);
            }
        }
    }

    private static void HandleAsset(string asset, ref int totalSprites)
    {
        if (!string.IsNullOrEmpty(m_RequiredRootFolder) && !asset.Contains("Assets/" + m_RequiredRootFolder))
        {
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(asset);

        if (importer != null)
        {
            if(!(importer is TextureImporter))
            {
                return;
            }
            if (((TextureImporter)importer).textureType == TextureImporterType.Sprite)
            {
                string imagePath = asset;
                string xmlPath = imagePath.Substring(0, imagePath.LastIndexOf('.')) + ".xml";
                if (File.Exists(xmlPath))
                {
                    //document
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlPath);
                    //TODO: validate document.
                    HandleSpritesheet((TextureImporter)importer, document, ref totalSprites);
                }
            }
        }
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPath)
    {
        int totalSprites = 0;
        foreach (var asset in importedAssets)
        {
            HandleAsset(asset, ref totalSprites);
        }
        foreach (var asset in movedAssets)
        {
            HandleAsset(asset, ref totalSprites);
        }
        foreach (var asset in movedFromAssetPath)
        {
            HandleAsset(asset, ref totalSprites);
        }
        if (totalSprites > 0)
        {
            Debug.Log("[Kenney] Successfully added " + totalSprites + " sprites!");
        }
    }
#endregion
}

