namespace CodeBarsGenerator.Service
{
    public interface IBarcodeService
    {
        byte[] GerarCodigoBarras(string codigo);
        byte[] GerarCodigoBarrasPuro(string codigo);
        byte[] GerarCodigoBarrasVertical(string codigo,int anguloRotacao = 90);
        byte[] GerarCodigoBarrasGs1(string codigoComParenteses);
    }
}
