using System.Text;
using System.Text.RegularExpressions;

namespace CodeBarsGenerator.Helper
{
   
     public static class Gs1Converter
    {
        // AIs de tamanho FIXO (não precisam de separador depois).
        // Lista não exaustiva — adicione outros conforme forem usados.
        private static readonly Dictionary<string, int> AisTamanhoFixo = new()
        {
            { "00", 18 }, // SSCC
            { "01", 14 }, // GTIN
            { "02", 14 }, // GTIN de itens contidos
            { "11", 6 },  // Data de produção
            { "12", 6 },  // Data de vencimento (cartão)
            { "13", 6 },  // Data de empacotamento
            { "15", 6 },  // Data de validade mínima
            { "16", 6 },  // Data de venda
            { "17", 6 },  // Data de validade
            { "20", 2 },  // Variante do produto
        };

        /// <summary>
        /// Converte "(01)0789...(17)200727(30)0024(10)5A1234" para o formato
        /// que o ZXing espera: FNC1 + dados, com GS (\u001D) separando campos
        /// variáveis que não estão no final da string.
        /// </summary>
        public static string ConverterParaGs1(string codigoComParenteses)
        {
            var matches = Regex.Matches(codigoComParenteses, @"\((\d{2,4})\)([^\(]+)");

            if (matches.Count == 0)
                throw new ArgumentException("Código GS1 inválido: nenhum AI encontrado.");

            var sb = new StringBuilder();
            sb.Append('\u00F1'); // FNC1 inicial (ZXing.Net)

            for (int i = 0; i < matches.Count; i++)
            {
                var ai = matches[i].Groups[1].Value;
                var valor = matches[i].Groups[2].Value;
                bool ehUltimo = i == matches.Count - 1;

                sb.Append(ai).Append(valor);

                bool tamanhoFixo = AisTamanhoFixo.ContainsKey(ai);

                if (!tamanhoFixo && !ehUltimo)
                    sb.Append('\u001D'); // GS - separador de campo
            }

            return sb.ToString();
        }
    }   
    
}