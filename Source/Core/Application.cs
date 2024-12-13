namespace Source.Core
{
    public class Application
    {
        public void Launch()
        {
            InitializeConsoleSettings();

            LaunchGame();
        }

        private void InitializeConsoleSettings()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private void LaunchGame()
        {
            var game = new Game();

            game.Launch();
        }
    }
}