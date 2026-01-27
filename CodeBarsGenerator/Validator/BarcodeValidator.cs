using System.Text.RegularExpressions;

namespace CodeBarsGenerator.Validator
{
    public static class BarcodeValidator
    {
        public static void Validar(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código vazio.");

            if (!Regex.IsMatch(codigo, @"^[a-zA-Z0-9]+$"))
                throw new ArgumentException("Código inválido.");
        }
    }
}
