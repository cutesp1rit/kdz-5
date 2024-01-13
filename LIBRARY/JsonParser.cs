﻿using System.Text;

namespace LIBRARY
{
    public static class JsonParser
    {
        public enum State
        {
            Field,
            ContentField,
            ContentFieldForId, // это специальное состояние для id, так как оно идет без кавычек, а как число
            ContentFileForMassiv // состояние для полей с данными из массивов
        }
        public static void ReadJson()
        {
            string[] massivOfFields = { "store_id", "store_name", "location", "employees", "products" };
            string[] information = new string[3]; // сохранение информации для некоторых полей
            
            Object[] myObjects = new object[0];
            
            string allStrings = File.ReadAllText(fPath);
            if (allStrings==null || allStrings.Length == 0) // если файл пустой или null выбрасываем исключение
            {
                // throw new ArgumentNullException();
            }

            int indexOfObject = 0; // индекс для заноса объектов в массив
            int indexOfLetters = 0; // индекс по проходу всего файла
            string nameField=""; string contentOfField="";
            
            State state = State.Field;
            while (indexOfLetters < allStrings.Length)
            {
                
                char symbol = allStrings[indexOfLetters];
                switch (state)
                {
                    case State.Field when symbol == '"': // считываем поле
                        indexOfLetters++;
                        StringBuilder field = new StringBuilder();
                        while (allStrings[indexOfLetters] != '"')
                        {
                            field.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }

                        nameField = field.ToString();
                        indexOfLetters++;
                        if (nameField == "store_id")
                        {
                            state = State.ContentFieldForId;
                        }
                        else
                        {
                            state = State.ContentField;
                        }
                        break;
                    case State.ContentFieldForId when symbol == ':':
                        // возможно может быть ошибка из-за enter, но вроде нет
                        StringBuilder fieldContentId = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != ',')
                        {
                            fieldContentId.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }
                        indexOfLetters++;

                        contentOfField = fieldContentId.ToString().Trim(' ');
                        state = State.Field;
                        information[0] = contentOfField;
                        break;
                    case State.ContentField when symbol == '"':
                        StringBuilder fieldContent = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != '"')
                        {
                            fieldContent.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }
                        indexOfLetters++;
                        
                        contentOfField = fieldContent.ToString();
                        state = State.Field;
                        
                        if (nameField == "store_name")
                        {
                            information[1] = contentOfField;
                        }
                        else // значит это location
                        {
                            information[2] = contentOfField;
                        }
                        break;
                    case State.String when symbol == '"':
                        state = State.Program;
                        indexOfLetters++;
                        break;
                    case State.Field when symbol == '}':
                        // и еще создать объект здесь надо
                        indexOfObject++;
                        indexOfLetters++;
                        break;
                    default:
                        indexOfLetters++;
                        break;
                }
            }
        }
    }
}