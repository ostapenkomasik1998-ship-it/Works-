namespace QrScanner.Services;

public interface ILinkOpener
{
    /// <summary>Відкриває текст як веб-посилання у браузері. false — якщо це не посилання або сталася помилка.</summary>
    Task<bool> OpenAsync(string value);
}
