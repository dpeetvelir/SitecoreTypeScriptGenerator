﻿
using SitecoreTypeScriptGenerator.Models.TypeScript;

namespace SitecoreTypeScriptGenerator.Processor
{
    internal class ProcessorUtils
    {
        public const string BaseTemplatesId = "12c33f3f-86c5-43a5-aeb4-5598cec45116";

        public static string RootGenerationPath { get; set; }
        public static DirectoryInfo GetRootGenerationDirectory()
        {
            if(string.IsNullOrWhiteSpace(RootGenerationPath))
            {
                throw new ArgumentNullException(nameof(RootGenerationPath));
            }

            var directory = new DirectoryInfo(RootGenerationPath);
            if (!directory.Exists)
            {
                throw new Exception($"output directory root does not exist: {RootGenerationPath}");
            }
            return directory;
        }

        public static bool DoesItemHaveBaseTemplates(Item item)
        {
            return item?.SharedFields?.Any(x => x?.ID == BaseTemplatesId) ?? false;
        }

        public static List<Guid>? GetBaseTemplateIds(Item item)
        {
            var baseTempaltes = item?.SharedFields?.FirstOrDefault(x => x?.ID == BaseTemplatesId);
            if (baseTempaltes?.Value == null)
            {
                return null;
            }

            List<Guid> ids = new List<Guid>();
            var tokens = baseTempaltes.Value.Split(["|", Environment.NewLine, "\n"], StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                ids.Add(Guid.Parse(token.Trim()));
            }
            return ids;
        }

        public static string GetTypeScriptFieldTypeForImport(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.SingleLineText:
                case FieldType.MultiLineText:
                case FieldType.RichText:
                    return "Field";
                case FieldType.GeneralLink:
                    return "LinkField";
                case FieldType.Image:
                    return "ImageField";
                case FieldType.ItemReference:
                    return ConfigLoader.GetBaseItemClassName();
                case FieldType.Checkbox:
                    return "Field";
                case FieldType.Integer:
                case FieldType.Number:
                    return "Field";
                case FieldType.Date:
                    return "DateField";
                case FieldType.ItemReferenceArray:
                    return $"{ConfigLoader.GetBaseItemClassName()}[]";
                default:
                    // "Unknown";
                    return string.Empty;
            }
        }

        public static string GetTypeScriptFieldType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.SingleLineText:
                case FieldType.MultiLineText:
                case FieldType.RichText:
                    return "Field<string>";
                case FieldType.GeneralLink:
                    return "LinkField";
                case FieldType.Image:
                    return "ImageField";
                case FieldType.ItemReference:
                    return ConfigLoader.GetBaseItemClassName();
                case FieldType.Checkbox:
                    return "Field<boolean>";
                case FieldType.Integer:
                case FieldType.Number:
                    return "Field<number>";
                case FieldType.Date:
                    return "DateField";
                case FieldType.ItemReferenceArray:
                    return $"{ConfigLoader.GetBaseItemClassName()}[]";
                default:
                    // "Unknown";
                    return string.Empty;
            }
        }
    }
}
