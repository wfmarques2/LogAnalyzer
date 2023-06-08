using System;
using System.IO;
using System.Globalization;

public class Program
{
    public static void Main(string[] args)
    {
        // Define o caminho do arquivo de log
        Console.WriteLine("Digite o caminho do arquivo de log: ");
        string path = Console.ReadLine() ?? "";

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            // Abre o arquivo de log
            StreamReader reader = File.OpenText(path);

            // Inicializa as variáveis
            int totalEntries = 0;
            int totalErrors = 0;
            DateTime startTime = DateTime.MaxValue;
            DateTime endTime = DateTime.MinValue;

            // Lê o arquivo de log linha por linha
            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                // Divide a linha em data/hora e mensagem
                string[] parts = line.Split(' ');

                // Verifica se a linha possui a quantidade de partes correta
                if (parts.Length < 3)
                {
                    Console.WriteLine("Arquivo com formatação inválida!");
                    return;
                }

                string dateAndTimeString = parts[0] + " " + parts[1];
                string message = parts[2];

                // Converte a data/hora para o formato adequado
                DateTime dateTime;
                if (!DateTime.TryParseExact(dateAndTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine("Arquivo com formatação inválida!");
                    return;
                }

                // Verifica se a linha contém um erro
                if (message.Contains("ERRO"))
                {
                    totalErrors++;
                }

                // Atualiza a data/hora inicial e final
                if (dateTime < startTime)
                {
                    startTime = dateTime;
                }
                if (dateTime > endTime)
                {
                    endTime = dateTime;
                }

                // Incrementa o contador de entradas
                totalEntries++;
            }

            // Calcula o tempo decorrido
            TimeSpan elapsedTime = endTime - startTime;

            // Extrai as horas, minutos e segundos
            int hours = elapsedTime.Hours;
            int minutes = elapsedTime.Minutes;
            int seconds = elapsedTime.Seconds;

            // Exibe os resultados
            Console.WriteLine("Número total de entradas de log: {0}", totalEntries);
            Console.WriteLine("Número total de erros: {0}", totalErrors);
            Console.WriteLine("Tempo total decorrido: {0} hora(s), {1} minuto(s) e {2} segundo(s)",
                hours, minutes, seconds);

            reader.Close();
        }
        else
        {
            Console.WriteLine("Caminho inválido.");
        }
    }
}
