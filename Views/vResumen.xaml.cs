namespace ayepezExamen.Views;

public partial class vResumen : ContentPage
{
	public vResumen(string usuario, string nombre, string apellido, int edad, DateTime fechaInicio, string plan, bool casillero, double montoInicial, double pagoMensual, double pagoTotal)
	{
		InitializeComponent();

		lblUsuario.Text = "Usuario conectado: " + usuario;
		lblNombre.Text = nombre;
		lblApellido.Text = apellido;
		lblEdad.Text = edad.ToString();
		lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
		lblPlan.Text = plan;
		lblCasillero.Text = casillero ? "Sí" : "No";
		lblMontoInicial.Text = "$ " + montoInicial.ToString("N2");
		lblPagoMensual.Text = "$ " + pagoMensual.ToString("N2");
		lblPagoTotal.Text = "$ " + pagoTotal.ToString("N2");
	}

	private async void btnExportar_Clicked(object? sender, EventArgs e)
	{
		try
		{
			string contenido =
				"RESUMEN DEL SOCIO\n" +
				"Nombre: " + lblNombre.Text + "\n" +
				"Apellido: " + lblApellido.Text + "\n" +
				"Edad: " + lblEdad.Text + "\n" +
				"Fecha de inicio: " + lblFechaInicio.Text + "\n" +
				"Plan: " + lblPlan.Text + "\n" +
				"Casillero: " + lblCasillero.Text + "\n" +
				"Monto Inicial: " + lblMontoInicial.Text + "\n" +
				"Pago mensual: " + lblPagoMensual.Text + "\n" +
				"Pago Total: " + lblPagoTotal.Text + "\n";

			string nombreArchivo = "socio_" + lblNombre.Text + "_" + lblApellido.Text + ".txt";
			string ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);
			await File.WriteAllTextAsync(ruta, contenido);

			await DisplayAlertAsync("EXPORTADO", "Archivo: " + nombreArchivo + "\nRuta: " + FileSystem.AppDataDirectory, "Cerrar");
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private async void btnCerrarSesion_Clicked(object? sender, EventArgs e)
	{
		await Navigation.PopToRootAsync();
	}
}
