namespace APP
{
    public class Program
    {
        public enum State
        {
            String,
            Comment,
            Program
        }
        public static void Main()
        {
            Console.WriteLine("Введите код программы:");
            string? code = Console.ReadLine();
            State state = State.Program;
            int commentCount = 0;
            int stringCount = 0;
            foreach (var symbol in code ?? "")
            {
                switch (state)
                {
                    case State.Program when symbol == '/':
                        commentCount++;
                        state = State.Comment;
                        break;
                    case State.Program when symbol == '"':
                        stringCount++;
                        state = State.String;
                        break;
                    case State.Comment when symbol == '\n':
                        state = State.Program;
                        break;
                    case State.String when symbol == '"':
                        state = State.Program;
                        break;
                }
            }

            Console.WriteLine($"Количество строк: {stringCount}");
            Console.WriteLine($"Количество комментариев: {commentCount}");
        }
    }

}