namespace ayepezExamen.Views;

public partial class vLogin : ContentPage
{
	string[,] usuarios =
	{
		{ "recepcion", "gym2026" },
		{ "entrenador", "fit_01" },
		{ "uisrael", "2026" },
		{ "gerente", "admin123" }
	};

	public vLogin()
	{
		InitializeComponent();
	}

	private async void btnIniciarSesion_Clicked(object? sender, EventArgs e)
	{
		try
		{
			string usuario = txtUsuario.Text?.Trim() ?? "";
			string contrasena = txtContrasena.Text ?? "";

			if (usuario == "" || contrasena == "")
			{
				await DisplayAlertAsync("ERROR", "Ingresa el usuario y la contraseña", "Cerrar");
				return;
			}

			for (int i = 0; i < usuarios.GetLength(0); i++)
			{
				if (usuarios[i, 0] == usuario && usuarios[i, 1] == contrasena)
				{
					txtUsuario.Text = "";
					txtContrasena.Text = "";
					await Navigation.PushAsync(new vRegistro(usuario));
					return;
				}
			}

			await DisplayAlertAsync("ERROR", "Usuario o contraseña incorrectos", "Cerrar");
			txtContrasena.Text = "";
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("ERROR", ex.Message, "Cerrar");
		}
	}

	private async void btnAcercaDe_Clicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new vAcercaDe());
	}
}
