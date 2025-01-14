using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

class Program
{
    // Clase para almacenar la información de una nota
    class NotaMusical
    {
        public string Nota { get; set; }
        public string Figura { get; set; }
        public double Duracion { get; set; }

        public NotaMusical(string nota, string figura, double duracion)
        {
            Nota = nota;
            Figura = figura;
            Duracion = duracion;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Ingrese notas en el formato (Nota, Figura). Escriba 'fin' para terminar.");
        Console.WriteLine("Figuras válidas: redonda, blanca, negra, corchea, semicorchea");
        Console.WriteLine("Notas válidas: do, re, mi, fa, sol, la, si");

        List<NotaMusical> listaNotas = new List<NotaMusical>();

        // Mapeo de duraciones relativas
        Dictionary<string, double> duracionesRelativas = new Dictionary<string, double>
        {
            { "redonda", 4 },
            { "blanca", 2 },
            { "negra", 1 },
            { "corchea", 0.5 },
            { "semicorchea", 0.25 }
        };

        string input;
        while (true)
        {
            Console.Write("Ingrese una nota y figura: ");
            input = Console.ReadLine()?.Trim();

            if (input?.ToLower() == "fin")
                break;

            try
            {
                string[] partes = input.Split(',');
                if (partes.Length != 2)
                    throw new Exception("El formato debe ser (Nota, Figura).");

                string nota = partes[0].Trim().ToLower();
                string figura = partes[1].Trim().ToLower();

                if (!duracionesRelativas.ContainsKey(figura))
                    throw new Exception($"Figura inválida: {figura}");

                if (!new[] { "do", "re", "mi", "fa", "sol", "la", "si" }.Contains(nota))
                    throw new Exception($"Nota inválida: {nota}");

                // Duración relativa inicial (1 segundo por negra)
                listaNotas.Add(new NotaMusical(nota, figura, duracionesRelativas[figura]));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Pedir duración principal
        double duracionNegra;
        while (true)
        {
            Console.Write("Ingrese la duración en segundos de una negra (entre 0.1 y 5.0): ");
            if (double.TryParse(Console.ReadLine(), out duracionNegra) && duracionNegra >= 0.1 && duracionNegra <= 5.0)
                break;

            Console.WriteLine("Duración inválida. Debe ser un valor numérico entre 0.1 y 5.0.");
        }

        // Ajustar las duraciones reales según la negra
        foreach (var nota in listaNotas)
        {
            nota.Duracion *= duracionNegra;
        }

        // Ruta de la carpeta de sonidos
        string rutaCarpetaSonidos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notas");

        // Reproducción de las notas
        Console.WriteLine("Reproduciendo las notas...");

        foreach (var nota in listaNotas)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaCarpetaSonidos, $"{nota.Nota}.wav");

                if (!File.Exists(rutaArchivo))
                    throw new FileNotFoundException($"No se encontró el archivo de sonido: {rutaArchivo}");

                using (SoundPlayer player = new SoundPlayer(rutaArchivo))
                {
                    Console.WriteLine($"Reproduciendo {nota.Nota} ({nota.Figura}) por {nota.Duracion:F2} segundos...");
                    player.PlaySync(); // Reproduce la nota
                }

                // Pausa según la duración de la nota
                System.Threading.Thread.Sleep((int)(nota.Duracion * 1000));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reproducir la nota {nota.Nota}: {ex.Message}");
            }
        }

        Console.WriteLine("Reproducción finalizada.");
    }
}

