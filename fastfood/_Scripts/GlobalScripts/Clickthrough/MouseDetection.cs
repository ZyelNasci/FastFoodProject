using Godot;

public partial class MouseDetection : Node
{
	
	// Autoloaded
	
	private ApiManager _api;

	public override void _Ready()
	{
		_api = GetNode<ApiManager>("/root/ApiManager");
		
		// initializing as click-through
		_api.SetClickThrough(true);
	}
	
	// it is better to detect the pixels only when rendered, so PhysicsProcess is recommended
	// also can throttle the detection every few frames is possible
	public override void _PhysicsProcess(double delta)
	{
		DetectPassthrough();
	}

	
	// Detection of what color is the pixel under the mouse cursor, based on the viewport texture
	// This can become expensive if done every frame and in more complex scenes.
	// We will use this to determine whether the window should be clickable or not
	// You can choose any other method of detection!
	private void DetectPassthrough()
	{
		Viewport viewport = GetViewport();
		Image img = viewport.GetTexture().GetImage();
		Rect2 rect = viewport.GetVisibleRect();

		Vector2 mousePosition = viewport.GetMousePosition();

		// Verifica se o mouse está dentro da área visível
		if (!rect.HasPoint(mousePosition))
		{
			img.Dispose();
			return; // sai da função se estiver fora
		}

		int viewX = (int)(mousePosition.X + rect.Position.X);
		int viewY = (int)(mousePosition.Y + rect.Position.Y);

		int x = (int)(img.GetSize().X * viewX / rect.Size.X);
		int y = (int)(img.GetSize().Y * viewY / rect.Size.Y);

		// Verifica se x e y estão dentro dos limites da imagem
		if (x >= 0 && x < img.GetSize().X && y >= 0 && y < img.GetSize().Y)
		{
			Color pixel = img.GetPixel(x, y);
			SetClickability(pixel.A > 0.5f);
		}

		img.Dispose();
	}
	
	// instead of calling the API every frame, we check if the state is changed and then call it if necessary
	private bool _clickthrough = true;
	private void SetClickability(bool clickable)
	{
		if (clickable != _clickthrough)
		{
			_clickthrough = clickable;
			// clickthrough means NOT clickable
			_api.SetClickThrough(!clickable);
		}
	}
}
