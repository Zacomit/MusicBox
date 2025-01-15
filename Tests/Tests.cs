using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicBox;
using Xunit;

namespace MusicBox.Tests{

	public class MusicTests{

		[Fact]
		public void TestNotaMusicalCreacion(){
			
    			string nota = "do";
 			string figura = "negra";
    			double duracion = 1.0;

    			var notaMusical = new NotaMusical(nota, figura, duracion);
    
    			Assert.Equal(nota, notaMusical.Nota);
    			Assert.Equal(figura, notaMusical.Figura);
    			Assert.Equal(duracion, notaMusical.Duracion);
		}

		[Fact]
		public void TestListaDobleAgregarNota(){
			
    			var lista = new ListaDoble();
    			var nota = new NotaMusical("si", "blanca", 2.0);

    			lista.Agregar(nota);

    			Assert.NotNull(lista.Cabeza);
    			Assert.Equal(nota, lista.Cabeza.Nota);
		}

		[Fact]
		public void TestListaDobleReproducirNormal(){

    			var lista = new ListaDoble();
    			lista.Agregar(new NotaMusical("do", "negra", 1.0));
    			lista.Agregar(new NotaMusical("re", "blanca", 2.0));
    			lista.Agregar(new NotaMusical("mi", "corchea", 0.5));

    			var notas = lista.ReproducirNormal().ToList();

    			Assert.Equal(3, notas.Count);
    			Assert.Equal("do", notas[0].Nota);
    			Assert.Equal("re", notas[1].Nota);
    			Assert.Equal("mi", notas[2].Nota);
		}

		[Fact]
		public void TestListaDobleReproducirReverso(){

    			var lista = new ListaDoble();
    			lista.Agregar(new NotaMusical("do", "negra", 1.0));
    			lista.Agregar(new NotaMusical("la", "semicorchea", 0.25));
    			lista.Agregar(new NotaMusical("fa", "redonda", 4));

    			var notas = lista.ReproducirReverso().ToList();

    			Assert.Equal(3, notas.Count);
    			Assert.Equal("fa", notas[0].Nota);
    			Assert.Equal("la", notas[1].Nota);
    			Assert.Equal("do", notas[2].Nota);
		}

		[Fact]
		public void TestArchivoNotaExiste(){
		
			var nota = new NotaMusical("si", "corchea", 0.5);
			string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notas/si");

			Assert.True(File.Exists(ruta));
		}




		



	}





}
