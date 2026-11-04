using Raylib_cs;
using SnakeGame;

class Program
{
    static void Main(string[] args)
    {
        const int cellSize = 25; // size of cell
        const int gridSize = 20; // size of space

        const int width = cellSize * gridSize;
        const int height = cellSize * gridSize;

        Individ individ = new Individ();
        IndividEvaluator evaluator = new IndividEvaluator();
        evaluator.Evaluate(individ);
        TrainingManager trainingManager = new();
        trainingManager.Train(200, 100, 4);

        NeuralNetwork bestNetwork = ModelStorage.Load("Model_3/best_snake_199.json");

        GameRenderer renderer = new GameRenderer(cellSize, gridSize);
        GameObservation observation = new GameObservation();
        GameEngine game = new GameEngine(cellSize, gridSize);
        AIController aIcontrol = new AIController();
        Snake snake = game.Snake;
        Food food = game.Food;
        // int x, y;

        float timer = 0; // for automatic move
        float moveInterval = 0.25f; // our fps = 60, so snake will move 60/moveInterval per second


        Raylib.InitWindow(width, height, "Neuron Snake");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {   
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                game.Restart();
                timer = 0;
            }
            // автоматическое движение
            if (!game.IsGameOver)
            {
                timer += Raylib.GetFrameTime();

                if (timer >= moveInterval)
                {
                    double[] inputs = observation.GetState(game);
                    double[] outputs = bestNetwork.Forward(inputs);
                    int action = bestNetwork.GetAction(outputs);
                    
                    // Console.WriteLine(
                    //     $"Straight: {outputs[0]:F3} | " +
                    //     $"Right: {outputs[1]:F3} | " +
                    //     $"Left: {outputs[2]:F3}"
                    // );
                    Console.WriteLine(action);
                    Direction aidir = aIcontrol.GetDirection(snake.CurrentDirection, action);

                    game.Snake.ChangeDirection(aidir);
                    
                    game.Update();
                    timer -= moveInterval;
                }

                // обработка передвижения
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

    }

}

