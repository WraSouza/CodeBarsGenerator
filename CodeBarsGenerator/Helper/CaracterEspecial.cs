using System.Text.RegularExpressions;

namespace CodeBarsGenerator.Helper
{
    public static class CaracterEspecial
    {
        public static bool ContemCaracterEspecial(string codigo)
        {
            return Regex.IsMatch(codigo, @"[^a-zA-Z0-9]");
        }
    }
}
