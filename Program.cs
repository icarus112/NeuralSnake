using Raylib_cs;
using SnakeGame;

class Program
{
    static void Main(string[] args)
    {
        int[] architecture = { 11, 16, 16, 3 };

        // Загрузка нейронной сети
        NeuralNetwork bestNetwork = ModelStorage.Load("Model_3/best_snake_300.json");

        // Обучение нейронной сети
        // TrainingManager trainingManager = new();

        // trainingManager.Train(
        //     generations: 300,
        //     populationSize: 100,
        //     numOfSaves: 4,
        //     layers: architecture
        // );

        // Запуск игры
        RunSnake(bestNetwork);
    }

    static void RunSnake(NeuralNetwork bestNetwork)
    {
        const int cellSize = 25;
        const int gridSize = 20;

        const int width = cellSize * gridSize;
        const int height = cellSize * gridSize;

        float timer = 0;
        float moveInterval = 0.25f;

        GameRenderer renderer = new GameRenderer(cellSize, gridSize);
        GameObservation observation = new GameObservation();
        GameEngine game = new GameEngine(cellSize, gridSize);
        AIController aiController = new AIController();

        Raylib.InitWindow(width, height, "Neuron Snake");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            // Перезапуск игры
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                game.Restart();
                timer = 0;
            }

            if (!game.IsGameOver)
            {
                timer += Raylib.GetFrameTime();

                // Автоматическое движение змейки
                while (timer >= moveInterval && !game.IsGameOver)
                {
                    timer -= moveInterval;

                    double[] inputs = observation.GetState(game);
                    double[] outputs = bestNetwork.Forward(inputs);

                    Console.WriteLine(
                        $"Straight: {outputs[0]:F3} | " +
                        $"Right: {outputs[1]:F3} | " +
                        $"Left: {outputs[2]:F3}"
                    );

                    int action = bestNetwork.GetAction(outputs);

                    Direction direction = aiController.GetDirection(
                        game.Snake.CurrentDirection,
                        action
                    );

                    game.Snake.ChangeDirection(direction);

                    game.Update();
                }

                // Ручное управление
                if (Raylib.IsKeyPressed(KeyboardKey.D))
                {
                    game.Snake.ChangeDirection(Direction.Right);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.A))
                {
                    game.Snake.ChangeDirection(Direction.Left);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.W))
                {
                    game.Snake.ChangeDirection(Direction.Up);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.S))
                {
                    game.Snake.ChangeDirection(Direction.Down);
                }
            }

            renderer.Draw(game);
        }

        Raylib.CloseWindow();
    }
}