using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Clase para un nodo de la lista doblemente enlazada
    class NodoNota
    {
        public NotaMusical Nota { get; set; }
        public NodoNota Anterior { get; set; }
        public NodoNota Siguiente { get; set; }

        public NodoNota(NotaMusical nota)
        {
            Nota = nota;
            Anterior = null;
            Siguiente = null;
        }
    }

    // Clase para la lista doblemente enlazada
    class ListaDoble
    {
        public NodoNota Cabeza { get; private set; }
        public NodoNota Cola { get; private set; }

        public void Agregar(NotaMusical nota)
        {
            NodoNota nuevoNodo = new NodoNota(nota);
            if (Cabeza == null)
            {
                Cabeza = Cola = nuevoNodo;
            }
            else
            {
                Cola.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = Cola;
                Cola = nuevoNodo;
            }
        }

        public IEnumerable<NotaMusical> ReproducirNormal()
        {
            NodoNota actual = Cabeza;
            while (actual != null)
            {
                yield return actual.Nota;
                actual = actual.Siguiente;
            }
        }

        public IEnumerable<NotaMusical> ReproducirReverso()
        {
            NodoNota actual = Cola;
            while (actual != null)
            {
                yield return actual.Nota;
                actual = actual.Anterior;
            }
        }
    }

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

        ListaDoble listaNotas = new ListaDoble();

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

                listaNotas.Agregar(new NotaMusical(nota, figura, duracionesRelativas[figura]));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        double duracionNegra;
        while (true)
        {
            Console.Write("Ingrese la duración en segundos de una negra (entre 0.1 y 5.0): ");
            if (double.TryParse(Console.ReadLine(), out duracionNegra) && duracionNegra >= 0.1 && duracionNegra <= 5.0)
                break;

            Console.WriteLine("Duración inválida. Debe ser un valor numérico entre 0.1 y 5.0.");
        }

        foreach (var nota in listaNotas.ReproducirNormal())
        {
            nota.Duracion *= duracionNegra;
        }

        string rutaCarpetaSonidos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notas");

        Console.WriteLine("Reproduciendo las notas en orden normal...");
        foreach (var nota in listaNotas.ReproducirNormal())
        {
            ReproducirNota(rutaCarpetaSonidos, nota).Wait();
        }

        Console.WriteLine("Reproduciendo las notas en orden inverso...");
        foreach (var nota in listaNotas.ReproducirReverso())
        {
            ReproducirNota(rutaCarpetaSonidos, nota).Wait();
        }

        Console.WriteLine("Reproducción finalizada.");
    }

    static async Task ReproducirNota(string rutaCarpetaSonidos, NotaMusical nota)
    {
        string rutaArchivo = Path.Combine(rutaCarpetaSonidos, $"{nota.Nota}.wav");

        if (!File.Exists(rutaArchivo))
        {
            Console.WriteLine($"No se encontró el archivo de sonido: {rutaArchivo}");
            return;
        }

        using (SoundPlayer player = new SoundPlayer(rutaArchivo))
        using (CancellationTokenSource cts = new CancellationTokenSource())
        {
            var token = cts.Token;

            var reproduccionTask = Task.Run(() =>
            {
                try
                {
                    player.PlayLooping(); // Inicia la reproducción en bucle
                }
                catch
                {
                    cts.Cancel();
                }
            }, token);

            try
            {
                await Task.Delay((int)(nota.Duracion * 1000), token); // Espera la duración correspondiente
                cts.Cancel(); // Cancela la reproducción
                player.Stop(); // Detiene el sonido
            }
            catch (TaskCanceledException)
            {
                // Manejo de cancelación (opcional)
            }
        }
    }
}
