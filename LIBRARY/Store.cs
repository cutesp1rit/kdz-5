using System.Text;

namespace LIBRARY;

public class Store : IComparable
{
    private readonly int _storeId;
    private readonly string _storeName;
    private readonly string _location;
    private readonly string[] _employees;
    private readonly string[] _products;

    public int StoreId
    {
        get { return _storeId;  }
    }

    public string StoreName
    {
        get { return _storeName; }
    }

    public string Location
    {
        get { return _location; }
    }

    public string[] Employees
    {
        get { return _employees; }
    }
    
    public string[] Products
    {
        get { return _products; }
    }

    public Store(int storeId, string storeName, string location, string[] employees, string[] products)
    {
        if (storeName == null || location == null || employees == null || products == null)
        {
            throw new ArgumentNullException();
        }

        _storeId = storeId;
        _storeName = storeName;
        _location = location;
        _employees = employees;
        _products = products;
    }

    /// <summary>
    /// Переопределенный метод ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder myString = new StringBuilder();
        myString.Append("Store_id: ");
        myString.Append(StoreId);
        myString.Append('\n');
        myString.Append("Store_name: ");
        myString.Append(StoreName);
        myString.Append('\n');
        myString.Append("Location: ");
        myString.Append(Location);
        myString.Append('\n');
        myString.Append("Employees: ");
        for (int i = 0; i < Employees.Length; i++)
        {
            myString.Append(Employees[i]);
            if (i != Employees.Length - 1)
            {
                myString.Append(", ");
            }
        }
        myString.Append('\n');
        myString.Append("Products: ");
        for (int i = 0; i < Products.Length; i++)
        {
            myString.Append(Products[i]);
            if (i != Products.Length - 1)
            {
                myString.Append(", ");
            }
        }
        myString.Append('\n');
        return myString.ToString();
    }

    /// <summary>
    /// Возвращает соответсвенное значение значению поля переданного в параметре
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public string WhatIsFieldString(string field)
    {
        if (field == "store_name")
        {
            return StoreName;
        }
        
        if (field == "location")
        {
            return Location;
        }

        if (field == "employees")
        {
            if (Employees.Length == 0)
            {
                return "-";
            }
            return Employees[0];
        }
        
        if (field == "products")
        {
            if (Products.Length == 0)
            {
                return "-";
            }
            return Products[0];
        }

        if (field == "store_id")
        {
            return StoreId.ToString();
        }

        return "";
    }

    /// <summary>
    /// Возвращает соответсвенный массив значению поля переданного в параметре
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public string[] WhatIsFieldArray(string field)
    {
        if (field == "employees")
        {
            return Employees;
        }

        if (field == "products")
        {
            return Products;
        }

        return new string[0];
    }
    /// <summary>
    /// Сравнение объектов через Id
    /// </summary>
    /// <param name="someObject"></param>
    /// <returns></returns>
    public int CompareTo(object? someObject)
    {
        // сравнивать можно через id
        if (this.StoreId > ((Store) someObject).StoreId)
        {
            return 1; 
        }
        if (this.StoreId < ((Store) someObject).StoreId)
        {
            return -1;
        }
        return 0;
    }
}