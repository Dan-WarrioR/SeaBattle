namespace Source.Core
{
    public class Application
    {
        public void Launch()
        {
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.CursorVisible = false;

			var game = new Game();

			game.Launch();
		}
    }
}