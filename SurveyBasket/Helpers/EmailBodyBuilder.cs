namespace SurveyBasket.Helpers;

public static class EmailBodyBuilder
{
    /// TODO: Create The function that generate the email body
    /// and take two parameters, one for template name and one 
    /// as dict for replacing the placeholder with the value.
    
    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {
        // TODO: init var for the path of html template
        var templatePath = $"{Directory.GetCurrentDirectory()}/Template/{template}.html";

        // TODO: read the template using stream reader and close stream
        var stream = new StreamReader(templatePath);
        var body = stream.ReadToEnd();
        stream.Close();

        // TODO: replace each placeholder in the template with the corresponding value from the templateModel
        foreach (var item in templateModel)
            body.Replace(item.Key, item.Value);

        return body;
    }

}
