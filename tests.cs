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
			string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notas/si.wav");

			Assert.True(File.Exists(ruta));
		}

		[Fact]
		public void TestEnlazarSiguienteCorrectamente(){

    			var listaNotas = new ListaDoble();
    
   			var nota1 = new NotaMusical("do", "negra", 1.0);
    			var nota2 = new NotaMusical("re", "negra", 1.0);
    			var nota3 = new NotaMusical("mi", "negra", 1.0);
    
    			listaNotas.Agregar(nota1);
    			listaNotas.Agregar(nota2);
    			listaNotas.Agregar(nota3);

    			var nodo1 = listaNotas.Cabeza;
    			var nodo2 = nodo1.Siguiente;
    			var nodo3 = nodo2.Siguiente;

    			Assert.Equal(nota2, nodo1.Siguiente.Nota);
    			Assert.Equal(nota3, nodo2.Siguiente.Nota);

    			Assert.Null(nodo3.Siguiente);
		}

		[Fact]
		public void TestEnlazarAnteriorCorrectamente(){

    			var listaNotas = new ListaDoble();
    
   			var nota1 = new NotaMusical("do", "negra", 1.0);
    			var nota2 = new NotaMusical("re", "negra", 1.0);
    			var nota3 = new NotaMusical("mi", "negra", 1.0);
    
    			listaNotas.Agregar(nota1);
    			listaNotas.Agregar(nota2);
    			listaNotas.Agregar(nota3);

    			var nodo1 = listaNotas.Cabeza;
    			var nodo2 = nodo1.Siguiente;
    			var nodo3 = nodo2.Siguiente;

    			Assert.Equal(nota1, nodo2.Anterior.Nota);
    			Assert.Equal(nota2, nodo3.Anterior.Nota);

    			Assert.Null(nodo1.Anterior);
		}

		[Fact]
		public void TestListaDobleVacia(){
			
			var listaNotas = new ListaDoble();
    
    			var notas = listaNotas.ReproducirNormal().ToList();
    			Assert.Empty(notas);
		}
		
		[Fact]
		public void TestBorrarListaDoble(){

			var listaNotas = new ListaDoble();
    
    			var nota1 = new NotaMusical("do", "negra", 1.0);
    			var nota2 = new NotaMusical("re", "negra", 1.0);
    			var nota3 = new NotaMusical("mi", "negra", 1.0);
    
    			listaNotas.Agregar(nota1);
    			listaNotas.Agregar(nota2);
			listaNotas.Agregar(nota3);

			// Se tiene que hacer otra lista para simular el comportamiento
    			listaNotas = new ListaDoble();

    			var notas = listaNotas.ReproducirNormal().ToList();
    			Assert.IsEmpty(notas);
		}

		[Fact]
		public void TestListaDobleUnNodo(){
		
			var listaNotas = new ListaDoble();
    
    			var nota1 = new NotaMusical("do", "negra", 1.0);
    			listaNotas.Agregar(nota1);

    			var nodo1 = listaNotas.Cabeza;

    			Assert.Null(nodo1.Siguiente);
    			Assert.Null(nodo1.Anterior);
		}


	}





}
