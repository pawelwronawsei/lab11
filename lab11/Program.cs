using System.Reflection.Metadata;

class PersonComparer : Comparer<Person>
{
    public override int Compare(Person? x, Person? y)
    {
        return x.Name.CompareTo(y.Name);
    }
}

class Person
{
    public string Name { get; set; }
    public string Email { get; set; }

    public override bool Equals(object? obj)
    {
        Console.WriteLine("Equals");
        if (this == obj) return true;
        Person? other = obj as Person;
        if(other == null) return false;
        return Name == other.Name && Email == other.Email;
    }

    public override int GetHashCode()
    {
        Console.WriteLine("GetHashCode");
        return HashCode.Combine(Name, Email);
    }

    public override string? ToString()
    {
        return $"Name: {Name}";
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        //IEnumerableDemo();
        //CollectionDemo();
        //IListDemo();
        //ISetDemo();
        //IDictionaryDemo();
        CollectionsForCustomTypesDemo();
        //SortDemo();
    }

    public static void SortDemo()
    {
        List<string> list = new List<string>() { "Abc", "aancd", "ajks", "sgre", "wg", "rr" };
        //list.Sort();
        Console.WriteLine(string.Join(", ", list));

        ISet<string> sorted = new SortedSet<string>(list);
        Console.WriteLine(string.Join(", ", sorted));
    }

    public static void CollectionsForCustomTypesDemo()
    {
        List<Person> list = new List<Person>()
        {
            new Person() { Name = "Adam", Email = "Adam@wsei.pl"},
            new Person() { Name = "Ewa", Email = "Ewa@wsei.pl"},
            new Person() { Name = "Karol", Email = "Karol@wsei.pl"}
        };

        Console.WriteLine(list.Contains(new Person() { Name = "Karol", Email = "kk@wsei.pl" }));

        ISet<Person> set = new HashSet<Person>(list);
        set.Add(new Person() { Name = "Karol", Email = "kk@wsei.pl" });
        list.Sort(new PersonComparer());
        Console.WriteLine(string.Join(", ", list));
    }

    public static void IDictionaryDemo()
    {
        IDictionary<string, Person> phoneBook = new Dictionary<string, Person>() 
        {
            {"111222333", new Person() { Name = "Adam", Email = "Adam@wsei.pl"} },
            {"223456789", new Person() { Name = "Ewa", Email = "Ewa@wsei.pl"} },
            {"333444555", new Person() { Name = "Karol", Email = "Karol@wsei.pl"} }
        };

        foreach(var item in phoneBook)
        {
            Console.WriteLine($"Nr tel.: {item.Key}, Imię: {item.Value.Name}");
        }

        string phone = Console.ReadLine();
        if (phoneBook.ContainsKey(phone))
        {
            Console.WriteLine(phoneBook[phone].Name);
        }
        else
        {
            Console.WriteLine("Brak osoby o takim numerze.");
        }

        //dodaj do phoneBook nową osobe z nr 444444444
        phoneBook.Add("444444444", new Person() { Name = "Wojtek", Email = "Wojtek@wsei.pl" });

        //usuń osobę o nr 111222333
        phoneBook.Remove("111222333");

        //wyświetl te osoby, których nr zaczyna się od 2
        foreach(var itemKey in phoneBook.Keys)
        {
            if (itemKey.StartsWith("2"))
            {
                Console.WriteLine(phoneBook[itemKey].Name);
            }
        }
    }

    public static void ISetDemo()
    {
        ISet<int> set = new HashSet<int>() { 2, 5, 7, 2, 3, 6, 8, 9, 3 };

        Console.WriteLine(string.Join(", ", set));
        Console.WriteLine(set.Add(7)); //Zwraca true jeśli można dodać, czyli jesli set nie zawierał 7, w przeciwnym razie false
        //utwórz sumę zbioru set + zbioru 4 i 10
        set.UnionWith(new int[] { 4, 10 });

        //utwórz zbiór evensSet liczb parzystych od 2 do 8
        //zmodyfikuj set aby zawierał różnice z evensSet
        ISet<int> evensSet = new HashSet<int>(EvensGenerator(2, 8));
        set.ExceptWith(evensSet);
        Console.WriteLine(string.Join(", ", set));

        //wordsDistinct to zbiór z words bez powtórzeń
        string[] words = { "ale", "i", "w", "kot", "ale", "i", "w" };
        ISet<string> wordsDistinct = new HashSet<string>(words);
        Console.WriteLine(string.Join(", ", wordsDistinct));
    }

    public static void IListDemo()
    {
        IList<string> list = new List<string>() { "Adam", "Ewa", "Karol", "Robert", "Ewa" };
        //usuń następną os za pierwszą Ewą
        int i = list.IndexOf("Ewa");
        
        if(i > -1)
        {
            if(i + 1 < list.Count)
            {
                list.RemoveAt(i + 1);
            }
        }

        //zamień pierwszą Ewę na Ewelinę
        int indexOfFirstEwa = list.IndexOf("Ewa");
        if(i > -1)
        {
            list[i] = "Ewelina";
        }
        Console.WriteLine(string.Join(", ", list));

        //wstaw imię Beata przed Robertem
        int indexOfKarol = list.IndexOf("Robert");
        if (indexOfKarol > -1)
        {
            list.Insert(indexOfKarol, "Beata");
        }

    }

    public static void CollectionDemo()
    {
        ICollection<string> collection = new List<string>() { "Adam", "Ewa", "Roman", "Robert" }; //Add, Remove, Count, Contains

        Console.WriteLine(collection.Contains("Beata"));

        collection.Add("Wojtek");

        Console.WriteLine(collection.Remove("Ewa")); //da true jeśli się powiodło, da false jeśli Ewy nie ma w liście | USUWA pierwsze na które natrafi

        Console.WriteLine(collection.Count);

        ICollection<string> copy = new List<string>(collection); //tworzy kopię powyższej kolekcji

        ICollection<string> copy2 = new List<string>(new string[] { "A", "B" });


        Console.WriteLine("KOLEKCJA LICZB PARZYSTYCH:");
        ICollection<int> evens = new List<int>(EvensGenerator(3, 15));
        foreach(int i in evens)
        {
            Console.WriteLine(i);
        }

        ICollection<int> toBeRemoved = new List<int>();

        foreach(var i in evens)
        {
            if(i > 10)
            {
                toBeRemoved.Add(i);
            }
        }

        foreach(var i in toBeRemoved)
        {
            evens.Remove(i);
        }
    }

    public static void IEnumerableDemo()
    {
        IEnumerable<int> arr = new int[] { 1, 2, 3, 4, 5 };

        foreach(int i in arr)
        {
            Console.WriteLine(i);
        }

        IEnumerable<string> arr2 = new string[] { "Adam", "Ewa", "Karol" };

        IEnumerable<int> evens = EvensGenerator(3, 8);

        for(var enumerator = evens.GetEnumerator(); enumerator.MoveNext();)
        {
            int item = enumerator.Current;
        }

        foreach(var item in EvensGenerator(3, 8)){
            Console.WriteLine(item);
        }
    }

    public static IEnumerable<int> EvensGenerator(int start, int end) 
    {
        for(int i = start + start % 2; i <= end; i += 2){
            yield return i;
        }    
    }
}