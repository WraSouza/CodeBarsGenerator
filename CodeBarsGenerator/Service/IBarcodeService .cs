namespace CodeBarsGenerator.Service
{
    public interface IBarcodeService
    {
        byte[] GerarCodigoBarras(string codigo);
    }
}
