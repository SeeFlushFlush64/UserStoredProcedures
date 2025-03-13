namespace PalaganasTechnicalExam.Helpers
{
    public static class GoogleDriveHelper
    {
        public static string ConvertGoogleDriveLink(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return string.Empty;


            var match = System.Text.RegularExpressions.Regex.Match(originalUrl, @"drive\.google\.com\/file\/d\/([a-zA-Z0-9_-]+)\/");

            if (match.Success && match.Groups.Count > 1)
            {
                string fileId = match.Groups[1].Value;
                return $"https://drive.google.com/thumbnail?id={fileId}";
            }


            return originalUrl;
        }
    }
}
