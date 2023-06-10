namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public enum SupportedTypes
    {
        DOCX,
        XLSX,
        PDF,
        JPEG
    }

    public static class Mime
    {
        public const string PDF = "application/pdf";
        public const string DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string JPEG = "image/jpeg";
    }

    public static class SupportedTypesExtension
    {
        public static string ToMime(this SupportedTypes type)
            => type switch
            {
                SupportedTypes.DOCX => Mime.DOCX,
                SupportedTypes.XLSX => Mime.XLSX,
                SupportedTypes.PDF => Mime.PDF,
                SupportedTypes.JPEG => Mime.JPEG,
                _ => throw new NotImplementedException($"Unknown file type")
            };

        public static string ToFileExtension(this SupportedTypes type) => type.ToString().ToLower();
    }
}
