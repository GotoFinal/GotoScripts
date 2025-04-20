using UnityEditor;

namespace GotoFinal.CrunchBegone.Editor
{
    public class NoCrunch : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer && importer.crunchedCompression)
            {
                importer.crunchedCompression = false;
            }
        }
    }
}