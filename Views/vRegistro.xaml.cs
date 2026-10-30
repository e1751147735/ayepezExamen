using System.Globalization;
using System.Text.RegularExpressions;

namespace ayepezExamen.Views;

public partial class vRegistro : ContentPage
{
	const double costoMembresia = 1200;
	const double porcentajeMinimo = 0.10;
	const double porcentajeMaximo = 0.50;
	const double porcentajeCuota = 0.02;
	const double valorCasillero = 10;
	const int numeroCuotas = 5;
	const int edadMinima = 16;
	const int edadMaxima = 80;
	const double montoMinimo = costoMembresia * porcentajeMinimo;
	const double montoMaximo = costoMembresia * porcentajeMaximo;

	string usuario;

	public vRegistro(string usuarioConectado)
	{
		InitializeComponent();
		usuario = usuarioConectado;
		lblUsuario.Text = "Usuario conectado: " + usuario;
		dpFechaInicio.MinimumDate = DateTime.Today;
		dpFechaInicio.Date = DateTime.Today;
		txtMontoInicial.Placeholder = "Entre " + montoMinimo + " y " + montoMaximo;
	}

	private async void btnCalcular_Clicked(object? sender, EventArgs e)
	{
		try
		{
			double? montoInicial = await ValidarMontoInicial();
			if (montoInicial == null)
			{
				return;
			}

			txtPagoMensual.Text = CalcularPagoMensual(montoInicial.Value).ToString("N2");
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private async void btnVerResumen_Clicked(object? sender, EventArgs e)
	{
		try
		{
			string nombre = txtNombre.Text?.Trim() ?? "";
			string apellido = txtApellido.Text?.Trim() ?? "";
			string edadIngresada = txtEdad.Text?.Trim() ?? "";

			if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
			{
				await DisplayAlertAsync("ERROR", "El nombre es obligatorio y solo puede contener letras", "Cerrar");
				return;
			}

			if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
			{
				await DisplayAlertAsync("ERROR", "El apellido es obligatorio y solo puede contener letras", "Cerrar");
				return;
			}

			if (!Regex.IsMatch(edadIngresada, @"^\d+$"))
			{
				await DisplayAlertAsync("ERROR", "La edad es obligatoria y solo puede contener números", "Cerrar");
				return;
			}

			int edad = int.Parse(edadIngresada);

			if (edad < edadMinima)
			{
				await DisplayAlertAsync("ERROR", "Para registrarse debe tener " + edadMinima + " años o más", "Cerrar");
				return;
			}

			if (edad > edadMaxima)
			{
				await DisplayAlertAsync("ERROR", "La edad no puede ser mayor a " + edadMaxima + " años", "Cerrar");
				return;
			}

			if (dpFechaInicio.Date == null || dpFechaInicio.Date < DateTime.Today)
			{
				await DisplayAlertAsync("ERROR", "La fecha de inicio no puede ser anterior a hoy", "Cerrar");
				return;
			}

			if (pkPlan.SelectedIndex == -1)
			{
				await DisplayAlertAsync("ERROR", "Selecciona el plan", "Cerrar");
				return;
			}

			double? montoInicial = await ValidarMontoInicial();
			if (montoInicial == null)
			{
				return;
			}

			DateTime fechaInicio = dpFechaInicio.Date.Value;
			string plan = pkPlan.Items[pkPlan.SelectedIndex];
			bool casillero = swCasillero.IsToggled;
			double pagoMensual = CalcularPagoMensual(montoInicial.Value);
			double pagoTotal = montoInicial.Value + pagoMensual * numeroCuotas;

			txtPagoMensual.Text = pagoMensual.ToString("N2");

			await Navigation.PushAsync(new vResumen(usuario, nombre, apellido, edad, fechaInicio, plan, casillero, montoInicial.Value, pagoMensual, pagoTotal));
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private async Task<double?> ValidarMontoInicial()
	{
		txtPagoMensual.Text = "";
		string montoIngresado = txtMontoInicial.Text?.Trim().Replace(",", ".") ?? "";

		if (!Regex.IsMatch(montoIngresado, @"^\d+(\.\d+)?$"))
		{
			await DisplayAlertAsync("ERROR", "El monto inicial es obligatorio y solo puede contener números", "Cerrar");
			return null;
		}

		double montoInicial = double.Parse(montoIngresado, CultureInfo.InvariantCulture);

		if (montoInicial < montoMinimo || montoInicial > montoMaximo)
		{
			await DisplayAlertAsync("ERROR", "El monto inicial debe estar entre " + montoMinimo + " y " + montoMaximo, "Cerrar");
			return null;
		}

		return montoInicial;
	}

	private double CalcularPagoMensual(double montoInicial)
	{
		double pagoMensual = (costoMembresia - montoInicial) / numeroCuotas + costoMembresia * porcentajeCuota;

		if (swCasillero.IsToggled)
		{
			pagoMensual += valorCasillero;
		}

		return pagoMensual;
	}
}
