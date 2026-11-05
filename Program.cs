using Raylib_cs;
using SnakeGame;

class Program
{
    static void Main(string[] args)
    {

        // TrainSnake();

        // Запуск игры
        // RunSnake();
        TrainActioner();

    }

    static void TrainSnake()
    {
        int[] architecture = { 11, 16, 16, 3 };


        // Обучение нейронной сети
        SnakeTrain trainingManager = new();

        trainingManager.Train(
            generations: 300,
            populationSize: 80,
            numOfSaves: 6,
            layers: architecture
        );
    }

    static void RunSnake()
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

        // Загрузка нейронной сети
        NeuralNetwork bestNetwork = ModelStorage.Load("/SnakeModels/Model_5/best_snake_300.json");

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

    static void TrainActioner()
    {
        int maxProjects = 10;
        int projectCount = 4;


        double[] inputs = new double[maxProjects * 3 + 1];

        int index = 0;

        foreach (var pj in Projects)
        {
            inputs[index++] = pj.Cost;
            inputs[index++] = pj.Profit;
            inputs[index++] = pj.SuccessProbability;
        }
    }
}