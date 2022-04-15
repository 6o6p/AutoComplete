using System;
using System.Collections.Generic;

namespace AutoComplete
{
    public struct FullName
    {
        public string Name;
        public string Surname;
        public string Patronymic;

        public override string ToString()
        {
            var result = new List<string>(3);

            if (!string.IsNullOrEmpty(Surname)) //Кажется, если name состоит из пробелов, то в выводе будет два пробела :) Тут бы подошел string.IsNullOrWhitespace()
                result.Add(Surname.Trim());     


            if (!string.IsNullOrEmpty(Name))
                result.Add(Name.Trim());

            if (!string.IsNullOrEmpty(Patronymic))
                result.Add(Patronymic.Trim());

            return string.Join(' ', result);
        }
    }

    public class AutoCompleter
    {
        private readonly List<string> _fullNames = new List<string>(); //Можно было бы воспользоваться структурой SortedList, чтобы не сортировать при каждом добавлении

        public void AddToSearch(List<FullName> fullNames)
        {
            foreach (var fullName in fullNames)
            {
                _fullNames.Add(fullName.ToString());
            }

            _fullNames.Sort();
        }

        public List<string> Search(string prefix)
        {
            if (prefix.Length > 100 || prefix.Trim() == String.Empty) //string.IsNullOrWhitespace()
                throw new ArgumentOutOfRangeException("Запрос не может быть пустым или иметь длину больше 100 символов");

            var leftBorder = GetLeftBorder(prefix);
            var rightBorder = GetRightBorder(prefix);

            return _fullNames.GetRange(leftBorder, rightBorder - leftBorder);
        }

        private int GetLeftBorder(string prefix) => 
              ~GetIndexByPredicate(prefix, (itemA, itemB) =>      //Можно было бы реализовать компаратор, было бы эффективнее.
                    string.Compare(itemA, itemB, StringComparison.OrdinalIgnoreCase) >= 0 
              );

        private int GetRightBorder(string prefix) =>
               ~GetIndexByPredicate(prefix, (itemA, itemB) => 
                    string.Compare(itemA, itemB, StringComparison.OrdinalIgnoreCase) > 0 
                 && !itemA.StartsWith(itemB, StringComparison.OrdinalIgnoreCase)
               );

        private int GetIndexByPredicate(string prefix, Func<string, string, bool> predicate) =>
            _fullNames.BinarySearch(prefix, Comparer<string>.Create((itemA, itemB) => predicate(itemA, itemB) ? 1 : -1));
    }
}