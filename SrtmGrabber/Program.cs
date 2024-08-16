using System.IO.Compression;

const string srtmDownloadBasicUrl = "https://srtm.csi.cgiar.org/wp-content/uploads/files/srtm_5x5/TIFF/";
const string pathToSaveDawnloadedFiles = "C:\\Users\\karuba.timofei\\Desktop\\SRTM";

var httpClient = new HttpClient
{
    BaseAddress = new Uri(srtmDownloadBasicUrl)
};

for (int longitudeIndex = 22; longitudeIndex <= 72; longitudeIndex++)
{
    for (int latitudeIndex = 1; latitudeIndex <= 24; latitudeIndex++)
    {
        Console.WriteLine($"({longitudeIndex} {latitudeIndex}): {(longitudeIndex - 1) * 24 + latitudeIndex} from {72 * 24}");

        var srtmFileName = String.Format("srtm_{0}_{1}.zip",
            longitudeIndex < 10 ? $"0{longitudeIndex}" : longitudeIndex,
            latitudeIndex < 10 ? $"0{latitudeIndex}" : latitudeIndex);

        using var request = new HttpRequestMessage(HttpMethod.Head, srtmFileName);
        using var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            using var httpStream = await httpClient.GetStreamAsync(srtmFileName);

            var srtmFilePath = Path.Combine(pathToSaveDawnloadedFiles, srtmFileName[..^4]);
            ZipFile.ExtractToDirectory(httpStream, pathToSaveDawnloadedFiles, overwriteFiles: true);
        }
    }
}