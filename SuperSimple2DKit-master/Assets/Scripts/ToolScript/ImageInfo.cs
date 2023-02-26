using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ImageInfo : MonoBehaviour
{
    [MenuItem("Tools/Print Image Info")]
    public static void PrintImageInfo()
    {
        string artFolder = "Assets/Art";

        StringBuilder sb = new StringBuilder();

        string[] folderPaths = Directory.GetDirectories(artFolder, "*", SearchOption.AllDirectories);

        foreach (string folderPath in folderPaths)
        {
            string folderName = new DirectoryInfo(folderPath).Name;
            sb.AppendLine($"Folder: {folderName}");

            string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string imagePath in imageFiles)
            {
                if (IsSupportedImage(imagePath))
                {
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
                    TextureImporter importer = AssetImporter.GetAtPath(imagePath) as TextureImporter;

                    if (importer != null && importer.textureType == TextureImporterType.Sprite)
                    {
                        sb.Append(string.Format("Name: {0}, Size: {1} x {2}, Scale: {3}, File size: {4:F2} MB\n", 
                            texture.name, texture.width, texture.height, importer.spritePixelsPerUnit, GetFileSizeInMB(imagePath)));
                    }
                }
            }

            sb.AppendLine();
        }

        string outputPath = Path.Combine(Application.dataPath, "ImageInfo.txt");

        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.Write(sb.ToString());
        }

        Debug.Log("Image info saved to " + outputPath);
    }

    private static bool IsSupportedImage(string imagePath)
    {
        string extension = Path.GetExtension(imagePath).ToLower();
        return extension == ".psd" || extension == ".psb" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".svg";
    }

    private static float GetFileSizeInGB(string imagePath)
    {
        return new FileInfo(imagePath).Length / 1024f / 1024f / 1024f;
    }
    private static float GetFileSizeInMB(string imagePath)
{
    return new FileInfo(imagePath).Length / 1024f / 1024f;
}

}
