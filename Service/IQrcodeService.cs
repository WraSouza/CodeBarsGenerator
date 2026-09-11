namespace CodeBarsGenerator.Service
{
    public interface IQrcodeService
    {
        byte[] GerarQrCode(string codigo);
    }
}
