using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FileManager : MonoBehaviour
{
    static Dictionary<string, GameObject> prefabCache;
    public static GameObject LoadPrefab(string path) {
        GameObject output = null;
        if(prefabCache == null)
        {
            prefabCache = new Dictionary<string, GameObject>();
        }
        if (prefabCache.ContainsKey(path)) {

            if (prefabCache[path] == null)
            {
                prefabCache.Remove(path);
            }
            else
            {
                output = prefabCache[path];
            }
        }

        if (output == null)
        {
            output = Resources.Load<GameObject>("prefab/" + path);
            if (output != null)
            {
                prefabCache.Add(path, output);
            }
        }
        return output;
    }

    static Dictionary<string, Texture2D> textureCache;
    public static Texture2D LoadTexture(string path)
    {
        Texture2D output = null;
        if (textureCache == null)
        {
            textureCache = new Dictionary<string, Texture2D>();
        }
        if (textureCache.ContainsKey(path))
        {

            if (textureCache[path] == null)
            {
                textureCache.Remove(path);
            }
            else
            {
                output = textureCache[path];
            }
        }

        if (output == null)
        {
            output = Resources.Load<Texture2D>("Texture/" + path);
            if (output != null)
            {
                textureCache.Add(path, output);
            }
        }
        return output;
    }

    static Dictionary<string, SpriteAtlas> atlasCache;
    public static SpriteAtlas LoadSpriteAtlas(string path)
    {
        SpriteAtlas output = null;
        if (atlasCache == null)
        {
            atlasCache = new Dictionary<string, SpriteAtlas>();
        }
        if (atlasCache.ContainsKey(path))
        {

            if (atlasCache[path] == null)
            {
                atlasCache.Remove(path);
            }
            else
            {
                output = atlasCache[path];
            }
        }

        if (output == null)
        {
            output = Resources.Load<SpriteAtlas>("spriteAtlas/" + path);
            if (output != null)
            {
                atlasCache.Add(path, output);
            }
        }
        return output;
    }

    //該快取由spriteCachManager 負責
    public static Sprite LoadSprite(string spriteAtlas_name, string img_name)
    {
        return spriteCachManager.GetSprite(spriteAtlas_name, img_name) ;
    }

    static Dictionary<string, AudioClip> audioCache;
    public static AudioClip LoadAudio(string type, string audio_name)
    {
        AudioClip output = null;
        string path = string.Format("audio/{0}/{1}", type, audio_name);
        if (audioCache == null)
        {
            audioCache = new Dictionary<string, AudioClip>();
        }
        if (audioCache.ContainsKey(path))
        {

            if (audioCache[path] == null)
            {
                audioCache.Remove(path);
            }
            else
            {
                output = audioCache[path];
            }
        }

        if (output == null)
        {
            output = Resources.Load<AudioClip>(path);
            if (output != null)
            {
                audioCache.Add(path, output);
            }
        }

        return output;
    }
}
