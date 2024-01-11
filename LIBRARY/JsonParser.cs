namespace LIBRARY
{
    public static class JsonParser
    {
        public enum State
        {
            Field,
            ContentField,
            ContentFieldForId, // это специальное состояние для id, так как оно идет без кавычек
            ContentFileForMassiv // состояние для полей с данными из массивов
        }
        public static void ReadJson()
        {
            string[] massivOfFields = { "store_id", "store_name", "location", "employees", "products" };
            Object[] myObjects = new object[][0];
            
            string allStrings = File.ReadAllText(fPath);
            if (allStrings==null || allStrings.Length == 0) // если файл пустой или null выбрасываем исключение
            {
                // throw new ArgumentNullException();
            }

            int indexOfObject = 0; // индекс для заноса объектов в массив
            int indexOfLetters = 0; // индекс по проходу всего файла
            string nameField; string contentOfField;
            
            State state = State.Field;
            while (indexOfLetters < allStrings.Length)
            {
                
                char symbol = allStrings[indexOfLetters];
                switch (state)
                {
                    case State.Field when symbol == '"':
                        commentCount++;
                        state = State.Comment;
                        indexOfLetters++;
                        break;
                    case State.ContentField when symbol == '"':
                        stringCount++;
                        state = State.String;
                        indexOfLetters++;
                        break;
                    case State.Comment when symbol == '\n':
                        state = State.Program;
                        indexOfLetters++;
                        break;
                    case State.String when symbol == '"':
                        state = State.Program;
                        indexOfLetters++;
                        break;
                    case State.Field when symbol == '}':
                        indexOfObject++;
                        indexOfLetters++;
                        break;
                }
            }
        }
    }
}