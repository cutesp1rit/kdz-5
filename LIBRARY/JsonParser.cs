using System.Text;

namespace LIBRARY
{
    public static class JsonParser
    {
        enum State
        {
            Field,
            ContentField,
            ContentFieldForId, // это специальное состояние для id, так как оно идет без кавычек, а как число
            ContentFiledForMassiv // состояние для полей с данными из массивов
        }
        // проверь, чтобы со списками все работало!!!!!
        public static List<Store> ReadJson(string allStrings, List<Store> objects)
        {
            string[] massivOfFields = { "store_id", "store_name", "location", "employees", "products" };
            string[] information = { "", "", ""}; // сохранение информации для некоторых полей
            string[] employeesMassiv = new string[0]; // массив для всех сотрудников
            string[] productsMassiv = new string[0]; // массив для всех продуктов
            
            int indexOfLetters = 0; // индекс по проходу всего файла
            string nameField=""; string contentOfField="";
            StringBuilder contentForMassiv = new StringBuilder(); // строим строку с данными, чтобы потом сделать массив
            
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
                        // возможно может быть ошибка из-за \n or \r, но вроде нет
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
                    case State.ContentField when symbol == '[':
                        state = State.ContentFiledForMassiv;
                        indexOfLetters++;
                        break;
                    case State.ContentFiledForMassiv when symbol == '"':
                        StringBuilder content = new StringBuilder();
                        indexOfLetters++;
                        while (allStrings[indexOfLetters] != '"')
                        {
                            content.Append(allStrings[indexOfLetters]);
                            indexOfLetters++;
                        }
                        indexOfLetters++;

                        contentForMassiv.Append(content);
                        contentForMassiv.Append(';');
                        break;
                    case State.ContentFiledForMassiv when symbol == ']':
                        state = State.Field;
                        indexOfLetters++;
                        if (nameField == "employees")
                        {
                            employeesMassiv = contentForMassiv.ToString().Split(';')[..^1];
                            // обрезаем массив, так как при сплите самый последний элемент - пустота
                        }
                        else // значит это products
                        {
                            productsMassiv = contentForMassiv.ToString().Split(';')[..^1];
                        }
                        contentForMassiv = new StringBuilder(); // обнуляем
                        break;
                    case State.Field when symbol == '}':
                        // и еще создать объект здесь надо, а также массив обнулить и все переменные обнулить, наверное
                        int id = 0;
                        // если id не целый или null, или там лишние символы
                        if (!int.TryParse(information[0], out id))
                        {
                            id = -1; // или может здесь лучше исключение выкинуть
                        }
                        
                        objects.Add(new Store(id, information[1], information[2], employeesMassiv,
                            productsMassiv));
                        
                        // обнуляем все массивы для новых будущих объектов
                        information[0] = ""; information[1] = ""; information[2] = "";
                        employeesMassiv = new string[0]; 
                        productsMassiv = new string[0];
                        indexOfLetters++;
                        break;
                    case State.ContentField when symbol == ',':
                        // вместо значения был null --> оставляем пустое значение в таком случае
                        state = State.Field;
                        indexOfLetters++;
                        break;
                    default:
                        indexOfLetters++;
                        break;
                }
            }

            return objects;
        }
    }
}