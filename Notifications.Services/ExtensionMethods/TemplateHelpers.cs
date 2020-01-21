namespace Notifications.Services.ExtensionMethods
{
    public static class TemplateHelpers
    {
        public static string ReplaceTemplateField(this string templateBody, string templateFieldName,
            string replacementValue)
        {
            return templateBody.Replace($"{{{templateFieldName}}}", replacementValue);
        }
    }
}