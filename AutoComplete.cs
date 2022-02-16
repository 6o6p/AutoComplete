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

            if (!string.IsNullOrEmpty(Surname))
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
        private readonly List<string> _fullNames = new List<string>();

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
            if (prefix.Length > 100 || prefix.Trim() == String.Empty)
                throw new ArgumentOutOfRangeException("Запрос не может быть пустым или иметь длину больше 100 символов");

            var leftBorder = GetLeftBorder(prefix);
            var rightBorder = GetRightBorder(prefix);

            return _fullNames.GetRange(leftBorder, rightBorder - leftBorder);
        }

        private int GetLeftBorder(string prefix) => 
              ~GetIndexByPredicate(prefix, (itemA, itemB) => 
                    string.Compare(itemA, itemB, StringComparison.OrdinalIgnoreCase) >= 0
              );

        private int GetRightBorder(string prefix) =>
               ~GetIndexByPredicate(prefix, (itemA, itemB) => 
                    string.Compare(itemA, itemB, StringComparison.OrdinalIgnoreCase) > 0 
                 && !itemA.StartsWith(itemB, StringComparison.OrdinalIgnoreCase)
               );

        private int GetIndexByPredicate(string prefix, Func<string, string, bool> predicate) =>
            _fullNames.BinarySearch(prefix, Comparer<string>.Create((itemA, itemB) => predicate(itemA, itemB) ? 1 : -1));


        ///// <summary>
        ///// Возвращает индекс первого элемента, который содержит <paramref name="prefix"/>. 
        ///// Если такого элемента нет, возвращает число элементов в коллекции поиска.
        ///// Регистр игнорируется.
        ///// Не очень красиво настроен предикат. Работает. Тесты проходит.
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <returns></returns>
        //private int GetLeftBorder(string prefix) => GetIndexOfElementToRightOfPredicate(prefix, index =>
        //    string.Compare(_fullNames[index], prefix, StringComparison.OrdinalIgnoreCase) < 0
        //);

        ///// <summary>
        ///// Возвращает индекс первого элемента, который не содержит <paramref name="prefix"/> и лексикографически больше него. 
        ///// Если такого элемента нет, возвращает ноль.
        ///// Регистр игнорируется.
        ///// Не очень красиво настроен предикат. Работает. Тесты проходит.
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <returns></returns>
        //private int GetRightBorder(string prefix) => GetIndexOfElementToRightOfPredicate(prefix, index =>
        //    _fullNames[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
        // || string.Compare(_fullNames[index], prefix, StringComparison.OrdinalIgnoreCase) < 0
        //);

        ///// <summary>
        ///// Производит бинарный поиск <paramref name="prefix"/> по коллекции с условием <paramref name="predicate"/>.
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //private int GetIndexOfElementToRightOfPredicate(string prefix, Predicate<int> predicate)
        //{
        //    var left = -1;
        //    var right = _fullNames.Count;
        //    var middle = 0;

        //    while (left + 1 != right)
        //    {
        //        middle = left + (right - left) / 2;

        //        if (predicate(middle))
        //            left = middle;
        //        else
        //            right = middle;
        //    }
        //    return right;
        //}
    }
}